using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using Common;

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

    [HarmonyPatch(typeof(Vehicle))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    [HarmonyPatch(new Type[] { typeof(int), typeof(TechType), typeof(bool) })]
    public class Vehicle_OnUpgradeModuleChange_Patch
    {
        static void Postfix(Vehicle __instance, int slotID, TechType techType, bool added)
        {
            if (techType == ScannerModule.TechTypeID && added)
            {
                if (__instance.GetType() == typeof(SeaMoth))
                {
                    var seamoth_control = __instance.gameObject.AddOrGetComponent<ScannerModuleSeamoth>();
                    seamoth_control.moduleSlotID = slotID;
                    return;
                }
                else if (__instance.GetType() == typeof(Exosuit))
                {
                    var exosuit_control = __instance.gameObject.AddOrGetComponent<ScannerModuleExosuit>();
                    exosuit_control.moduleSlotID = slotID - 2;
                    return;
                }
                else
                {
                    Debug.Log("[ScannerModule] Error! Unidentified Vehicle Type!");
                }
            }
        }
    }    
}
