using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;


namespace QualityModsProgram
{
    [BepInPlugin("quality-mods-program.plugins.starmap-buffs", "Starmap Buffs Plugin", "1.0.0.0")]
    public class StarmapBuffs : BaseUnityPlugin
    {        
        private Harmony _harmony;

        private class PluginConfig
        {
            public static ConfigEntry<bool> PinStarWithShiftClick;
            public static ConfigEntry<bool> ShowStarDetailsOnHover;
        }

        void Awake()
        {
            PluginConfig.PinStarWithShiftClick = Config.Bind(
                "StarmapBuffs",
                "PinStarWithShiftClick",
                true,
                "Pin stars on the star map by shift-clicking and add a (Pinned) suffix to pinned star names in the star detail view.");
            _harmony = Harmony.CreateAndPatchAll(typeof(PinStarWithShiftClickPatch));

            PluginConfig.ShowStarDetailsOnHover = Config.Bind(
                "StarmapBuffs",
                "ShowStarDetailsOnHover",
                true,
                "Show star details on hover.");            
            _harmony.PatchAll(typeof(ShowStarDetailsOnHoverPatch));
        }

        void OnDestroy()
        {
            // Make sure we unpatch ourselves when reloaded by BepInEx script engine hot-loading.
            _harmony.UnpatchSelf();
        }

        private class PinStarWithShiftClickPatch
        {
            [HarmonyPrefix]
            [HarmonyPatch(typeof(UIStarmap), "OnStarClick")]
            public static bool UIStarmap_OnStarClickPatch(
                UIStarmapStar star,
                ref UIStarmap __instance
            )
            {
                if (PluginConfig.PinStarWithShiftClick.Value && Input.GetKey(KeyCode.LeftShift))
                {
                    UIRoot.instance.uiGame.spaceGuide.ToggleStarPin(star.star.id);
                    // Refresh the star detail view so that the title instantly shows the (Pinned) suffix.
                    // DSP only updates this once every 30 game ticks.
                    UIRoot.instance.uiGame.starDetail.RefreshDynamicProperties();
                    return false;
                }
                else
                {
                    return true;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UIStarDetail), "RefreshDynamicProperties")]
            public static void UIStarDetail_RefreshDynamicPropertiesPatch(ref UIStarDetail __instance, ref Text ___nameText)
            {
                if (PluginConfig.PinStarWithShiftClick.Value && UIRoot.instance.uiGame.spaceGuide.IsStarPinned(__instance.star.id))
                {
                    ___nameText.text = __instance.star.displayName + " (Pinned)";
                }
                else
                {
                    ___nameText.text = __instance.star.displayName;
                }
            }
        }

        private class ShowStarDetailsOnHoverPatch
        {
            private static StarData _activeHoverStar;

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UIStarmap), "UpdateCursorView")]
            public static void UIStarmap_UpdateCursorViewPatch(
                ref UIStarmap __instance
            )
            {
                if (PluginConfig.ShowStarDetailsOnHover.Value &&
                    __instance.mouseHoverStar != null &&
                    _activeHoverStar != __instance.mouseHoverStar.star)
                {
                    _activeHoverStar = __instance.mouseHoverStar.star;
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UIGame), "_OnUpdate")]
            public static void UIGame_OnUpdatePatch(ref UIGame __instance)
            {
                if (PluginConfig.ShowStarDetailsOnHover.Value &&
                    _activeHoverStar != null &&
                    __instance.starmap.isFullOpened)
                {
                    // UIGame._OnUpdate calls SetPlanetDetail() and SetStarDetail() internally on every tick, this patch runs
                    // after _OnUpdate and overrides this logic.
                    __instance.SetPlanetDetail(null);
                    __instance.SetStarDetail(_activeHoverStar);
                }
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UIStarmap), "OnPlanetClick")]
            public static void UIStarmap_OnPlanetClickPatch()
            {
                // When a user clicks on a planet, we want to default back to normal behavior.
                _activeHoverStar = null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UIStarmap), "_OnClose")]
            public static void UIStarmap_OnClosePatch()
            {
                // When a user closes the star map, we want to default back to normal behavior.
                _activeHoverStar = null;
            }

            [HarmonyPostfix]
            [HarmonyPatch(typeof(DSPGame), "EndGame")]
            public static void DSPGame_EndGamePatch()
            {
                // Clear our shared state when exiting or loading a new game, otherwise the stored StarData is
                // invalid and DSP throws an exception.
                _activeHoverStar = null;
            }
        }
    }
}
