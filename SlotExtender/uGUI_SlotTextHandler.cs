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
        
        private static Dictionary<string, Text> ALLSLOTS_Text = new Dictionary<string, Text>();
        
        public void Awake()
        {
            Instance = this;            

            uGUI_Equipment uGUIequipment = gameObject.GetComponent<uGUI_Equipment>();

            Dictionary<string, uGUI_EquipmentSlot>  ALLSLOTS = (Dictionary<string, uGUI_EquipmentSlot>)uGUIequipment.GetPrivateField("allSlots");

            foreach (KeyValuePair<string, uGUI_EquipmentSlot> item in ALLSLOTS)
            {
                if (SlotHelper.ALLSLOTS.TryGetValue(item.Key, out SlotData slotData))
                {
                    Text text = AddTextToSlot(item.Value.transform, slotData);

                    ALLSLOTS_Text.Add(slotData.SlotID, text);
                }
            }
        }

        public void UpdateSlotText()
        {
            foreach (KeyValuePair<string, SlotData> kvp in SlotHelper.ALLSLOTS)
            {
                ALLSLOTS_Text[kvp.Key].text = kvp.Value.KeyCodeName;
            }
        }

        // based on RandyKnapp's MoreQuickSlots Subnautica mod: "CreateNewText()" method
        // found on GitHub:https://github.com/RandyKnapp/SubnauticaModSystem

        private Text AddTextToSlot(Transform parent, SlotData slotData)
        {
            Text text = Instantiate(HandReticle.main.interactPrimaryText);
            text.gameObject.layer = parent.gameObject.layer;
            text.gameObject.name = slotData.SlotConfigIDName;
            text.transform.SetParent(parent, false);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.gameObject.SetActive(true);
            text.enabled = true;
            text.text = slotData.KeyCodeName;
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
