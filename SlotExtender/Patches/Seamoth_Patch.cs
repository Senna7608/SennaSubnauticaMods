using System.Linq;
using System.Reflection.Emit;
using System.Collections.Generic;
using HarmonyLib;
using Common;
using SlotExtender.Configuration;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(SeaMoth), "slotIDs", MethodType.Getter)]
    public static class Seamoth_slotIDs_Patch
    {
        [HarmonyPrefix]
        internal static bool Prefix(ref string[] __result)
        {
            __result = SlotHelper.SessionSeamothSlotIDs;
            return false;
        }
    }    

    [HarmonyPatch(typeof(SeaMoth), "Start")]
    public static class Seamoth_Start_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(SeaMoth __instance)
        {           
            __instance.gameObject.EnsureComponent<SlotExtender>();

            SNLogger.Log("SlotExtender", $"Component added to instance: {__instance.name} ID: {__instance.GetInstanceID()}");                      
        }
    }


    internal static class SeamothStorageInputPatches
    {
        private static ItemsContainer GetStorageInSlot(Vehicle vehicle, int slotID, TechType techType) =>
             vehicle.GetStorageInSlot(slotID, techType) ?? vehicle.GetStorageInSlot(slotID + SEConfig.STORAGE_SLOTS_OFFSET, techType);

        // substitute call for 'this.seamoth.GetStorageInSlot()' with method above
        private static IEnumerable<CodeInstruction> SubstSlotGetter(IEnumerable<CodeInstruction> cins)
        {
            var list = cins.ToList();
            var Vehicle_GetStorageInSlot = AccessTools.Method(typeof(Vehicle), "GetStorageInSlot");
            int index = list.FindIndex(ci => ci.opcode == OpCodes.Callvirt && Equals(ci.operand, Vehicle_GetStorageInSlot));
            
            if (index > 0)
                list[index] = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SeamothStorageInputPatches), nameof(GetStorageInSlot)));

            return list;
        }

        [HarmonyPatch(typeof(SeamothStorageInput), "OpenPDA")]
        private static class OpenPDA_Patch
        {            
            internal static bool Prepare() => SEConfig.STORAGE_SLOTS_OFFSET > 0;
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> cins) => SubstSlotGetter(cins);
        }

        [HarmonyPatch(typeof(SeamothStorageInput), "OnHandClick")]
        private static class OnHandClick_Patch
        {
            internal static bool Prepare() => SEConfig.STORAGE_SLOTS_OFFSET > 0;
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> cins) => SubstSlotGetter(cins);
        }
        
        [HarmonyPatch(typeof(SeamothStorageInput), "OpenFromExternal")]
        private static class OpenFromExternal_Patch
        {
            internal static bool Prepare() => SEConfig.STORAGE_SLOTS_OFFSET > 0;
            internal static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> cins) => SubstSlotGetter(cins);
        }
    }
    
    [HarmonyPatch(typeof(Vehicle), "OnUpgradeModuleChange")]
    internal static class Vehicle_OnUpgradeModuleChange_Patch
    {
        internal static bool Prepare() => SEConfig.STORAGE_SLOTS_OFFSET > 0;

        internal static void Postfix(Vehicle __instance, int slotID, TechType techType, bool added)
        {
            if (__instance is SeaMoth seamoth)
            {
                // any non-storage module added in seamoth slots 1-4 disables corresponding storage, checking if we need to enable it again
                if (slotID < 4 && techType != TechType.VehicleStorageModule)
                {
                    if (__instance.GetSlotItem(slotID + SEConfig.STORAGE_SLOTS_OFFSET)?.item.GetTechType() == TechType.VehicleStorageModule)
                        seamoth.storageInputs[slotID].SetEnabled(true);
                }
                else // if we adding/removing storage module in linked slots, we need to activate/deactivate corresponing storage unit
                if (slotID >= SEConfig.STORAGE_SLOTS_OFFSET && slotID < SEConfig.STORAGE_SLOTS_OFFSET + 4 && techType == TechType.VehicleStorageModule)
                {
                    seamoth.storageInputs[slotID - SEConfig.STORAGE_SLOTS_OFFSET].SetEnabled(added);
                }
            }
        }
    }
    
}
