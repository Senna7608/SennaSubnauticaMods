using System.IO;
using System.Reflection;
using UnityEngine;
using SMLHelper.V2.Utility;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace RepulsionCannonArm
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    [BepInDependency("com.ahk1221.smlhelper", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.senna.moddedarmshelper", BepInDependency.DependencyFlags.HardDependency)]
    internal class RepulsionCannonArm : BaseUnityPlugin
    {
        private const string GUID = "com.senna.repulsioncannonarm";
        private const string MODNAME = "RepulsionCannonArm";
        private const string VERSION = "1.4";

        internal ManualLogSource BepinLogger;
        internal RepulsionCannonArm mInstance;
        internal Harmony hInstance;
       
        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");

            Main.illumTex = ImageUtils.LoadTextureFromFile($"{Main.modFolder}/Assets/Exosuit_Arm_Repulsion_Cannon_illum.png");

            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

            new RepulsionCannonArmPrefab().Patch();            
        }          
    }

    internal static class Main
    {
        public static Texture2D illumTex;
        internal static readonly string modFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
