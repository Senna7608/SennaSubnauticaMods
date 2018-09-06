using System.Collections.Generic;
using UnityEngine;

namespace SlotExtender
{
    internal class Initialize_uGUI : MonoBehaviour
    {
        internal static Initialize_uGUI Instance { get; private set; }

        private bool isPatched = false;

        internal uGUI_EquipmentSlot temp_slot;

        internal static readonly Vector2[] seamoth_iconPos = new Vector2[]
        {
            new Vector2(0f, -149f),
            new Vector2(-143, -5),
            new Vector2(0, 136),
            new Vector2(143, -5),
            new Vector2(0, -5)
        };

        internal static readonly Vector2[] exosuit_iconPos = new Vector2[]
        {
            new Vector2(0, 136),
            new Vector2(80, 0),
            new Vector2(-80, 0),
            new Vector2(0, -149)
        };

        internal void Awake()
        {
            Instance = this;            
        }

        internal void Add_uGUIslots(uGUI_Equipment instance, Dictionary<string, uGUI_EquipmentSlot> allSlots)
        {
            if (!isPatched)
            {
                foreach (uGUI_EquipmentSlot slot in instance.GetComponentsInChildren<uGUI_EquipmentSlot>(true))
                {
                    //always slot1 includes the background image, therefore instantiate the slot2 to avoid duplicates
                    if (slot.name == "SeamothModule2")
                    {
                        for (int i = 4; i < SlotHelper.ExpandedSeamothSlotIDs.Length; i++)
                        {
                            temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                            temp_slot.name = SlotHelper.ExpandedSeamothSlotIDs[i];
                            temp_slot.slot = SlotHelper.ExpandedSeamothSlotIDs[i];
                            temp_slot.rectTransform.anchoredPosition = seamoth_iconPos[i - 4];
                            allSlots.Add(SlotHelper.ExpandedSeamothSlotIDs[i], temp_slot);
                        }                        
                    }
                    
                    if (slot.name == "ExosuitModule2")
                    {
                        for (int i = 6; i < SlotHelper.ExpandedExosuitSlotIDs.Length; i++)
                        {
                            temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                            temp_slot.name = SlotHelper.ExpandedExosuitSlotIDs[i];
                            temp_slot.slot = SlotHelper.ExpandedExosuitSlotIDs[i];
                            temp_slot.rectTransform.anchoredPosition = exosuit_iconPos[i - 6];
                            allSlots.Add(SlotHelper.ExpandedExosuitSlotIDs[i], temp_slot);
                        }                        
                    }                    
                }
                                
                Debug.Log("[SlotExtender] uGUI_EquipmentSlots Patched!");
                isPatched = true;
            }
        }        
    }
}
