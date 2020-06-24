using System.Collections.Generic;
using UnityEngine;
using SlotExtender.Configuration;
using Common;
using UnityEngine.UI;

namespace SlotExtender
{
    public class uGUI_SlotTextHandler : MonoBehaviour
    {
        public static uGUI_SlotTextHandler Instance { get; private set; }

        private Dictionary<string, Text> Exosuit_SlotText = new Dictionary<string, Text>();
        private Dictionary<string, Text> SeaMoth_SlotText = new Dictionary<string, Text>();
        private Dictionary<string, uGUI_EquipmentSlot> ALLSLOTS;

        public void Awake()
        {
            Instance = GetComponent<uGUI_SlotTextHandler>();

            RefreshText();

            uGUI_Equipment uGUIequipment = gameObject.GetComponent<uGUI_Equipment>();

            ALLSLOTS = (Dictionary<string, uGUI_EquipmentSlot>)uGUIequipment.GetPrivateField("allSlots");

            foreach (KeyValuePair<string, uGUI_EquipmentSlot> item in ALLSLOTS)
            {
                if (item.Value.name.StartsWith("ExosuitModule"))
                {
                    int.TryParse(item.Key.Substring(13), out int slotNum);
                    string slot = $"Slot{slotNum}";
                    Text text = AddTextToSlotIcon(item.Value.transform, SEConfig.SLOTKEYS[slot], slot);
                    Exosuit_SlotText.Add(text.gameObject.name, text);
                }
                else if (item.Value.name.StartsWith("SeaMothModule"))
                {
                    int.TryParse(item.Key.Substring(14), out int slotNum);
                    string slot = $"Slot{slotNum}";
                    Text text = AddTextToSlotIcon(item.Value.transform, SEConfig.SLOTKEYS[slot], slot);
                    SeaMoth_SlotText.Add(text.gameObject.name, text);
                }
                else if (item.Value.name.StartsWith("SeaMothArmLeft"))
                {
                    Text text = AddTextToSlotIcon(item.Value.transform, SEConfig.SLOTKEYS["ArmLeft"], "ArmLeft");
                    SeaMoth_SlotText.Add(text.gameObject.name, text);
                }
                else if (item.Value.name.StartsWith("SeaMothArmRight"))
                {
                    Text text = AddTextToSlotIcon(item.Value.transform, SEConfig.SLOTKEYS["ArmRight"], "ArmRight");
                    SeaMoth_SlotText.Add(text.gameObject.name, text);
                }
            }
        }

        public void RefreshText()
        {
            foreach (KeyValuePair<string, string> kvp in SEConfig.SLOTKEYS)
            {
                try
                {
                    Exosuit_SlotText[kvp.Key].text = kvp.Value;
                    SeaMoth_SlotText[kvp.Key].text = kvp.Value;
                }
                catch
                {
                    return;
                }
            }
        }

        //based on RandyKnapp's MoreQuickSlots Subnautica mod: "CreateNewText()" method
        //found on GitHub:https://github.com/RandyKnapp/SubnauticaModSystem

        private Text AddTextToSlotIcon(Transform parent, string slotKey, string slotName)
        {
            Text TMProtext = Instantiate(HandReticle.main.interactPrimaryText);
            TMProtext.gameObject.layer = parent.gameObject.layer;
            TMProtext.gameObject.name = slotName;
            TMProtext.transform.SetParent(parent, false);
            TMProtext.transform.localScale = new Vector3(1, 1, 1);
            TMProtext.gameObject.SetActive(true);
            TMProtext.enabled = true;
            TMProtext.text = slotKey;
            TMProtext.fontSize = 17;
            TMProtext.color = SEConfig.TEXTCOLOR;
            RectTransformExtensions.SetParams(TMProtext.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), parent);
            TMProtext.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            TMProtext.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
            TMProtext.rectTransform.anchoredPosition = new Vector2(0, 70);
            TMProtext.alignment = TextAnchor.MiddleCenter;
            TMProtext.raycastTarget = false;

            return TMProtext;
        }
    }
}
