# ModConfigDynVarBridge

A [NeosModLoader](https://github.com/zkxs/NeosModLoader) mod for [Neos VR](https://neos.com/) that exposes Mod configs to DynamicVariables in your Userspace and applys changes to the config.


**WIP**

values are currently only linked from DynamicVariables to config

config changes from other mods don't get applied to the DynamicVariable


## Installation
1. Install [NeosModLoader](https://github.com/zkxs/NeosModLoader).
1. Place [ModConfigDynVarBridge.dll](https://github.com/KyuubiYoru/ModConfigDynVarBridge/releases/download/1.0.0/ModConfigDynVarBridge.dll) into your `nml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\NeosVR\nml_mods` for a default install. You can create it if it's missing, or if you launch the game once with NeosModLoader installed it will create the folder for you.
1. Start the game. If you want to verify that the mod is working you can check your Neos logs.
