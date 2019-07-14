using UnityEngine;
using Common.GUIHelper;
using System.Collections.Generic;
using RuntimeHelper.Visuals;
using RuntimeHelper.Components;
using RuntimeHelper.Configuration;
using RuntimeHelper.Objects;
using Common;

namespace RuntimeHelper
{
    public partial class RuntimeHelper : MonoBehaviour
    {        
        private List<Transform> TRANSFORMS = new List<Transform>();
        private List<GuiItem> guiItems_transforms = new List<GuiItem>();
        private Vector2 scrollpos_transforms = Vector2.zero;
        private int ScrollView_transforms_retval = -1;
        private int current_transform_index = 0;

        private int addedChildObjectCount = 0;

        private GameObject baseObject, selectedObject, tempObject;          
        
        private string OBJECTINFO = string.Empty;
        private string COLLIDERINFO = string.Empty;                       
        
        private Vector3 lPos, lScale, lRot;
        public bool isDirty = false;       

        private string sizeText = string.Empty;
        private bool showCollider = true;
        
        private bool isExistsCollider = false;
        private bool isRootList = false;

        private static readonly List<string> EDIT_MODE = new List<string>();

        private bool showLocal = true;

        public RuntimeHelper ()
        {
            if (Main.Instance == null)
            {
                Main.Instance = FindObjectOfType(typeof(RuntimeHelper)) as RuntimeHelper;

                if (Main.Instance == null)
                {
                    GameObject runtimeHelper = new GameObject("RuntimeHelper");
                    Main.Instance = runtimeHelper.GetOrAddComponent<RuntimeHelper>();
                }
            }            
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);

            OutputWindow_Awake();
            OutputWindow_Log("Runtime Helper started");

            baseObject = gameObject;
            selectedObject = gameObject;           

            InitFullTechTypeList(ref techTypeDatas);            

            EditWindow_Awake();

            ObjectWindow_Awake();            
        }

        private void Start()
        {
            OnBaseObjectChange();
        }

        private void SetDirty(bool value)
        {
            isDirty = value;

            if (isDirty)
                showRendererWindow = false;            
        }

        private void SetScrollPos(ref Vector2 scrollpos, int index)
        {
            scrollpos.y = index * 24;
        }

        private void OnDestroy()
        {
            foreach (GameObject containerBase in Main.AllVisuals)
            {
                if (containerBase.IsNotNull())
                {
                    DestroyImmediate(containerBase);
                }
            }

            Main.AllVisuals.Clear();

            Destroy(this);
        }               
        
        private void RefreshTransformsList()
        {            
            baseObject.ScanObjectTransforms(ref TRANSFORMS);           
            guiItems_transforms.SetScrollViewItems(TRANSFORMS.InitTransformNamesList(), 278f);

            isRootList = false;
        }

        private void PrintObjectInfo()
        {
            Vector3 tr, qt, sc;

            if (showLocal)
            {
                tr = selectedObject.transform.localPosition;                
                qt = selectedObject.transform.localEulerAngles;
            }
            else
            {
                tr = selectedObject.transform.position;                
                qt = selectedObject.transform.eulerAngles;
            }

            sc = selectedObject.transform.localScale;

            OBJECTINFO =
                $"{selectedObject.transform.name} :\n\n" +
                $"Position{" ",4}x:{tr.x,8:F2},  y:{tr.y,8:F2},  z:{tr.z,8:F2}\n" +
                $"Scale{" ",7}x:{sc.x,8:F2},  y:{sc.y,8:F2},  z:{sc.z,8:F2}\n" +
                $"Rotation{" ",3}x:{qt.x,8:F0},  y:{qt.y,8:F0},  z:{qt.z,8:F0}\n";
        }

        private void PrintColliderInfo()
        {
            COLLIDERINFO = 
                $"Collider type: {colliderModify.ColliderType}\n\n" +                
                $"Center x:{colliderModify.Center.x,6:F2}  y:{colliderModify.Center.y,6:F2}  z:{colliderModify.Center.z,6:F2}\n" + sizeText;            
        }

        private void OnGUI()
        {
            if (isDirty)
                return;

            if (selectedObject.IsNull())
                return;

            Rect windowRect = SNWindow.CreateWindow(new Rect(0, 30, 298, 700), "Runtime Helper v.1.0 (Public Beta)");                       

            GUI.Label(new Rect(windowRect.x + 5, windowRect.y, 290, 25), $"Base : {baseObject.name}", SNStyles.GetGuiItemStyle(GuiItemType.LABEL, GuiColor.Green, TextAnchor.MiddleLeft));

            ScrollView_transforms_retval = SNScrollView.CreateScrollView(new Rect(windowRect.x + 5, windowRect.y + 22, windowRect.width - 10, 212), ref scrollpos_transforms, ref guiItems_transforms, isRootList ? "Active Scenes Root Transforms" : "Transforms of", isRootList ? string.Empty : baseObject.name, 10);

            GUI.Label(new Rect(windowRect.x + 5, windowRect.y + 300, 40, 22), "Base :", SNStyles.GetGuiItemStyle(GuiItemType.LABEL, GuiColor.Green, TextAnchor.MiddleLeft));


            if (GUI.Button(new Rect(windowRect.x + 60, windowRect.y + 300, 60, 22), "Set", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                OnBaseObjectChange();
            }

            if (GUI.Button(new Rect(windowRect.x + 125, windowRect.y + 300, 60, 22), "Refresh", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                OnRefresHBase();
            }

            if (GUI.Button(new Rect(windowRect.x + 190, windowRect.y + 300, 103, 22), "Get Roots", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                GetRoots();
            }

            GUI.Label(new Rect(windowRect.x + 5, windowRect.y + 325, 40, 22), "Object :", SNStyles.GetGuiItemStyle(GuiItemType.LABEL, GuiColor.Green, TextAnchor.MiddleLeft));

            if (GUI.Button(new Rect(windowRect.x + 60, windowRect.y + 325, 38, 22), selectedObject.activeSelf ? "Off" : "On", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (selectedObject == gameObject)
                {
                    OutputWindow_Log($"Object [{selectedObject.name}] active state cannot be modified!", LogType.Warning);
                    return;
                }

                selectedObject.SetActive(!selectedObject.activeSelf);
                OutputWindow_Log($"Object [{selectedObject.name}] active state now: {selectedObject.activeSelf.ToString()}");
            }

            if (GUI.Button(new Rect(windowRect.x + 100, windowRect.y + 325, 48, 22), "Copy", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (selectedObject == gameObject)
                {
                    OutputWindow_Log($"Object [{selectedObject.name}] is cannot be copied to TEMP!", LogType.Warning);
                }
                else
                {
                    selectedObject.CopyObject(out tempObject);
                    OutputWindow_Log($"Object [{tempObject.name}] is copied to TEMP and ready for paste.");
                }
            }

            if (tempObject.IsNotNull())
            {
                if (GUI.Button(new Rect(windowRect.x + 150, windowRect.y + 325, 48, 22), "Paste", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
                {
                    OnPasteObject();
                }
            }

            if (GUI.Button(new Rect(windowRect.x + 200, windowRect.y + 325, 93, 22), "Destroy", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (selectedObject == gameObject)
                {
                    OutputWindow_Log($"Object [{selectedObject.name}] is cannot be destroyed! Use 'Exit Program' button!", LogType.Warning);
                }
                else if (selectedObject.IsRoot())
                {
                    OutputWindow_Log("Root objects cannot be destroyed!", LogType.Warning);
                }
                else
                {
                    OnObjectDestroy(true);
                }
            }

            GUI.TextArea(new Rect(windowRect.x + 5, windowRect.y + 355, windowRect.width - 10, 100), OBJECTINFO);

            if (isExistsCollider)
            {
                GUI.TextArea(new Rect(windowRect.x + 5, windowRect.y + 465, windowRect.width - 10, 72), COLLIDERINFO);
            }

            GUI.Label(new Rect(windowRect.x + 5, windowRect.y + 542, 145, 22), "Transform shorthands:", SNStyles.GetGuiItemStyle(GuiItemType.LABEL, GuiColor.Green, TextAnchor.MiddleLeft));

            if (GUI.Button(new Rect(windowRect.x + 150, windowRect.y + 542, 140, 22), showLocal ? "Relative to: Local" : "Relative to: World", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                showLocal = !showLocal;

                if (showLocal)
                {
                    OutputWindow_Log("Transform info now relative to local space.");
                }
                else
                {
                    OutputWindow_Log("Transform info now relative to world space.");
                }
                
            }

            if (GUI.Button(new Rect(windowRect.x + 5, windowRect.y + 567, 140, 22), "Set vectors to Default", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (showLocal)
                {
                    selectedObject.transform.SetLocalsToZero();
                    OutputWindow_Log($"Object [{selectedObject.name}] local vectors set to: pos: (0,0,0); rot: (0,0,0); scale: (1,1,1)");
                }
                else
                {
                    selectedObject.transform.SetWorldToZero();
                    OutputWindow_Log($"Object [{selectedObject.name}] world vectors set to: pos: (0,0,0); rot: (0,0,0); scale: (1,1,1)");
                }

                
            }

            if (GUI.Button(new Rect(windowRect.x + 150, windowRect.y + 567, 140, 22), "Set position to Zero", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (showLocal)
                {
                    selectedObject.transform.SetLocalPositionToZero();
                    OutputWindow_Log($"Object [{selectedObject.name}] local position set to zero.");
                }
                else
                {
                    selectedObject.transform.SetPositionToZero();
                    OutputWindow_Log($"Object [{selectedObject.name}] world position set to zero.");
                }                
            }

            if (GUI.Button(new Rect(windowRect.x + 5, windowRect.y + 592, 140, 22), "Set Rotation to Zero", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (showLocal)
                {
                    selectedObject.transform.SetLocalRotationToZero();
                    OutputWindow_Log($"Object [{selectedObject.name}] local rotation set to zero.");
                }
                else
                {
                    selectedObject.transform.SetRotationToZero();
                    OutputWindow_Log($"Object [{selectedObject.name}] world rotation set to zero.");
                }                
            }

            if (GUI.Button(new Rect(windowRect.x + 150, windowRect.y + 592, 140, 22), "Set Scale to One", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                selectedObject.transform.SetLocalScaleToOne();
                OutputWindow_Log($"Object [{selectedObject.name}] local scale set to one.");
            }

            if (GUI.Button(new Rect(windowRect.x + 5, windowRect.y + 617, 140, 22), "Reset Transform", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                DrawObjectBounds dob = selectedObject.GetOrAddVisualBase(BaseType.Object).GetComponent<DrawObjectBounds>();                
                selectedObject.transform.SetTransformInfo(ref dob.transformBase);
                OutputWindow_Log($"Object [{selectedObject.name}] transform set to original values.");
            }

            if (isExistsCollider && showCollider)
            {
                if (GUI.Button(new Rect(windowRect.x + 150, windowRect.y + 617, 140, 22), "Reset Collider", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
                {
                    DrawColliderBounds dcb = selectedObject.GetOrAddVisualBase(BaseType.Collider).GetComponent<DrawColliderBounds>();                    
                    selectedObject.ResetCollider(dcb.ColliderBases[dcb.cInstanceID], dcb.cInstanceID);
                    dcb.colliderBase = dcb.ColliderBases[dcb.cInstanceID];
                    dcb.IsDraw(true);
                    GetColliderInfo();
                    OutputWindow_Log($"Collider [{dcb.ColliderBases[dcb.cInstanceID].ColliderType.ToString()}] values set to original.");
                }
            }            

            if (GUI.Button(new Rect(windowRect.x + 5, (windowRect.y + windowRect.height) - 27, windowRect.width - 10, 22), "Exit Program", SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                OnDestroy();                               
            }

            OutputWindow_OnGUI();

            EditWindow_OnGUI();

            ObjectWindow_OnGUI();

            ComponentWindow_OnGUI();

            RendererWindow_OnGUI();
        }             
        
        public void Update()
        {
            if (selectedObject.IsNull())
            {
                OutputWindow_Log("Selected object unexpectedly destroyed!", LogType.Warning);
                OnObjectDestroy(false);
            }

            if (baseObject.IsNull())
            {
                OutputWindow_Log("Base object unexpectedly destroyed!", LogType.Warning);
                OnObjectDestroy(false);
            }

            if (isDirty)
                return;

            PrintObjectInfo();

            if (isExistsCollider)
            {
                PrintColliderInfo();
            }

            if (Input.GetKeyDown(RuntimeHelper_Config.KEYBINDINGS["ToggleMouse"]))
            {
                UWE.Utils.lockCursor = !UWE.Utils.lockCursor;
            }
            
            if (ScrollView_transforms_retval != -1)
            {                
                current_transform_index = ScrollView_transforms_retval;
                OnObjectChange(TRANSFORMS[current_transform_index].gameObject);                               
            }

            EditWindow_Update();

            ComponentWindow_Update();

            if (showRendererWindow)
                RendererWindow_Update();

            GetObjectVectors();

            if (isExistsCollider)
            {
                switch (colliderModify.ColliderType)
                {
                    case ColliderType.BoxCollider:
                        sizeText = $"Size x:{colliderModify.Size.x,6:F2}  y:{colliderModify.Size.y,6:F2}  z:{colliderModify.Size.z,6:F2}\n";
                        break;
                    case ColliderType.CapsuleCollider:
                        sizeText = $"Radius:{colliderModify.Radius,6:F2}  height:{colliderModify.Height,6:F2} direction:{EditModeStrings.DIRECTION[colliderModify.Direction]}\n";
                        break;
                    case ColliderType.SphereCollider:
                        sizeText = $"Radius:{colliderModify.Radius,6:F2}\n";
                        break;
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (isExistsCollider)
                {
                    showCollider = !showCollider;                    
                    SetColliderDrawing(showCollider, null);
                }
            }            

            if (Event.current == null)
            {
                OutputWindow_Log("Event.current is not ready!", LogType.Warning);
                return;
            }

            if (Event.current.Equals(Event.KeyboardEvent("up")))
            {
                value = scaleFactor;
                EditObjectVectors();
            }
            else if (Event.current.Equals(Event.KeyboardEvent("down")))
            {
                value = -scaleFactor;                
                EditObjectVectors();                
            }            
        }

        
        private void GetObjectVectors()
        {         
            if (showLocal)
            {
                lRot = selectedObject.transform.localEulerAngles;
                lPos = selectedObject.transform.localPosition;                
            }
            else
            {
                lRot = selectedObject.transform.eulerAngles;
                lPos = selectedObject.transform.position;                
            }

            lScale = selectedObject.transform.localScale;
        }

        private void SetObjectVectors()
        {
            if (showLocal)
            {
                selectedObject.transform.localPosition = new Vector3(lPos.x, lPos.y, lPos.z);                               
            }
            else
            {
                selectedObject.transform.position = new Vector3(lPos.x, lPos.y, lPos.z);                
            }

            selectedObject.transform.localScale = new Vector3(lScale.x, lScale.y, lScale.z);
        }        

        private void EditObjectVectors()
        {
            switch (EDIT_MODE[current_editmode_index])
            {                
                case "Rotation: x":                    
                    selectedObject.transform.Rotate(Vector3.right, value < 0 ? -1 : 1, showLocal? Space.Self : Space.World);
                    break;

                case "Rotation: y":                    
                    selectedObject.transform.Rotate(Vector3.up, value < 0 ? -1 : 1, showLocal ? Space.Self : Space.World);
                    break;

                case "Rotation: z":                    
                    selectedObject.transform.Rotate(Vector3.forward, value < 0 ? -1 : 1, showLocal ? Space.Self : Space.World);
                    break;

                case "Position: x":
                    lPos.x += value;
                    break;
                case "Position: y":
                    lPos.y += value;
                    break;
                case "Position: z":
                    lPos.z += value;
                    break;
                case "Scale: x":
                    lScale.x += value;
                    break;
                case "Scale: y":
                    lScale.y += value;
                    break;
                case "Scale: z":
                    lScale.z += value;
                    break;
                case "Collider Size: x":
                    colliderModify.Size = new Vector3(colliderModify.Size.x + value, colliderModify.Size.y, colliderModify.Size.z);
                    break;
                case "Collider Size: y":
                    colliderModify.Size = new Vector3(colliderModify.Size.x, colliderModify.Size.y + value, colliderModify.Size.z);
                    break;
                case "Collider Size: z":
                    colliderModify.Size = new Vector3(colliderModify.Size.x, colliderModify.Size.y, colliderModify.Size.z + value);
                    break;
                case "Collider Center: x":
                    colliderModify.Center = new Vector3(colliderModify.Center.x + value, colliderModify.Center.y, colliderModify.Center.z);
                    break;
                case "Collider Center: y":
                    colliderModify.Center = new Vector3(colliderModify.Center.x, colliderModify.Center.y + value, colliderModify.Center.z);
                    break;
                case "Collider Center: z":
                    colliderModify.Center = new Vector3(colliderModify.Center.x, colliderModify.Center.y, colliderModify.Center.z + value);
                    break;
                case "Collider Radius":
                    colliderModify.Radius += value;
                    break;
                case "Collider Height":
                    colliderModify.Height += value;
                    break;
                case "Collider Direction":

                    switch (colliderModify.Direction)
                    {
                        case 0:
                            colliderModify.Direction = 1;
                            break;
                        case 1:
                            colliderModify.Direction = 2;
                            break;
                        case 2:
                            colliderModify.Direction = 0;
                            break;
                    }
                    break;                
            }

            SetObjectVectors();
            
            if (isExistsCollider)
            {
                switch (colliderModify.ColliderType)
                {
                    case ColliderType.BoxCollider:
                        bc.center = colliderModify.Center;
                        bc.size = colliderModify.Size;
                        SetColliderDrawing(true, bc);
                        break;
                    case ColliderType.CapsuleCollider:
                        cc.center = colliderModify.Center;
                        cc.radius = colliderModify.Radius;
                        cc.height = colliderModify.Height;
                        cc.direction = colliderModify.Direction;
                        SetColliderDrawing(true, cc);
                        break;
                    case ColliderType.SphereCollider:
                        sc.center = colliderModify.Center;
                        sc.radius = colliderModify.Radius;
                        SetColliderDrawing(true, sc);
                        break;
                }

                
            }
            
        }        
    }
}