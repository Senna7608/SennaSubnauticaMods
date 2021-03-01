using UnityEngine;
using ModdedArmsHelper.API.ArmHandlers;
using ModdedArmsHelper.API;

namespace APIBasedExosuitArms.Handlers
{
    internal class APIBasedTorpedoArmHandler : TorpedoArmHandler, IExosuitArm
    {
        public override void Awake()
        {
            base.Awake();

            handTarget.onHandHover.AddListener(OnHoverTorpedoStorage);
            handTarget.onHandClick.AddListener(OnOpenTorpedoStorage);
        }

        public override void Start()
        {
        }

        GameObject IExosuitArm.GetGameObject()
        {            
            return gameObject;
        }        

        void IExosuitArm.SetSide(Exosuit.Arm arm)
        {
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }

            if (arm == Exosuit.Arm.Right)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                container = Exosuit.GetStorageInSlot(Exosuit.GetSlotIndex("ExosuitArmRight"), ArmTag.techType);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                container = Exosuit.GetStorageInSlot(Exosuit.GetSlotIndex("ExosuitArmLeft"), ArmTag.techType);
            }

            if (container != null)
            {
                container.onAddItem += OnAddItem;
                container.onRemoveItem += OnRemoveItem;
            }

            UpdateVisuals();
        }

        bool IExosuitArm.OnUseDown(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, true);
        }
        
        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, false);
        }
        
        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            Animator.SetBool("use_tool", false);
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
            Animator.SetBool("use_tool", false);
        }
        
        private bool TryShoot(out float cooldownDuration, bool verbose)
        {            
            TorpedoType[] torpedoTypes = Vehicle.torpedoTypes;
            TorpedoType torpedoType = null;

            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                if (container.Contains(torpedoTypes[i].techType))
                {
                    torpedoType = torpedoTypes[i];
                    break;
                }
            }

            float num = Mathf.Clamp(Time.time - timeFirstShot, 0f, 5f);
            float num2 = Mathf.Clamp(Time.time - timeSecondShot, 0f, 5f);
            float b = 5f - num;
            float b2 = 5f - num2;
            float num3 = Mathf.Min(num, num2);

            if (num3 < 1f)
            {
                cooldownDuration = 0f;
                return false;
            }
            if (num >= 5f)
            {
                if (Shoot(torpedoType, siloFirst, verbose))
                {
                    timeFirstShot = Time.time;
                    cooldownDuration = Mathf.Max(1f, b2);
                    return true;
                }
            }
            else
            {
                if (num2 < 5f)
                {
                    cooldownDuration = 0f;
                    return false;
                }
                if (Shoot(torpedoType, siloSecond, verbose))
                {
                    timeSecondShot = Time.time;
                    cooldownDuration = Mathf.Max(1f, b);
                    return true;
                }
            }

            Animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return false;
        }
        
        private bool Shoot(TorpedoType torpedoType, Transform siloTransform, bool verbose)
        {
            if (Vehicle.TorpedoShot(container, torpedoType, siloTransform))
            {
                Utils.PlayFMODAsset(fireSound, siloTransform, 20f);
                Animator.SetBool("use_tool", true);

                if (container.count == 0)
                {
                    Utils.PlayFMODAsset(torpedoDisarmed, transform, 1f);
                }
                return true;
            }
            if (verbose)
            {
                ErrorMessage.AddError(Language.main.Get("VehicleTorpedoNoAmmo"));
            }
            return false;
        }
        
        private void OnAddItem(InventoryItem item)
        {
            UpdateVisuals();
        }
        
        private void OnRemoveItem(InventoryItem item)
        {
            UpdateVisuals();
        }
        
        private void UpdateVisuals()
        {            
            int num = 0;
            TorpedoType[] torpedoTypes = Vehicle.torpedoTypes;

            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                num += container.GetCount(torpedoTypes[i].techType);
            }

            visualTorpedoReload.SetActive(num >= 3);
            visualTorpedoSecond.SetActive(num >= 2);
            visualTorpedoFirst.SetActive(num >= 1);
        }
        
        public void OnHoverTorpedoStorage(HandTargetEventData eventData)
        {            
            if (container == null)
            {
                return;
            }

            HandReticle.main.SetInteractText("SeamothTorpedoStorage");
            HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
        }
        
        public void OnOpenTorpedoStorage(HandTargetEventData eventData)
        {
            OpenTorpedoStorageExternal(eventData.transform);
        }
        
        public void OpenTorpedoStorageExternal(Transform useTransform)
        {
            if (container == null)
            {
                return;
            }

            Inventory.main.SetUsedStorage(container, false);
            Player.main.GetPDA().Open(PDATab.Inventory, useTransform, null, -1f);
        }
        
        private void OnDestroy()
        {
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }

            handTarget.onHandHover.RemoveListener(OnHoverTorpedoStorage);
            handTarget.onHandClick.RemoveListener(OnOpenTorpedoStorage);
        }             
    }
}