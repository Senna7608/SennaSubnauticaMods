using UnityEngine;
using UWE;

namespace SeamothArms
{
    public enum ArmTemplate
    {
        DrillArm,
        ClawArm,
        PropulsionArm,
        GrapplingArm,
        TorpedoArm        
    };

    //for future use
    public struct NewSeamothArm
    {
        public ArmTemplate armBase;
        public TechType techType;
        public Mesh newMesh;
        public Material[] newMeshMaterials;
        public ISeamothArm armControl;
    }

    public enum Arm
    {
        None,
        Left,
        Right
    }

    public partial class SeamothArmManager
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
                return currentSelectedArm == Arm.None ? false : true;
            }
        }

        public enum ObjectType
        {
            None,
            Pickupable,
            Drillable
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
            if (currentSelectedArm == Arm.Left)
            {
                return LeftArmType;
            }

            if (currentSelectedArm == Arm.Right)
            {
                return RightArmType;
            }

            return TechType.None;
        }
        

        public Event<bool> onDockedChanged = new Event<bool>();

        public GameObject GetActiveTarget()
        {
            return activeTarget;
        }

        public bool HasClaw()
        {
            bool resultLeft = false;
            bool resultRight = false;

            if (leftArm != null)
                resultLeft = leftArm.HasClaw();

            if (rightArm != null)
                resultRight = rightArm.HasClaw();

            return resultLeft || resultRight;
        }

        public bool HasDrill()
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

        public void RemoveArm(Arm arm)
        {
            armsDirty = true;

            if (arm == Arm.Left)
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

        public void AddArm(Arm arm, TechType techType)
        {
            armsDirty = true;

            if (arm == Arm.Left)
            {
                if (leftArm != null)
                {
                    Destroy(leftArm.GetGameObject());
                    leftArm = null;
                }

                leftArm = SpawnArm(techType, leftArmAttach);                
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

                rightArm = SpawnArm(techType, rightArmAttach);                
                rightArm.SetSide(Right);
                rightArm.SetRotation(Right, seamoth.docked);
                currentRightArmType = techType;
            }

            SetLightsPosition();
            vfxConstructing.Regenerate();            
            armsDirty = false;
            UpdateColliders();
        }

        public void ResetArms()
        {
            if (leftArm != null)
            {
                leftArm.Reset();
            }

            if (rightArm != null)
            {
                rightArm.Reset();
            }
        }
    }
}
