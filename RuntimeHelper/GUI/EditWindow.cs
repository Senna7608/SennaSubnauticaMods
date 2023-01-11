using Common.GUIHelper;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
    {
        private GuiItemEvent ScrollView_editmode_event;
        private int current_editmode_index = 0;
        private List<GuiItem> guiItems_editmode = new List<GuiItem>();
        private Vector2 scrollpos_editmode = Vector2.zero;
        private static Rect EditWindow_Rect = new Rect(300, 30, 248, 270);
        private Rect EditWindow_drawRect;
        private float value = 0.01f;

        private static readonly List<string> EDIT_MODE = new List<string>();

        private Event<object> onScaleFactorChanged = new Event<object>();

        private float scaleFactor;

        private Vector2 rtPos, rtSize, rtPivot, rtAnchorMin, rtAnchorMax, rtOffsetMin, rtOffsetMax;

        private void RefreshEditModeList()
        {
            current_editmode_index = 0;

            EDIT_MODE.Clear();

            if (!isRectTransform)
            {
                foreach (string item in EditModeStrings.OBJECT_MODES)
                {
                    EDIT_MODE.Add(item);
                }

                if (isColliderSelected)
                {
                    foreach (KeyValuePair<ColliderType, string[]> pair in EditModeStrings.COLLIDER_MODES)
                    {
                        if (pair.Key == colliderModify.ColliderType)
                        {
                            foreach (string modes in pair.Value)
                            {
                                EDIT_MODE.Add(modes);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (string item in EditModeStrings.RECTTRANSFORM_MODES)
                {
                    EDIT_MODE.Add(item);
                }
            }

            guiItems_editmode.SetScrollViewItems(EDIT_MODE, EditWindow_Rect.width - 20);
            guiItems_editmode.SetStateInverseTAB(current_editmode_index);
        }

        private void EditWindow_Awake()
        {
            EditModeStrings.Init();

            RefreshEditModeList();

            EditWindow_drawRect = SNWindow.InitWindowRect(EditWindow_Rect, true);
            scaleFactor = value;
            onScaleFactorChanged.AddHandler(this, new Event<object>.HandleFunction(OnScaleFactorChanged));
            
        }

        private void OnScaleFactorChanged(object newValue)
        {
            scaleFactor = (float)newValue;            
        }

        private void EditWindow_OnGUI()
        {
            SNWindow.CreateWindow(EditWindow_Rect, "Edit Window");

            ScrollView_editmode_event = SNScrollView.CreateScrollView(new Rect(EditWindow_drawRect.x + 5, EditWindow_drawRect.y, EditWindow_drawRect.width - 10, 168), ref scrollpos_editmode, ref guiItems_editmode, "Current mode:", EDIT_MODE[current_editmode_index], 7);

            SNHorizontalSlider.CreateHorizontalSlider(new Rect(EditWindow_drawRect.x + 5, EditWindow_drawRect.y + 200, EditWindow_drawRect.width - 10, 35), ref scaleFactor, 0.01f, 1f, "Scale Factor:", onScaleFactorChanged);
        }

        private void EditWindow_Update()
        {
            if (ScrollView_editmode_event.ItemID != -1 && ScrollView_editmode_event.MouseButton == 0)
            {
                current_editmode_index = ScrollView_editmode_event.ItemID;
            }
        }

        private void EditRectTransform()
        {
            RectTransform rt = objects[selected_component] as RectTransform;
            rtPos = rt.anchoredPosition;
            rtSize = rt.sizeDelta;
            rtPivot = rt.pivot;
            rtAnchorMin = rt.anchorMin;
            rtAnchorMax = rt.anchorMax;
            rtOffsetMin = rt.offsetMin;
            rtOffsetMax = rt.offsetMax;

            switch (EDIT_MODE[current_editmode_index])
            {
                case "Position: x":
                    rtPos.x += value;
                    break;
                case "Position: y":
                    rtPos.y += value;
                    break;
                case "Width":
                    rtSize.x += value;
                    break;
                case "Height":
                    rtSize.y += value;
                    break;
                case "Scale: x,y":
                    lScale.x += value;
                    lScale.y += value;
                    break;
                case "Scale: x":
                    lScale.x += value;
                    break;
                case "Scale: y":
                    lScale.y += value;
                    break;
                case "Pivot: x":
                    rtPivot.x += value;
                    break;
                case "Pivot: y":
                    rtPivot.y += value;
                    break;
                case "AnchorMin: x":
                    rtAnchorMin.x += value;
                    break;
                case "AnchorMin: y":
                    rtAnchorMin.y += value;
                    break;
                case "AnchorMax: x":
                    rtAnchorMax.x += value;
                    break;
                case "AnchorMax: y":
                    rtAnchorMax.y += value;
                    break;
                case "OffsetMin: x":
                    rtOffsetMin.x += value;
                    break;
                case "OffsetMin: y":
                    rtOffsetMin.y += value;
                    break;
                case "OffsetMax: x":
                    rtOffsetMax.x += value;
                    break;
                case "OffsetMax: y":
                    rtOffsetMax.y += value;
                    break;
            }

            SetRectTransform();
        }

        private void SetRectTransform()
        {
            RectTransform rt = objects[selected_component] as RectTransform;

            rt.anchoredPosition = rtPos;
            rt.sizeDelta = rtSize;
            rt.offsetMin = rtOffsetMin;
            rt.offsetMax = rtOffsetMax;
            rt.anchorMin = rtAnchorMin;
            rt.anchorMax = rtAnchorMax;            
            rt.pivot = rtPivot;            
        }
    }
}
