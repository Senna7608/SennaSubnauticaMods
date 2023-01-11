using System.IO;
using System.Reflection;
using APIBasedExosuitArms.Craftables;
using BepInEx;
using BepInEx.Logging;

namespace APIBasedExosuitArms
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.senna.slotextender", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.senna.moddedarmshelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class APIBasedExosuitArms : BaseUnityPlugin
    {
        private const string GUID = "com.senna.apibasedexosuitarms";
        private const string MODNAME = "APIBasedExosuitArms";
        private const string VERSION = "1.0";

        internal ManualLogSource BepinLogger;
        internal APIBasedExosuitArms mInstance;        

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");
            
            new APIBasedClawArm().Patch();
            new APIBasedDrillArm().Patch();
            new APIBasedPropulsionArm().Patch();
            new APIBasedGrapplingArm().Patch();
            new APIBasedTorpedoArm().Patch();            
        }
    }

    internal static class Main
    {
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
