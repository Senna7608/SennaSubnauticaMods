using Common;
using Harmony;

namespace SeamothEnergyShield
{
    [HarmonyPatch(typeof(SeaMoth))]
    [HarmonyPatch("OnUpgradeModuleChange")]
    public class SeaMoth_OnUpgradeModuleChange_Patch
    {
        [HarmonyPostfix]
        static void Postfix(SeaMoth __instance, int slotID, TechType techType, bool added)
        {
            if (techType == SeamothShieldPrefab.TechTypeID && added)
            {
                __instance.gameObject.GetOrAddComponent<SeamothShieldControl>();                              
            }
        }
    }
}
