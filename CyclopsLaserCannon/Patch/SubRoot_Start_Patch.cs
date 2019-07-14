using Common;
using Harmony;
using UnityEngine;

namespace CyclopsLaserCannonModule.Patch
{
    [HarmonyPatch(typeof(SubRoot))]
    [HarmonyPatch("Start")]
    public class SubRoot_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(SubRoot __instance)
        {
            if (__instance.gameObject.name.Equals("__LIGHTMAPPED_PREFAB__"))
            {
                return;
            }           

            __instance.gameObject.AddIfNeedComponent<CannonControl>();
            SNLogger.Log($"[CyclopsLaserCannonModule] SubRoot patched on this Cyclops instance: {__instance.gameObject.GetInstanceID()}. CannonControl component added.");                       
        }
    }
}
