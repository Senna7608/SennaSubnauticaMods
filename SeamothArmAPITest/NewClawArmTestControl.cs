using SeamothArms.API;
using SeamothArms.API.ArmControl;
using UnityEngine;

namespace SeamothArmAPITest
{
    public class NewClawArmTestControl : ClawArmControl, ISeamothArmControl
    {        
        GameObject ISeamothArmControl.GetGameObject()
        {
            return gameObject;
        }

        void ISeamothArmControl.SetSide(SeamothArm arm)
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

        bool ISeamothArmControl.OnUseDown(out float cooldownDuration)
        {
            return TryUse(out cooldownDuration);
        }

        bool ISeamothArmControl.OnUseHeld(out float cooldownDuration)
        {
            return TryUse(out cooldownDuration);
        }

        bool ISeamothArmControl.OnUseUp(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return true;
        }

        bool ISeamothArmControl.OnAltDown()
        {
            return false;
        }

        void ISeamothArmControl.Update(ref Quaternion aimDirection)
        {
        }

        void ISeamothArmControl.Reset()
        {
        }

        private bool TryUse(out float cooldownDuration)
        {
            if (Time.time - timeUsed >= cooldownTime)
            {    
                Pickupable pickupable = null;
                PickPrefab x = null;

                GameObject activeTarget = ArmServices.main.GetActiveTarget(ThisSeamoth.gameObject);

                if (activeTarget)
                {
                    pickupable = activeTarget.GetComponent<Pickupable>();
                    x = activeTarget.GetComponent<PickPrefab>();
                }
                if (pickupable != null && pickupable.isPickupable)
                {

                    if (ArmServices.main.GetRoomForItem(ThisSeamoth.gameObject, pickupable) != null)
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
            GameObject activeTarget = ArmServices.main.GetActiveTarget(ThisSeamoth.gameObject);

            if (activeTarget)
            {
                Pickupable pickupable = activeTarget.GetComponent<Pickupable>();
                PickPrefab component = activeTarget.GetComponent<PickPrefab>();

                ItemsContainer container = ArmServices.main.GetRoomForItem(ThisSeamoth.gameObject, pickupable);

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

        bool ISeamothArmControl.HasClaw()
        {
            return true;
        }

        bool ISeamothArmControl.HasDrill()
        {
            return false;
        }

        float ISeamothArmControl.GetEnergyCost()
        {
            return energyCost;
        }

        void ISeamothArmControl.SetRotation(SeamothArm arm, bool isDocked)
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
