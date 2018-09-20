using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlotExtender
{
    internal class Initialize_uGUI : MonoBehaviour
    {
        internal static Initialize_uGUI Instance { get; private set; }

        private bool isPatched = false;

        internal uGUI_EquipmentSlot temp_slot;

        internal static readonly Vector2[] seamoth_slotPos = new Vector2[9]
        {
            new Vector2(143f, -149f),   //slot 1
            new Vector2(0f, -149f),     //slot 2
            new Vector2(-143f, -149f),  //slot 3

            new Vector2(143f, -6.5f),   //slot 4
            new Vector2(0f, -6.5f),     //slot 5
            new Vector2(-143f, -6.5f),  //slot 6

            new Vector2(143f, 136f),    //slot 7
            new Vector2(0f, 136f),      //slot 8
            new Vector2(-143f, 136f)    //slot 9
        };

        internal static readonly Vector2[] exosuit_slotPos = new Vector2[8]
        {
            new Vector2(-143f, 136f),   //slot 1
            new Vector2(0f, 136f),      //slot 2
            new Vector2(143f, 136f),    //slot 3

            new Vector2(-80, -5.5f),    //slot 4
            new Vector2(80, -5.5f),     //slot 5

            new Vector2(-143f, -149f),  //slot 6
            new Vector2(0f, -149f),     //slot 7
            new Vector2(143f, -149f)    //slot 8            
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
                    //always slot1 includes the background image, therefore instantiate the slot2 for avoid duplicates
                    if (slot.name == "SeamothModule2")
                    {
                        for (int i = 4; i < SlotHelper.ExpandedSeamothSlotIDs.Length; i++)
                        {
                            temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                            temp_slot.name = SlotHelper.ExpandedSeamothSlotIDs[i];
                            temp_slot.slot = SlotHelper.ExpandedSeamothSlotIDs[i];                                                        
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
                            allSlots.Add(SlotHelper.ExpandedExosuitSlotIDs[i], temp_slot);
                        }                        
                    }                    
                }
                
                foreach (KeyValuePair<string, uGUI_EquipmentSlot> item in allSlots)
                {
                    if (item.Value.name.Contains("SeamothModule"))
                    {                        
                        int.TryParse(item.Key.Substring(13, 1), out int slotNum);
                        item.Value.rectTransform.anchoredPosition = seamoth_slotPos[slotNum - 1];
                        AddSlotNumbers(item.Value.gameObject.transform, slotNum.ToString());
                    }

                    if (item.Value.name.Contains("ExosuitModule"))
                    {
                        int.TryParse(item.Key.Substring(13, 1), out int slotNum);
                        item.Value.rectTransform.anchoredPosition = exosuit_slotPos[slotNum - 1];
                        AddSlotNumbers(item.Value.gameObject.transform, slotNum.ToString());
                    }
                }

                Debug.Log("[SlotExtender] uGUI_EquipmentSlots Patched!");
                isPatched = true;
            }
        }

        //based on RandyKnapp's MoreQuickSlots Subnautica mod: "CreateNewText()" method
        //found on GitHub:https://github.com/RandyKnapp/SubnauticaModSystem

        internal void AddSlotNumbers(Transform parent, string slotNumbers)
        {
            Text text = Instantiate(HandReticle.main.interactPrimaryText);
            text.gameObject.layer = parent.gameObject.layer;
            text.gameObject.name = "SlotText" + slotNumbers;
            text.transform.SetParent(parent, false);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.gameObject.SetActive(true);
            text.enabled = true;
            text.text = slotNumbers;
            text.fontSize = 17;
            text.color = Color.green;
            RectTransformExtensions.SetParams(text.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), parent);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
            text.rectTransform.anchoredPosition = new Vector3(0, 70);
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;            
        }
    }
}
