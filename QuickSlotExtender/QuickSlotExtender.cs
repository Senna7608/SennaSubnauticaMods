using System;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using HarmonyLib;
using Common;
using QuickSlotExtender.Configuration;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Bootstrap;
using SMLHelper.V2.Handlers;
using System.IO;

namespace QuickSlotExtender
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    internal class QuickSlotExtender : BaseUnityPlugin
    {
        private const string GUID = "com.senna.quickslotextender";
        private const string MODNAME = "QuickSlotExtender";
        private const string VERSION = "1.0";

        internal ManualLogSource BepinLogger;
        internal QuickSlotExtender mInstance;
        internal Harmony hInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            QSEConfig.LoadConfig();

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            SNLogger.Debug("Harmony Patches installed");

            SceneManager.sceneLoaded += OnSceneLoaded;

            IngameMenuHandler.Main.RegisterOnQuitEvent(OnQuitEvent);
        }

        private static void OnQuitEvent()
        {
            Main.isPatched = false;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "XMenu")
            {                
                QSEConfig.InitConfig();
                
                new QSECommand();
                
                GameInput.OnBindingsChanged += GameInput_OnBindingsChanged;

                Main.isExists_SlotExtender = Chainloader.PluginInfos.ContainsKey("com.senna.slotextender");

                if (Main.isExists_SlotExtender)
                    SNLogger.Log("SlotExtender found! trying to work together..");
            }
        }

        internal static void GameInput_OnBindingsChanged()
        {
            Main.isKeyBindigsUpdate = true;
            
            QSEConfig.InitSLOTKEYS();

            if (Main.Instance != null)
                Main.Instance.ReadSlotExtenderConfig();

            Main.isKeyBindigsUpdate = false;
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

    internal static class Main
    {
        internal static bool isExists_SlotExtender = false;
        internal static bool isKeyBindigsUpdate = false;
        internal static QSEHandler Instance;
        internal static bool isPatched;
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);        
    }
}
