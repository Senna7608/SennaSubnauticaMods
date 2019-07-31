using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using SMLHelper.V2.Utility;
using Common.GUIHelper;
using RuntimeHelper.FileHelper;
using System;
using RuntimeHelper.Renderers;
using System.Reflection;
using RuntimeHelper.Components;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        private List<string> componentInfoNames = new List<string>();
        private List<GuiItem> guiItems_componentInfo = new List<GuiItem>();
        private bool showComponentInfoWindow;
        private Vector2 scrollPos_ComponentInfo;

        public void ComponentInfoWindow_Awake(Component component)
        {
            componentInfoNames.Clear();

            componentInfoNames = component.CreateComponentInfoList();

            guiItems_componentInfo.SetScrollViewItems(componentInfoNames, 530f);
        }

        private void ComponentInfoWindow_OnGUI()
        {
            if (!showComponentInfoWindow)
                return;

            Rect windowrect = SNWindow.CreateWindow(new Rect(700, 732, 550, 348), "Component Information Window");

            SNScrollView.CreateScrollView(new Rect(windowrect.x + 5, windowrect.y, windowrect.width - 10, windowrect.height - 60), ref scrollPos_ComponentInfo, ref guiItems_componentInfo, $"Information of this", components[selected_component].GetType().ToString().Split('.').GetLast() , 10);
                      
        }
              
    }
}
