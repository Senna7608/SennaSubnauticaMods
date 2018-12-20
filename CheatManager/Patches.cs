using Harmony;

namespace CheatManager
{
    internal class Patches
    {
        [HarmonyPatch(typeof(SeaMoth))]
        [HarmonyPatch("Awake")]
        internal class SeaMoth_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(SeaMoth __instance)
            {
                __instance.gameObject.AddComponent<VehicleOverDrive>();
            }
        }

        [HarmonyPatch(typeof(Exosuit))]
        [HarmonyPatch("Awake")]
        internal class Exosuit_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(Exosuit __instance)
            {
                __instance.gameObject.AddComponent<VehicleOverDrive>();
            }
        }


        [HarmonyPatch(typeof(CyclopsMotorMode))]
        [HarmonyPatch("Start")]
        internal class CyclopsMotorMode_Start_Patch
        {
            [HarmonyPostfix]
            internal static void Postfix(CyclopsMotorMode __instance)
            {
                __instance.gameObject.AddComponent<CyclopsOverDrive>();
            }
        }

        [HarmonyPatch(typeof(Seaglide))]
        [HarmonyPatch("Awake")]
        internal class Seaglide_Awake_Patch
        {
            internal static void Postfix(Seaglide __instance)
            {
                __instance.gameObject.AddComponent<SeaglideOverDrive>();
            }
        }
    }
}
