using UnityEngine;
using ModdedArmsHelper.API.ArmHandlers;

namespace APIBasedExosuitArms.Handlers
{
    internal class APIBasedGrapplingArmHandler : ExosuitGrapplingArmHandler, IExosuitArm
    {
        GameObject IExosuitArm.GetGameObject()
        {
            return gameObject;
        }

        GameObject IExosuitArm.GetInteractableRoot(GameObject target)
        {
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
            animator.SetBool("use_tool", true);

            if (!rope.isLaunching)
            {
                rope.LaunchHook(maxDistance);
            }

            cooldownDuration = 2f;

            return true;
        }
        
        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }
        
        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("use_tool", false);
            ResetHook();
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
            animator.SetBool("use_tool", false);
            ResetHook();
        }
        
        private void OnDestroy()
        {            
            if (hook)
            {                
                Destroy(hook.gameObject);
            }
            if (rope != null)
            {                
                Destroy(rope.gameObject);                
            }
            
        }
        
        private void ResetHook()
        {            
            rope.Release();
            hook.Release();
            hook.SetFlying(false);
            hook.transform.parent = front;
            hook.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        
        public void OnHit()
        {
            hook.transform.parent = null;
            hook.transform.position = front.transform.position;
            hook.SetFlying(true);            
            GameObject x = null;
            Vector3 a = default(Vector3);

            UWE.Utils.TraceFPSTargetPosition(exosuit.gameObject, 100f, ref x, ref a, false);

            if (x == null || x == hook.gameObject)
            {
                a = MainCamera.camera.transform.position + MainCamera.camera.transform.forward * 25f;
            }

            Vector3 a2 = Vector3.Normalize(a - hook.transform.position);

            hook.rb.velocity = a2 * 25f;
            Utils.PlayFMODAsset(shootSound, front, 15f);

            grapplingStartPos = exosuit.transform.position;
        }
        
        public void FixedUpdate()
        {
            if (hook.attached)
            {
                grapplingLoopSound.Play();
                
                Vector3 value = hook.transform.position - front.position;
                Vector3 a = Vector3.Normalize(value);
                float magnitude = value.magnitude;

                if (magnitude > 1f)
                {
                    if (!IsUnderwater() && exosuit.transform.position.y + 0.2f >= grapplingStartPos.y)
                    {
                        a.y = Mathf.Min(a.y, 0f);
                    }

                    exosuit.GetComponent<Rigidbody>().AddForce(a * exosuitGrapplingAccel, ForceMode.Acceleration);
                    hook.GetComponent<Rigidbody>().AddForce(-a * 400f, ForceMode.Force);
                }

                rope.SetIsHooked();
            }
            else if (hook.flying)
            {
                if ((hook.transform.position - front.position).magnitude > maxDistance)
                {
                    ResetHook();
                }
                grapplingLoopSound.Play();
            }
            else
            {
                grapplingLoopSound.Stop();
            }
        }

        private bool IsUnderwater()
        {
            return exosuit.transform.position.y < worldForces.waterDepth + 2f && !exosuit.precursorOutOfWater;
        }

        public bool GetIsGrappling()
        {
            return hook != null && hook.attached;
        }
    }
}