using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;

namespace LaserCannon
{
    public static class Main
    {
        public static LaserCannonControl Instance { get; internal set; }        

        public static void Load()
        {
            try
            {
                LaserCannonConfig.LoadConfig();
                var laserCannon = new LaserCannonPrefab();
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
                Language.main.OnLanguageChanged += LaserCannonConfig.OnLanguageChanged;
            }
        }
    }

    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleChange")]    
    public class SeaMoth_OnUpgradeModuleChange_Patch
    {
        [HarmonyPostfix]
        static void Postfix(SeaMoth __instance, int slotID, TechType techType, bool added)
        {            
            if (techType == LaserCannonPrefab.TechTypeID && added)
            {
                var control = __instance.gameObject.GetOrAddComponent<LaserCannonControl>();
                control.moduleSlotID = slotID;
                Main.Instance = control;               
            }                                  
        }
    }
}
