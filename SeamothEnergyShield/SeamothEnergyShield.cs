using System.IO;
using System.Reflection;
using HarmonyLib;
using BepInEx;
using BepInEx.Logging;

namespace SeamothEnergyShield
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class SeamothEnergyShield : BaseUnityPlugin
    {
        private const string GUID = "com.senna.seamothenergyshield";
        private const string MODNAME = "SeamothEnergyShield";
        private const string VERSION = "1.5";

        internal static ManualLogSource BepinLogger;
        internal static SeamothEnergyShield mInstance;
        internal static Harmony hInstance;        

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            new SeamothShieldPrefab().Patch();

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        }
    }

    internal static class Main
    {
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
