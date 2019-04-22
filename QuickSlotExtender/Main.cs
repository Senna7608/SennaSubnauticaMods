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
        private static QSEConfig qsEConfig;
        public static bool isExists_SlotExdener = false;
        public static QSHandler Instance { get; internal set; }
        

        public static void Load()
        {
            isExists_SlotExdener = RefHelp.IsNamespaceExists("SlotExtender");

            if (isExists_SlotExdener)
                SNLogger.Log($"[{Config.PROGRAM_NAME}] SlotExtender found! trying to work together..");

            try
            {
                HarmonyInstance.Create("Subnautica.QuickSlotExtender.mod").PatchAll(Assembly.GetExecutingAssembly());
                SNLogger.Log($"[{Config.PROGRAM_NAME}] Patches installed");
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
                UnityHelper.EnableConsole();
                //loading config from file
                Config.LoadConfig();
                Config.InitConfig();
                //add console commad for configuration window
                qsEConfig = new QSEConfig();
                //add an action if changed controls
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {            
            //input changed, refreshing key bindings
            Config.InitSLOTKEYS();

            if (Instance != null)
                Instance.ReadSlotExtenderConfig();
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
