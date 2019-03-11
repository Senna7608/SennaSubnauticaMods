using UnityEngine;
using Common.GUIHelper;
using System;
using System.Collections.Generic;

namespace Common.RuntimeHelper
{
    public class RuntimeObjectManager : MonoBehaviour
    {
        private List<GameObject> lineContainers;
        
        private BoxCollider bc;
        private CapsuleCollider cc;
        private SphereCollider sc;

        private ColliderInfo colliderDefaults, colliderChanges;
        
        public float lineWidth = 0.01f;
        public Color lineColor = Color.green;

        private int selection = 0;
        private int currentTRindex = 0;        
        private float value;
        private Vector3 lPos, lScale, lRot;                

        private string sizeText;
        private bool showCollider = true;

        private static readonly List<string> EDIT_MODE = new List<string>();

        public void Awake()
        {
            Collider collider = gameObject.GetComponentInChildren<Collider>();

            if (lineContainers != null)
            {
                ColliderHelper.ClearLineContainers(ref lineContainers);
            }

            if (collider == null)
            {
                colliderDefaults.type = ColliderType.None;
            }

            Type thisCollider = collider.GetType();

            if (thisCollider == typeof(BoxCollider))
            {                             
                bc = (BoxCollider)collider;
                colliderDefaults.transforms = new List<Transform>();
                foreach (Transform transform in gameObject.GetAllComponentsInChildren<Transform>())
                {
                    colliderDefaults.transforms.Add(transform);
                }
                               
                colliderDefaults.type = ColliderType.BoxCollider;
                colliderDefaults.name = bc.name;                
                colliderDefaults.size = bc.size;
                colliderDefaults.center = bc.center;
                lineContainers = new List<GameObject>();
                ColliderHelper.CreateBCLineContainers(ref lineContainers);
                ColliderHelper.AddLineRendererToContainers(ref lineContainers, lineWidth, lineColor);
                
                foreach (GameObject container in lineContainers)
                {
                    container.transform.SetParent(bc.transform);
                    container.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                EDIT_MODE.Clear();
                
                foreach (string item in EditModeStrings.OBJECT_MODES)
                {
                    EDIT_MODE.Add(item);
                }

                foreach (string item in EditModeStrings.BOXCOLLIDER_MODES)
                {
                    EDIT_MODE.Add(item);
                }
            }
            else if (thisCollider == typeof(CapsuleCollider))
            {                
                cc = (CapsuleCollider)collider;
                colliderDefaults.transforms = new List<Transform>();

                foreach (Transform transform in gameObject.GetAllComponentsInChildren<Transform>())
                {
                    colliderDefaults.transforms.Add(transform);
                }
                colliderDefaults.type = ColliderType.CapsuleCollider;
                colliderDefaults.name = cc.name;
                colliderDefaults.center = cc.center;
                colliderDefaults.radius = cc.radius;
                colliderDefaults.direction = cc.direction;
                colliderDefaults.height = cc.height;
                lineContainers = new List<GameObject>();
                ColliderHelper.CreateCCLineContainers(ref lineContainers);
                ColliderHelper.AddLineRendererToContainers(ref lineContainers, lineWidth, lineColor);

                foreach (GameObject container in lineContainers)
                {
                    container.transform.SetParent(cc.transform);
                    container.transform.localRotation = Quaternion.Euler(0, 0, 0);                    
                }

                EDIT_MODE.Clear();

                foreach (string item in EditModeStrings.OBJECT_MODES)
                {
                    EDIT_MODE.Add(item);
                }

                foreach (string item in EditModeStrings.CAPSULECOLLIDER_MODES)
                {
                    EDIT_MODE.Add(item);
                }
            }
            else if (thisCollider == typeof(SphereCollider))
            {
                sc = (SphereCollider)collider;
                colliderDefaults.transforms = new List<Transform>();
                foreach (Transform transform in gameObject.GetAllComponentsInChildren<Transform>())
                {
                    colliderDefaults.transforms.Add(transform);
                }
                colliderDefaults.type = ColliderType.SphereCollider;
                colliderDefaults.name = sc.name;
                colliderDefaults.center = sc.center;
                colliderDefaults.radius = sc.radius;
                lineContainers = new List<GameObject>();
                ColliderHelper.CreateSCLineContainers(ref lineContainers);
                ColliderHelper.AddLineRendererToContainers(ref lineContainers, lineWidth, lineColor);

                foreach (GameObject container in lineContainers)
                {
                    container.transform.SetParent(sc.transform);
                    container.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                EDIT_MODE.Clear();

                foreach (string item in EditModeStrings.OBJECT_MODES)
                {
                    EDIT_MODE.Add(item);
                }

                foreach (string item in EditModeStrings.SPHERECOLLIDER_MODES)
                {
                    EDIT_MODE.Add(item);
                }
            }
            else
            {
                Debug.LogError("Unsupported collider [" + string.Format("{0}", collider.GetType() + "]"));
                DestroyImmediate(this);
                return;
            }

            colliderChanges = colliderDefaults;            
        }

        
        public void OnGUI()
        {
            if (!isActiveAndEnabled)
                return;
            SNWindow.CreateWindow(new Rect(10, 200, 300, 330),"Runtime Object Manager v.1.0");
            GUI.Label(new Rect(15, 225, 300, 320),
                $"Edit mode: {EDIT_MODE[selection]}\n\n" + 
                
                $"GameObject: {colliderChanges.name}\n" +
                $"Collider type: {colliderChanges.type}\n\n" +
                $"Transform: {currentTRindex + 1} of {colliderChanges.transforms.Count}\n" +
                $"name: {colliderChanges.transforms[currentTRindex].name}\n\n" +
                
                $"Position x: {colliderChanges.transforms[currentTRindex].gameObject.transform.localPosition.x,6:F2}  y: {colliderChanges.transforms[currentTRindex].gameObject.transform.localPosition.y,6:F2}  z: {colliderChanges.transforms[currentTRindex].gameObject.transform.localPosition.z,6:F2}\n" +
                $"Rotation(Euler) x: {colliderChanges.transforms[currentTRindex].gameObject.transform.localEulerAngles.x,3:F0}  y: {colliderChanges.transforms[currentTRindex].gameObject.transform.localEulerAngles.y,3:F0}  z: {colliderChanges.transforms[currentTRindex].gameObject.transform.localEulerAngles.z,3:F0}\n" +
                $"Scale x: {colliderChanges.transforms[currentTRindex].gameObject.transform.localScale.x,6:F2}  y: {colliderChanges.transforms[currentTRindex].gameObject.transform.localScale.y,6:F2}  z: {colliderChanges.transforms[currentTRindex].gameObject.transform.localScale.z,6:F2}\n\n" +                
                sizeText +                
                $"Collider center x: {colliderChanges.center.x,6:F2}  y: {colliderChanges.center.y,6:F2}  z: {colliderChanges.center.z,6:F2}\n\n" +
                "Help:\n" +
                "press 'Y' to switch on/of collider visualization\n" +
                "press 'X' to reset collider values\n" +
                "press left/right arrow to change edit mode\n" +
                "press up/down arrow to change the selected value\n"               
                ,SNStyles.GetGuiItemStyle(GuiItemType.LABEL, GuiColor.Green, TextAnchor.MiddleLeft));
        }
        

        public void LateUpdate()
        {
            if (!showCollider)
                return;

            switch (colliderChanges.type)
            {
                case ColliderType.BoxCollider:
                    ColliderHelper.DrawBoxColliderBounds(ref lineContainers, bc);
                    break;
                case ColliderType.CapsuleCollider:
                    ColliderHelper.DrawCapsuleColliderBounds(ref lineContainers, cc);
                    break;
                case ColliderType.SphereCollider:
                    ColliderHelper.DrawSphereColliderBounds(ref lineContainers, sc);
                    break;
            }
        }

        
        public void Update()
        {
            if (!isActiveAndEnabled)
                return;

            GetVectors();

            switch (colliderChanges.type)
            {
                case ColliderType.BoxCollider:                    
                    sizeText = $"Collider size x: {colliderChanges.size.x,6:F2}  y: {colliderChanges.size.y,6:F2}  z: {colliderChanges.size.z,6:F2}\n";
                    break;
                case ColliderType.CapsuleCollider:                    
                    sizeText = $"Collider radius: {colliderChanges.radius,6:F2}  height: {colliderChanges.height  ,6:F2} direction: {EditModeStrings.DIRECTION[colliderChanges.direction]}\n";
                    break;
                case ColliderType.SphereCollider:
                    sizeText = $"Collider radius: {colliderChanges.radius,6:F2}\n";
                    break;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                showCollider = !showCollider;
                ColliderHelper.DisableContainers(ref lineContainers);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                switch (colliderChanges.type)
                {
                    case ColliderType.BoxCollider:
                        bc.size = colliderDefaults.size;
                        bc.center = colliderDefaults.center;
                        colliderChanges = colliderDefaults;
                        break;
                    case ColliderType.CapsuleCollider:
                        cc.radius = colliderDefaults.radius;
                        cc.center = colliderDefaults.center;
                        cc.direction = colliderDefaults.direction;
                        cc.height = colliderDefaults.height;
                        colliderChanges = colliderDefaults;
                        break;
                    case ColliderType.SphereCollider:
                        sc.radius = colliderDefaults.radius;
                        sc.center = colliderDefaults.center;                       
                        colliderChanges = colliderDefaults;
                        break;
                }
            }

            if (Event.current.Equals(Event.KeyboardEvent("up")))
            {
                if (EDIT_MODE[selection] == "TRANSFORM")
                {
                    currentTRindex++;
                    if (currentTRindex == colliderChanges.transforms.Count)
                    {
                        currentTRindex = 0;
                    }
                    return;
                }

                value = 0.01f;
                SetVectors();
            }
            else if (Event.current.Equals(Event.KeyboardEvent("down")))
            {
                if (EDIT_MODE[selection] == "TRANSFORM")
                {
                    currentTRindex--;
                    if (currentTRindex < 0)
                    {
                        currentTRindex = colliderChanges.transforms.Count - 1;
                    }
                    return;
                }

                value = -0.01f;                
                SetVectors();                
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                selection--;
                if (selection < 0)
                {
                    selection = EDIT_MODE.Count -1;
                }                
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                selection++;
                if (selection == EDIT_MODE.Count)
                {
                    selection = 0;
                }                
            }
        }

        private void GetVectors()
        {
            lRot = colliderChanges.transforms[currentTRindex].gameObject.transform.localEulerAngles;
            lPos = colliderChanges.transforms[currentTRindex].gameObject.transform.localPosition;
            lScale = colliderChanges.transforms[currentTRindex].gameObject.transform.localScale;
        }

        private void SetVectors()
        {
            switch (EDIT_MODE[selection])
            {                
                case "ROTATION: x":
                    lRot.x += value * 100;
                    break;
                case "ROTATION: y":
                    lRot.y += value * 100;
                    break;
                case "ROTATION: z":
                    lRot.z += value * 100;
                    break;
                case "POSITION: x":
                    lPos.x += value;
                    break;
                case "POSITION: y":
                    lPos.y += value;
                    break;
                case "POSITION: z":
                    lPos.z += value;
                    break;
                case "SCALE":
                    lScale.x += value;
                    lScale.y += value;
                    lScale.z += value;
                    break;
                case "COLLIDER SIZE: x":
                    colliderChanges.size = new Vector3(colliderChanges.size.x + value, colliderChanges.size.y, colliderChanges.size.z);                    
                    break;
                case "COLLIDER SIZE: y":
                    colliderChanges.size = new Vector3(colliderChanges.size.x, colliderChanges.size.y + value, colliderChanges.size.z);                    
                    break;
                case "COLLIDER SIZE: z":
                    colliderChanges.size = new Vector3(colliderChanges.size.x, colliderChanges.size.y, colliderChanges.size.z + value);                   
                    break;
                case "COLLIDER CENTER: x":
                    colliderChanges.center = new Vector3(colliderChanges.center.x + value, colliderChanges.center.y, colliderChanges.center.z);
                    break;
                case "COLLIDER CENTER: y":
                    colliderChanges.center = new Vector3(colliderChanges.center.x, colliderChanges.center.y + value, colliderChanges.center.z);
                    break;
                case "COLLIDER CENTER: z":
                    colliderChanges.center = new Vector3(colliderChanges.center.x, colliderChanges.center.y, colliderChanges.center.z + value);
                    break;
                case "COLLIDER RADIUS":
                    colliderChanges.radius += value;
                    break;
                case "COLLIDER HEIGHT":
                    colliderChanges.height += value;
                    break;
                case "COLLIDER DIRECTION":
                    if (colliderChanges.direction == 0)
                        colliderChanges.direction = 1;
                    else if (colliderChanges.direction == 1)
                        colliderChanges.direction = 2;
                    else if (colliderChanges.direction == 2)
                        colliderChanges.direction = 0;
                    break;
            }

            colliderChanges.transforms[currentTRindex].gameObject.transform.localPosition = new Vector3(lPos.x, lPos.y, lPos.z);
            colliderChanges.transforms[currentTRindex].gameObject.transform.localRotation = Quaternion.Euler(lRot.x, lRot.y, lRot.z);
            colliderChanges.transforms[currentTRindex].gameObject.transform.localScale = new Vector3(lScale.x, lScale.y, lScale.z);
            
            switch (colliderChanges.type)
            {
                case ColliderType.BoxCollider:
                    bc.center = colliderChanges.center;
                    bc.size = colliderChanges.size;
                    break;
                case ColliderType.CapsuleCollider:
                    cc.center = colliderChanges.center;
                    cc.radius = colliderChanges.radius;
                    cc.height = colliderChanges.height;
                    cc.direction = colliderChanges.direction;
                    break;
                case ColliderType.SphereCollider:
                    sc.center = colliderChanges.center;
                    sc.radius = colliderChanges.radius;                    
                    break;
            }
        }        
    }
}