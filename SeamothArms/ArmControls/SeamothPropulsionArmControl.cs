using Common;
using SeamothArms.API;
using UnityEngine;

namespace SeamothArms.ArmControls
{
    internal class SeamothPropulsionArmControl : MonoBehaviour, ISeamothArmControl
    {
        public SeaMoth ThisSeamoth
        {
            get
            {
                return GetComponentInParent<SeaMoth>();
            }
        }
                
        public PropulsionCannon propulsionCannon;
                
        private bool usingTool;       

        private void Awake()
        {
            ThisSeamoth.gameObject.EnsureComponent<ImmuneToPropulsioncannon>();
            propulsionCannon = GetComponent<PropulsionCannon>();
            propulsionCannon.energyInterface = ThisSeamoth.GetComponent<EnergyInterface>();
            propulsionCannon.shootForce = 60f;
            propulsionCannon.attractionForce = 145f;
            propulsionCannon.massScalingFactor = 0.005f;
            propulsionCannon.pickupDistance = 25f;
            propulsionCannon.maxMass = 1800f;
            propulsionCannon.maxAABBVolume = 400f;
        }

        
        GameObject ISeamothArmControl.GetGameObject()
        {
            return gameObject;
        }
        
        void ISeamothArmControl.SetSide(SeamothArm arm)
        {
            if (propulsionCannon == null)
            {
                propulsionCannon = GetComponent<PropulsionCannon>();
            }

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
        
        bool ISeamothArmControl.OnUseDown(out float cooldownDuration)
        {
            usingTool = true;
            cooldownDuration = 1f;
            return propulsionCannon.OnShoot();
        }
        
        bool ISeamothArmControl.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }
        
        bool ISeamothArmControl.OnUseUp(out float cooldownDuration)
        {
            usingTool = false;
            cooldownDuration = 0f;
            return true;
        }
        
        bool ISeamothArmControl.OnAltDown()
        {
            if (propulsionCannon.IsGrabbingObject())
            {
                propulsionCannon.ReleaseGrabbedObject();
            }           

            return true;
        }
                
        void ISeamothArmControl.Update(ref Quaternion aimDirection)
        {
            propulsionCannon.usingCannon = usingTool;
            propulsionCannon.UpdateActive();
        }
        
        void ISeamothArmControl.Reset()
        {
            propulsionCannon.usingCannon = (usingTool = false);
            propulsionCannon.ReleaseGrabbedObject();
        }                

        bool ISeamothArmControl.HasClaw()
        {
            return false;
        }

        bool ISeamothArmControl.HasDrill()
        {
            return false;
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
           
        float ISeamothArmControl.GetEnergyCost()
        {
            return 0;
        }
    }
}
