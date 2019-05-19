using Common.GUIHelper;
using RuntimeHelper.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        private static Rect ObjectWindow_Rect = new Rect(0, 732, 298, 258);
        private Rect ObjectWindow_drawRect;

        private List<GuiItem> guiItems_TechTypes = new List<GuiItem>();
        private Vector2 scrollPos_TechTypes = Vector2.zero;
        private int selected_TechType = 0;

        public void ObjectWindow_Awake()
        {
            guiItems_TechTypes.SetScrollViewItems(techTypeDatas.InitTechTypeNamesList(), 278f);
            guiItems_TechTypes.SetStateInverseTAB(selected_TechType);
        }

        public void ObjectWindow_OnGUI()
        {
            ObjectWindow_drawRect = SNWindow.CreateWindow(ObjectWindow_Rect, "Object Window");

            if (GUI.Button(new Rect(ObjectWindow_drawRect.x + 5, ObjectWindow_drawRect.y + 5, 120, 22), "Add new empty"))
            {
                if (!isDirty)
                    AddNewEmptyObject();
            }

            if (GUI.Button(new Rect(ObjectWindow_drawRect.x + 130, ObjectWindow_drawRect.y + 5, 163, 22), "Add selected TechType"))
            {
                AddNewTechObject(techTypeDatas[selected_TechType].TechType);
            }

            int selValue = SNScrollView.CreateScrollView(new Rect(ObjectWindow_drawRect.x + 5, ObjectWindow_drawRect.y + 30, ObjectWindow_drawRect.width - 10, ObjectWindow_drawRect.height - 5), ref scrollPos_TechTypes, ref guiItems_TechTypes, "Selected TechType:", techTypeDatas[selected_TechType].Name, 7);

            if (selValue != -1)
            {
                selected_TechType = selValue;
            }           
        }

        private void AddNewEmptyObject()
        {
            SetDirty(true);

            ReleaseObjectDrawing();

            GameObject newChild = new GameObject($"newChild_{addedChildObjectCount}");

            newChild.transform.SetParent(selectedObject.transform, false);
            newChild.transform.SetAllToZero();            
            addedChildObjectCount++;
            RefreshTransformsList();
            OnObjectChange(newChild);
            OutputWindow_Log($"New empty GameObject added, name: [{newChild.name}], parent: {newChild.transform.parent.name}, root: {newChild.transform.root.name}");
        }

        public void AddNewTechObject(TechType techType)
        {
            SetDirty(true);

            ReleaseObjectDrawing();

            GameObject newChild;

            OutputWindow_Log($"Trying to instantiate selected TechType: {techType.ToString()} from prefab.");

            try
            {
                newChild = Instantiate(CraftData.GetPrefabForTechType(techType), Vector3.zero, Quaternion.identity);
            }
            catch
            {
                AddNewEmptyObject();
                return;
            }           

            newChild.transform.SetParent(selectedObject.transform, false);
            newChild.transform.SetAllToZero();
            string objectName = newChild.name;
            newChild.name = "newChild_" + objectName;

            foreach (Component component in newChild.GetComponents<Component>())
            {
                Type componentType = component.GetType();

                if (componentType == typeof(Transform))
                    continue;
                if (componentType == typeof(Renderer))
                    continue;
                if (componentType == typeof(Mesh))
                    continue;
                if (componentType == typeof(Shader))
                    continue;

                Destroy(component);
            }

            RefreshTransformsList();
            OnObjectChange(newChild);

            OutputWindow_Log($"New TechType based GameObject added, name: [{newChild.name}], parent: {newChild.transform.parent.name}, root: {newChild.transform.root.name}");
        }
    }
}
