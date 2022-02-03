using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using FrooxEngine;
using HarmonyLib;
using NeosModLoader;

namespace ModConfigDynVarBridge
{
    public class ModConfigDynVarBridge : NeosMod
    {
        public override string Name => "ModConfigDynVarBridge";
        public override string Author => "KyuubiYoru";
        public override string Version => "1.0.1";
        public override string Link => "https://github.com/GithubUsername/RepoName/";

        private static ModConfiguration _config;

        private static Slot _configSlot;

        public override void OnEngineInit()
        {
            _config = GetConfiguration();
            Harmony harmony = new Harmony("net.KyuubiYoru.ModConfigDynVarBridge");
            harmony.PatchAll();
            
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
                        if (config != null)
                        {
                            Slot modConfigSlot = _configSlot.AddSlot(mod.Name);
                            DynamicVariableSpace space =
                                (DynamicVariableSpace)__instance.World.RootSlot.AttachComponent(typeof(DynamicVariableSpace));
                            space.OnlyDirectBinding.Value = true;
                            space.SpaceName.Value = mod.Name;

                            foreach (ModConfigurationKey key in config.ConfigurationItemDefinitions)
                            {
                                if (!key.InternalAccessOnly)
                                {
                                    Debug("Try to add"+key.Name);
                                    MethodInfo method = typeof(UserspacePatch).GetMethod(nameof(SetupDynVar));
                                    MethodInfo generic = method?.MakeGenericMethod(key.ValueType());
                                    generic?.Invoke(__instance, new object[] {modConfigSlot, mod, key});
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug("OnAttach");
                    Debug(e.Message);
                    Debug(e.StackTrace);
                }
            }

            public static void SetupDynVar<T>(Slot slot, NeosModBase mod, ModConfigurationKey key)
            {
                try
                {
                    DynamicValueVariable<T> dynVar = (DynamicValueVariable<T>) slot.AttachComponent(typeof(DynamicValueVariable<T>));
                    if (dynVar == null)
                    {
                        Debug("dynVar is null");
                    }

                    mod.GetConfiguration().TryGetValue((ModConfigurationKey<T>) key, out T value);
                    dynVar.Value.Value = value;
                    dynVar.VariableName.Value = $"{mod.Name}/{key.Name}";
                    dynVar.Value.Changed += changeable => mod.GetConfiguration().Set(key, ((SyncField<T>) changeable).Value);

                    //key.OnChanged += value => dynVar.Value.Value = (T)value;
                }
                catch (Exception e)
                {
                    Debug(e.Message);
                    Debug(e.ToString());
                    
                }
            }
        }
    }
}