using System;
using System.Reflection;
using Harmony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CheatManager
{
    public class Main
    {
        public static void Load()
        {
            try
            {
                HarmonyInstance.Create("subnautica.cheatmanager.mod").PatchAll(Assembly.GetExecutingAssembly());
                Patch();
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
                CheatManager.Load();
#if DEBUG
                //TestWindow.InitTestWindow(true);
#endif
            }

            if (scene.name == "StartScreen")
            {
                Logger.Load();                
            }
        }
        
        private static void Patch()
        {
            DevConsole.disableConsole = false;
        }

    }  
}
