using Common;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using UnityEngine;
using UWE;

namespace ModdedArmsHelper
{
    internal sealed partial class SeamothArmManager
    {
        public TechType LeftArmType
        {
            get
            {
                return currentLeftArmType;
            }
        }

        public TechType RightArmType
        {
            get
            {
                return currentRightArmType;
            }
        }

        public ISeamothArm LeftArm
        {
            get
            {
                return leftArm;
            }
        }

        public ISeamothArm RightArm
        {
            get
            {
                return rightArm;
            }
        }

        public int LeftArmSlotID
        {
            get
            {
                return seamoth.GetSlotIndex("SeamothArmLeft");
            }
        }

        public int RightArmSlotID
        {
            get
            {
                return seamoth.GetSlotIndex("SeamothArmRight");
            }
        }       

        public bool IsArmSlotSelected
        {
            get
            {
                return currentSelectedArm == SeamothArm.None ? false : true;
            }
        }        

        public bool IsAnyArmAttached
        {
            get
            {
                return (leftArm == null && rightArm == null) ? false : true;
            }
        }

        public TechType GetSelectedArmTechType()
        {
            if (currentSelectedArm == SeamothArm.Left)
            {
                return LeftArmType;
            }

            if (currentSelectedArm == SeamothArm.Right)
            {
                return RightArmType;
            }

            return TechType.None;
        }

        public ISeamothArm GetSelectedArm()
        {
            if (currentSelectedArm == SeamothArm.Left)
            {
                return leftArm;
            }

            if (currentSelectedArm == SeamothArm.Right)
            {
                return rightArm;
            }

            return null;
        }

        public Event<bool> onDockedChanged = new Event<bool>();

        public GameObject GetActiveTarget()
        {
            return activeTarget;
        }

        internal bool HasClaw()
        {
            bool resultLeft = false;
            bool resultRight = false;

            if (leftArm != null)
                resultLeft = leftArm.HasClaw();

            if (rightArm != null)
                resultRight = rightArm.HasClaw();

            return resultLeft || resultRight;
        }

        internal bool HasDrill()
        {
            bool resultLeft = false;
            bool resultRight = false;

            if (leftArm != null)
                resultLeft = leftArm.HasDrill();

            if (rightArm != null)
                resultRight = rightArm.HasDrill();

            return resultLeft || resultRight;
        }

        public bool HasRoomForItem(Pickupable pickupable)
        {
            containers.Clear();

            seamoth.GetAllStorages(containers);

            foreach (ItemsContainer container in containers)
            {
                if (container.HasRoomFor(pickupable))
                {
                    return true;
                }
            }

            return false;
        }

        public ItemsContainer GetRoomForItem(Pickupable pickupable)
        {
            containers.Clear();

            seamoth.GetAllStorages(containers);

            foreach (ItemsContainer container in containers)
            {
                if (container.HasRoomFor(pickupable))
                {
                    return container;
                }
            }

            return null;
        }

        internal void RemoveArm(SeamothArm arm)
        {
            armsDirty = true;

            if (arm == SeamothArm.Left)
            {
                Destroy(leftArm.GetGameObject());
                leftArm = null;
                currentLeftArmType = TechType.None;                
            }
            else
            {
                Destroy(rightArm.GetGameObject());
                rightArm = null;
                currentRightArmType = TechType.None;                
            }

            SetLightsPosition();

            armsDirty = false;
        }

        internal void AddArm(SeamothArm arm, TechType techType)
        {
            SNLogger.Debug($"AddArm: arm: {arm}, techType: {techType}");

            armsDirty = true;

            if (arm == SeamothArm.Left)
            {
                if (leftArm != null)
                {
                    Destroy(leftArm.GetGameObject());
                    leftArm = null;
                }

                leftArm = Main.armsGraphics.SpawnArm(techType, leftArmAttach);                
                leftArm.SetSide(Left);
                leftArm.SetRotation(Left, seamoth.docked);
                currentLeftArmType = techType;
            }
            else
            {
                if (rightArm != null)
                {
                    Destroy(rightArm.GetGameObject());
                    rightArm = null;
                }

                rightArm = Main.armsGraphics.SpawnArm(techType, rightArmAttach);                
                rightArm.SetSide(Right);
                rightArm.SetRotation(Right, seamoth.docked);
                currentRightArmType = techType;
            }

            SetLightsPosition();
            vfxConstructing.Regenerate();            
            armsDirty = false;
            UpdateColliders();

            UpdateArmRenderers();
        }

        internal void ResetArms()
        {
            leftArm?.ResetArm();
            rightArm?.ResetArm();            
        }
    }
}
