"""This Python script copies all Assemblies required to build Dyson Sphere Program BepInEx patches.

Assemblies will be copied into the libs directory, created in the same folder as this script resides.
"""
from typing import Iterable
import os
import shutil
import winreg

DSP_LIBS_TO_COPY = (
    "Assembly-CSharp.dll",
    "UnityEngine.CoreModule.dll",
    "UnityEngine.UI.dll",
    "UnityEngine.dll",
)

BEPINEX_LIBS_TO_COPY = (
    "0Harmony.dll",
    "BepInEx.Harmony.dll",
    "BepInEx.dll",
)

STEAM_REGISTRY_KEY = "SOFTWARE\Wow6432Node\Valve\Steam"


def get_steam_install_path() -> str:
    """Return Steam install path, raises Exception if it couldn't be found in the registry."""
    try:
        with winreg.OpenKeyEx(winreg.HKEY_LOCAL_MACHINE, STEAM_REGISTRY_KEY) as key:
            value, _ = winreg.QueryValueEx(key, "InstallPath")
            return value
    except FileNotFoundError:
        raise Exception("Could not find Steam install path.")


def copy_libs(
    libs_to_copy: Iterable[str],
    source_path: str,
    destination_path: str
) -> None:
    """Copy libraries from source to destination."""
    for lib in libs_to_copy:
        source_lib_path = os.path.join(source_path, lib)
        if not os.path.exists(source_lib_path):
            raise IOError(f"Library {lib} not found in {source_path}.")
        destination_lib_path = os.path.join(destination_path, lib)
        print(f"Copying {lib} from \"{source_path}\" to \"{destination_path}\".")
        shutil.copyfile(source_lib_path, destination_lib_path)


def main():
    destination_path = os.path.join(os.path.dirname(__file__), "libs")
    if not os.path.exists(destination_path):
        print(f"Creating \"{destination_path}\".")
        os.mkdir(destination_path)

    steam_path = get_steam_install_path()
    if not os.path.exists(steam_path):
        raise Exception(f"Steam path found in Registy \"{steam_path}\", but it doesn't exist!")
    dsp_path = os.path.join(steam_path, "steamapps\common\Dyson Sphere Program")
    dsp_libs_path = os.path.join(dsp_path, "DSPGAME_Data\Managed")
    dsp_bepinex_path = os.path.join(dsp_path, "BepInEx\core")

    if not os.path.exists(dsp_bepinex_path):
        raise Exception("BepInEx not installed!")

    copy_libs(DSP_LIBS_TO_COPY, dsp_libs_path, destination_path)
    copy_libs(BEPINEX_LIBS_TO_COPY, dsp_bepinex_path, destination_path)


if __name__ == "__main__":
    main()