using Harmony;
using Common;

namespace CheatManager
{
    internal class Patches
    {
        [HarmonyPatch(typeof(SeaMoth))]
        [HarmonyPatch("Awake")]
        internal class SeaMoth_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(SeaMoth __instance)
            {
                __instance.gameObject.AddIfNeedComponent<SeamothOverDrive>();
            }
        }

        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("Awake")]
        internal class Exosuit_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Exosuit __instance)
            {
                __instance.gameObject.AddIfNeedComponent<ExosuitOverDrive>();
            }
        }
        
        [HarmonyPatch(typeof(CyclopsMotorMode))]
        [HarmonyPatch("Start")]
        internal class CyclopsMotorMode_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(CyclopsMotorMode __instance)
            {
                __instance.gameObject.AddIfNeedComponent<CyclopsOverDrive>();
            }
        }

        [HarmonyPatch(typeof(CyclopsMotorMode))]
        [HarmonyPatch("ChangeCyclopsMotorMode")]
        internal class CyclopsMotorMode_ChangeCyclopsMotorMode_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(CyclopsMotorMode __instance, CyclopsMotorMode.CyclopsMotorModes newMode)
            {                
                __instance.gameObject.GetComponent<CyclopsOverDrive>().onCyclopsMotorModeChanged.Trigger(newMode);
            }
        }

        [HarmonyPatch(typeof(Seaglide))]
        [HarmonyPatch("Awake")]
        internal class Seaglide_Awake_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Seaglide __instance)
            {
                __instance.gameObject.AddIfNeedComponent<SeaglideOverDrive>();
            }
        }

        [HarmonyPatch(typeof(DevConsole))]
        [HarmonyPatch("Submit")]
        internal class DevConsole_Submit_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(DevConsole __instance, string value, bool __result)
            {
                if (__result)
                {
                    if (Main.Instance != null)
                    {
                        lock(value)
                        {
                            Main.Instance.onConsoleCommandEntered.Trigger(value);
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(FiltrationMachine))]
        [HarmonyPatch("OnConsoleCommand_filterfast")]
        internal class FiltrationMachine_OnConsoleCommand_filterfast_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(FiltrationMachine __instance)
            {                
                if (Main.Instance != null)
                {                    
                    Main.Instance.onFilterFastChanged.Trigger((bool)__instance.GetPrivateField("fastFiltering"));
                }                
            }
        }
    }
}
