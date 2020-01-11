using Common.GUIHelper;
using RuntimeHelper.Visuals;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        private static Rect ComponentWindow_Rect = new Rect(300, 732, 398, 258);
        private Rect ComponentWindow_drawRect;

        private List<Component> components = new List<Component>();
        private List<GuiItem> guiItems_Components = new List<GuiItem>();               
        
        private Vector2 scrollPos_Components = Vector2.zero;

        private List<string> componentNames = new List<string>();

        private int selected_component = 0;
        private GuiItemEvent ScrollView_components_event;

        private BoxCollider bc;
        private CapsuleCollider cc;
        private SphereCollider sc;

        private ColliderInfo colliderModify;

        private PropertyInfo changeEnabledproperty = null;

        private bool enabledValue;

        private bool isColliderSelected = false;


        private Dictionary<int, ComponentInfo> componentInfos = new Dictionary<int, ComponentInfo>();

        private void RefreshComponentsList()
        {
            components.Clear();
            componentNames.Clear();

            try
            {
                selectedObject.GetComponents(components);

                foreach (Component component in components)
                {
                    componentNames.Add(GetComponentShortType(component));
                }
            }
            catch
            {

            }

            guiItems_Components.SetScrollViewItems(componentNames, 378f);

            selected_component = 0;

            changeEnabledproperty = null;
        }

        private void ComponentWindow_OnGUI()
        {
            if (isDirty)
                return;            

            ComponentWindow_drawRect = SNWindow.CreateWindow(ComponentWindow_Rect, "Component Window");

            ScrollView_components_event = SNScrollView.CreateScrollView(new Rect(ComponentWindow_drawRect.x + 5, ComponentWindow_drawRect.y, ComponentWindow_drawRect.width - 10, 168), ref scrollPos_Components, ref guiItems_Components, "Components of", selectedObject.name, 7);

            if (changeEnabledproperty != null)
            {
                if (GUI.Button(new Rect(ComponentWindow_drawRect.x + 5, (ComponentWindow_drawRect.y + ComponentWindow_drawRect.height) - 27, 50, 22), enabledValue ? "Off" : "On", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
                {
                    enabledValue = !enabledValue;
                    changeEnabledproperty.SetValue(components[selected_component], enabledValue, BindingFlags.Instance | BindingFlags.Public, null, null, null);

                    showComponentInfoWindow = false;
                    ComponentInfoWindow_Awake(components[selected_component]);
                    showComponentInfoWindow = true;

                    OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_STATE_MODIFIED], LogType.Warning, GetComponentShortType(components[selected_component]), enabledValue);
                }
            }

            if (GUI.Button(new Rect(ComponentWindow_drawRect.x + 60, (ComponentWindow_drawRect.y + ComponentWindow_drawRect.height) - 27, 150, 22), ComponentWindow[0], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                showComponentInfoWindow = false;
                RemoveComponent(components[selected_component]);
            }

            if (GUI.Button(new Rect(ComponentWindow_drawRect.x + 220, (ComponentWindow_drawRect.y + ComponentWindow_drawRect.height) - 27, 150, 22), ComponentWindow[1], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                showRendererWindow = false;
                showComponentInfoWindow = false;
                showAddComponentWindow = true;                
            }
        }
        
        private void ComponentWindow_Update()
        {
            if (ScrollView_components_event.ItemID != -1 && ScrollView_components_event.MouseButton == 0)
            {
                showAddComponentWindow = false;

                selected_component = ScrollView_components_event.ItemID;

                int componentID = components[selected_component].GetInstanceID();

                if (!componentInfos.ContainsKey(componentID))
                {
                    componentInfos.Add(componentID, new ComponentInfo(components[selected_component]));
                }

                Type componentType = components[selected_component].GetType();
                                
                if (IsSupportedCollider(components[selected_component]))
                {
                    isColliderSelected = true;
                    GetColliderInfo();                    
                    DrawColliderControl dcc = selectedObject.GetComponentInChildren<DrawColliderControl>();                    
                    dcc.DrawSelectedCollider(componentID, true);                    
                    RefreshEditModeList();
                }
                else
                {
                    isColliderSelected = false;
                    RefreshEditModeList();
                }
               
                if (IsSupportedRenderer(components[selected_component]))
                {
                    showComponentInfoWindow = false;
                    RendererWindow_Awake();
                }                
                else
                {
                    showRendererWindow = false;                    
                    ComponentInfoWindow_Awake(components[selected_component]);
                    showComponentInfoWindow = true;
                }                

                if (IsComponentInBlacklist(components[selected_component]))
                {
                    changeEnabledproperty = null;
                    return;
                }

                changeEnabledproperty = components[selected_component].GetType().GetProperty("enabled");

                if (changeEnabledproperty != null)
                {
                    enabledValue = (bool)changeEnabledproperty.GetValue(components[selected_component], BindingFlags.Instance | BindingFlags.Public, null, null, null);                    
                }

            }

        }

        private bool IsSupportedCollider(Component component)
        {
            Type componentType = component.GetType();

            if (componentType == typeof(BoxCollider))
            {
                return true;
            }
            else if (componentType == typeof(SphereCollider))
            {
                return true;
            }
            else if (componentType == typeof(CapsuleCollider))
            {
                return true;
            }
            return false;
        }

        private bool IsSupportedRenderer(Component component)
        {
            Type componentType = component.GetType();

            if (componentType == typeof(MeshRenderer))
            {
                return true;
            }
            else if (componentType == typeof(SkinnedMeshRenderer))
            {
                return true;
            }

            return false;
        }


        private void RemoveComponent(Component component)
        {
            if (IsComponentInBlacklist(component))
            {
                OutputWindow_Log(WARNING_TEXT[WARNINGS.COMPONENT_IN_BLACKLIST], LogType.Warning, GetComponentShortType(component));
                return;
            }

            if (IsSupportedCollider(component))
            {
                int colliderID = component.GetInstanceID();
                DrawColliderControl dcc = selectedObject.GetComponentInChildren<DrawColliderControl>();
                dcc.RemoveColliderDrawing(colliderID);
                isColliderSelected = false;
            }

            try
            {
                DestroyImmediate(component);
                OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_REMOVED], LogType.Warning, GetComponentShortType(component), selectedObject.name);
            }
            catch
            {
                OutputWindow_Log(MESSAGE_TEXT[MESSAGES.COMPONENT_CANNOT_REMOVED], LogType.Warning, GetComponentShortType(component), selectedObject.name);
            }

            UpdateVisuals();
        }

        private void GetColliderInfo()
        {
            Collider collider = (Collider)components[selected_component];

            switch (collider.GetColliderType())
            {
                case ColliderType.BoxCollider:
                    bc = (BoxCollider)collider;
                    colliderModify.ColliderType = ColliderType.BoxCollider;
                    colliderModify.Size = bc.size;
                    colliderModify.Center = bc.center;
                    return;

                case ColliderType.CapsuleCollider:
                    cc = (CapsuleCollider)collider;
                    colliderModify.ColliderType = ColliderType.CapsuleCollider;
                    colliderModify.Center = cc.center;
                    colliderModify.Radius = cc.radius;
                    colliderModify.Direction = cc.direction;
                    colliderModify.Height = cc.height;
                    return;

                case ColliderType.SphereCollider:
                    sc = (SphereCollider)collider;
                    colliderModify.ColliderType = ColliderType.SphereCollider;
                    colliderModify.Center = sc.center;
                    colliderModify.Radius = sc.radius;
                    return;
            }
        }

        public static readonly List<Type> Components_Blacklist = new List<Type>()
        {
            typeof(Transform),
            typeof(RectTransform),
            typeof(CanvasRenderer),
            typeof(MeshRenderer),
            typeof(SkinnedMeshRenderer),            
            typeof(Mesh),
            typeof(MeshFilter),
            typeof(Shader),
            typeof(RuntimeHelper),
            typeof(DrawObjectBounds),
            typeof(DrawColliderBounds),
            typeof(TracePlayerPos)
        };

        private bool IsComponentInBlacklist(Component component)
        {
            Type componentType = component.GetType();

            foreach (Type type in Components_Blacklist)
            {
                if (componentType == type)
                {                    
                    return true;
                }
                else
                {
                    continue;
                }
            }

            return false;
        }
        
        private string GetComponentShortType(Component component)
        {
            return component.GetType().ToString().Split('.').GetLast();
        }
    }
}
