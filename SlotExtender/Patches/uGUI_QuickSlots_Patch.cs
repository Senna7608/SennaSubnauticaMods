using HarmonyLib;

namespace SlotExtender.Patches
{
    [HarmonyPatch(typeof(uGUI_QuickSlots), "SetBackground")]    
    internal static class uGUI_QuickSlots_SetBackground_Patch
    {        
        internal static Atlas.Sprite atlasSpriteExosuitArm = null;

        [HarmonyPrefix]
        internal static bool Prefix(uGUI_QuickSlots __instance, ref uGUI_ItemIcon icon, TechType techType, bool highlighted)
        {
            if (CraftData.GetEquipmentType(techType) == (EquipmentType)ModdedEquipmentType.SeamothArm)
            {
                if (icon == null)
                {
                    return false;
                }
                
                if (atlasSpriteExosuitArm == null)
                {
                    atlasSpriteExosuitArm = new Atlas.Sprite(__instance.spriteExosuitArm, false);
                }                               
                
                icon.SetBackgroundSprite(atlasSpriteExosuitArm);

                return false;
            }

            return true;
        }        
    }
}
