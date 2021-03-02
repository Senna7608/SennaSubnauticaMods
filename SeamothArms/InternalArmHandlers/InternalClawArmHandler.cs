using ModdedArmsHelper.API;
using ModdedArmsHelper.API.ArmHandlers;
using ModdedArmsHelper.API.Interfaces;
using UnityEngine;

namespace SeamothArms.InternalArmHandlers
{
    internal class InternalClawArmHandler : ClawArmHandler, ISeamothArm
    {
        public override void Start()
        {
        }

        GameObject ISeamothArm.GetGameObject()
        {
            return gameObject;
        }
        
        void ISeamothArm.SetSide(SeamothArm arm)
        {
            if (arm == SeamothArm.Right)
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
                Pickupable pickupable = null;
                PickPrefab x = null;                

                GameObject activeTarget = ArmServices.main.GetActiveTarget(seamoth);

                if (activeTarget)
                {
                    pickupable = activeTarget.GetComponent<Pickupable>();
                    x = activeTarget.GetComponent<PickPrefab>();
                }
                if (pickupable != null && pickupable.isPickupable)
                {
                    
                    if (ArmServices.main.GetRoomForItem(seamoth, pickupable) != null)
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
            if (seamoth.CanPilot() && seamoth.GetPilotingMode())
            {
                Vector3 position = default(Vector3);
                GameObject targetObject = null;

                UWE.Utils.TraceFPSTargetPosition(seamoth.gameObject, 6.5f, ref targetObject, ref position, true);

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
                    VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), seamoth.gameObject.transform);
                    targetObject.SendMessage("BashHit", this, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public void OnPickup()
        {            
            GameObject activeTarget = ArmServices.main.GetActiveTarget(seamoth);

            if (activeTarget)
            {
                Pickupable pickupable = activeTarget.GetComponent<Pickupable>();
                PickPrefab component = activeTarget.GetComponent<PickPrefab>();

                ItemsContainer container = ArmServices.main.GetRoomForItem(seamoth, pickupable);

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

        bool ISeamothArm.HasPropCannon()
        {
            return false;
        }

        float ISeamothArm.GetEnergyCost()
        {
            return energyCost;
        }

        void ISeamothArm.SetRotation(SeamothArm arm, bool isDocked)
        {
            if (seamoth == null)
            {
                return;
            }

            if (isDocked)
            {                
                SubRoot subRoot = seamoth.GetComponentInParent<SubRoot>();
                
                if (subRoot.isCyclops)
                {
                    if (arm == SeamothArm.Right)
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
                    if (arm == SeamothArm.Right)
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
                if (arm == SeamothArm.Right)
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