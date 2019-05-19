using Common.GUIHelper;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        private int ScrollView_editmode_retval = -1;
        private int current_editmode_index = 0;
        private List<GuiItem> guiItems_editmode = new List<GuiItem>();
        private Vector2 scrollpos_editmode = Vector2.zero;
        private static Rect EditWindow_Rect = new Rect(300, 30, 248, 300);
        private Rect EditWindow_drawRect;
        private float value = 0.01f;

        private Event<object> onScaleFactorChanged = new Event<object>();

        private float scaleFactor;

        private void RefreshEditModeList()
        {
            current_editmode_index = 0;

            EDIT_MODE.Clear();

            foreach (string item in EditModeStrings.OBJECT_MODES)
            {
                EDIT_MODE.Add(item);
            }
            
            if (isExistsCollider)
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

            ScrollView_editmode_retval = SNScrollView.CreateScrollView(new Rect(EditWindow_drawRect.x + 5, EditWindow_drawRect.y, EditWindow_drawRect.width - 10, 168), ref scrollpos_editmode, ref guiItems_editmode, "Current mode:", EDIT_MODE[current_editmode_index], 7);

            SNHorizontalSlider.CreateHorizontalSlider(new Rect(EditWindow_drawRect.x + 5, EditWindow_drawRect.y + 200, EditWindow_drawRect.width - 10, 35), ref scaleFactor, 0.01f, 1f, "Scale Factor:", onScaleFactorChanged);
        }

        private void EditWindow_Update()
        {
            if (ScrollView_editmode_retval != -1)
            {
                current_editmode_index = ScrollView_editmode_retval;
            }
        }

    }
}
