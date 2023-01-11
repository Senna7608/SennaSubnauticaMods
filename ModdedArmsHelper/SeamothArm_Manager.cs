using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common;
using Common.Helpers;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;

namespace ModdedArmsHelper
{
    internal sealed partial class SeamothArmManager : MonoBehaviour
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

        private const SeamothArm Left = SeamothArm.Left;
        private const SeamothArm Right = SeamothArm.Right;
        private SeamothArm currentSelectedArm;

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
        
        private readonly List<GameObject> GeoCache = new List<GameObject>();
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

        private ObjectHelper objectHelper;

        private void Awake()
        {
            seamoth = GetComponent<SeaMoth>();

            objectHelper = ArmServices.main.objectHelper;

            vfxConstructing = GetComponent<VFXConstructing>();

            light_left = objectHelper.FindDeepChild(gameObject, "light_left").transform;
            light_left_default = new PureTransform(light_left.localPosition, light_left.localRotation, light_left.localScale);
            light_left_UP = new PureTransform(new Vector3(0f, 0.68f, 1f), Quaternion.Euler(0, 0, 0), light_left.localScale);

            light_right = objectHelper.FindDeepChild(gameObject, "light_right").transform;
            light_right_default = new PureTransform(light_right.localPosition, light_right.localRotation, light_right.localScale);
            light_right_DOWN = new PureTransform(new Vector3(0f, -1.21f, 1.04f), Quaternion.Euler(35, 0, 0), light_left.localScale);
            
            GameObject leftArmAttachPoint = objectHelper.CreateGameObject("leftArmAttachPoint", seamoth.torpedoTubeLeft, new Vector3(-0.93f, 0f, -0.74f), new Vector3(346, 23, 0));
            leftArmAttach = leftArmAttachPoint.transform;

            GameObject rightArmAttachPoint = objectHelper.CreateGameObject("rightArmAttachPoint", seamoth.torpedoTubeRight, new Vector3(0.93f, 0f, -0.74f), new Vector3(346, 337, 360));
            rightArmAttach = rightArmAttachPoint.transform;

            GameObject leftAimForward = objectHelper.CreateGameObject("leftAimForward", seamoth.transform);
            aimTargetLeft = leftAimForward.transform;

            GameObject rightAimForward = objectHelper.CreateGameObject("rightAimForward", seamoth.transform);
            aimTargetRight = rightAimForward.transform;                    
        }
        
        public void WakeUp()
        {
            SNLogger.Log("Received SlotExtender 'WakeUp' message.");            

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
            SNLogger.Debug($"OnEquip: slot: {slot}, techType: {item.techType}");

            if (IsSeamothArm(item.techType, out TechType techType))
            {
                int slotID = seamoth.GetSlotIndex(slot);

                if (slotID == LeftArmSlotID)
                {
                    AddArm(SeamothArm.Left, techType);
                    return;
                }
                else if (slotID == RightArmSlotID)
                {
                    AddArm(SeamothArm.Right, techType);
                    return;
                }
            }
        }

        private void OnUnequip(string slot, InventoryItem item)
        {
            SNLogger.Debug($"OnUnequip: slot: {slot}, techType: {item.techType}");

            if (IsSeamothArm(item.item.GetTechType(), out TechType techType))
            {
                int slotID = seamoth.GetSlotIndex(slot);

                if (slotID == LeftArmSlotID)
                {
                    RemoveArm(SeamothArm.Left);
                    return;
                }
                else if (slotID == RightArmSlotID)
                {
                    RemoveArm(SeamothArm.Right);
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
                    currentSelectedArm = SeamothArm.Left;
                }
                else if (slotID == RightArmSlotID)
                {
                    currentSelectedArm = SeamothArm.Right;
                }
            }
            else
            {
                currentSelectedArm = SeamothArm.None;
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

#if TESTOUTSIDE

            pilotingMode = true;
            flag2 = true;

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentSelectedArm != SeamothArm.Left)
                {
                    seamoth.SlotKeyDown(LeftArmSlotID);
                }
                
                seamoth.SendMessage("SlotLeftDown");
            }
                        
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentSelectedArm != SeamothArm.Right)
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

                    if (currentSelectedArm == SeamothArm.Left && leftArm != null)
                    {
                        leftArm.Update(ref quaternion);
                        hasPropCannon = leftArm.HasPropCannon();
                    }
                    else if (currentSelectedArm == SeamothArm.Right && rightArm != null)
                    {
                        rightArm.Update(ref rotation);
                        hasPropCannon = rightArm.HasPropCannon();
                    }                     

                    if (hasPropCannon)
                    {
                        UpdateUIText(hasPropCannon);
                    }

                    if (GameInput.GetButtonDown(GameInput.Button.AltTool))
                    {
                        if (currentSelectedArm == SeamothArm.Left && leftArm != null)
                        {
                            leftArm.OnAltDown();                            
                        }
                        else if (currentSelectedArm == SeamothArm.Right && rightArm != null)
                        {
                            rightArm.OnAltDown();
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
                    sb.AppendLine(LanguageCache.GetButtonFormat("PropulsionCannonToRelease", GameInput.Button.AltTool));
                }
                
                lastHasPropCannon = hasPropCannon;
                uiStringPrimary = sb.ToString();

                lastHasPropCannon = hasPropCannon;
                uiStringPrimary = sb.ToString();
            }

            HandReticle.main.SetTextRaw(HandReticle.TextType.Use, uiStringPrimary);
            HandReticle.main.SetTextRaw(HandReticle.TextType.UseSubscript, string.Empty);
            hasInitStrings = true;
        }

        private void OnDockedChanged(bool isDocked)
        {
            UpdateColliders();

            if (leftArm != null)
            {
                leftArm.SetRotation(SeamothArm.Left, isDocked);
            }

            if (rightArm != null)
            {
                rightArm.SetRotation(SeamothArm.Right, isDocked);
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

            TargetObjectType objectType = TargetObjectType.None;

            if (canPickup || canDrill)
            {
                Targeting.GetTarget(seamoth.gameObject, 4.8f, out targetObject, out float num);
            }

            if (targetObject)
            {
                GameObject rootObject = UWE.Utils.GetEntityRoot(targetObject);

                rootObject = (!(rootObject != null)) ? targetObject : rootObject;

                if (rootObject.GetComponentProfiled<Pickupable>())
                {
                    targetObject = rootObject;
                    objectType = TargetObjectType.Pickupable;
                }
                else if (rootObject.GetComponentProfiled<Drillable>())
                {
                    targetObject = rootObject;
                    objectType = TargetObjectType.Drillable;
                }
                else
                {
                    targetObject = null;
                }
            }

            activeTarget = targetObject;

            if (activeTarget && currentSelectedArm != SeamothArm.None)
            {
                if (canDrill && objectType == TargetObjectType.Drillable && GetSelectedArm().HasDrill())
                {
                    GUIHand component = Player.main.GetComponent<GUIHand>();
                    GUIHand.Send(activeTarget, HandTargetEventType.Hover, component);

                }
                else if (canPickup && objectType == TargetObjectType.Pickupable && GetSelectedArm().HasClaw())
                {
                    Pickupable pickupable = activeTarget.GetComponent<Pickupable>();
                    TechType techType = pickupable.GetTechType();                    
                    
                    HandReticle.main.SetText(HandReticle.TextType.Hand, LanguageCache.GetPickupText(techType), false);
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

        private void GetArmModels()
        {
            GeoCache.Clear();

            if (leftArm != null)
            {
                GameObject leftModel = objectHelper.FindDeepChild(leftArm.GetGameObject(), "ArmModel");

                GeoCache.Add(leftModel);
            }

            if (rightArm != null)
            {
                GameObject rightModel = objectHelper.FindDeepChild(rightArm.GetGameObject(), "ArmModel");

                GeoCache.Add(rightModel);
            }
        }
        
        private void UpdateArmRenderers()
        {
            GetArmModels();

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

        private void CheckArmSlots()
        {
            if (IsSeamothArm(seamoth.modules.GetTechTypeInSlot("SeamothArmLeft"), out TechType leftArmtechType))
            {
                AddArm(SeamothArm.Left, leftArmtechType);
            }

            if (IsSeamothArm(seamoth.modules.GetTechTypeInSlot("SeamothArmRight"), out TechType rightArmTechType))
            {
                AddArm(SeamothArm.Right, rightArmTechType);
            }
        }

        private bool IsSeamothArm(TechType techType, out TechType result)
        {
            SNLogger.Debug($"IsSeamothArm: techType: {techType}");

            if (Main.armsGraphics.ModdedArmPrefabs.TryGetValue(techType, out GameObject prefab))
            {
                if (prefab.GetComponent<ArmTag>().armType == ArmType.SeamothArm)
                {
                    result = techType;
                    return true;
                }
            }            

            result = TechType.None;
            return false;
        }
    }
}
