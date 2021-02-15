"""
Builds release based on assembly versions.
$ pip install pywin32
"""
import os
import win32api
import zipfile
import json

from typing import Dict, List, Tuple, Union

AssemblyVersion = Tuple[int, int, int, int]

GITHUB_URL = "https://github.com/brotchie/Quality-Mods-Program"
THUNDERSTORE_USERNAME = "brotchie"

RELEASES = [
    ("MinerInfo",
    ".\\Miner-Info\\bin\\Release\\Quality-Mods-Program-Miner-Info.dll",
    "Shows the maximum miner output per second for all miners on a vein."),
    ("StarmapBuffs",
    ".\\Starmap-Buffs\\bin\\Release\\Quality-Mods-Program-Starmap-Buffs.dll",
    "Shift-click to pin stars and hover to view star details in the starmap."),
]


def get_assembly_version(path: str) -> AssemblyVersion:
    """Returns the version of a .Net assembly."""
    info = win32api.GetFileVersionInfo(path, "\\")
    ms = info["FileVersionMS"]
    ls = info["FileVersionLS"]

    return win32api.HIWORD(ms), win32api.LOWORD(ms), win32api.HIWORD(ls), win32api.LOWORD(ls)


def build_manifest_json(
    name: str,
    version: str,
    description: str
) -> Dict[str, Union[str, List[str]]]:
    """Returns a manifest.json for a dsp.thunderstore.io release."""
    return ({
        "name": name,
        "version_number": version,
        "website_url": GITHUB_URL,
        "description": description,
        "dependencies": []
    })


def main():
    for mod_name, release_build, mod_description in RELEASES:
        major_version, minor_version, build_number, _ = get_assembly_version(release_build)
        release_version = f"{major_version}.{minor_version}.{build_number}"
        manifest = build_manifest_json(mod_name, release_version, mod_description)

        icon_path = os.path.join(".\\release", mod_name, "icon.png")
        if not os.path.exists(icon_path):
            raise IOError(f"Missing {icon_path}.")

        readme_path = os.path.join(".\\release", mod_name, "README.md")
        if not os.path.exists(readme_path):
            raise IOError(f"Missing {readme_path}.")

        zip_path = os.path.join(f".\\release\{THUNDERSTORE_USERNAME}-{mod_name}-{release_version}.zip")

        if os.path.exists(zip_path):
            print(f"{zip_path} already exists, skipping.")

        with zipfile.ZipFile(zip_path, "w") as zf:
            zf.writestr("icon.png", open(icon_path, "rb").read())
            zf.writestr("README.md", open(readme_path).read())
            zf.writestr("manifest.json", json.dumps(manifest))
            zf.writestr(os.path.basename(release_build), open(release_build, "rb").read())

if __name__ == "__main__":
    main()