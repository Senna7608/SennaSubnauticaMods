using HarmonyLib;

namespace LaserCannon
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class SeaMoth_OnUpgradeModuleChange_Patch
    {
        [HarmonyPostfix]
        static void Postfix(SeaMoth __instance, int slotID, TechType techType, bool added)
        {
            if (techType == LaserCannonPrefab.TechTypeID && added)
            {
                var control = __instance.gameObject.EnsureComponent<LaserCannonControl>();
                control.moduleSlotID = slotID;
            }
        }
    }
}
