using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.ArmHandlers;
using ModdedArmsHelper.API.Interfaces;

namespace SeamothArms.InternalArmHandlers
{
    internal class InternalPropulsionArmHandler : PropulsionArmHandler, ISeamothArm
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
                propulsionCannon.localObjectOffset = new Vector3(0.75f, 0f, 0f);
            }
            else
            {
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                propulsionCannon.localObjectOffset = new Vector3(-0.75f, 0f, 0f);
            }
        }
        
        bool ISeamothArm.OnUseDown(out float cooldownDuration)
        {
            usingTool = true;
            cooldownDuration = 1f;
            return propulsionCannon.OnShoot();
        }
        
        bool ISeamothArm.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }
        
        bool ISeamothArm.OnUseUp(out float cooldownDuration)
        {
            usingTool = false;
            cooldownDuration = 0f;
            return true;
        }
        
        bool ISeamothArm.OnAltDown()
        {
            if (propulsionCannon.IsGrabbingObject())
            {
                propulsionCannon.ReleaseGrabbedObject();
            }

            /*
            else if (Seamoth != null && propulsionCannon.HasChargeForShot())
            {
                List<IItemsContainer> containers = new List<IItemsContainer>();
                Seamoth.GetAllStorages(containers);

                if (containers.Count < 1)
                {
                    ErrorMessage.AddMessage(Language.main.Get("SeamothPropulsionCannonNoContainers"));
                }
                else if (!propulsionCannon.OnReload(containers))
                {
                    ErrorMessage.AddMessage(Language.main.Get("SeamothPropulsionCannonNoItems"));
                }
                
            }
            */

            return true;
        }
                
        void ISeamothArm.Update(ref Quaternion aimDirection)
        {
            propulsionCannon.usingCannon = usingTool;
            propulsionCannon.UpdateActive();
        }
        
        void ISeamothArm.Reset()
        {
            propulsionCannon.usingCannon = (usingTool = false);
            propulsionCannon.ReleaseGrabbedObject();
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
            return true;
        }

        void ISeamothArm.SetRotation(SeamothArm arm, bool isDocked)
        {
            if (Seamoth == null)
            {
                return;
            }

            if (isDocked)
            {
                SubRoot subRoot = Seamoth.GetComponentInParent<SubRoot>();

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
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
           
        float ISeamothArm.GetEnergyCost()
        {
            return 0;
        }
    }
}
