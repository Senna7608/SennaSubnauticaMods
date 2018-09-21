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

        private const float Unit = 145f;

        private const float TopRow = Unit;
        private const float MiddleRow = 0f;
        private const float BottomRow = -Unit;

        private const float CenterOddColumn = 0f;
        private const float OutsideRightOddColumn = Unit;
        private const float OutsideRightEvenColumn = OutsideRightOddColumn * 1.5f;
        private const float InsideRightEvenColumn = OutsideRightEvenColumn / 3f;
        private const float InsideLeftEvenColumn = -InsideRightEvenColumn;
        private const float OutsideLeftEvenColumn = -OutsideRightEvenColumn;
        private const float OutsideLeftOddColumn = -OutsideRightOddColumn;

        internal static readonly Vector2[] seamoth_slotPos = new Vector2[9]
        {
            new Vector2(OutsideRightEvenColumn, BottomRow), //slot 1
            new Vector2(InsideRightEvenColumn, BottomRow),  //slot 2
            new Vector2(InsideLeftEvenColumn, BottomRow),   //slot 3
            new Vector2(OutsideLeftEvenColumn, BottomRow),  //slot 4

            new Vector2(OutsideRightOddColumn, MiddleRow), //slot 5
            new Vector2(CenterOddColumn, MiddleRow),       //slot 6
            new Vector2(OutsideLeftOddColumn, MiddleRow),  //slot 7

            new Vector2(InsideLeftEvenColumn, TopRow),  //slot 8
            new Vector2(InsideRightEvenColumn, TopRow),   //slot 9
        };

        internal static readonly Vector2[] exosuit_slotPos = new Vector2[8]
        {
            new Vector2(OutsideLeftOddColumn, TopRow),  //slot 1
            new Vector2(CenterOddColumn, TopRow),       //slot 2
            new Vector2(OutsideRightOddColumn, TopRow), //slot 3

            new Vector2(InsideLeftEvenColumn, MiddleRow),  //slot 4
            new Vector2(InsideRightEvenColumn, MiddleRow), //slot 5

            new Vector2(OutsideLeftOddColumn, BottomRow), //slot 6
            new Vector2(CenterOddColumn, BottomRow),      //slot 7
            new Vector2(OutsideRightOddColumn, BottomRow) //slot 8
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
                    // slot1 always includes the background image, therefore instantiate the slot2 to avoid duplicate background images
                    if (slot.name == "SeamothModule2")
                    {
                        foreach (string slotID in SlotHelper.NewSeamothSlotIDs)
                        {
                            temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                            temp_slot.name = slotID;
                            temp_slot.slot = slotID;
                            allSlots.Add(slotID, temp_slot);
                        }                        
                    }
                    else if (slot.name == "ExosuitModule2")
                    {
                        foreach (string slotID in SlotHelper.NewExosuitSlotIDs)
                        {
                            temp_slot = Instantiate(slot, slot.gameObject.transform.parent);
                            temp_slot.name = slotID;
                            temp_slot.slot = slotID;
                            allSlots.Add(slotID, temp_slot);
                        }                        
                    }
                }

                foreach (KeyValuePair<string, uGUI_EquipmentSlot> item in allSlots)
                {
                    if (item.Value.name.StartsWith("SeamothModule"))
                    {
                        int.TryParse(item.Key.Substring(13), out int slotNum);
                        item.Value.rectTransform.anchoredPosition = seamoth_slotPos[slotNum - 1];
                        AddSlotNumbers(item.Value.gameObject.transform, slotNum.ToString());
                    }

                    if (item.Value.name.StartsWith("ExosuitModule"))
                    {
                        int.TryParse(item.Key.Substring(13), out int slotNum);
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
            text.rectTransform.anchoredPosition = new Vector2(0, 70);
            text.alignment = TextAnchor.MiddleCenter;
            text.raycastTarget = false;
        }
    }
}
