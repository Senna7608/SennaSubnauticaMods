using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using RuntimeHelper.Configuration;
using System.Collections.Generic;
using RuntimeHelper.Command;
using QModManager.API.ModLoading;

namespace RuntimeHelper
{
    [QModCore]
    public static class Main
    {
        public static RuntimeHelper Instance { get; internal set; }
        public static List<GameObject> AllVisuals = new List<GameObject>();
        public static CommandRoot commandRoot = null;

        [QModPatch]
        public static void Load()
        {
            try
            {                
                RuntimeHelper_Config.LoadConfig();
                RuntimeHelper_Config.InitConfig();
                DevConsole.disableConsole = false;           

                //add console commad
                commandRoot = new CommandRoot("RHCommandGO", true);
                commandRoot.AddCommand<RuntimeHelperCommand>();

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
                if (RuntimeHelper_Config.AUTOSTART)
                {
                    new RuntimeHelper();
                }
            }           
        }
    }  
}
