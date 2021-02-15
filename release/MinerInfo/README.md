# Show Vein Max Miner Output Per Second

When the Vein Distribution Details Display is on, adds an extra line of text to the Vein Group label with the total miner max output per second.

## How to use

Turn on Vein Distribution Details Display.


## How do I see the rate in items-per-minute rather than items-per-second?

Open `quality-mods-program.plugins.miner-info.cfg` in your `steamapps/common/Dyson Sphere Program/BepInEx/config` folder (after running DSP with the plugin installed once). Now edit the config and set the `ShowItemsPerSecond` config value to `false`.

## Installation

Install BepInEx then copy `Quality-Mods-Program-Miner-Info.dll` into `steamapps/common/Dyson Sphere Program/BepInEx/plugins`.

See detailed installation instructions [here](https://github.com/brotchie/Quality-Mods-Program#installation).

*A mod in the [Quality Mods Program](https://github.com/brotchie/Quality-Mods-Program) series.*

## Changelog

Version 1.0.1
 - Add `ShowItemsPerSecond` config option. Setting to `false` shows rate in items per minute, rather than items per second.

Version 1.0.0
- Initial release
