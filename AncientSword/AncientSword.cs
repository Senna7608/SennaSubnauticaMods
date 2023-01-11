using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;

namespace AncientSword
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class AncientSword : BaseUnityPlugin
    {
        private const string GUID = "com.senna.ancientsword";
        private const string MODNAME = "AncientSword";
        private const string VERSION = "1.5";

        internal ManualLogSource BepinLogger;
        internal AncientSword mInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            new SwordPrefab().Patch();                                        
        }
    }

    internal static class Main
    {
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
