using System.Collections.Generic;
using UnityEngine;

namespace SlotExtender
{
    internal class Initialize_uGUI : MonoBehaviour
    {
        internal static Initialize_uGUI Instance { get; private set; }

        private bool isPatched = false;

        internal uGUI_EquipmentSlot seamoth_slot_5;
        internal uGUI_EquipmentSlot seamoth_slot_6;
        internal uGUI_EquipmentSlot seamoth_slot_7;
        internal uGUI_EquipmentSlot seamoth_slot_8;

        internal uGUI_EquipmentSlot exosuit_slot_5;
        internal uGUI_EquipmentSlot exosuit_slot_6;
        internal uGUI_EquipmentSlot exosuit_slot_7;
        internal uGUI_EquipmentSlot exosuit_slot_8;

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

                    if (slot.name == "SeamothModule1")
                    {
                        seamoth_slot_5 = Instantiate(slot, slot.gameObject.transform.parent);
                        seamoth_slot_5.name = "SeamothModule5";
                        seamoth_slot_5.slot = "SeamothModule5";                        
                        seamoth_slot_5.rectTransform.anchoredPosition = new Vector2(0, -149);                       
                        allSlots.Add("SeamothModule5", seamoth_slot_5);                        
                    }

                    if (slot.name == "SeamothModule2")
                    {
                        seamoth_slot_6 = Instantiate(slot, slot.gameObject.transform.parent);
                        seamoth_slot_6.name = "SeamothModule6";
                        seamoth_slot_6.slot = "SeamothModule6";
                        seamoth_slot_6.rectTransform.anchoredPosition = new Vector2(-143, -5);                        
                        allSlots.Add("SeamothModule6", seamoth_slot_6);                        
                    }

                    if (slot.name == "SeamothModule3")
                    {
                        seamoth_slot_7 = Instantiate(slot, slot.gameObject.transform.parent);
                        seamoth_slot_7.name = "SeamothModule7";
                        seamoth_slot_7.slot = "SeamothModule7";
                        seamoth_slot_7.rectTransform.anchoredPosition = new Vector2(0, 136);                        
                        allSlots.Add("SeamothModule7", seamoth_slot_7);                        
                    }

                    if (slot.name == "SeamothModule4")
                    {
                        seamoth_slot_8 = Instantiate(slot, slot.gameObject.transform.parent);
                        seamoth_slot_8.name = "SeamothModule8";
                        seamoth_slot_8.slot = "SeamothModule8";
                        seamoth_slot_8.rectTransform.anchoredPosition = new Vector2(143, -5);                        
                        allSlots.Add("SeamothModule8", seamoth_slot_8);                        
                    }

                    if (slot.name == "ExosuitModule1")
                    {
                        exosuit_slot_5 = Instantiate(slot, slot.gameObject.transform.parent);
                        exosuit_slot_5.name = "ExosuitModule5";
                        exosuit_slot_5.slot = "ExosuitModule5";
                        exosuit_slot_5.rectTransform.anchoredPosition = new Vector2(0, 136);                        
                        allSlots.Add("ExosuitModule5", exosuit_slot_5);                        
                    }

                    if (slot.name == "ExosuitModule2")
                    {
                        exosuit_slot_6 = Instantiate(slot, slot.gameObject.transform.parent);
                        exosuit_slot_6.name = "ExosuitModule6";
                        exosuit_slot_6.slot = "ExosuitModule6";
                        exosuit_slot_6.rectTransform.anchoredPosition = new Vector2(80, 0);                       
                        allSlots.Add("ExosuitModule6", exosuit_slot_6);                       
                    }

                    if (slot.name == "ExosuitModule3")
                    {
                        exosuit_slot_7 = Instantiate(slot, slot.gameObject.transform.parent);
                        exosuit_slot_7.name = "ExosuitModule7";
                        exosuit_slot_7.slot = "ExosuitModule7";
                        exosuit_slot_7.rectTransform.anchoredPosition = new Vector2(-80, 0);                        
                        allSlots.Add("ExosuitModule7", exosuit_slot_7);                       
                    }

                    if (slot.name == "ExosuitModule4")
                    {
                        exosuit_slot_8 = Instantiate(slot, slot.gameObject.transform.parent);
                        exosuit_slot_8.name = "ExosuitModule8";
                        exosuit_slot_8.slot = "ExosuitModule8";
                        exosuit_slot_8.rectTransform.anchoredPosition = new Vector2(0, -149);                        
                        allSlots.Add("ExosuitModule8", exosuit_slot_8);                        
                    }
                }

                Debug.Log("[SlotExtender] uGUI_EquipmentSlots Patched!");
                isPatched = true;
            }
        }        
    }
}
