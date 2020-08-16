using Common;
using UnityEngine;

namespace SeamothArms.ArmControls
{
    public class SeamothClawArmControl : MonoBehaviour, ISeamothArm
    {
        public SeaMoth ThisSeamoth
        {
            get
            {
                return GetComponentInParent<SeaMoth>();
            }
        }

        public Animator animator;
        public const float kGrabDistance = 4.8f;        
        public FMODAsset hitTerrainSound;        
        public FMODAsset hitFishSound;
        public FMODAsset pickupSound;
        public Transform front;
        public VFXEventTypes vfxEventType;
        public VFXController fxControl;
        public float cooldownPunch = 1f;
        public float cooldownPickup = 1.533f;
        private const float attackDist = 5.2f;
        private const float damage = 50f;
        private const DamageType damageType = DamageType.Normal;
        private float timeUsed = float.NegativeInfinity;
        private float cooldownTime;
        private bool shownNoRoomNotification = true;  
        private const float energyCost = 0.5f;

        private void Awake()
        {                                         
            animator = GetComponent<Animator>();
            fxControl = GetComponent<VFXController>();
            vfxEventType = VFXEventTypes.impact;
            
            foreach (FMODAsset asset in GetComponents<FMODAsset>())
            {
                if (asset.name == "claw_hit_terrain")
                    hitTerrainSound = asset;

                if (asset.name == "claw_hit_fish")
                    hitFishSound = asset;

                if (asset.name == "claw_pickup")
                    pickupSound = asset;
            }


            front = Main.graphics.objectHelper.FindDeepChild(gameObject, "wrist").transform;
        }        

        GameObject ISeamothArm.GetGameObject()
        {
            return gameObject;
        }
        
        void ISeamothArm.SetSide(Arm arm)
        {
            if (arm == Arm.Right)
            {
                transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);                
            }
            else
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);                               
            }            
        }
        
        bool ISeamothArm.OnUseDown(out float cooldownDuration)
        {
            return TryUse(out cooldownDuration);
        }
        
        bool ISeamothArm.OnUseHeld(out float cooldownDuration)
        {
            return TryUse(out cooldownDuration);
        }
        
        bool ISeamothArm.OnUseUp(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return true;
        }
        
        bool ISeamothArm.OnAltDown()
        {
            return false;
        }

        void ISeamothArm.Update(ref Quaternion aimDirection)
        {
        }
        
        void ISeamothArm.Reset()
        {
        }
        
        private bool TryUse(out float cooldownDuration)
        {
            if (Time.time - timeUsed >= cooldownTime)
            {
                SeamothArmManager control = ThisSeamoth.GetComponent<SeamothArmManager>();

                Pickupable pickupable = null;
                PickPrefab x = null;

                GameObject activeTarget = control.GetActiveTarget();

                if (activeTarget)
                {
                    pickupable = activeTarget.GetComponent<Pickupable>();
                    x = activeTarget.GetComponent<PickPrefab>();
                }
                if (pickupable != null && pickupable.isPickupable)
                {
                    
                    if (control.GetRoomForItem(pickupable) != null)
                    {
                        animator.SetTrigger("use_tool");
                        cooldownTime = (cooldownDuration = cooldownPickup);
                        shownNoRoomNotification = false;
                        return true;
                    }
                    if (!shownNoRoomNotification)
                    {
                        ErrorMessage.AddMessage(Language.main.Get("ContainerCantFit"));
                        shownNoRoomNotification = true;
                    }
                    
                }
                else
                {
                    if (x != null)
                    {
                        animator.SetTrigger("use_tool");
                        cooldownTime = (cooldownDuration = cooldownPickup);
                        return true;
                    }
                    animator.SetTrigger("bash");
                    cooldownTime = (cooldownDuration = cooldownPunch);
                    fxControl.Play(0);
                    return true;
                }
            }
            cooldownDuration = 0f;
            return false;
        }



        public void OnHit()
        {            
            if (ThisSeamoth.CanPilot() && ThisSeamoth.GetPilotingMode())
            {
                Vector3 position = default(Vector3);
                GameObject targetObject = null;

                UWE.Utils.TraceFPSTargetPosition(ThisSeamoth.gameObject, 6.5f, ref targetObject, ref position, true);

                if (targetObject == null)
                {
                    InteractionVolumeUser component = Player.main.gameObject.GetComponent<InteractionVolumeUser>();
                    if (component != null && component.GetMostRecent() != null)
                    {
                        targetObject = component.GetMostRecent().gameObject;
                    }
                }
                if (targetObject)
                {
                    LiveMixin liveMixin = targetObject.FindAncestor<LiveMixin>();
                    if (liveMixin)
                    {
                        bool flag = liveMixin.IsAlive();
                        liveMixin.TakeDamage(50f, position, DamageType.Normal, null);
                        Utils.PlayFMODAsset(hitFishSound, front, 50f);
                    }
                    else
                    {
                        Utils.PlayFMODAsset(hitTerrainSound, front, 50f);
                    }
                    VFXSurface component2 = targetObject.GetComponent<VFXSurface>();
                    Vector3 euler = MainCameraControl.main.transform.eulerAngles + new Vector3(300f, 90f, 0f);
                    VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), ThisSeamoth.gameObject.transform);
                    targetObject.SendMessage("BashHit", this, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public void OnPickup()
        {
            SeamothArmManager control = ThisSeamoth.GetComponent<SeamothArmManager>();
            GameObject activeTarget = control.GetActiveTarget();

            if (activeTarget)
            {
                Pickupable pickupable = activeTarget.GetComponent<Pickupable>();
                PickPrefab component = activeTarget.GetComponent<PickPrefab>();

                ItemsContainer container = control.GetRoomForItem(pickupable);

                if (pickupable != null && pickupable.isPickupable && container != null)
                {
                    pickupable = pickupable.Initialize();
                    InventoryItem item = new InventoryItem(pickupable);
                    container.UnsafeAdd(item);
                    Utils.PlayFMODAsset(pickupSound, front, 5f);
                }
                else if (component != null && component.AddToContainer(container))
                {
                    component.SetPickedUp();
                }
            }
        }
                
        bool ISeamothArm.HasClaw()
        {
            return true;
        }

        bool ISeamothArm.HasDrill()
        {
            return false;
        }

        float ISeamothArm.GetEnergyCost()
        {
            return energyCost;
        }

        void ISeamothArm.SetRotation(Arm arm, bool isDocked)
        {
            if (ThisSeamoth == null)
            {
                return;
            }

            if (isDocked)
            {                
                SubRoot subRoot = ThisSeamoth.GetComponentInParent<SubRoot>();
                
                if (subRoot.isCyclops)
                {
                    if (arm == Arm.Right)
                    {
                        transform.localRotation = Quaternion.Euler(37, 327, 300);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(37, 33, 60);
                    }
                }
                else
                {
                    if (arm == Arm.Right)
                    {
                        transform.localRotation = Quaternion.Euler(20, 352, 20);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(20, 8, 340);
                    }
                }
            }
            else                
            {
                if (arm == Arm.Right)
                {
                    transform.localRotation = Quaternion.Euler(355, 343, 19);
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(355, 17, 341);
                }
            }
        }
    }

}