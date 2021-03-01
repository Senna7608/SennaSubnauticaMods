using Common;
using Common.Helpers;
using SeamothArms.API;
using UnityEngine;

namespace SeamothArms.ArmControls
{
    internal class SeamothGrapplingArmControl : MonoBehaviour, ISeamothArmControl
    {
        public SeaMoth ThisSeamoth
        {
            get
            {
                return GetComponentInParent<SeaMoth>();
            }
        }

        public WorldForces worldForces;
        private const float energyCost = 0.5f;
        public Animator animator;
        
        public Transform front;
        public GameObject hookPrefab;
        
        public VFXGrapplingRope rope;
        public FMOD_CustomLoopingEmitter grapplingLoopSound;
        public FMODAsset shootSound;

        private SeamothGrapplingHook hook;

        private const float maxDistance = 50f;
        private const float damage = 5f;
        private const DamageType damageType = DamageType.Collide;        
        private const float seamothGrapplingAccel = 25f;
        private const float targetGrapplingAccel = 400f;
        private Vector3 grapplingStartPos = Vector3.zero;

        public ObjectHelper objectHelper { get; private set; }

        private void Awake()
        {
            objectHelper = Main.graphics.objectHelper;

            animator = GetComponent<Animator>();

            front = objectHelper.FindDeepChild(gameObject, "hook").transform;
            ExosuitGrapplingArm component = GetComponent<ExosuitGrapplingArm>();
            hookPrefab = Instantiate(component.hookPrefab);
            rope = objectHelper.GetObjectClone(component.rope);

            DestroyImmediate(hookPrefab.GetComponent<GrapplingHook>());
            DestroyImmediate(component);            

            hook = hookPrefab.AddComponent<SeamothGrapplingHook>();
            hook.transform.parent = front;
            hook.transform.localPosition = Vector3.zero;
            hook.transform.localRotation = Quaternion.identity;
            hook.transform.localScale = new Vector3(1f, 1f, 1f);

            rope.attachPoint = hook.transform;

            grapplingLoopSound = GetComponent<FMOD_CustomLoopingEmitter>();

            shootSound = ScriptableObject.CreateInstance<FMODAsset>();
            shootSound.path = "event:/sub/exo/hook_shoot";
            shootSound.name = "hook_shoot";
        }

        private void Start()
        {
            worldForces = ThisSeamoth.GetComponent<WorldForces>();            
        }

        GameObject ISeamothArmControl.GetGameObject()
        {
            return gameObject;
        }

        void ISeamothArmControl.SetSide(SeamothArm arm)
        {
            if (arm == SeamothArm.Right)
            {
                transform.localScale = new Vector3(-0.80f, 0.80f, 0.80f);                
            }
            else
            {
                transform.localScale = new Vector3(0.80f, 0.80f, 0.80f);
            }
        }
        
        bool ISeamothArmControl.OnUseDown(out float cooldownDuration)
        {
            animator.SetBool("use_tool", true);

            if (!rope.isLaunching)
            {
                rope.LaunchHook(maxDistance);
            }

            cooldownDuration = 2f;

            return true;
        }
        
        bool ISeamothArmControl.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }
        
        bool ISeamothArmControl.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("use_tool", false);
            ResetHook();
            cooldownDuration = 0f;
            return true;
        }
        
        bool ISeamothArmControl.OnAltDown()
        {
            return false;
        }
                
        void ISeamothArmControl.Update(ref Quaternion aimDirection)
        {
        }
        
        void ISeamothArmControl.Reset()
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

            UWE.Utils.TraceFPSTargetPosition(ThisSeamoth.gameObject, 100f, ref x, ref a, false);

            if (x == null || x == hook.gameObject)
            {
                a = MainCamera.camera.transform.position + MainCamera.camera.transform.forward * 25f;
            }

            Vector3 a2 = Vector3.Normalize(a - hook.transform.position);

            hook.rb.velocity = a2 * 25f;
            Utils.PlayFMODAsset(shootSound, front, 15f);

            grapplingStartPos = ThisSeamoth.transform.position;
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
                    if (!IsUnderwater() && ThisSeamoth.transform.position.y + 0.2f >= grapplingStartPos.y)
                    {
                        a.y = Mathf.Min(a.y, 0f);
                    }

                    ThisSeamoth.GetComponent<Rigidbody>().AddForce(a * seamothGrapplingAccel, ForceMode.Acceleration);
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
                    transform.localRotation = Quaternion.Euler(346, 0, 0);
                }
            }
            else
            {
                
                transform.localRotation = Quaternion.Euler(346, 0, 0);
            }
        }

        public float GetEnergyCost()
        {
            return energyCost;
        }

        private bool IsUnderwater()
        {
            return ThisSeamoth.transform.position.y < worldForces.waterDepth + 2f && !ThisSeamoth.precursorOutOfWater;
        }

        public bool GetIsGrappling()
        {
            return hook != null && hook.attached;
        }
    }
}
