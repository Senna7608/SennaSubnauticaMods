using System;
using System.Reflection;
using Harmony;
using UnityEngine;

namespace LaserCannon
{
    public static class Main
    {
        public static string langMain = "English";

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

            langMain = Language.main.GetCurrentLanguage();
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
                        Debug.Log($"[LaserCannon] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
                    }
                    else
                    {
                        __instance.GetComponentInChildren<LaserCannon_Seamoth>().enabled = true;
                        var laserCannon = __instance.GetComponentInChildren<LaserCannon_Seamoth>();
                        laserCannon.slotID = slotID;
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
