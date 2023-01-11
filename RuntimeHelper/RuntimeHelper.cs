using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using RuntimeHelper.Configuration;
using System.Collections.Generic;
using RuntimeHelper.Command;
using BepInEx;
using BepInEx.Logging;
using System.IO;
using System.Reflection;

namespace RuntimeHelper
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class RuntimeHelper : BaseUnityPlugin
    {
        private const string GUID = "com.senna.runtimehelper";
        private const string MODNAME = "RuntimeHelper";
        private const string VERSION = "1.6";

        internal ManualLogSource BepinLogger;
        internal RuntimeHelper mInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            RuntimeHelper_Config.LoadConfig();
            RuntimeHelper_Config.InitConfig();

            Main.commandRoot = new CommandRoot("RHCommandGO", true);
            Main.commandRoot.AddCommand<RuntimeHelperCommand>();

            SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "XMenu")
            {
                if (RuntimeHelper_Config.AUTOSTART)
                {
                    new RuntimeHelperManager();
                }
            }           
        }
    }

    internal static class Main
    {
        internal static RuntimeHelperManager Instance;
        internal static List<GameObject> AllVisuals = new List<GameObject>();
        internal static CommandRoot commandRoot = null;
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
