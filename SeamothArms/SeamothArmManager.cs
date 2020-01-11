using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common;
using static Common.GameHelper;
using SeamothArms.ArmPrefabs;
using System.Collections;

namespace SeamothArms
{
    public partial class SeamothArmManager : MonoBehaviour
    {
        public SeamothArmManager Instance
        {
            get
            {
                return isSeamothArmSlotsReady ? (this) : null;
            }            
        }

        private EnergyMixin EnergySource
        {
            get
            {
                return seamoth.GetComponent<EnergyMixin>();
            }
        }

        private string[] colorParams = new string[]
        {
            "_Color",
            "_Tint",
            "_Color",
            "_Color2",
            "_Color3"
        };

        private string[] specParams = new string[]
        {
            "_SpecColor",
            string.Empty,
            "_SpecColor",
            "_SpecColor2",
            "_SpecColor3"
        };

        private const Arm Left = Arm.Left;
        private const Arm Right = Arm.Right;
        private Arm currentSelectedArm;

        private VFXConstructing vfxConstructing;

        private ISeamothArm leftArm;
        private ISeamothArm rightArm;

        public TechType currentLeftArmType;
        public TechType currentRightArmType;

        private SeaMoth seamoth;
        public bool armsDirty;

        private Transform leftArmAttach;
        private Transform rightArmAttach;

        private GameObject activeTarget;

        private GameObject SeamothArmPrefabs;
        private List<IItemsContainer> containers = new List<IItemsContainer>();
        private Transform light_left, light_right;

        private PureTransform light_left_default;
        private PureTransform light_right_default;

        private PureTransform light_left_UP;
        private PureTransform light_right_DOWN;

        private bool hasInitStrings;
        private bool lastHasPropCannon;
        private StringBuilder sb = new StringBuilder();
        private string uiStringPrimary;

        public Transform aimTargetLeft;
        public Transform aimTargetRight;
        
        private bool isSeamothArmSlotsReady = false;

        private void Awake()
        {
            seamoth = GetComponent<SeaMoth>();

            vfxConstructing = GetComponent<VFXConstructing>();

            light_left = gameObject.FindChildInMaxDepth("light_left").transform;
            light_left_default = new PureTransform(light_left.localPosition, light_left.localRotation, light_left.localScale);
            light_left_UP = new PureTransform(new Vector3(0f, 0.68f, 1f), Quaternion.Euler(0, 0, 0), light_left.localScale);

            light_right = gameObject.FindChildInMaxDepth("light_right").transform;
            light_right_default = new PureTransform(light_right.localPosition, light_right.localRotation, light_right.localScale);
            light_right_DOWN = new PureTransform(new Vector3(0f, -1.21f, 1.04f), Quaternion.Euler(35, 0, 0), light_left.localScale);

            SeamothArmPrefabs = CreateGameObject("SeamothArmPrefabs", gameObject.transform);

            SeamothArmPrefabs.SetActive(false);

            InitializeArmsCache();

            GameObject leftArmAttachPoint = CreateGameObject("leftArmAttachPoint", seamoth.torpedoTubeLeft, new Vector3(-0.93f, 0f, -0.74f), new Vector3(346, 23, 0));
            leftArmAttach = leftArmAttachPoint.transform;

            GameObject rightArmAttachPoint = CreateGameObject("rightArmAttachPoint", seamoth.torpedoTubeRight, new Vector3(0.93f, 0f, -0.74f), new Vector3(346, 337, 360));
            rightArmAttach = rightArmAttachPoint.transform;

            GameObject leftAimForward = CreateGameObject("leftAimForward", seamoth.transform);
            aimTargetLeft = leftAimForward.transform;

            GameObject rightAimForward = CreateGameObject("rightAimForward", seamoth.transform);
            aimTargetRight = rightAimForward.transform;

            UpdateArmRenderers();            
        }
        
        public void WakeUp()
        {
            SNLogger.Log("[SeamothArmManager] Received WakeUp message.");

            isSeamothArmSlotsReady = true;

            quickSlotTimeUsed = (float[])seamoth.GetPrivateField("quickSlotTimeUsed", BindingFlags.SetField);
            quickSlotCooldown = (float[])seamoth.GetPrivateField("quickSlotCooldown", BindingFlags.SetField);
            quickSlotToggled = (bool[])seamoth.GetPrivateField("quickSlotToggled", BindingFlags.SetField);
            quickSlotCharge = (float[])seamoth.GetPrivateField("quickSlotCharge", BindingFlags.SetField);

            seamoth.onToggle += OnToggleSlot;
            seamoth.modules.onEquip += OnEquip;
            seamoth.modules.onUnequip += OnUnequip;

            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));

            onDockedChanged.AddHandler(this, new Event<bool>.HandleFunction(OnDockedChanged));

            CheckArmSlots();
        }


        private void OnEquip(string slot, InventoryItem item)
        {
            if (IsSeamothArm(item.item.GetTechType(), out TechType techType))
            {
                int slotID = seamoth.GetSlotIndex(slot);

                if (slotID == LeftArmSlotID)
                {
                    AddArm(Arm.Left, techType);
                    return;
                }
                else if (slotID == RightArmSlotID)
                {
                    AddArm(Arm.Right, techType);
                    return;
                }
            }
        }


        private void OnUnequip(string slot, InventoryItem item)
        {
            if (IsSeamothArm(item.item.GetTechType(), out TechType techType))
            {
                int slotID = seamoth.GetSlotIndex(slot);

                if (slotID == LeftArmSlotID)
                {
                    RemoveArm(Arm.Left);
                    return;
                }
                else if (slotID == RightArmSlotID)
                {
                    RemoveArm(Arm.Right);
                    return;
                }
            }
        }


        private void OnToggleSlot(int slotID, bool state)
        {
            if (state)
            {
                if (slotID == LeftArmSlotID)
                {
                    currentSelectedArm = Arm.Left;
                }
                else if (slotID == RightArmSlotID)
                {
                    currentSelectedArm = Arm.Right;
                }
            }
            else
            {
                currentSelectedArm = Arm.None;
                ResetArms();
            }
        }
        
        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {
            if (newMotorMode != Player.MotorMode.Vehicle)
            {                
                ResetArms();                
            }
        } 
                
        private void Update()
        {
            if (!isSeamothArmSlotsReady || armsDirty)
            {                
                return;
            }

            bool pilotingMode = seamoth.GetPilotingMode();
            bool flag2 = pilotingMode && !seamoth.docked;

#if DEBUG

            pilotingMode = true;
            flag2 = true;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentSelectedArm != Arm.Left)
                {
                    seamoth.SlotKeyDown(LeftArmSlotID);
                }
                
                seamoth.SendMessage("SlotLeftDown");
            }
                        
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentSelectedArm != Arm.Right)
                {
                    seamoth.SlotKeyDown(RightArmSlotID);
                }

                seamoth.SendMessage("SlotLeftDown");
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                seamoth.SendMessage("SlotLeftUp");
            }

#endif

            if (pilotingMode)
            {
                if (AvatarInputHandler.main.IsEnabled())
                {                    
                    Vector3 eulerAngles = seamoth.transform.eulerAngles;
                    eulerAngles.x = MainCamera.camera.transform.eulerAngles.x;
                    Quaternion quaternion = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
                    Quaternion rotation = quaternion;                    
                    
                    if (flag2)
                    {
                        aimTargetLeft.transform.position = MainCamera.camera.transform.position + quaternion * Vector3.forward * 100f;
                        aimTargetRight.transform.position = MainCamera.camera.transform.position + rotation * Vector3.forward * 100f;
                    }
                    
                    bool hasPropCannon = false;

                    if (currentSelectedArm == Arm.Left && leftArm != null)
                    {
                        leftArm.Update(ref quaternion);                        
                        hasPropCannon = LeftArmType == SeamothPropulsionArmPrefab.TechTypeID;
                    }

                    if (currentSelectedArm == Arm.Right && rightArm != null)
                    {
                        rightArm.Update(ref rotation);                        
                        hasPropCannon = RightArmType == SeamothPropulsionArmPrefab.TechTypeID;
                    }                     

                    UpdateUIText(hasPropCannon);

                    if (GameInput.GetButtonDown(GameInput.Button.AltTool))
                    {
                        if (rightArm != null && !rightArm.OnAltDown())
                        {
                            if (leftArm != null)
                            {
                                leftArm.OnAltDown();
                            }
                        }
                    }
                }

                UpdateActiveTarget(HasClaw(), HasDrill());
            }            
        }

        private void UpdateUIText(bool hasPropCannon)
        {
            if (!hasInitStrings || lastHasPropCannon != hasPropCannon)
            {
                sb.Length = 0;
                sb.AppendLine(LanguageCache.GetButtonFormat("PressToExit", GameInput.Button.Exit));

                if (hasPropCannon)
                {
                    string[] splittedHint = LanguageCache.GetButtonFormat("PropulsionCannonToRelease", GameInput.Button.AltTool).Split(' ');
                                        
                    sb.AppendLine($"{splittedHint[0]} {splittedHint.GetLast()}");
                }

                lastHasPropCannon = hasPropCannon;
                uiStringPrimary = sb.ToString();
            }

            HandReticle.main.SetUseTextRaw(uiStringPrimary, string.Empty);
            hasInitStrings = true;
        }

        private void OnDockedChanged(bool isDocked)
        {
            UpdateColliders();

            if (leftArm != null)
            {
                leftArm.SetRotation(Arm.Left, isDocked);
            }

            if (rightArm != null)
            {
                rightArm.SetRotation(Arm.Right, isDocked);
            }
        }

                
        private void UpdateColliders()
        {            
            if (leftArm != null)
            {
                Collider[] collidersLeft = leftArm.GetGameObject().GetComponentsInChildren<Collider>(true);

                for (int j = 0; j < collidersLeft.Length; j++)
                {
                    collidersLeft[j].enabled = !seamoth.docked;
                }                
            }

            if (rightArm != null)
            {
                Collider[] collidersRight = rightArm.GetGameObject().GetComponentsInChildren<Collider>(true);

                for (int k = 0; k < collidersRight.Length; k++)
                {
                    collidersRight[k].enabled = !seamoth.docked;
                }
            }
        }


        private void UpdateActiveTarget(bool canPickup, bool canDrill)
        {
            GameObject targetObject = null;

            ObjectType objectType = ObjectType.None;

            if (canPickup || canDrill)
            {
                Targeting.GetTarget(seamoth.gameObject, 4.8f, out targetObject, out float num, null);
            }

            if (targetObject)
            {
                GameObject rootObject = UWE.Utils.GetEntityRoot(targetObject);

                rootObject = (!(rootObject != null)) ? targetObject : rootObject;

                if (rootObject.GetComponentProfiled<Pickupable>())
                {
                    targetObject = rootObject;
                    objectType = ObjectType.Pickupable;
                }
                else if (rootObject.GetComponentProfiled<Drillable>())
                {
                    targetObject = rootObject;
                    objectType = ObjectType.Drillable;
                }
                else
                {
                    targetObject = null;
                }
            }

            activeTarget = targetObject;

            if (activeTarget && currentSelectedArm != Arm.None)
            {
                if (canDrill && objectType == ObjectType.Drillable && GetSelectedArmTechType() == SeamothDrillArmPrefab.TechTypeID)
                {
                    GUIHand component = Player.main.GetComponent<GUIHand>();
                    GUIHand.Send(activeTarget, HandTargetEventType.Hover, component);

                }
                else if (canPickup && objectType == ObjectType.Pickupable && GetSelectedArmTechType() == SeamothClawArmPrefab.TechTypeID)
                {
                    Pickupable pickupable = activeTarget.GetComponent<Pickupable>();
                    TechType techType = pickupable.GetTechType();
                    
                    HandReticle.main.SetInteractText(LanguageCache.GetPickupText(techType), false, HandReticle.Hand.Left);
                    HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
                }
            }
        }
               

        private void SetLightsPosition()
        {
            if (IsAnyArmAttached)
            {
                light_left.transform.localPosition = light_left_UP.pos;
                light_left.transform.localRotation = light_left_UP.rot;

                light_right.transform.localPosition = light_right_DOWN.pos;
                light_right.transform.localRotation = light_right_DOWN.rot;
            }
            else
            {
                light_left.transform.localPosition = light_left_default.pos;
                light_left.transform.localRotation = light_left_default.rot;

                light_right.transform.localPosition = light_right_default.pos;
                light_right.transform.localRotation = light_right_default.rot;
            }
        }                      
        
        private void UpdateArmRenderers()
        {
            SubName subName = seamoth.GetComponent<SubName>();
            
            List<Renderer> armRenderers = new List<Renderer>();

            foreach (GameObject geo in GeoCache)
            {
                Renderer[] renderers = geo.GetComponents<Renderer>();

                for (int i = 0; i < renderers.Length; i++)
                {
                    armRenderers.Add(renderers[i]);
                }
            }

            int armMaterials = 0;

            for (int i = 0; i < armRenderers.Count; i++)
            {
                armMaterials += armRenderers[i].materials.Length;
            }

            for (int i = 0; i < subName.rendererInfo.Length; i++)
            {
                SubName.ColorData colorData = subName.rendererInfo[i];

                switch (i)
                {
                    case 0:
                    case 3:
                    case 4:

                        int arraySize = colorData.renderers.Length;

                        Array.Resize(ref colorData.renderers, arraySize + armMaterials);

                        int m = arraySize;

                        for (int j = 0; j < armRenderers.Count; j++)
                        {
                            for (int k = 0; k < armRenderers[j].materials.Length; k++)
                            {
                                colorData.renderers[m] = default(SubName.RenderData);
                                colorData.renderers[m].renderer = armRenderers[j];
                                colorData.renderers[m].materialIndex = k;
                                colorData.renderers[m].colorProperties = new SubName.PropertyData[]
                                {
                                    new SubName.PropertyData(false, colorParams[i]),
                                    new SubName.PropertyData(true, specParams[i])
                                };

                                m++;
                            }
                        }

                        break;

                    default:
                        continue;
                }                
            }
            
            Vector3[] hsbArray = new Vector3[subName.rendererInfo.Length];

            for (int i = 0; i < subName.rendererInfo.Length; i++)
            {
                hsbArray[i] = subName.rendererInfo[i].HSB;
            }

            subName.DeserializeColors(hsbArray);           
        }        
        /*
        private bool CheckSeamothArmSlots()
        {
            bool left = seamoth.GetSlotIndex("SeamothArmLeft") == -1 ? false : true;
            bool right = seamoth.GetSlotIndex("SeamothArmRight") == -1 ? false : true;            

            return left && right;
        }

        private IEnumerator WaitForSlotExtender()
        {
            while (!CheckSeamothArmSlots())
            {
                yield return null;
            }

            isSeamothArmSlotsReady = true;

            seamoth.onToggle += OnToggleSlot;            

            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));

            onDockedChanged.AddHandler(this, new Event<bool>.HandleFunction(OnDockedChanged));

            yield break;
        }
        */

        //for future use
        public void AddNewArmType(NewSeamothArm newSeamothArm)
        {
        }

        private void CheckArmSlots()
        {
            if (IsSeamothArm(seamoth.modules.GetTechTypeInSlot("SeamothArmLeft"), out TechType leftArmtechType))
            {
                AddArm(Arm.Left, leftArmtechType);
            }

            if (IsSeamothArm(seamoth.modules.GetTechTypeInSlot("SeamothArmRight"), out TechType rightArmTechType))
            {
                AddArm(Arm.Right, rightArmTechType);
            }

        }


        private bool IsSeamothArm(TechType techType, out TechType result)
        {
            if (techType == SeamothClawArmPrefab.TechTypeID)
            {
                result = SeamothClawArmPrefab.TechTypeID;
                return true;
            }
            else if (techType == SeamothDrillArmPrefab.TechTypeID)
            {
                result = SeamothDrillArmPrefab.TechTypeID;
                return true;
            }
            else if (techType == SeamothGrapplingArmPrefab.TechTypeID)
            {
                result = SeamothGrapplingArmPrefab.TechTypeID;
                return true;
            }
            else if (techType == SeamothPropulsionArmPrefab.TechTypeID)
            {
                result = SeamothPropulsionArmPrefab.TechTypeID;
                return true;
            }
            else if (techType == SeamothTorpedoArmPrefab.TechTypeID)
            {
                result = SeamothTorpedoArmPrefab.TechTypeID;
                return true;
            }

            result = TechType.None;
            return false;
        }
    }
}
