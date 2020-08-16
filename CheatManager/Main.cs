using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CheatManager.Configuration;
using Common.Helpers;
using Common;
using CheatManager.NewCommands;
using QModManager.API.ModLoading;

namespace CheatManager
{
    [QModCore]
    public static class Main
    {
        public static GameObject CheatManagerGO { get; private set; }
        public static CheatManager Instance { get; private set; }
        public static CmLogger CmLogger { get; private set; }
        public static CmInfoBar CmInfoBar { get; private set; }        

        [QModPatch]
        public static void Load()
        {
            try
            {
                CmConfig.Config_Load();

                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.CheatManager.mod");
                DevConsole.disableConsole = false;
                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);                               
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }                    
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
            {
                if (Instance == null)
                {
                    Instance = CheatManagerGO.AddComponent<CheatManager>();
                }
            }
            
            if (scene.name == "StartScreen")
            {
                if (!CheatManagerGO)
                {
                    CheatManagerGO = new GameObject("CheatManagerGO");                    
                }

                if (CmConfig.isInfoBarEnabled && CmInfoBar == null)
                {
                    CmInfoBar = CheatManagerGO.AddComponent<CmInfoBar>();
                }

                if (CmConfig.isConsoleEnabled && CmLogger == null)
                {
                    CmLogger = CheatManagerGO.AddComponent<CmLogger>();
                }

                new CmConfigCommand();
            }
        }
            
    }  
}
