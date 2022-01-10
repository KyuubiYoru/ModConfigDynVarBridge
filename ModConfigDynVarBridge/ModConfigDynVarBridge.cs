using System;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace ModConfigDynVarBridge
{
    public class ModConfigDynVarBridge : NeosMod
    {
        public override string Name => "ModConfigDynVarBridge";
        public override string Author => "KyuubiYoru";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/GithubUsername/RepoName/";

        private static ModConfiguration _config;

        private static Slot _configSlot;

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.KyuubiYoru.ModConfigDynVarBridge");
            harmony.PatchAll();
            _config = GetConfiguration();
        }

        [HarmonyPatch(typeof(Userspace))]
        public class UserspacePatch
        {
            [HarmonyPostfix]
            [HarmonyPatch("OnAttach")]
            static void OnAttachPostFix(Userspace __instance)
            {
                try
                {
                    _configSlot = __instance.World.RootSlot.AddSlot("ModConfigs");

                    foreach (NeosModBase mod in ModLoader.Mods())
                    {
                        ModConfiguration config = mod.GetConfiguration();
                        if (config !=null)
                        {
                            foreach (ModConfigurationKey key in config.ConfigurationItemDefinitions)
                            {

                            }
                        }
                    }

                    //IkCullingPatch.UserSpaceWorld =
                    //    (DynamicVariableSpace)IkCullingPatch.ConfigSlot.AttachComponent(typeof(DynamicVariableSpace));
                    //IkCullingPatch.UserSpaceWorld.OnlyDirectBinding.Value = true;
                    //IkCullingPatch.UserSpaceWorld.SpaceName.Value = "IkCullingConfig";

                    //DynamicValueVariable<bool> value = (DynamicValueVariable<bool>)IkCullingPatch.ConfigSlot.AttachComponent(typeof(DynamicValueVariable<bool>));
                    //value.VariableName.Value = "IkCullingConfig/Enable";
                    //value.Value.Changed += changeable => Config.Set(Enabled, ((SyncField<bool>)changeable).Value);



                }
                catch (Exception e)
                {
                    Debug("OnAttach");
                    Debug(e.Message);
                    Debug(e.StackTrace);
                }
            }
        }
    }
}