using Common.GUIHelper;
using RuntimeHelper.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
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

            if (GUI.Button(new Rect(ObjectWindow_drawRect.x + 5, ObjectWindow_drawRect.y + 5, 120, 22), ObjectWindow[0], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (!isDirty)
                    AddNewEmptyObject();
            }

            if (GUI.Button(new Rect(ObjectWindow_drawRect.x + 130, ObjectWindow_drawRect.y + 5, 163, 22), ObjectWindow[1], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                StartCoroutine(AddNewTechObjectAsync(techTypeDatas[selected_TechType].TechType));
            }

            GuiItemEvent selValue = SNScrollView.CreateScrollView(new Rect(ObjectWindow_drawRect.x + 5, ObjectWindow_drawRect.y + 30, ObjectWindow_drawRect.width - 10, ObjectWindow_drawRect.height - 5), ref scrollPos_TechTypes, ref guiItems_TechTypes, "Selected TechType:", techTypeDatas[selected_TechType].Name, 7);

            if (selValue.ItemID != -1 && selValue.MouseButton == 0)
            {
                selected_TechType = selValue.ItemID;
            }           
        }

        private void AddNewEmptyObject()
        {
            SetDirty(true);

            ReleaseObjectDrawing();

            GameObject newChild = new GameObject($"newChild_{addedChildObjectCount}");

            newChild.transform.SetParent(baseObject.transform, false);
            newChild.transform.SetLocalsToZero();            
            addedChildObjectCount++;
            RefreshTransformsList();
            OnObjectChange(newChild);
            OutputWindow_Log(MESSAGE_TEXT[MESSAGES.NEW_EMPTY_OBJECT_ADDED], newChild.name, newChild.transform.parent.name, newChild.transform.root.name);
        }

        public IEnumerator AddNewTechObjectAsync(TechType techType)
        {
            CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync(techType);
            yield return request;

            OutputWindow_Log(MESSAGE_TEXT[MESSAGES.TRY_TO_ADD_A_TECH_OBJECT], (int)techType, techType.ToString());

            GameObject result = request.GetResult();

            if (result == null)
            {
                OutputWindow_Log(WARNING_TEXT[WARNINGS.OBJECT_CANNOT_INSTANTIATE], LogType.Error, (int)techType, techType.ToString());
                SetDirty(false);
                yield break;
            }

            SetDirty(true);

            GameObject newChild = Instantiate(result, Vector3.zero, Quaternion.Euler(Vector3.zero));

            ReleaseObjectDrawing();

            newChild.transform.SetParent(baseObject.transform, false);
            newChild.transform.SetLocalsToZero();
            string objectName = newChild.name;
            newChild.name = "newChild_" + objectName;

            foreach (Component component in newChild.GetComponents<Component>())
            {
                Type componentType = component.GetType();

                bool isValid = false;

                foreach (Type type in ValidTypes)
                { 
                    if (componentType == type)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (isValid)
                {
                    if (componentType == typeof(Locomotion))
                    {
                        ((Locomotion)component).enabled = false;
                        OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_STATE_MODIFIED], LogType.Warning, GetComponentShortType(component), "False");
                    }
                    if (componentType == typeof(SplineFollowing))
                    {
                        ((SplineFollowing)component).enabled = false;
                        OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_STATE_MODIFIED], LogType.Warning, GetComponentShortType(component), "False");
                    }
                    if (componentType == typeof(SwimBehaviour))
                    {
                        ((SwimBehaviour)component).enabled = false;
                        OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_STATE_MODIFIED], LogType.Warning, GetComponentShortType(component), "False");
                    }
                    if (componentType == typeof(LiveMixin))
                    {
                        ((LiveMixin)component).enabled = false;
                        OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_STATE_MODIFIED], LogType.Warning, GetComponentShortType(component), "False");
                    }
                    continue;
                }                
                else
                {
                    try
                    {
                        DestroyImmediate(component);
                        OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_REMOVED], LogType.Warning, GetComponentShortType(component), newChild.name);
                    }
                    catch
                    {
                        OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_CANNOT_REMOVED], LogType.Error, GetComponentShortType(component), newChild.name);
                    }
                }
            }

            RefreshTransformsList();            
            OnObjectChange(newChild);

            OutputWindow_Log(MESSAGE_TEXT[MESSAGES.NEW_TECH_OBJECT_ADDED], newChild.name, newChild.transform.parent.name, newChild.transform.root.name);

            yield break;
        }
        
        private static List<Type> ValidTypes = new List<Type>
        {
            typeof(BoxCollider),
            typeof(CapsuleCollider),
            //typeof(LargeWorldEntity),
            typeof(Mesh),
            typeof(MeshFilter),
            typeof(MeshRenderer),
            typeof(Renderer),
            typeof(Shader),
            typeof(SkyApplier),
            typeof(SphereCollider),
            typeof(Transform),
            typeof(VFXFabricating),
            typeof(Rigidbody),
            typeof(Locomotion),
            typeof(SplineFollowing),
            typeof(SwimBehaviour),
            typeof(LiveMixin),
            typeof(SkinnedMeshRenderer)







        };
    }
}
