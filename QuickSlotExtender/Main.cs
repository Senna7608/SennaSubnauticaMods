using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using HarmonyLib;
using Common;
using Common.Helpers;
using QuickSlotExtender.Configuration;
using QModManager.API.ModLoading;

namespace QuickSlotExtender
{
    [QModCore]
    public static class Main
    {
        public static bool isExists_SlotExtender = false;
        public static bool isKeyBindigsUpdate = false;
        public static QSEHandler Instance { get; internal set; }
        public static bool isPatched;

        [QModPatch]
        public static void Load()
        {
            //load and init config from file   
            QSEConfig.LoadConfig();

            isExists_SlotExtender = ReflectionHelper.IsNamespaceExists("SlotExtender");

            if (isExists_SlotExtender)
                SNLogger.Log("QuickSlotExtender", "SlotExtender found! trying to work together..");

            try
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Subnautica.QuickSlotExtender.mod");

                SNLogger.Debug("QuickSlotExtender", "Harmony Patches installed");

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
                isPatched = false;
                //enabling game console
                DevConsole.disableConsole = false;
                //init config
                QSEConfig.InitConfig();
                //add console commad for configuration window
                new QSECommand();
                //add an action if changed controls
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {
            isKeyBindigsUpdate = true;

            //input changed, refreshing key bindings
            QSEConfig.InitSLOTKEYS();

            if (Instance != null)
                Instance.ReadSlotExtenderConfig();

            isKeyBindigsUpdate = false;
        }

        public static object GetAssemblyClassPublicField(string className, string fieldName, BindingFlags bindingFlags = BindingFlags.Default)
        {
            try
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Type[] types = assemblies[i].GetTypes();

                    for (int j = 0; j < types.Length; j++)
                    {
                        if (types[j].FullName == className)
                        {
                            return types[j].GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | bindingFlags).GetValue(types[j]);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }    
}
