using ModdedArmsHelper.API.ArmHandlers;
using UnityEngine;

namespace APIBasedExosuitArms.Handlers
{
    public class APIBasedClawArmHandler : ClawArmHandler, IExosuitArm
    {
        public override void Start()
        {            
        }

        GameObject IExosuitArm.GetGameObject()
        {            
            return gameObject;
        }

        void IExosuitArm.SetSide(Exosuit.Arm arm)
        {
            if (arm == Exosuit.Arm.Right)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        bool IExosuitArm.OnUseDown(out float cooldownDuration)
        {
            return TryUse(out cooldownDuration);
        }

        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            return TryUse(out cooldownDuration);
        }

        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return true;
        }

        bool IExosuitArm.OnAltDown()
        {
            return false;
        }

        void IExosuitArm.Update(ref Quaternion aimDirection)
        {
        }

        void IExosuitArm.Reset()
        {
        }

        private bool TryUse(out float cooldownDuration)
        {
            if (Time.time - timeUsed >= cooldownTime)
            {    
                Pickupable pickupable = null;
                PickPrefab x = null;

                GameObject activeTarget = Exosuit.GetActiveTarget();

                if (activeTarget)
                {
                    pickupable = activeTarget.GetComponent<Pickupable>();
                    x = activeTarget.GetComponent<PickPrefab>();
                }
                if (pickupable != null && pickupable.isPickupable)
                {

                    if (Exosuit.storageContainer.container.HasRoomFor(pickupable))
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

        public void OnPickup()
        {
            GameObject activeTarget = Exosuit.GetActiveTarget();

            if (activeTarget)
            {
                Pickupable pickupable = activeTarget.GetComponent<Pickupable>();
                PickPrefab component = activeTarget.GetComponent<PickPrefab>();

                bool hasRoomForItem = Exosuit.storageContainer.container.HasRoomFor(pickupable);

                if (pickupable != null && pickupable.isPickupable && hasRoomForItem)
                {
                    pickupable = pickupable.Initialize();
                    InventoryItem item = new InventoryItem(pickupable);
                    Exosuit.storageContainer.container.UnsafeAdd(item);
                    Utils.PlayFMODAsset(pickupSound, front, 5f);
                }
                else if (component != null && component.AddToContainer(Exosuit.storageContainer.container))
                {
                    component.SetPickedUp();
                }
            }
        }

        public void OnHit()
        {
            if (Exosuit.CanPilot() && Exosuit.GetPilotingMode())
            {
                Vector3 position = default(Vector3);
                GameObject targetObject = null;

                UWE.Utils.TraceFPSTargetPosition(Exosuit.gameObject, 6.5f, ref targetObject, ref position, true);

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
                    VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), Exosuit.gameObject.transform);
                    targetObject.SendMessage("BashHit", this, SendMessageOptions.DontRequireReceiver);
                }
            }
        }        
    }
}
