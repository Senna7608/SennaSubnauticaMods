using Common.GUIHelper;
using RuntimeHelper.Visuals;
using System;
using System.Collections.Generic;
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
        private int ScrollView_components_retval = -1;

        private BoxCollider bc;
        private CapsuleCollider cc;
        private SphereCollider sc;

        private ColliderInfo colliderModify;

        private void RefreshComponentsList()
        {
            components.Clear();
            componentNames.Clear();

            try
            {
                selectedObject.GetComponents(components);

                foreach (Component component in components)
                {
                    componentNames.Add(component.GetType().ToString());
                }
            }
            catch
            {

            }

            guiItems_Components.SetScrollViewItems(componentNames, 378f);            
        }

        private void ComponentWindow_OnGUI()
        {
            if (isDirty)
                return;            

            ComponentWindow_drawRect = SNWindow.CreateWindow(ComponentWindow_Rect, "Component Window");

            ScrollView_components_retval = SNScrollView.CreateScrollView(new Rect(ComponentWindow_drawRect.x + 5, ComponentWindow_drawRect.y, ComponentWindow_drawRect.width - 10, 168), ref scrollPos_Components, ref guiItems_Components, "Components of", selectedObject.name, 7);

            if (GUI.Button(new Rect(ComponentWindow_drawRect.x + 5, (ComponentWindow_drawRect.y + ComponentWindow_drawRect.height) - 27, 150, 22), "Remove Component", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                RemoveComponent();
            }
        }

        private void ComponentWindow_Update()
        {
            if (ScrollView_components_retval != -1)
            {
                selected_component = ScrollView_components_retval;
                
                if (IsSupportedCollider(components[selected_component]))
                {
                    GetColliderInfo();                    
                    SetColliderDrawing(true, (Collider)components[selected_component]);
                    RefreshEditModeList();
                }
                else
                {                    
                    SetColliderDrawing(false, null);
                    RefreshEditModeList();
                }
                
                if (IsSupportedRenderer(components[selected_component]))
                {
                    RendererWindow_Awake();                    
                }
                else
                {
                    showRendererWindow = false;
                    ClearUndoArray();
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


        private void RemoveComponent()
        {
            Type componentType = components[selected_component].GetType();

            foreach (Type type in Components_Blacklist)
            {
                if (componentType == type)
                {
                    OutputWindow_Log($"Component [{componentType}] is in Blacklist and cannot be removed!", LogType.Warning);
                    return;
                }
                else
                {
                    continue;
                }
            }

            if (IsSupportedCollider(components[selected_component]))
            {
                SetColliderDrawing(false, null);
            }

            Destroy(components[selected_component]);

            RefreshComponentsList();
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
    }
}
