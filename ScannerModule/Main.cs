using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace ScannerModule
{
    class Main
    {
        public static void Load()
        {
            try
            {
                var scannermodule = new ScannerModule();
                scannermodule.Patch();

                HarmonyInstance.Create("Subnautica.ScannerModule.mod").PatchAll(Assembly.GetExecutingAssembly());                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }            

        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Start")]
    public class SeaMoth_Start_Patch
    {
        static void Prefix(SeaMoth __instance)
        {
            __instance.gameObject.AddComponent<ScannerModuleSeamoth>();                       
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Start")]
    public class Exosuit_Start_Patch
    {
        static void Prefix(Exosuit __instance)
        {
            __instance.gameObject.AddComponent<ScannerModuleExosuit>();
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleToggle")]
    public class SeaMoth_OnUpgradeModuleToggle_Patch
    {
        static void Postfix(SeaMoth __instance, int slotID, bool active)
        {
            var scannermodule = __instance.gameObject.GetComponent<ScannerModuleSeamoth>();

            if (scannermodule != null)
            {
                if (__instance.GetSlotBinding(slotID) == ScannerModule.TechTypeID)
                {
                    scannermodule.toggle = active;
                }                             
            }
        }
    }
    
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("SlotKeyDown")]
    public class Exosuit_SlotKeyDown_Patch
    {
        static void Postfix(Exosuit __instance, int slotID)
        {
            var scannermodule = __instance.gameObject.GetComponent<ScannerModuleExosuit>();

            if (scannermodule != null)
            {
                if (__instance.GetSlotBinding(slotID) == ScannerModule.TechTypeID)
                {
                    scannermodule.toggle = !scannermodule.toggle;
                }
                else
                {
                    scannermodule.toggle = false;
                }
            }
        }
    }

    /*
    [HarmonyPatch(typeof(ScannerTool))]
    [HarmonyPatch("Start")]
    public class ScannerTool_Start_Patch
    {
        static void Postfix(ScannerTool __instance)
        {
            __instance.gameObject.AddComponent<ScannerToolDebugger>();
        }
    }
    */


}
