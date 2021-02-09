# Quality Mods Program
Quality-of-Life mods for Dyson Sphere Program. Currently three mods:

1. Show vein max miner output per second,
2. Pin star with shift click,
3. Show star details on hover.

See below for details of each mod, **installation instructions are at the bottom**.

## Miner Info Mod
Plugin: `Quality-Mods-Program-Miner-Info.dll`

**Show Vein Max Miner Output Per Second**
When the Vein Distribution Details Display is on, add an extra line of text to the Vein Group label with the miners max output per second.

Config Option: `ShowVeinMaxMinerOutputPerSecond`

![Vein Max Miner Output Label](images/vein-max-miner-output.jpg?raw=true)

## Starmap Buffs Mod
Plugin: `Quality-Mods-Program-Starmap-Buffs.dll`

**Pin Star With Shift Click**
Hold shift while clicking a star in the star map to pin it. Pinned stars will show a "(Pinned)" suffix after their name in the star detail view.

Config Option: `PinStarWithShiftClick`

**Show Star Details On Hover**
Mouse over a star in the star map to view its details. This makes it really easy to scan stars for available resources rather than clicking and then scrolling out.

Config Option: `ShowStarDetailsOnHover`

# Installation

1. Find your Dyson Sphere Program (DSP) game folder, most likely `C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program`. You should see `DSPGAME.exe` in this folder.
2. Download the [latest x64 BepInEx release](https://github.com/BepInEx/BepInEx/releases). Will be a zip file name similar to `BepInEx_x64_N.N.N.N.zip`.
3. Unzip the BepInEx zip files into your DSP folder so that `doorstop_config.ini` sits alongside `DSPGAME.exe`.
4. Start DSP from Steam (Note, it sometimes won't work if you try and run DSPGAME.exe directly!). 
5. Exit DSP immediately after you see the menu.
6. Check in the `BepInEx` folder in your DSP game folder, you should see a `plugins` folder, this is where you're going to put the Quality Mods Program plugins. This folder will typically be `C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program\BepInEx\plugins`.
7. Download the latest Quality Mods Program plugins (DLLs) from the [Release page](releases/) into your plugins folder from the previous step.
8. Start DSP from Steam.
9. Exit DSP immediately after you see the menu.
10. Go to your `BepInEx` config folder. This folder will typically be `C:\Program Files (x86)\Steam\steamapps\common\Dyson Sphere Program\BepInEx\config`.
11. If everything is working as expected you should see `quality-mods-program.plugins.miner-info.cfg` and `quality-mods-program.plugins.starmap-buffs.cfg`. Edit these if you want to turn off any features (all features are on-by-default).
12. Everything is working now! Launch DSP from Steam and enjoy.
