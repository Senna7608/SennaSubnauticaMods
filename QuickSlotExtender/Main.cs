using Harmony;
using System;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using QuickSlotExtender.Configuration;

namespace QuickSlotExtender
{
    public static class Main
    {        
        private static QSECommand qseCommand;
        public static bool isExists_SlotExtender = false;
        public static QSEHandler Instance { get; internal set; }        

        public static void Load()
        {
            isExists_SlotExtender = RefHelp.IsNamespaceExists("SlotExtender");

            if (isExists_SlotExtender)
                SNLogger.Log($"[{QSEConfig.PROGRAM_NAME}] SlotExtender found! trying to work together..");

            try
            {
                HarmonyInstance.Create("Subnautica.QuickSlotExtender.mod").PatchAll(Assembly.GetExecutingAssembly());
                SNLogger.Log($"[{QSEConfig.PROGRAM_NAME}] Patches installed");
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
                //enabling game console
                GameHelper.EnableConsole();
                //loading config from file
                QSEConfig.LoadConfig();
                QSEConfig.InitConfig();
                //add console commad for configuration window
                qseCommand = new QSECommand();
                //add an action if changed controls
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {            
            //input changed, refreshing key bindings
            QSEConfig.InitSLOTKEYS();

            if (Instance != null)
            {
                Instance.ReadSlotExtenderConfig();
            }
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
