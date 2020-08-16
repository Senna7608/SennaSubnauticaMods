using System.Collections.Generic;
using UnityEngine;

namespace RepulsionCannonArm
{
    public class RepulsionCannonArmControl : MonoBehaviour, IExosuitArm
    {
        public Exosuit exosuit;
        private bool callBubblesFX;

        public VFXController fxControl;
        public GameObject bubblesFX;
        public FMODAsset shootSound;
        public Animator animator;
        public Transform muzzle;

        private List<IPropulsionCannonAmmo> iammo = new List<IPropulsionCannonAmmo>();

        private void Start()
        {
            exosuit = GetComponentInParent<Exosuit>();
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
            OnShoot();
            animator.SetBool("cangrab_propulsioncannon", true);
            cooldownDuration = 1f;
            return true;
        }
        
        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }
        
        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("cangrab_propulsioncannon", false);
            cooldownDuration = 0f;
            return true;
        }
                
        bool IExosuitArm.OnAltDown()
        {            
            return false;
        }
        
        void IExosuitArm.Update(ref Quaternion aimDirection)
        {            
            if (callBubblesFX)
            {                
                Utils.PlayOneShotPS(bubblesFX, muzzle.position, muzzle.rotation, null);
                callBubblesFX = false;
            }            
        }
        
        void IExosuitArm.Reset()
        {
            animator.SetBool("cangrab_propulsioncannon", false);
        }

        public void OnShoot()
        {
            EnergyInterface energyInterface = exosuit.GetComponent<EnergyInterface>();

            energyInterface.GetValues(out float charge, out float capacity);

            if (charge > 0f)
            {
                float d = Mathf.Clamp01(charge / 4f);

                Vector3 forward = MainCamera.camera.transform.forward;
                Vector3 position = MainCamera.camera.transform.position;

                int hits = UWE.Utils.SpherecastIntoSharedBuffer(position, 1f, forward, 35f, ~(1 << LayerMask.NameToLayer("Player")), QueryTriggerInteraction.UseGlobal);

                float targetMass = 0f;

                for (int i = 0; i < hits; i++)
                {
                    RaycastHit raycastHit = UWE.Utils.sharedHitBuffer[i];

                    Vector3 point = raycastHit.point;

                    float magnitude = (position - point).magnitude;

                    float d2 = 1f - Mathf.Clamp01((magnitude - 1f) / 35f);

                    GameObject targetObject = UWE.Utils.GetEntityRoot(raycastHit.collider.gameObject);

                    if (targetObject == null)
                    {
                        targetObject = raycastHit.collider.gameObject;
                    }

                    Rigidbody component = targetObject.GetComponent<Rigidbody>();

                    if (component != null)
                    {
                        targetMass += component.mass;

                        bool flag = true;                        

                        targetObject.GetComponents(iammo);

                        for (int j = 0; j < iammo.Count; j++)
                        {
                            if (!iammo[j].GetAllowedToShoot())
                            {
                                flag = false;
                                break;
                            }
                        }

                        iammo.Clear();

                        if (flag && !(raycastHit.collider is MeshCollider) && (targetObject.GetComponent<Pickupable>() != null || targetObject.GetComponent<Living>() != null || (component.mass <= 2600f && UWE.Utils.GetAABBVolume(targetObject) <= 800f)))
                        {
                            float d3 = 1f + component.mass * 0.005f;
                            Vector3 velocity = forward * d2 * d * 140f / d3;
                            ShootObject(component, velocity);
                        }
                    }
                }

                energyInterface.ConsumeEnergy(4f);

                fxControl.Play();

                callBubblesFX = true;
                
                Utils.PlayFMODAsset(shootSound, transform, 20f);                
            }
        }

        private void ShootObject(Rigidbody rb, Vector3 velocity)
        {
            rb.isKinematic = false;

            rb.velocity = velocity;

            PropulseCannonAmmoHandler propulseCannonAmmoHandler = rb.gameObject.EnsureComponent<PropulseCannonAmmoHandler>();            

            propulseCannonAmmoHandler.ResetHandler(false, false);

            propulseCannonAmmoHandler.OnShot(false);
        }
    }
}

