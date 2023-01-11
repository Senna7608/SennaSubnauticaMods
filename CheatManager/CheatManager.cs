using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using CheatManager.Configuration;
using BepInEx;
using BepInEx.Logging;
using System.IO;
using SMLHelper.V2.Handlers;

namespace CheatManager
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class CheatManager : BaseUnityPlugin
    {
        private const string GUID = "com.senna.cheatmanager";
        private const string MODNAME = "CheatManager";
        private const string VERSION = "2.7";

        internal ManualLogSource BepinLogger;
        internal CheatManager mInstance;        
        internal Harmony hInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            CmConfig.Load();
            CmConfig.Set();

            OptionsPanelHandler.RegisterModOptions(new CM_Options());

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            SceneManager.sceneLoaded += OnSceneLoaded;                              
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
            {
                if (Main.Instance == null)
                {
                    Main.Instance = Main.CheatManagerGO.AddComponent<CheatManagerControl>();
                }
            }
            
            if (scene.name == "XMenu")
            {
                if (Main.CheatManagerGO == null)
                {
                    Main.CheatManagerGO = new GameObject("CheatManagerGO");                    
                }               

                if (Main.CmInfoBar == null)
                {
                    Main.CmInfoBar = Main.CheatManagerGO.AddComponent<CmInfoBar>();
                }

                if (Main.CmLogger == null)
                {
                    Main.CmLogger = Main.CheatManagerGO.AddComponent<CmLogger>();
                }
            }
        }            
    }

    internal static class Main
    {
        internal static GameObject CheatManagerGO;
        internal static CheatManagerControl Instance;
        internal static CmLogger CmLogger;
        internal static CmInfoBar CmInfoBar;
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
