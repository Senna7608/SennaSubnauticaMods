using System.Collections.Generic;
using UnityEngine;
using Common.GUIHelper;
using RuntimeHelper.Components;
using System.Reflection;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        private List<string> componentInfoNames = new List<string>();
        private List<GuiItem> guiItems_componentInfo = new List<GuiItem>();
        private bool showComponentInfoWindow;
        private Vector2 scrollPos_ComponentInfo;
        private GuiItemEvent ScrollView_componentInfoEvent;
               
        public void ComponentInfoWindow_Awake(Component component)
        {
            GetComponentInfoText(component);

            guiItems_componentInfo.SetScrollViewItems(componentInfoNames, 530f);
        }

        private void ComponentInfoWindow_OnGUI()
        {
            if (!showComponentInfoWindow)
                return;

            Rect windowrect = SNWindow.CreateWindow(new Rect(700, 732, 550, 348), "Component Information Window");

            ScrollView_componentInfoEvent = SNScrollView.CreateScrollView(new Rect(windowrect.x + 5, windowrect.y, windowrect.width - 10, windowrect.height - 60), ref scrollPos_ComponentInfo, ref guiItems_componentInfo, $"Information of this", components[selected_component].GetType().ToString().Split('.').GetLast() , 10);
        }

        private void ComponentInfoWindow_Update()
        {
            if (!showComponentInfoWindow)
                return;

            if (ScrollView_componentInfoEvent.ItemID != -1 && ScrollView_componentInfoEvent.MouseButton == 0)
            {
                int pos = ScrollView_componentInfoEvent.ItemID;

                int fieldPos = componentInfoNames.IndexOf("Fields:");

                if (pos == 0 || pos == fieldPos)
                    return;

                string objectName = componentInfoNames[pos].Split(' ')[0];

                print($"objectName: {objectName}");


            }
        }


        private void GetComponentInfoText(Component component)
        {
            componentInfoNames.Clear();

            ComponentInfo componentInfo = componentInfos[component.GetInstanceID()];

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            componentInfoNames.Add("Properties:");

            foreach (PropertyInfo propertyInfo in componentInfo._PropertyInfo)
            {
                try
                {
                    componentInfoNames.Add($"{propertyInfo.Name} = [{propertyInfo.GetValue(component, bindingFlags, null, null, null).ToString()}]");                   
                }
                catch
                {
                    continue;
                }
            }

            componentInfoNames.Add("Fields:");
            
            foreach (FieldInfo fieldInfo in componentInfo._FieldInfo)
            {
                try
                {
                    componentInfoNames.Add($"{fieldInfo.Name} = [{fieldInfo.GetValue(componentInfo._Component).ToString()}]");                    
                }
                catch
                {
                    continue;
                }
            }


            

        }

              
    }
}
