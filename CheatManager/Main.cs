using System;
using Harmony;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Reflection;

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

            isExistsSMLHelperV2 = IsNamespaceExists("SMLHelper.V2");            
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

        public static bool IsNamespaceExists(string desiredNamespace)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace == desiredNamespace)
                        return true;
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(SeaMoth))]
        [HarmonyPatch("Start")]
        internal class SeaMoth_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(SeaMoth __instance)
            {
                __instance.gameObject.AddComponent<SeamothOverDrive>();
            }
        }

        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("Start")]
        internal class Exosuit_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Exosuit __instance)
            {
                __instance.gameObject.AddComponent<ExosuitOverDrive>();
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
