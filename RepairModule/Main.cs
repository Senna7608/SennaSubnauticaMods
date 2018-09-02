using System;
using System.Reflection;
using Harmony;
using UnityEngine;

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
    public class Vehicle_OnUpgradeModuleChange_Patch
    {
        static void Postfix(Vehicle __instance, int slotID, TechType techType, bool added)
        {
            if (techType == RepairModule.TechTypeID)
            {
                if (added)
                {
                    if (__instance.GetComponent<RepairModuleControl>() == null)
                    {
                        var control = __instance.gameObject.AddComponent<RepairModuleControl>();                        

                        if (__instance.GetType() == typeof(Exosuit))
                        {                            
                            control.thisVehicle = __instance.GetComponent<Exosuit>();
                            control.moduleSlotID = slotID - 2;
                        }
                        else
                        {                            
                            control.thisVehicle = __instance.GetComponent<SeaMoth>();
                            control.moduleSlotID = slotID;
                        }
                        Debug.Log($"[RepairModule] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
                    }
                    else
                    {                        
                        var control = __instance.gameObject.GetComponent<RepairModuleControl>();                        

                        if (__instance.GetType() == typeof(Exosuit))
                        {                            
                            control.thisVehicle = __instance.gameObject.GetComponent<Exosuit>();
                            control.moduleSlotID = slotID - 2;
                        }
                        else
                        {                            
                            control.thisVehicle = __instance.gameObject.GetComponent<SeaMoth>();
                            control.moduleSlotID = slotID;
                        }
                        control.enabled = true;
                    }
                }
                else
                {                    
                    __instance.gameObject.GetComponent<RepairModuleControl>().enabled = false;
                }
            }
        }
    }        
}
