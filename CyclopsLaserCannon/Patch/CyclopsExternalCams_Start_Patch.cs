using Common;
using Harmony;

namespace CyclopsLaserCannonModule.Patch
{
    [HarmonyPatch(typeof(CyclopsExternalCams))]
    [HarmonyPatch("Start")]
    public class CyclopsExternalCams_Start_Patch
    {
        private static bool isPatched = false;

        [HarmonyPostfix]
        public static void Postfix(CyclopsExternalCams __instance)
        {
            if (isPatched)
                return;

            __instance.gameObject.AddOrGetComponent<CannonControl>();           

            SNLogger.Log("[CyclopsLaserCannonModule] CyclopsExternalCams patched. CannonControl component added.");

            isPatched = true;
        }
    }    
}
