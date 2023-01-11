using ModdedArmsHelper.API;
using ModdedArmsHelper.API.ArmHandlers;
using System.Collections;
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

        GameObject IExosuitArm.GetInteractableRoot(GameObject target)
        {
            Pickupable componentInParent = target.GetComponentInParent<Pickupable>();

            if (componentInParent != null && componentInParent.isPickupable)
            {
                return componentInParent.gameObject;
            }

            PickPrefab componentProfiled = target.GetComponentProfiled<PickPrefab>();

            if (componentProfiled != null)
            {
                return componentProfiled.gameObject;
            }

            BreakableResource componentInParent2 = target.GetComponentInParent<BreakableResource>();

            if (componentInParent2 != null)
            {
                return componentInParent2.gameObject;
            }

            return null;
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

        void IExosuitArm.ResetArm()
        {
        }

        private bool TryUse(out float cooldownDuration)
        {
            if (Time.time - timeUsed >= cooldownTime)
            {    
                Pickupable pickupable = null;
                PickPrefab x = null;

                GameObject activeTarget = exosuit.GetActiveTarget();

                if (activeTarget)
                {
                    pickupable = activeTarget.GetComponent<Pickupable>();
                    x = activeTarget.GetComponent<PickPrefab>();
                }
                if (pickupable != null && pickupable.isPickupable)
                {

                    if (exosuit.storageContainer.container.HasRoomFor(pickupable))
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
            if (exosuit.CanPilot() && exosuit.GetPilotingMode())
            {
                Vector3 position = default(Vector3);
                GameObject targetObject = null;

                UWE.Utils.TraceFPSTargetPosition(exosuit.gameObject, 6.5f, ref targetObject, ref position, true);

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
                    VFXSurfaceTypeManager.main.Play(component2, vfxEventType, position, Quaternion.Euler(euler), exosuit.gameObject.transform);
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
                PickPrefab pickprefab = activeTarget.GetComponent<PickPrefab>();

                StartCoroutine(OnPickupAsync(pickupable, pickprefab));
            }
        }

        private IEnumerator OnPickupAsync(Pickupable pickupable, PickPrefab pickPrefab)
        {
            if (pickupable != null && pickupable.isPickupable && exosuit.storageContainer.container.HasRoomFor(pickupable))
            {
                pickupable.Initialize();

                InventoryItem item = new InventoryItem(pickupable);

                exosuit.storageContainer.container.UnsafeAdd(item);

                Utils.PlayFMODAsset(this.pickupSound, this.front, 5f);
            }
            else if (pickPrefab != null)
            {
                TaskResult<bool> result = new TaskResult<bool>();

                yield return pickPrefab.AddToContainerAsync(exosuit.storageContainer.container, result);

                if (pickPrefab != null && result.Get())
                {
                    pickPrefab.SetPickedUp();
                }
            }

            yield break;
        }
    }
}
