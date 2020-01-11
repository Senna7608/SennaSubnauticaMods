using Common;
using Harmony;

namespace RepairModule
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("Start")]
    public class SeaMoth_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(SeaMoth __instance)
        {
            __instance.gameObject.AddIfNeedComponent<RepairModuleControl>();
        }
    }

    [HarmonyPatch(typeof(Exosuit))]
    [HarmonyPatch("Start")]
    public class Exosuit_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Exosuit __instance)
        {
            __instance.gameObject.AddIfNeedComponent<RepairModuleControl>();
        }
    }
}
