using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlotExtender.Configuration;
using Common;

namespace SlotExtender
{
    internal class Initialize_uGUI : MonoBehaviour
    {
        internal static Initialize_uGUI Instance { get; private set; }

        private Dictionary<string, Text> Seamoth_SlotText = new Dictionary<string, Text>();
        private Dictionary<string, Text> Exosuit_SlotText = new Dictionary<string, Text>();

        private bool isPatched = false;
        private uGUI_EquipmentSlot temp_slot;
        private const float Unit = 200f;
        private const float RowStep = Unit * 2.2f / 3;
        private const float TopRow = Unit;
        private const float SecondRow = TopRow - RowStep;
        private const float ThirdRow = SecondRow - RowStep;
        private const float FourthRow = ThirdRow - RowStep;
        private const float FifthRow = FourthRow - RowStep;
        private const float CenterColumn = 0f;
        private const float RightColumn = RowStep;
        private const float LeftColumn = -RowStep;

        private readonly Vector2[] slotPos = new Vector2[12]
        {
            new Vector2(LeftColumn, TopRow), //slot 1
            new Vector2(CenterColumn, TopRow),  //slot 2
            new Vector2(RightColumn, TopRow),   //slot 3

            new Vector2(LeftColumn, SecondRow),  //slot 4
            new Vector2(CenterColumn, SecondRow), //slot 5
            new Vector2(RightColumn, SecondRow),   //slot 6

            new Vector2(LeftColumn, ThirdRow),  //slot 7
            new Vector2(CenterColumn, ThirdRow),  //slot 8
            new Vector2(RightColumn, ThirdRow),   //slot 9

            new Vector2(LeftColumn, FourthRow),   //slot 10
            new Vector2(CenterColumn, FourthRow),  //slot 11
            new Vector2(RightColumn, FourthRow)  //slot 12
        };

        public void Awake()
        {
            Instance = gameObject.GetComponent<Initialize_uGUI>();
            RefreshText();
        }

        public void OnDestroy()
        {
            Destroy(Instance);
        }
        
        internal void Add_uGUIslots(uGUI_Equipment instance, Dictionary<string, uGUI_EquipmentSlot> allSlots)
        {
            if (!isPatched)
            {
                foreach (uGUI_EquipmentSlot slot in instance.GetComponentsInChildren<uGUI_EquipmentSlot>(true))
                {
                    // reposition the background image
                    if (slot.name == "SeamothModule1")
                    {                        
                        slot.transform.Find("Seamoth").transform.localPosition = new Vector3(RightColumn, FourthRow);
                    }                    

                    // slot1 always includes the background image, therefore instantiate the slot2 to avoid duplicate background images
                    if (slot.name == "SeamothModule2")
                    {
                        foreach (string slotID in SlotHelper.NewSeamothSlotIDs)
                        {
                            temp_slot = Instantiate(slot, slot.transform.parent);
                            temp_slot.name = slotID;
                            temp_slot.slot = slotID;
                            allSlots.Add(slotID, temp_slot);
                        }                        
                    }
                    else if (slot.name == "ExosuitModule2")
                    {
                        foreach (string slotID in SlotHelper.NewExosuitSlotIDs)
                        {
                            temp_slot = Instantiate(slot, slot.transform.parent);
                            temp_slot.name = slotID;
                            temp_slot.slot = slotID;
                            allSlots.Add(slotID, temp_slot);
                        }                        
                    }
                }

                foreach (KeyValuePair<string, uGUI_EquipmentSlot> item in allSlots)
                {
                    try
                    {
                        if (item.Value.name.StartsWith("SeamothModule"))
                        {
                            int.TryParse(item.Key.Substring(13), out int slotNum);
                            item.Value.rectTransform.anchoredPosition = slotPos[slotNum - 1];
                            Text text = AddTextToSlotIcon(item.Value.transform, SEConfig.SLOTKEYS[$"Slot{slotNum}"], slotNum);
                            Seamoth_SlotText.Add(text.gameObject.name, text);
                        }
                    }
                    catch
                    {
                        SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] Seamoth: Add text to slot error!");
                    }

                    try
                    {
                        if (item.Value.name.StartsWith("ExosuitModule"))
                        {
                            int.TryParse(item.Key.Substring(13), out int slotNum);
                            item.Value.rectTransform.anchoredPosition = slotPos[slotNum - 1];
                            Text text = AddTextToSlotIcon(item.Value.transform, SEConfig.SLOTKEYS[$"Slot{slotNum}"], slotNum);
                            Exosuit_SlotText.Add(text.gameObject.name, text);
                        }
                    }
                    catch
                    {
                        SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] Exosuit: Add text to slot error!");
                    }

                    if (item.Value.name == "ExosuitArmLeft")
                    {
                        item.Value.rectTransform.anchoredPosition = new Vector3(LeftColumn, FifthRow);
                    }

                    if (item.Value.name == "ExosuitArmRight")
                    {
                        item.Value.rectTransform.anchoredPosition = new Vector3(RightColumn, FifthRow);
                    }                    
                }

                SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] uGUI_EquipmentSlots Patched!");
                isPatched = true;
            }
        }

        
        internal void RefreshText()
        {
            foreach (KeyValuePair<string, string> kvp in SEConfig.SLOTKEYS)
            {
                try
                {
                    Seamoth_SlotText[kvp.Key].text = kvp.Value;
                    Exosuit_SlotText[kvp.Key].text = kvp.Value;
                }
                catch
                {                    
                    return;
                }                
            }
        }

        //based on RandyKnapp's Subnautica mod: MoreQuickSlots -> "CreateNewText()" method
        //found on GitHub:https://github.com/RandyKnapp/SubnauticaModSystem

        private Text AddTextToSlotIcon(Transform parent, string slotKey, int slotNum)
        {
            Text text = Instantiate(HandReticle.main.interactPrimaryText);            
            text.gameObject.layer = parent.gameObject.layer;
            text.gameObject.name = $"Slot{slotNum}";
            text.transform.SetParent(parent, false);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.gameObject.SetActive(true);
            text.enabled = true;
            text.text = slotKey;
            text.fontSize = 17;
            text.color = SEConfig.TEXTCOLOR;
            RectTransformExtensions.SetParams(text.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), parent);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);            
            text.rectTransform.anchoredPosition = new Vector2(0, 70);
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;

            return text;
        }
    }
}
