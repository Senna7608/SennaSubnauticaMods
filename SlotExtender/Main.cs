using Harmony;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SlotExtender
{
    public static class Main
    {
        public static void Load()
        {
            try
            {                
                HarmonyInstance.Create("Subnautica.SlotExtender.mod").PatchAll(Assembly.GetExecutingAssembly());
                SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);                
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            } 

            DevConsole.disableConsole = false;
            Config.Config.InitConfig();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                Config.ConsoleCommand.Load();
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;
                Debug.Log("[SlotExtender] Information: enter 'sxconfig' command for configuration window.");
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {
            Config.Config.InitSLOTKEYS();

            if (Initialize_uGUI.Instance)
                Initialize_uGUI.Instance.RefreshText();
        }
    }    
}
