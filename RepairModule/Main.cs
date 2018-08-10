using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace RepairModule
{
    class Main
    {
        public static void Load()
        {
            try
            {
                var repairmodule = new RepairModule();
                repairmodule.Patch();

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
            __instance.gameObject.AddComponent<RepairModuleSeamoth>();                       
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Start")]
    public class Exosuit_Start_Patch
    {
        static void Prefix(Exosuit __instance)
        {
            __instance.gameObject.AddComponent<RepairModuleExosuit>();
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleToggle")]
    public class SeaMoth_OnUpgradeModuleToggle_Patch
    {
        static void Postfix(SeaMoth __instance, int slotID, bool active)
        {
            var repairmodule = __instance.GetComponent<RepairModuleSeamoth>();

            if (repairmodule != null)
            {
                if (__instance.GetSlotBinding(slotID) == RepairModule.TechTypeID)
                {                
                    repairmodule.slotID = slotID;                   
                    repairmodule.toggle = active;                   
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
            var repairmodule = __instance.GetComponent<RepairModuleExosuit>();

            if (repairmodule != null)
            {
                if (__instance.GetSlotBinding(slotID) == RepairModule.TechTypeID)
                {
                    repairmodule.slotID = slotID;
                    repairmodule.toggle = !repairmodule.toggle;
                }
                else
                {
                    repairmodule.toggle = false;
                }
            }
        }
    }
}
