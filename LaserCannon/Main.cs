using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace LaserCannon
{
    class Main
    {
        public static void Load()
        {
            try
            {
                var laserCannon = new LaserCannon();
                laserCannon.Patch();

                HarmonyInstance.Create("Subnautica.LaserCannon.mod").PatchAll(Assembly.GetExecutingAssembly());                
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
                if (techType == LaserCannon.TechTypeID)
                {
                    if (added)
                    {
                        if (__instance.GetComponentInChildren<LaserCannon_Seamoth>() == null)
                        {
                            __instance.gameObject.AddComponent<LaserCannon_Seamoth>();                        
                            var laserCannon = __instance.GetComponentInChildren<LaserCannon_Seamoth>();
                            laserCannon.slotID = slotID;
                        }
                        else
                        {
                            __instance.GetComponentInChildren<LaserCannon_Seamoth>().enabled = true;
                        }
                    }
                    else
                    {
                        __instance.GetComponentInChildren<LaserCannon_Seamoth>().enabled = false;
                    }
                }                                  
            }
    }
}
