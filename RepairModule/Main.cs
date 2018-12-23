//#define DEBUG_REPAIR_MODULE
using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using Common;

namespace RepairModule
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                var repairmodule = new RepairModule();
                repairmodule.Patch();

                HarmonyInstance.Create("Subnautica.RepairModule.mod").PatchAll(Assembly.GetExecutingAssembly());                
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
            if (techType == RepairModule.TechTypeID && added)
            {
                var control = __instance.gameObject.AddOrGetComponent<RepairModuleControl>();
                
                if (__instance.GetType() == typeof(Exosuit))
                    control.moduleSlotID = slotID - 2;
                else
                    control.moduleSlotID = slotID;                
            }
        }
    }        
}
