using System.Collections.Generic;
using UnityEngine;

namespace SlotExtender
{
    internal class Initialize_uGUI : MonoBehaviour
    {
        internal static Initialize_uGUI Instance { get; private set; }

        private bool isPatched = false;

        internal uGUI_EquipmentSlot temp_slot;        

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
                    //always slot1 includes the background image, therefore instantiate the slot2 to eliminate duplicate image
                    if (slot.name == "SeamothModule2")
                    {
                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "SeamothModule5";
                        temp_slot.slot = "SeamothModule5";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(0, -149);
                        allSlots.Add("SeamothModule5", temp_slot);

                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "SeamothModule6";
                        temp_slot.slot = "SeamothModule6";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(-143, -5);
                        allSlots.Add("SeamothModule6", temp_slot);

                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "SeamothModule7";
                        temp_slot.slot = "SeamothModule7";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(0, 136);
                        allSlots.Add("SeamothModule7", temp_slot);

                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "SeamothModule8";
                        temp_slot.slot = "SeamothModule8";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(143, -5);
                        allSlots.Add("SeamothModule8", temp_slot);
                    }
                    
                    if (slot.name == "ExosuitModule2")
                    {
                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "ExosuitModule5";
                        temp_slot.slot = "ExosuitModule5";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(0, 136);
                        allSlots.Add("ExosuitModule5", temp_slot);

                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "ExosuitModule6";
                        temp_slot.slot = "ExosuitModule6";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(80, 0);                       
                        allSlots.Add("ExosuitModule6", temp_slot);

                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "ExosuitModule7";
                        temp_slot.slot = "ExosuitModule7";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(-80, 0);
                        allSlots.Add("ExosuitModule7", temp_slot);

                        temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                        temp_slot.name = "ExosuitModule8";
                        temp_slot.slot = "ExosuitModule8";
                        temp_slot.rectTransform.anchoredPosition = new Vector2(0, -149);
                        allSlots.Add("ExosuitModule8", temp_slot);
                    }                    
                }

                Debug.Log("[SlotExtender] uGUI_EquipmentSlots Patched!");
                isPatched = true;
            }
        }        
    }
}
