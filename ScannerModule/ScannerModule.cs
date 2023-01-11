using System.Reflection;
using HarmonyLib;
using Common.Helpers;
using System.IO;
using BepInEx;
using BepInEx.Logging;

namespace ScannerModule
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class ScannerModule : BaseUnityPlugin
    {
        private const string GUID = "com.senna.scannermodule";
        private const string MODNAME = "ScannerModule";
        private const string VERSION = "1.5";

        internal ManualLogSource BepinLogger;
        internal ScannerModule mInstance;
        internal Harmony hInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            Main.objectHelper = new ObjectHelper();

            new ScannerModulePrefab().Patch();

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        }
    }

    internal static class Main
    {
        internal static ObjectHelper objectHelper;
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}