using Common.GUIHelper;
using System;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        private GuiItemEvent ScrollView_marklist_event;
        private int current_marklist_index = 0;
        private List<GuiItem> guiItems_marklist = new List<GuiItem>();
        private Vector2 scrollpos_marklist = Vector2.zero;
        private static Rect MarkWindow_Rect = new Rect(300, 305, 248, 270);
        private Rect MarkWindow_drawRect;
        private List<GameObject> MARKED_OBJECTS = new List<GameObject>();

        private List<string> markList = new List<string>();

        private bool isEmpty;

        private void RefreshMarkObjectsList()
        {
            current_marklist_index = 0;

            markList.Clear();            

            foreach (GameObject item in MARKED_OBJECTS)
            {
                markList.Add(item.name);
            }

            if (markList.Count == 0)
            {
                markList.Add("<Empty>");

                isEmpty = true;
            }
            else
            {
                isEmpty = false;
            }

            guiItems_marklist.SetScrollViewItems(markList, MarkWindow_Rect.width - 20);
            guiItems_marklist.SetStateInverseTAB(current_marklist_index);
        }

        private void MarkWindow_Awake()
        {
            RefreshMarkObjectsList();

            MarkWindow_drawRect = SNWindow.InitWindowRect(MarkWindow_Rect, true);
        }


        private void MarkWindow_OnGUI()
        {
            SNWindow.CreateWindow(MarkWindow_Rect, "Marked Objects Window");

            ScrollView_marklist_event = SNScrollView.CreateScrollView(new Rect(MarkWindow_drawRect.x + 5, MarkWindow_drawRect.y, MarkWindow_drawRect.width - 10, 168), ref scrollpos_marklist, ref guiItems_marklist, "Selected:", markList[current_marklist_index], 7);

            if (GUI.Button(new Rect(MarkWindow_drawRect.x + 5, MarkWindow_drawRect.y + 218, 40, 22), MarkWindow[0], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isEmpty)
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_SET_MARKED], LogType.Warning);
                }
                else
                {
                    OnBaseObjectChange(MARKED_OBJECTS[current_marklist_index]);
                }
            }

            if (GUI.Button(new Rect(MarkWindow_drawRect.x + 50, MarkWindow_drawRect.y + 218, 60, 22), MarkWindow[1], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isEmpty)
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_REMOVE_MARKED], LogType.Warning);
                }
                else
                {
                    RemoveFromMarkList(MARKED_OBJECTS[current_marklist_index]);
                }
            }

        }

        private void MarkWindow_Update()
        {
            if (ScrollView_marklist_event.ItemID != -1 && ScrollView_marklist_event.MouseButton == 0)
            {
                current_marklist_index = ScrollView_marklist_event.ItemID;
            }
        }


        private void AddToMarkList(GameObject go)
        {
            if (MARKED_OBJECTS.Contains(go))
            {
                OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_MARK], LogType.Warning);
            }
            else
            {
                MARKED_OBJECTS.Add(go);

                OutputWindow_Log(MESSAGE_TEXT[MESSAGES.OBJECT_MARKED], go.name);
            }

            RefreshMarkObjectsList();
        }

        private void RemoveFromMarkList(GameObject go)
        {
            if (MARKED_OBJECTS.Contains(go))
            {
                MARKED_OBJECTS.Remove(go);

                OutputWindow_Log(MESSAGE_TEXT[MESSAGES.OBJECT_UNMARKED], go.name);
            }


            RefreshMarkObjectsList();
        }


        





    }
}
