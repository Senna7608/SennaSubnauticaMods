using System.IO;
using System.Reflection;
using SeamothArms.ArmPrefabs;
using BepInEx;
using BepInEx.Logging;

namespace SeamothArms
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.senna.slotextender", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.senna.moddedarmshelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class SeamothArms : BaseUnityPlugin
    {
        private const string GUID = "com.senna.seamotharms";
        private const string MODNAME = "SeamothArms";
        private const string VERSION = "1.7";

        internal ManualLogSource BepinLogger;
        internal SeamothArms mInstance;
        
        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");
            
            SeamothClawArmFragmentPrefab seamothClawArmFragment = new SeamothClawArmFragmentPrefab();
            seamothClawArmFragment.Patch();
            new SeamothClawArmPrefab(seamothClawArmFragment).Patch();
                                
            SeamothDrillArmFragmentPrefab seamothDrillArmFragment = new SeamothDrillArmFragmentPrefab();
            seamothDrillArmFragment.Patch();
            new SeamothDrillArmPrefab(seamothDrillArmFragment).Patch();

            SeamothGrapplingArmFragmentPrefab seamothGrapplingArmFragment = new SeamothGrapplingArmFragmentPrefab();
            seamothGrapplingArmFragment.Patch();
            new SeamothGrapplingArmPrefab(seamothGrapplingArmFragment).Patch();

            SeamothPropulsionArmFragmentPrefab seamothPropulsionArmFragment = new SeamothPropulsionArmFragmentPrefab();
            seamothPropulsionArmFragment.Patch();
            new SeamothPropulsionArmPrefab(seamothPropulsionArmFragment).Patch();

            SeamothTorpedoArmFragmentPrefab seamothTorpedoArmFragment = new SeamothTorpedoArmFragmentPrefab();
            seamothTorpedoArmFragment.Patch();
            new SeamothTorpedoArmPrefab(seamothTorpedoArmFragment).Patch();            
        }       
    }

    internal static class Main
    {
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);        
    }
}