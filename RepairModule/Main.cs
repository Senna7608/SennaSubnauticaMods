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

                HarmonyInstance.Create("Subnautica.RepairModule.mod").PatchAll(Assembly.GetExecutingAssembly());                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }            

        }
    }
 
    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class Exosuit_OnUpgradeModuleChange_Patch
    {
        static void Postfix(Exosuit __instance, int slotID, TechType techType, bool added)
        {
            if (techType == RepairModule.TechTypeID)
            {
                if (added)
                {
                    if (__instance.GetComponentInChildren<RepairModuleExosuit>() == null)
                    {
                        __instance.gameObject.AddComponent<RepairModuleExosuit>();
                        var repairModule = __instance.GetComponentInChildren<RepairModuleExosuit>();
                        repairModule.moduleSlotID = slotID;
                    }
                    else
                    {
                        __instance.GetComponentInChildren<RepairModuleExosuit>().enabled = true;
                    }
                }
                else
                {
                    __instance.GetComponentInChildren<RepairModuleExosuit>().enabled = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class SeaMoth_OnUpgradeModuleChange_Patch
    {
        static void Postfix(SeaMoth __instance, int slotID, TechType techType, bool added)
        {
            if (techType == RepairModule.TechTypeID)
            {
                if (added)
                {
                    if (__instance.GetComponentInChildren<RepairModuleSeamoth>() == null)
                    {
                        __instance.gameObject.AddComponent<RepairModuleSeamoth>();
                        var repairModule = __instance.GetComponentInChildren<RepairModuleSeamoth>();
                        repairModule.slotID = slotID;
                    }
                    else
                    {
                        __instance.GetComponentInChildren<RepairModuleSeamoth>().enabled = true;
                    }
                }
                else
                {
                    __instance.GetComponentInChildren<RepairModuleSeamoth>().enabled = false;
                }
            }
        }
    }         
}
