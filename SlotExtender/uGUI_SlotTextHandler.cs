using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Common.Helpers;
using SlotExtender.Configuration;

namespace SlotExtender
{
    public class uGUI_SlotTextHandler : MonoBehaviour
    {
        public static uGUI_SlotTextHandler Instance { get; private set; }

        private Dictionary<string, TextMeshProUGUI> ALLSLOTS_Text = new Dictionary<string, TextMeshProUGUI>();

        public void Awake()
        {
            Instance = this;            

            uGUI_Equipment uGUIequipment = gameObject.GetComponent<uGUI_Equipment>();

            Dictionary<string, uGUI_EquipmentSlot>  ALLSLOTS = (Dictionary<string, uGUI_EquipmentSlot>)uGUIequipment.GetPrivateField("allSlots");

            foreach (KeyValuePair<string, uGUI_EquipmentSlot> item in ALLSLOTS)
            {
                if (SlotHelper.ALLSLOTS.TryGetValue(item.Key, out SlotData slotData))
                {
                    TextMeshProUGUI TMProText = AddTextToSlot(item.Value.transform, slotData);

                    ALLSLOTS_Text.Add(slotData.SlotID, TMProText);
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

        private TextMeshProUGUI AddTextToSlot(Transform parent, SlotData slotData)
        {
            TextMeshProUGUI TMProText = Instantiate(HandReticle.main.compTextHand);
            TMProText.gameObject.layer = parent.gameObject.layer;
            TMProText.gameObject.name = slotData.SlotConfigIDName;
            TMProText.transform.SetParent(parent, false);
            TMProText.transform.localScale = new Vector3(1, 1, 1);
            TMProText.gameObject.SetActive(true);
            TMProText.enabled = true;
            TMProText.text = slotData.KeyCodeName;
            TMProText.fontSize = 17;
            TMProText.color = SEConfig.TEXTCOLOR;
            RectTransformExtensions.SetParams(TMProText.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), parent);
            TMProText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            TMProText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
            TMProText.rectTransform.anchoredPosition = new Vector2(0, 70);
            TMProText.alignment = TextAlignmentOptions.Center;
            TMProText.raycastTarget = false;

            return TMProText;
        }
    }
}
