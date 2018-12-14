using System;
using Harmony;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Reflection;
using Common;

namespace CheatManager
{
    public static class Main
    {        
        internal static bool isExistsSMLHelperV2;       

        public static void Load()
        {
            try
            {
                Config.Config.InitConfig();
                HarmonyInstance.Create("Subnautica.CheatManager.mod").PatchAll(Assembly.GetExecutingAssembly());
                EnableConsole();
                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded); 
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogException(ex);
            }

            isExistsSMLHelperV2 = RefHelp.IsNamespaceExists("SMLHelper.V2");            
        }
        
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
            {
                CheatManager.Load();
            }

            if (scene.name == "StartScreen")
            {
                Logger.Load();                
            }
        }        

        private static void EnableConsole()
        {
            DevConsole.disableConsole = false;         
        }       
        
        [HarmonyPatch(typeof(SeaMoth))]
        [HarmonyPatch("Awake")]
        internal class SeaMoth_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(SeaMoth __instance)
            {
                __instance.gameObject.AddComponent<VehicleOverDrive>();
            }
        }

        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("Awake")]
        internal class Exosuit_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Exosuit __instance)
            {
                __instance.gameObject.AddComponent<VehicleOverDrive>();
            }
        }
        

        [HarmonyPatch(typeof(CyclopsMotorMode))]
        [HarmonyPatch("Start")]
        internal class CyclopsMotorMode_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(CyclopsMotorMode __instance)
            {
                __instance.gameObject.AddComponent<CyclopsOverDrive>();                
            }
        }
        
        [HarmonyPatch(typeof(Seaglide))]
        [HarmonyPatch("Awake")]
        internal class Seaglide_Awake_Patch
        {
            internal static void Postfix(Seaglide __instance)
            {
                __instance.gameObject.AddComponent<SeaglideOverDrive>();                
            }
        }        
    }  
}
