using HarmonyLib;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;

namespace Fahrenheit
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    [BepInProcess("Subnautica.exe")]
    internal class Fahrenheit : BaseUnityPlugin
    {
        private const string GUID = "com.senna.fahrenheit";
        private const string MODNAME = "Fahrenheit";
        private const string VERSION = "1.4";

        internal ManualLogSource BepinLogger;
        internal Fahrenheit mInstance;
        internal Harmony hInstance;

        internal void Awake()
        {
            mInstance = this;
            BepinLogger = BepInEx.Logging.Logger.CreateLogSource(MODNAME);
            BepinLogger.LogInfo("Awake");
            hInstance = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
        }
    }
}
