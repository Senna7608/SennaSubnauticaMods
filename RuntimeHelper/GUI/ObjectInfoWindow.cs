using System.Collections.Generic;
using UnityEngine;
using Common.GUIHelper;
using Common;
using static RuntimeHelper.Objects.PropertyHelper;
using static RuntimeHelper.Objects.FieldHelper;
using System;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
    {
        private List<string> objectInfoText = new List<string>();
        private List<GuiItem> guiItems_objectInfo = new List<GuiItem>();
        private bool showObjectInfoWindow;
        private Vector2 scrollPos_objectInfo;
        private GuiItemEvent ScrollView_objectInfo_event;
        private int current_objectInfo_index = 0;
        private bool showSetButton = false;
        private UnityEngine.Object selected;
        private ObjectProperties objectProperties;
        private ObjectFields objectFields;

        public void ObjectInfoWindow_Awake(UnityEngine.Object _object)
        {
            selected = _object;

            showSetButton = false;

            objectInfoText.Clear();

            //componentInfoNames = _object.CreateComponentInfoList();            

            objectProperties = new ObjectProperties(_object);

            objectInfoText.Add("Properties:");

            foreach (ObjectProperty objectProperty in objectProperties)
            {
                objectInfoText.Add(objectProperty.ToString());
            }

            objectFields = new ObjectFields(_object);

            objectInfoText.Add("Fields:");

            foreach (ObjectField objectField in objectFields)
            {
                objectInfoText.Add(objectField.ToString());
            }

            guiItems_objectInfo.SetScrollViewItems(objectInfoText, 530f);

        }

        private void ObjectInfoWindow_OnGUI()
        {
            if (!showObjectInfoWindow)
                return;

            Rect windowrect = SNWindow.CreateWindow(new Rect(700, 732, 550, 348), "Object Information Window");

            ScrollView_objectInfo_event = SNScrollView.CreateScrollView(new Rect(windowrect.x + 5, windowrect.y, windowrect.width - 10, windowrect.height - 60), ref scrollPos_objectInfo, ref guiItems_objectInfo, $"Information of this", componentNames[selected_component], 10);

            if (GUI.Button(new Rect(windowrect.x + 5, windowrect.y + (windowrect.height - 50), 120, 22), ComponentInfoWindow[0], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                AddToLog();
            }

            if (showSetButton)
            {
                if (GUI.Button(new Rect(windowrect.x + 130, windowrect.y + (windowrect.height - 50), 60, 22), ComponentInfoWindow[1], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
                {
                    SwitchGameObject();
                }
            }
        }

        private void ObjectInfoWindow_Update()
        {
            if (!showObjectInfoWindow)
            {
                return;
            }

            if (ScrollView_objectInfo_event.ItemID != -1 && ScrollView_objectInfo_event.MouseButton == 0)
            {
                current_objectInfo_index = ScrollView_objectInfo_event.ItemID;

                if (objectInfoText[current_objectInfo_index].Contains("UnityEngine.GameObject"))
                {
                    showSetButton = true;
                }
                else
                {
                    showSetButton = false;
                }
            }

            if (ScrollView_objectInfo_event.ItemID != -1 && ScrollView_objectInfo_event.MouseButton == 1)
            {
                int index = ScrollView_objectInfo_event.ItemID;

                if (IsBoolValue(index, out string name, out bool isProperty))
                {
                    if (isProperty)
                    {
                        ObjectProperty property = objectProperties.GetProperty(name);

                        bool oldValue = (bool)property.GetValue();

                        property.SetValue(!oldValue);
                    }
                    else
                    {
                        ObjectField field = objectFields.GetField(name);

                        bool oldValue = (bool)field.GetValue();

                        field.SetValue(!oldValue);
                    }

                    ObjectInfoWindow_Awake(objects[selected_component]);
                }
            }
        }

        private bool IsBoolValue(int index, out string name, out bool isProperty)
        {
            string[] splittedItem = objectInfoText[index].Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

            object value = null;

            try
            {
                value = objectProperties.GetProperty(splittedItem[0]);

                if (value == null)
                {
                    value = objectFields.GetField(splittedItem[0]);
                }
                else
                {
                    ObjectProperty property = value as ObjectProperty;

                    if (property.Type == typeof(bool))
                    {
                        isProperty = true;
                        name = splittedItem[0];
                        return true;
                    }
                    else
                    {
                        isProperty = false;
                        name = null;
                        return false;
                    }
                }

                if (value == null)
                {
                    isProperty = false;
                    name = null;
                    return false;
                }
                else
                {
                    ObjectField field = value as ObjectField;

                    if (field.Type == typeof(bool))
                    {
                        isProperty = false;
                        name = splittedItem[0];
                        return true;
                    }
                    else
                    {
                        isProperty = false;
                        name = null;
                        return false;
                    }
                }
            }
            catch
            {
                isProperty = false;
                name = null;
                return false;
            }
        }



        private bool IsArrayField(int index, out string name)
        {
            string[] splittedItem = objectInfoText[index].Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);

            object value = null;

            try
            {
                value = objectFields.GetField(splittedItem[0]);

                if (value == null)
                {
                    name = null;
                    return false;
                }                
                else
                {
                    ObjectField field = value as ObjectField;

                    if (field.Type.IsArray)
                    {                        
                        name = splittedItem[0];
                        return true;
                    }
                    else
                    {                        
                        name = null;
                        return false;
                    }
                }
            }
            catch
            {                
                name = null;
                return false;
            }
        }













        private void SwitchGameObject()
        {
            string[] splittedItem = objectInfoText[current_objectInfo_index].Split(new char[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                        
            GameObject probeGO = null;

            try
            {                
                probeGO = objectProperties.GetPropertyValue(splittedItem[0]) as GameObject;                

                if (probeGO == null)
                {
                    probeGO = objectFields.GetFieldValue(splittedItem[0]) as GameObject;                    
                }

                if (probeGO == null)
                {                    
                    throw new Exception("Can't set this object!");
                }
                else
                {
                    OnBaseObjectChange(probeGO);
                }
            }
            catch
            {
                OutputWindow_Log(ERROR_TEXT[ERRORS.CANNOT_SWITCH_TO_OBJECT], LogType.Error, probeGO.name);
            }
        }

        private void AddToLog()
        {
            SNLogger.Log("\n*** RuntimeHelper: Object Information Log ***");

            SNLogger.Log($"Object: {componentNames[selected_component]}");

            foreach (string item in objectInfoText)
            {
                SNLogger.Log(item);
            }

            SNLogger.Log("\n*** RuntimeHelper: End of Log ***");

            OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_INFORMATION_LOGGED], LogType.Log, componentNames[selected_component]);
        }
    }


}

