using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace QualityModsProgram
{
    [BepInPlugin("quality-mods-program.plugins.miner-info", "Miner Info Plugin", "1.0.0.0")]
    public class MinerInfo : BaseUnityPlugin
    {
        private Harmony _harmony;

        private class PluginConfig
        {
            public static ConfigEntry<bool> ShowVeinMaxMinerOutputPerSecond;
        }

        void Awake()
        {
            PluginConfig.ShowVeinMaxMinerOutputPerSecond = Config.Bind(
                "MinerInfo",
                "ShowVeinMaxMinerOutputPerSecond",
                true,
                "Show the maximum number of items per second output by all miners on a vein.");
            _harmony = Harmony.CreateAndPatchAll(typeof(ShowVeinMaxMinerOutputPerSecondPatch));

        }

        void OnDestroy()
        {
            // Make sure we unpatch ourselves when reloaded by BepInEx script engine hot-loading.
            _harmony.UnpatchSelf();
        }

        private class ShowVeinMaxMinerOutputPerSecondPatch
        {
            // Note, we return false along each return path to prevent DSP from calling into the original method.
            [HarmonyPrefix]
            [HarmonyPatch(typeof(UIVeinDetailNode), "_OnUpdate")]
            public static bool UIVeinDetailNode_OnUpdatePatch(
                ref UIVeinDetailNode __instance,
                ref Text ___infoText,
                ref int ___counter,
                ref long ___showingAmount,
                ref VeinProto ___veinProto)
            {
                if (__instance.inspectPlanet == null)
                {
                    __instance._Close();
                    return false;
                }
                PlanetData.VeinGroup veinGroup = __instance.inspectPlanet.veinGroups[__instance.veinGroupIndex];
                if (veinGroup.count == 0)
                {
                    __instance._Close();
                    return false;
                }
                if (___counter % 3 == 0)
                {
                    ___showingAmount = veinGroup.amount;
                    if (veinGroup.type != EVeinType.Oil)
                    {
                        // DSP keeps a global variable miningSpeedScale that starts at 1.0 and increases based on
                        // vein productivity research. e.g. At vein productivity level 4 this variable is 1.4.
                        float miningSpeedScale = __instance.inspectPlanet.factory.gameData.history.miningSpeedScale;

                        int minedVeinsCount = CountTotalMinedVeinsInVeinGroup(__instance.veinGroupIndex, __instance.inspectPlanet);
                        double itemsPerSecond = 0.5 * miningSpeedScale * minedVeinsCount;

                        string text = string.Concat(new string[]
                        {
                            veinGroup.count.ToString(),
                            "空格个".Translate(),
                            ___veinProto.name,
                            "储量".Translate(),
                            veinGroup.amount.ToString("#,##0"),
                        });
                        if (itemsPerSecond > 0)
                        {
                            text = string.Concat(new string[]
                            {
                                text,
                                "\nMiners max output : ",
                                itemsPerSecond.ToString("0.0"),
                                "/s"
                            });
                        }
                        ___infoText.text = text;
                    }
                    else
                    {
                        ___infoText.text = string.Concat(new string[]
                        {
                            veinGroup.count.ToString(),
                            "空格个".Translate(),
                            ___veinProto.name,
                            "产量".Translate(),
                            ((float)veinGroup.amount * VeinData.oilSpeedMultiplier).ToString("0.00"),
                            "/s"
                        });
                    }

                }
                ___counter++;
                return false;
            }

            private static int CountTotalMinedVeinsInVeinGroup(int veinGroupIndex, PlanetData planetData)
            {
                int minedVeinsCount = 0;
                foreach (VeinData veinData in planetData.factory.veinPool)
                {
                    if (veinData.groupIndex == veinGroupIndex)
                    {
                        minedVeinsCount += veinData.minerCount;
                    }
                }
                return minedVeinsCount;
            }
        }
    }
}
