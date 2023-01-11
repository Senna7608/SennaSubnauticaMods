using Common.Helpers;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SlotExtender.Patches
{
    internal static class SubRoot_Patch
    {
        [HarmonyPatch(typeof(SubRoot), "SetCyclopsUpgrades")]
        private static class SubRoot_SetCyclopsUpgrades_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(SubRoot __instance)            
            {
                if (__instance.upgradeConsole != null && __instance.GetComponent<LiveMixin>().IsAlive())
                {
                    __instance.shieldUpgrade = false;
                    __instance.sonarUpgrade = false;
                    __instance.vehicleRepairUpgrade = false;
                    __instance.decoyTubeSizeIncreaseUpgrade = false;
                    __instance.thermalReactorUpgrade = false;

                    int j = SlotHelper.SessionCyclopsSlots.Count;

                    TechType[] array = new TechType[j];

                    Equipment modules = __instance.upgradeConsole.modules;

                    for (int i = 0; i < j; i++)
                    {
                        string slot = SlotHelper.SessionCyclopsSlots[i].uGui_SlotName;

                        TechType techTypeInSlot = modules.GetTechTypeInSlot(slot);

                        switch (techTypeInSlot)
                        {
                            case TechType.CyclopsShieldModule:
                                __instance.shieldUpgrade = true;
                                break;

                            case TechType.CyclopsSonarModule:
                                __instance.sonarUpgrade = true;
                                break;

                            case TechType.CyclopsSeamothRepairModule:
                                __instance.vehicleRepairUpgrade = true;
                                break;

                            case TechType.CyclopsDecoyModule:
                                __instance.decoyTubeSizeIncreaseUpgrade = true;
                                break;

                            case TechType.CyclopsThermalReactorModule:
                                __instance.thermalReactorUpgrade = true;
                                break;

                            default:
                                if(techTypeInSlot != TechType.None)
                                {
                                    __instance.BroadcastMessage("CyclopsUpgradeModuleChange", techTypeInSlot, SendMessageOptions.RequireReceiver);
                                }
                                break;
                        }

                        array[i] = techTypeInSlot;
                    }

                    if (__instance.slotModSFX != null)
                    {
                        __instance.slotModSFX.Play();
                    }

                    __instance.BroadcastMessage("RefreshUpgradeConsoleIcons", array, SendMessageOptions.RequireReceiver);
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(SubRoot), "SetExtraDepth")]
        private static class SubRoot_SetExtraDepth_Patch
        {
            [HarmonyPrefix]
            public static bool Prefix(SubRoot __instance)
            {
                if (__instance.upgradeConsole != null)
                {
                    Dictionary<TechType, float> hullReinforcement = __instance.GetPrivateField("hullReinforcement", BindingFlags.Static) as Dictionary<TechType, float>;

                    Equipment modules = __instance.upgradeConsole.modules;

                    float num = 0f;

                    int j = SlotHelper.SessionCyclopsSlots.Count + 1;

                    for (int i = 1; i < j; i++)
                    {
                        string slot = string.Format("Module{0}", i);

                        TechType techTypeInSlot = modules.GetTechTypeInSlot(slot);

                        if (hullReinforcement.ContainsKey(techTypeInSlot))
                        {
                            float num2 = hullReinforcement[techTypeInSlot];

                            if (num2 > num)
                            {
                                num = num2;
                            }
                        }
                    }

                    __instance.gameObject.GetComponent<CrushDamage>().SetExtraCrushDepth(num);
                }

                return false;
            }
        }
    }
}
