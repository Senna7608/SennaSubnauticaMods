using UnityEngine;
using System.Collections.Generic;
using ModdedArmsHelper.API.ArmHandlers;

namespace APIBasedExosuitArms.Handlers
{
    internal class APIBasedPropulsionArmHandler : PropulsionArmHandler, IExosuitArm
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
            if (propulsionCannon.ValidateObject(target))
            {
                return target;
            }

            return null;
        }

        void IExosuitArm.SetSide(Exosuit.Arm arm)
        {
            if (arm == Exosuit.Arm.Right)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                propulsionCannon.localObjectOffset = new Vector3(0.75f, 0f, 0f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                propulsionCannon.localObjectOffset = new Vector3(-0.75f, 0f, 0f);
            }
        }
        
        bool IExosuitArm.OnUseDown(out float cooldownDuration)
        {
            usingTool = true;
            cooldownDuration = 1f;
            return propulsionCannon.OnShoot();
        }
        
        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }
        
        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            usingTool = false;
            cooldownDuration = 0f;
            return true;
        }
        
        bool IExosuitArm.OnAltDown()
        {
            if (propulsionCannon.IsGrabbingObject())
            {
                propulsionCannon.ReleaseGrabbedObject();
            }
            else if (exosuit != null && propulsionCannon.HasChargeForShot() && !propulsionCannon.OnReload(new List<IItemsContainer> { exosuit.storageContainer.container }))
            {
                ErrorMessage.AddMessage(Language.main.Get("ExosuitPropulsionCannonNoItems"));
            }

            return true;
        }
                
        void IExosuitArm.Update(ref Quaternion aimDirection)
        {
            propulsionCannon.usingCannon = usingTool;
            propulsionCannon.UpdateActive();
        }
        
        void IExosuitArm.ResetArm()
        {
            propulsionCannon.usingCannon = (usingTool = false);
            propulsionCannon.ReleaseGrabbedObject();
        }        
    }
}
