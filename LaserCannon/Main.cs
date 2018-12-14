using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LaserCannon
{
    public static class Main
    {
        public static void Load()
        {
            try
            {
                Config.InitConfig();
                var laserCannon = new LaserCannon();
                laserCannon.Patch();

                HarmonyInstance.Create("Subnautica.LaserCannon.mod").PatchAll(Assembly.GetExecutingAssembly());
                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                Language.main.OnLanguageChanged += Config.OnLanguageChanged;
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
                    if (__instance.GetComponent<LaserCannon_Seamoth>() == null)
                    {
                        var laserCannon = __instance.gameObject.AddComponent<LaserCannon_Seamoth>();                        
                        laserCannon.slotID = slotID;
                        Debug.Log($"[LaserCannon] Added component to instance: {__instance.name} ID: {__instance.GetInstanceID()}");
                    }
                    else
                    {
                        var laserCannon = __instance.GetComponent<LaserCannon_Seamoth>();
                        laserCannon.seamoth = __instance.GetComponent<SeaMoth>();
                        laserCannon.slotID = slotID;
                        laserCannon.enabled = true;
                    }
                }
                else
                {
                    __instance.GetComponent<LaserCannon_Seamoth>().enabled = false;
                }
            }                                  
        }
    }
}
