using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace ScannerModule
{
    public static class Main
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
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class SeaMoth_OnUpgradeModuleChange_Patch
    {
        static void Postfix(SeaMoth __instance, int slotID, TechType techType, bool added)
        {
            if (techType == ScannerModule.TechTypeID)
            {
                if (added)
                {
                    if (__instance.GetComponentInChildren<ScannerModuleSeamoth>() == null)
                    {
                        __instance.gameObject.AddComponent<ScannerModuleSeamoth>();
                        var scannerModule = __instance.GetComponentInChildren<ScannerModuleSeamoth>();                        
                        Debug.Log($"[ScannerModule] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
                    }
                    else
                    {
                        __instance.GetComponentInChildren<ScannerModuleSeamoth>().enabled = true;
                    }
                }
                else
                {
                    __instance.GetComponentInChildren<ScannerModuleSeamoth>().enabled = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class Exosuit_OnUpgradeModuleChange_Patch
    {
        static void Postfix(Exosuit __instance, int slotID, TechType techType, bool added)
        {
            if (techType == ScannerModule.TechTypeID)
            {
                if (added)
                {
                    if (__instance.GetComponentInChildren<ScannerModuleExosuit>() == null)
                    {
                        __instance.gameObject.AddComponent<ScannerModuleExosuit>();
                        var scannerModule = __instance.GetComponentInChildren<ScannerModuleExosuit>();
                        Debug.Log($"[ScannerModule] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
                    }
                    else
                    {
                        __instance.GetComponentInChildren<ScannerModuleExosuit>().enabled = true;
                    }
                }
                else
                {
                    __instance.GetComponentInChildren<ScannerModuleExosuit>().enabled = false;
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

}
