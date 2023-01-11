using System.IO;
using System.Reflection;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;

namespace RepairModule
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class RepairModule : BaseUnityPlugin
    {
        private const string GUID = "com.senna.repairmodule";
        private const string MODNAME = "RepairModule";
        private const string VERSION = "2.7";

        internal ManualLogSource BepinLogger;
        internal RepairModule mInstance;
        internal Harmony hInstance;
       
        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            new RepairModulePrefab().Patch();

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        }
    }

    internal static class Main
    {        
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
