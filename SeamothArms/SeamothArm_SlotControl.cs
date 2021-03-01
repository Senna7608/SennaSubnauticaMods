using SeamothArms.API;
using UnityEngine;

namespace SeamothArms
{
    internal partial class SeamothArmManager
    {
        private bool leftButtonDownProcessed;
        private bool rightButtonDownProcessed;
        private float[] quickSlotTimeUsed;
        private float[] quickSlotCooldown;
        protected bool[] quickSlotToggled;
        protected float[] quickSlotCharge;

        public void SlotArmDown()
        {
#if DEBUG
#else
            if (!Player.main.inSeamoth || !AvatarInputHandler.main.IsEnabled())
            {
                return;
            }
#endif           
            if (!seamoth.IsPowered())
            {
                return;
            }
            
            if (currentSelectedArm == SeamothArm.Left)
            {
                leftButtonDownProcessed = true;

                if (seamoth.GetSlotProgress(LeftArmSlotID) != 1f)
                {
                    return;
                }

                QuickSlotType quickSlotType = CraftData.GetQuickSlotType(currentLeftArmType);

                if (quickSlotType == QuickSlotType.Selectable && leftArm.OnUseDown(out float coolDown))
                {
                    if (EnergySource)
                    {
                        EnergySource.ConsumeEnergy(leftArm.GetEnergyCost());
                    }

                    quickSlotTimeUsed[LeftArmSlotID] = Time.time;
                    quickSlotCooldown[LeftArmSlotID] = coolDown;
                }
            }
            else if (currentSelectedArm == SeamothArm.Right)
            {
                rightButtonDownProcessed = true;

                if (seamoth.GetSlotProgress(RightArmSlotID) != 1f)
                {
                    return;
                }

                QuickSlotType quickSlotType = CraftData.GetQuickSlotType(currentRightArmType);

                if (quickSlotType == QuickSlotType.Selectable && rightArm.OnUseDown(out float coolDown))
                {
                    if (EnergySource)
                    {
                        EnergySource.ConsumeEnergy(rightArm.GetEnergyCost());
                    }

                    quickSlotTimeUsed[RightArmSlotID] = Time.time;
                    quickSlotCooldown[RightArmSlotID] = coolDown;
                }
            }
            
        }
                
        public void SlotArmHeld()
        {
#if OUTSIDE_TEST_ENABLED
#else
            if (!Player.main.inSeamoth || !AvatarInputHandler.main.IsEnabled())
            {
                return;
            }
#endif                                               
            if (!seamoth.IsPowered())
            {
                return;
            }

            if (currentSelectedArm == SeamothArm.Left)
            {
                if (!leftButtonDownProcessed)
                {
                    return;
                }

                if (seamoth.GetSlotProgress(LeftArmSlotID) != 1f)
                {
                    return;
                }

                QuickSlotType quickSlotType = CraftData.GetQuickSlotType(currentLeftArmType);

                if (quickSlotType == QuickSlotType.Selectable && leftArm.OnUseHeld(out float coolDown))
                {
                    quickSlotTimeUsed[LeftArmSlotID] = Time.time;
                    quickSlotCooldown[LeftArmSlotID] = coolDown;
                }
            }
            else if (currentSelectedArm == SeamothArm.Right)
            {
                if (!rightButtonDownProcessed)
                {
                    return;
                }

                if (seamoth.GetSlotProgress(RightArmSlotID) != 1f)
                {
                    return;
                }

                QuickSlotType quickSlotType = CraftData.GetQuickSlotType(currentRightArmType);

                if (quickSlotType == QuickSlotType.Selectable && rightArm.OnUseHeld(out float coolDown))
                {
                    quickSlotTimeUsed[RightArmSlotID] = Time.time;
                    quickSlotCooldown[RightArmSlotID] = coolDown;
                }
            }
        }
                

        public void SlotArmUp()
        {

#if OUTSIDE_TEST_ENABLED
#else
            if (!Player.main.inSeamoth || !AvatarInputHandler.main.IsEnabled())
            {
                return;
            }
#endif
            if (currentSelectedArm == SeamothArm.Left)
            {
                leftButtonDownProcessed = false;
                
                QuickSlotType quickSlotType = CraftData.GetQuickSlotType(currentLeftArmType);

                if (quickSlotType == QuickSlotType.Selectable)
                {
                    leftArm.OnUseUp(out float coolDown);
                }
                else if (quickSlotType == QuickSlotType.SelectableChargeable)
                {
                    if (!seamoth.IsPowered())
                    {
                        return;
                    }

                    if (seamoth.GetSlotProgress(LeftArmSlotID) != 1f)
                    {
                        return;
                    }

                    if (leftArm.OnUseUp(out float coolDown))
                    {
                        quickSlotTimeUsed[LeftArmSlotID] = Time.time;
                        quickSlotCooldown[LeftArmSlotID] = coolDown;
                    }

                    quickSlotCharge[LeftArmSlotID] = 0f;
                }
            }
            else if (currentSelectedArm == SeamothArm.Right)
            {
                rightButtonDownProcessed = false;
                
                QuickSlotType quickSlotType = CraftData.GetQuickSlotType(currentRightArmType);

                if (quickSlotType == QuickSlotType.Selectable)
                {
                    rightArm.OnUseUp(out float coolDown);
                }
                else if (quickSlotType == QuickSlotType.SelectableChargeable)
                {
                    if (!seamoth.IsPowered())
                    {
                        return;
                    }

                    if (seamoth.GetSlotProgress(RightArmSlotID) != 1f)
                    {
                        return;
                    }

                    if (rightArm.OnUseUp(out float coolDown))
                    {
                        quickSlotTimeUsed[RightArmSlotID] = Time.time;
                        quickSlotCooldown[RightArmSlotID] = coolDown;
                    }

                    quickSlotCharge[LeftArmSlotID] = 0f;
                }
            }
        }        
    }
}
