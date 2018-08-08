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
            __instance.gameObject.AddComponent<ScannerModuleComponent>();                       
        }
    }    

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleToggle")]
    public class SeaMoth_OnUpgradeModuleToggle_Patch
    {
        static void Postfix(SeaMoth __instance, int slotID, bool active)
        {            
            var techType = __instance.modules.GetTechTypeInSlot(ScannerModuleComponent.slotIDs[slotID]);
            
            if (techType == ScannerModule.TechTypeID)
            {
               var scannermodule =  __instance.gameObject.GetComponent<ScannerModuleComponent>();

                if (scannermodule != null)
                {
                    scannermodule.toggle = !scannermodule.toggle;                   
                }               
            }
        }
    }    
}
