using Harmony;
using Common;
using UnityEngine;

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
        [HarmonyPatch("Start")]
        internal class Seaglide_Awake_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Seaglide __instance)
            {
                __instance.gameObject.AddIfNeedComponent<SeaglideOverDrive>();
                SNLogger.Log($"[CheatManager] Added component [SeaGlideOverDrive] to instance [{__instance.gameObject.GetFullName()}]");
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

        
        
        /*
        [HarmonyPatch(typeof(Pickupable))]
        [HarmonyPatch("Initialize")]
        internal class Pickupable_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Pickupable __instance)
            {
                switch (__instance.name)
                {
                    case "Knife(Clone)":
                    case "HeatBlade(Clone)":
                    case "Flashlight(Clone)":
                    case "Scanner(Clone)":
                    case "AlienRifle(Clone)":
                    case "Builder(Clone)":
                    case "Welder(Clone)":
                    case "LaserCutter(Clone)":
                    case "StasisRifle(Clone)":
                    case "RepulsionCannon(Clone)":
                    case "PropulsionCannon(Clone)":
                    case "Gravsphere(Clone)":
                    case "AncientSword(Clone)":
                    case "Crowbar(Clone)":
                    case "Transfuser(Clone)":
                    case "LEDLight(Clone)":
                    case "DiveReel(Clone)":
                    case "SeaGlide(Clone)":
                    case "ExosuitPropulsionArmModule(Clone)":
                    case "ExosuitTorpedoArmModule(Clone)":
                    case "ExosuitDrillArmModule(Clone)":
                    case "CannonArm(Clone)":

                    __instance.gameObject.AddIfNeedComponent<RuntimeObjectManager>();
                    break;
                }
            }
        }
        */
    }
}
