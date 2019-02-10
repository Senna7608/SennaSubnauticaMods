using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using CheatManager.Configuration;
using CheatManager.NewCommands;

namespace CheatManager
{
    public static class Main
    {
        public static CheatManager Instance { get; private set; }
        public static CM_Logger CmLogger { get; private set; }
        public static CM_InfoBar CmInfoBar { get; private set; }

        internal static bool isConsoleEnabled { get; set; }
        internal static bool isInfoBarEnabled { get; set; }        

        internal static bool isExistsSMLHelperV2;        

        public static void Load()
        {
            try
            {               
                HarmonyInstance.Create("Subnautica.CheatManager.mod").PatchAll(Assembly.GetExecutingAssembly());
                UnityHelper.EnableConsole();
                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);

                Config.InitConfig();                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            isExistsSMLHelperV2 = RefHelp.IsNamespaceExists("SMLHelper.V2");            
        }
        
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
            {                
                Init();
                Instance.gameObject.AddIfNeedComponent<AlwaysDayConsoleCommand>();
                Instance.gameObject.AddIfNeedComponent<NoInfectConsoleCommand>();
                Instance.gameObject.AddIfNeedComponent<OverPowerConsoleCommand>();
            }
            else if (scene.name == "StartScreen")
            {
                //DisplayManager.OnDisplayChanged += Screen_OnDisplayChanged;               
                
                if (isInfoBarEnabled)
                    CmInfoBar = new CM_InfoBar();

                if (isConsoleEnabled)
                    CmLogger = new CM_Logger();

                CmConfig.Load();
            }
        }

        /*
        public static void Screen_OnDisplayChanged()
        {
            Debug.Log($"Resolution changed!");
        }
        */
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
