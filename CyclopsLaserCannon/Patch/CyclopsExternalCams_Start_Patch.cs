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
                    __instance.gameObject.AddIfNeedComponent<CannonControl>();                    
                    SNLogger.Log($"[CyclopsLaserCannonModule] CyclopsExternalCams patched on instance: {__instance.gameObject.GetInstanceID()}. CannonControl component added.");
                }
            }            
        }
    }
   
    /*
    [HarmonyPatch(typeof(uGUI_CameraCyclops))]
    [HarmonyPatch("Awake")]
    public class uGUI_CameraCyclops_Awake_Patch
    {
        public static bool isAdded = false;

        [HarmonyPostfix]
        public static void Postfix(uGUI_CameraCyclops __instance)
        {
            if (!isAdded)
            {
                __instance.gameObject.AddIfNeedComponent<SharedRuntimeHelper.RuntimeHelperShared>();
                isAdded = true;
            }
        }
    }
   */
}
