#define MQS_PATCHER

#if MQS_PATCHER
using Harmony;
using System.Collections.Generic;

namespace SlotExtender.Patchers
{
    [HarmonyPatch(typeof(MoreQuickSlots.GameController))]
    [HarmonyPatch("CreateNewText")]
    internal class MQS_GameController_CreateNewText_Patch
    {
        internal static void Prefix(ref string newText, int index)
        {
            List<string> slotkeys = new List<string>();

            foreach (KeyValuePair<string, string> kvp in Config.Config.SLOTKEYS)
            {
                slotkeys.Add(kvp.Value);
            }

            if (Player.main.inSeamoth && Player.main.GetPDA().state != PDA.State.Opening)
            {
                newText = slotkeys[index].ToString();
            }
            else if (Player.main.inExosuit && Player.main.GetPDA().state != PDA.State.Opening)
            {
                if (index < 2)
                {
                    newText = "";
                    return;
                }
                else
                {
                    newText = slotkeys[index - 2].ToString();
                    return;
                }
            }            
        }
    }    
}
#endif