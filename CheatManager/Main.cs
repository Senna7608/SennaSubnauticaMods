using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using CheatManager.Configuration;

namespace CheatManager
{
    public static class Main
    {
        public static CheatManager Instance { get; private set; }
        public static CM_Logger CmLogger { get; private set; }
        public static CM_InfoBar CmInfoBar { get; private set; }

        internal static bool isConsoleEnabled { get; set; }
        internal static bool isInfoBarEnabled { get; set; }
        internal static int OverPowerMultiplier { get; set; }

        internal static bool isExistsSMLHelperV2;        

        public static void Load()
        {
            try
            {               
                HarmonyInstance.Create("Subnautica.CheatManager.mod").PatchAll(Assembly.GetExecutingAssembly());
                UnityHelper.EnableConsole();
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
                Init();                
            }
            else if (scene.name == "StartScreen")
            {
                DisplayManager.OnDisplayChanged += Screen_OnDisplayChanged;
                Config.InitConfig();

                if (isConsoleEnabled)
                    CmLogger = new CM_Logger();

                if (isInfoBarEnabled)
                    CmInfoBar = new CM_InfoBar();

                CmConfig.Load();
            }
        }

        public static void Screen_OnDisplayChanged()
        {
            Debug.Log($"Resolution changed!");
        }

        public static CheatManager Init()
        {
            if (Instance == null)
            {
                Instance = UnityEngine.Object.FindObjectOfType(typeof(CheatManager)) as CheatManager;

                if (Instance == null)
                {
                    GameObject cheatmanager = new GameObject().AddComponent<CheatManager>().gameObject;
                    cheatmanager.name = "CheatManager";
                    Instance = cheatmanager.GetComponent<CheatManager>();
                }
            }
            return Instance;
        }
    }  
}
