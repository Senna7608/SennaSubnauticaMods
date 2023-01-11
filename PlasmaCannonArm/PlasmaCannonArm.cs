using BepInEx;
using BepInEx.Logging;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace PlasmaCannonArm
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.senna.moddedarmshelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class PlasmaCannonArm : BaseUnityPlugin
    {
        private const string GUID = "com.senna.plasmacannonarm";
        private const string MODNAME = "PlasmaCannonArm";
        private const string VERSION = "1.4";

        internal ManualLogSource BepinLogger;
        internal PlasmaCannonArm mInstance;        

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");
                            
            new PlasmaCannonArmPrefab().Patch();                

            Main.assetBundle = AssetBundle.LoadFromFile($"{Main.modFolder}/Assets/plasma_arm.asset");            
        }
    }

    internal static class Main
    {
        public static AssetBundle assetBundle;
        public static Material plasma_Material;
        public static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);        
    }
}
