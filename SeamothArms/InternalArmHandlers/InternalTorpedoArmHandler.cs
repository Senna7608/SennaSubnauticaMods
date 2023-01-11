﻿using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using ModdedArmsHelper.API.ArmHandlers;

namespace SeamothArms.InternalArmHandlers
{
    internal class InternalTorpedoArmHandler : TorpedoArmHandler, ISeamothArm
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

        GameObject ISeamothArm.GetGameObject()
        {            
            return gameObject;
        }

        GameObject ISeamothArm.GetInteractableRoot(GameObject target)
        {
            return null;
        }

        void ISeamothArm.SetSide(SeamothArm arm)
        {
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }

            if (arm == SeamothArm.Right)
            {
                transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
                container = seamoth.GetStorageInSlot(seamoth.GetSlotIndex("SeamothArmRight"), armTag.techType);
            }
            else
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                container = seamoth.GetStorageInSlot(seamoth.GetSlotIndex("SeamothArmLeft"), armTag.techType);
            }

            if (container != null)
            {
                container.onAddItem += OnAddItem;
                container.onRemoveItem += OnRemoveItem;
            }

            UpdateVisuals();
        }

        bool ISeamothArm.OnUseDown(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, true);
        }

        
        bool ISeamothArm.OnUseHeld(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, false);
        }

        
        bool ISeamothArm.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("use_tool", false);
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
        
        void ISeamothArm.ResetArm()
        {
            animator.SetBool("use_tool", false);
        }
        
        private bool TryShoot(out float cooldownDuration, bool verbose)
        {            
            TorpedoType[] torpedoTypes = vehicle.torpedoTypes;
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

            animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return false;
        }
        
        private bool Shoot(TorpedoType torpedoType, Transform siloTransform, bool verbose)
        {
            if (Vehicle.TorpedoShot(container, torpedoType, siloTransform))
            {
                Utils.PlayFMODAsset(fireSound, siloTransform, 20f);
                animator.SetBool("use_tool", true);

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
            TorpedoType[] torpedoTypes = vehicle.torpedoTypes;

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
            HandReticle.main.SetText(HandReticle.TextType.Hand, "SeamothTorpedoStorage", true, GameInput.Button.LeftHand);
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false, GameInput.Button.None);
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
            Player.main.GetPDA().Open(PDATab.Inventory, useTransform, null);
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

        bool ISeamothArm.HasClaw()
        {
            return false;
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
            return 0;
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
                        transform.localRotation = Quaternion.Euler(32, 336, 310);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(32, 24, 50);
                    }
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(346, 0, 0);
                }
            }
            else
            {
                transform.localRotation = Quaternion.Euler(346, 0, 0);                
            }
        }                    
    }
}