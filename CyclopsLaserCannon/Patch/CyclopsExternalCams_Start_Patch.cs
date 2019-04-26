using Common;
using Harmony;

namespace CyclopsLaserCannonModule.Patch
{
    [HarmonyPatch(typeof(CyclopsExternalCams))]
    [HarmonyPatch("Start")]
    public class CyclopsExternalCams_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(CyclopsExternalCams __instance)
        {
            if (__instance.gameObject.GetComponent<CannonControl>() == null)
            {
                __instance.gameObject.AddComponent<CannonControl>();

                SNLogger.Log($"[CyclopsLaserCannonModule] CyclopsExternalCams patched on CyclopsExternalCams {__instance.gameObject.GetInstanceID()}. CannonControl component added.");
            }
        }
    }    
}
