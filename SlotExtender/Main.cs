using Harmony;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;

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

            UnityHelper.EnableConsole();
            Config.Config.InitConfig();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "StartScreen")
            {
                //add console commad for configuration window
                Config.SEConfig.Load();
                //add an action if changed controls
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;                
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
