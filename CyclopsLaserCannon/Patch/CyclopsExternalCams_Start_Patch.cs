using Common;
using Harmony;
using UnityEngine;

namespace CyclopsLaserCannonModule.Patch
{
    [HarmonyPatch(typeof(CyclopsExternalCams))]
    [HarmonyPatch("Start")]
    public class CyclopsExternalCams_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(CyclopsExternalCams __instance)
        {
            lock (__instance)
            {
                GameObject CyclopsRoot = __instance.transform.parent.gameObject;

                if (CyclopsRoot.name.Equals("__LIGHTMAPPED_PREFAB__"))
                {
                    SNLogger.Log($"[CyclopsLaserCannonModule] Cyclops root object name: {CyclopsRoot.name}");
                    __instance.gameObject.AddIfNeedComponent<CannonControl>();
                    SNLogger.Log($"[CyclopsLaserCannonModule] CyclopsExternalCams patched on CyclopsExternalCams {__instance.gameObject.GetInstanceID()}. CannonControl component added.");
                }
            }
            
        }
    }    
}
