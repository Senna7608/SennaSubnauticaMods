using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Common;
using UWE;
using static Common.Helpers.GraphicsHelper;
using SMLHelper.V2.Utility;
using System.IO;
using BepInEx;
using BepInEx.Logging;

namespace CyclopsLaserCannonModule
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class CyclopsLaserCannon : BaseUnityPlugin
    {
        private const string GUID = "com.senna.cyclopslasercannon";
        private const string MODNAME = "CyclopsLaserCannon";
        private const string VERSION = "2.0";

        internal ManualLogSource BepinLogger;
        internal CyclopsLaserCannon mInstance;
        internal Harmony hInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");
            
            CannonConfig.Config_Load();

            Main.cannonPrefab = new CannonPrefab();
            Main.cannonPrefab.Patch();

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            SceneManager.sceneLoaded += OnSceneLoaded;            
        }        

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "XMenu")
            {
                Language.OnLanguageChanged += CannonConfig.OnLanguageChanged;                
            }
        }       
    }

    internal static class Main
    {
        internal static Material cannon_material;
        internal static Sprite buttonSprite;
        internal static bool isAssetsLoaded;
        internal static CannonPrefab cannonPrefab;
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static Event<string> onConfigurationChanged = new Event<string>();
    }
}
