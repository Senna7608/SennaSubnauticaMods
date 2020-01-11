using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using RuntimeHelper.Configuration;
using System.Collections.Generic;
using RuntimeHelper.Command;
using static Common.GameHelper;

namespace RuntimeHelper
{
    public static class Main
    {        
        public static RuntimeHelperCommand Command_Instance { get; internal set; }
        public static RuntimeHelper Instance { get; internal set; }
        public static List<GameObject> AllVisuals = new List<GameObject>();

        public static void Load()
        {
            try
            {                
                RuntimeHelper_Config.LoadConfig();
                RuntimeHelper_Config.InitConfig();
                EnableConsole();
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
                Command_Instance = new RuntimeHelperCommand();

                if (RuntimeHelper_Config.AUTOSTART)
                {
                    new RuntimeHelper();
                }
            }
            if (scene.name =="Main")
            {
                Command_Instance = new RuntimeHelperCommand();
            }
        }
    }  
}
