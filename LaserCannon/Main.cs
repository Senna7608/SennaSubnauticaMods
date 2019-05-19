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
        public static LaserCannon_Seamoth Instance { get; internal set; }        

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
    [HarmonyPatch(new Type[] { typeof(int), typeof(TechType), typeof(bool) })]
    public class SeaMoth_OnUpgradeModuleChange_Patch
    {
        [HarmonyPostfix]
        static void Postfix(SeaMoth __instance, int slotID, TechType techType, bool added)
        {            
            if (techType == LaserCannon.TechTypeID && added)
            {
                var control = __instance.gameObject.GetOrAddComponent<LaserCannon_Seamoth>();
                control.moduleSlotID = slotID;
                Main.Instance = control;               
            }                                  
        }
    }
}
