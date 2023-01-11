using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common.Helpers;
using QuickSlotExtender.Configuration;
using Common;
using TMPro;
using System.Collections;

namespace QuickSlotExtender
{
    public class QSEHandler : MonoBehaviour
    {
        public QSEHandler Instance { get; private set; }        
        public uGUI_QuickSlots quickSlots { get; private set; }
        public uGUI_ItemIcon[] icons { get; private set; }
        private bool islabelsAdded = false;
        public static object slotextender_SLOTKEYSLIST;
        public List<string> SLOTEXTENDER_SLOTKEYSLIST;
        public Utils.MonitoredValue<bool> onConsoleInputFieldActive = new Utils.MonitoredValue<bool>();
        private bool isConsoleActive = false;
        private bool waitForModeChange = false;

        public void Awake()
        {
            Instance = gameObject.GetComponent<QSEHandler>();
            quickSlots = gameObject.GetComponent<uGUI_QuickSlots>();

            if (Main.isExists_SlotExtender)
            {
                SLOTEXTENDER_SLOTKEYSLIST = new List<string>();

                ReadSlotExtenderConfig();
            }
        }

        public void ReadSlotExtenderConfig()
        {
            SLOTEXTENDER_SLOTKEYSLIST.Clear();

            try
            {
                slotextender_SLOTKEYSLIST = QuickSlotExtender.GetAssemblyClassPublicField("SlotExtender.Configuration.SEConfig", "SLOTKEYSLIST", BindingFlags.Static);                               

                foreach (string item in (List<string>)slotextender_SLOTKEYSLIST)
                {
                    SLOTEXTENDER_SLOTKEYSLIST.Add(item);
                }

                SNLogger.Debug($"SLOTEXTENDER_SLOTKEYSLIST.Count = [{SLOTEXTENDER_SLOTKEYSLIST.Count}]");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void Start()
        {
            onConsoleInputFieldActive.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(OnConsoleInputFieldActive));
            Player.main.playerModeChanged.AddHandler(this, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        private void OnPlayerModeChanged(Player.Mode newMode)
        {
            StartCoroutine(WaitForPlayerModeChangeFinished(newMode));
        }

        private IEnumerator WaitForPlayerModeChangeFinished(Player.Mode newMode)
        {
            while (Player.main.currentMountedVehicle == null)
            {
                waitForModeChange = true;
                yield return null;
            }

            waitForModeChange = false;
            yield break;
        }

        private void OnConsoleInputFieldActive(Utils.MonitoredValue<bool> isActive)
        {
            isConsoleActive = isActive.value;
        }

        public void OnDestroy()
        {
            onConsoleInputFieldActive.changedEvent.RemoveHandler(this, OnConsoleInputFieldActive);
            Player.main.playerModeChanged.RemoveHandler(this, OnPlayerModeChanged);
        }       

        
        internal void Update()
        {
            if (isConsoleActive)
                return;

            if (Main.isKeyBindigsUpdate)
                return;

            if (Player.main == null)
                return;

            if (Inventory.main == null)
                return;

            if (!islabelsAdded)
                AddQuickSlotText(quickSlots);

            if (Player.main.isPiloting)
                return;

            if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_6"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(5);
                return;
            }
            else if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_7"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(6);
                return;
            }
            else if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_8"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(7);
                return;
            }
            else if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_9"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(8);
                return;
            }
            else if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_10"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(9);
                return;
            }
            else if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_11"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(10);
                return;
            }
            else if (Input.GetKeyDown(QSEConfig.KEYBINDINGS["Slot_12"]))
            {
                Inventory.main.quickSlots.SlotKeyDown(11);
                return;
            }
        }

        internal void AddQuickSlotText(uGUI_QuickSlots instance)
        {
            if (waitForModeChange)
            {
                return;
            }

            if (instance == null)
            {
                return;
            }

            icons = (uGUI_ItemIcon[])instance.GetPrivateField("icons");

            if (icons == null)
            {
                return;
            }

            if (HandReticle.main == null)
            {
                return;
            }

            SNLogger.Debug($"icons.length: [{icons.Length}]");

            SNLogger.Debug($"SLOTKEYSLIST.Count: [{QSEConfig.SLOTKEYSLIST.Count}]");

            SNLogger.Debug($"Player.main.isPiloting: [{Player.main.isPiloting}]");

            SNLogger.Debug($"isExists_SlotExtender: [{Main.isExists_SlotExtender}]");

            SNLogger.Debug($"Player.main.inSeamoth: [{Player.main.inSeamoth}]");

            for (int i = 0; i < icons.Length; i++)
            {
                if (Main.isExists_SlotExtender)
                {
                    if (Player.main.GetPDA().state != PDA.State.Opening)
                    {
                        if (Player.main.inSeamoth)
                        {
                            AddTextToSlot(icons[i].transform, SLOTEXTENDER_SLOTKEYSLIST[i]);
                        }
                        else if (Player.main.inExosuit)
                        {
                            if (i < 2)
                            {
                                continue;
                            }
                            else
                            {
                                AddTextToSlot(icons[i].transform, SLOTEXTENDER_SLOTKEYSLIST[i - 2]);
                            }
                        }                        
                        else
                        {
                            AddTextToSlot(icons[i].transform, QSEConfig.SLOTKEYSLIST[i]);
                        }
                    }
                }
                else if (Player.main.inExosuit && Player.main.GetPDA().state != PDA.State.Opening)
                {
                    if (i < 2)
                    {
                        continue;
                    }
                    else
                    {
                        AddTextToSlot(icons[i].transform, QSEConfig.SLOTKEYSLIST[i - 2]);
                    }
                }
                else
                {
                    AddTextToSlot(icons[i].transform, QSEConfig.SLOTKEYSLIST[i]);
                }
            }

            islabelsAdded = true;
        }        

        private TextMeshProUGUI AddTextToSlot(Transform parent, string buttonText)
        {
            TextMeshProUGUI text = Instantiate(HandReticle.main.compTextHand);            
            text.gameObject.layer = parent.gameObject.layer;
            text.gameObject.name = "QSELabel";
            text.transform.SetParent(parent, false);
            text.transform.localScale = new Vector3(1, 1, 1);
            text.gameObject.SetActive(true);
            text.enabled = true;
            text.text = buttonText;
            text.fontSize = 17;
            text.color = QSEConfig.TEXTCOLOR;
            RectTransformExtensions.SetParams(text.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), parent);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 100);
            text.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 100);
            text.rectTransform.anchoredPosition = new Vector2(0, -40);
            text.alignment = TextAlignmentOptions.Center;
            text.raycastTarget = false;

            return text;
        }
    }
}
