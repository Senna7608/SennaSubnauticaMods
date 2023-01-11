using System.IO;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using HarmonyLib;
using Common.Helpers;
using UWE;
using BepInEx;
using BepInEx.Logging;

namespace LaserCannon
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class LaserCannon : BaseUnityPlugin
    {
        private const string GUID = "com.senna.lasercannon";
        private const string MODNAME = "LaserCannon";
        private const string VERSION = "1.9";

        internal ManualLogSource BepinLogger;
        internal LaserCannon mInstance;
        internal Harmony hInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            Main.objectHelper = new ObjectHelper();

            LaserCannonConfig.Config_Load();

            new LaserCannonPrefab().Patch();

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
        }       

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "XMenu")
            {
                Language.OnLanguageChanged += LaserCannonConfig.OnLanguageChanged;
            }
        }
    }

    internal static class Main
    {
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static ObjectHelper objectHelper;
        internal static Event<bool> OnConfigChanged = new Event<bool>();
    }
}
