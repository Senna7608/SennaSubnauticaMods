using UnityEngine;
using Common;

namespace SeamothArms.ArmControls
{
    public class SeamothDrillArmControl : MonoBehaviour, ISeamothArm
    {
        public SeaMoth ThisSeamoth
        {
            get
            {
                return GetComponentInParent<SeaMoth>();
            }
        }

        public Animator animator;
        
        public Transform front;        
        public Transform fxSpawnPoint;
        public VFXController fxControl;
        public VFXEventTypes vfxEventType;
        public FMOD_CustomLoopingEmitter loop;
        public FMOD_CustomLoopingEmitter loopHit;
        private const float energyCost = 0.5f;
        private const float attackDist = 4.8f;
        private const float damage = 4f;
        private const DamageType damageType = DamageType.Drill;
        private bool drilling;        
        private VFXSurfaceTypes prevSurfaceType = VFXSurfaceTypes.fallback;
        private ParticleSystem drillFXinstance;
        private GameObject drillTarget;
        private Quaternion smoothedDirection = Quaternion.identity;

        private void Awake()
        {                          
            animator = GetComponent<Animator>();            
            fxControl = GetComponent<VFXController>();
            vfxEventType = VFXEventTypes.exoDrill;
            fxSpawnPoint = Main.graphics.objectHelper.FindDeepChild(gameObject, "FXSpawnPoint").transform;

            FMOD_CustomLoopingEmitter[] emitters = GetComponents<FMOD_CustomLoopingEmitter>();

            for (int i = 0; i < emitters.Length; i++)
            {
                if (emitters[i].asset.name.Equals("drill_loop"))
                {
                    loop = emitters[i];
                    loop.followParent = true;
                }

                if (emitters[i].asset.name.Equals("drill_hit_loop"))
                {
                    loopHit = emitters[i];
                    loopHit.followParent = true;
                }
            }
        }         

        GameObject ISeamothArm.GetGameObject()
        {                       
            return gameObject;
        }


        public void SetSide(Arm arm)
        {
            if (arm == Arm.Right)
            {
                transform.localScale = new Vector3(-0.80f, 0.80f, 0.80f);                
            }
            else
            {
                transform.localScale = new Vector3(0.80f, 0.80f, 0.80f);                
            }            
        }


        bool ISeamothArm.OnUseDown(out float cooldownDuration)
        {
            animator.SetBool("use_tool", true);
            drilling = true;
            loop.Play();
            cooldownDuration = 0f;
            drillTarget = null;
            return true;
        }
            

        bool ISeamothArm.OnUseHeld(out float cooldownDuration)
        {
            cooldownDuration = 0f;
            return false;
        }            


        bool ISeamothArm.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("use_tool", false);
            drilling = false;
            StopEffects();
            cooldownDuration = 0f;
            return true;
        }
            

        bool ISeamothArm.OnAltDown()
        {
            return false;
        }

        private bool isResetted = false;
                       
        void ISeamothArm.Update(ref Quaternion aimDirection)
        {
            if (ThisSeamoth.docked && !isResetted)
            {
                animator.SetBool("use_tool", false);
                drilling = false;
                StopEffects();
                isResetted = true;
            }

            if (drillTarget != null)
            {
                Quaternion b = Quaternion.LookRotation(Vector3.Normalize(UWE.Utils.GetEncapsulatedAABB(drillTarget, -1).center - MainCamera.camera.transform.position), Vector3.up);
                smoothedDirection = Quaternion.Lerp(smoothedDirection, b, Time.deltaTime * 6f);
                aimDirection = smoothedDirection;
            }
            else
            {                
                smoothedDirection = Quaternion.Lerp(smoothedDirection, aimDirection, Time.deltaTime * 15f);
                aimDirection = smoothedDirection;
            }            
        }
        
        void ISeamothArm.Reset()
        {
            animator.SetBool("use_tool", false);
            drilling = false;
            StopEffects();
        }            
        
        public void OnHit()
        {
#if DEBUG
            if (true)
#else
            if (ThisSeamoth.CanPilot() && ThisSeamoth.GetPilotingMode())
#endif
            {
                Vector3 pos = Vector3.zero;
                GameObject hitObject = null;
                drillTarget = null;

                UWE.Utils.TraceFPSTargetPosition(ThisSeamoth.gameObject, attackDist, ref hitObject, ref pos, true);

                if (hitObject == null)
                {
                    InteractionVolumeUser component = Player.main.gameObject.GetComponent<InteractionVolumeUser>();

                    if (component != null && component.GetMostRecent() != null)
                    {
                        hitObject = component.GetMostRecent().gameObject;
                    }
                }
                if (hitObject && drilling)
                {
                    var drillable = hitObject.FindAncestor<SeamothDrillable>();

                    loopHit.Play();
                    
                    if (drillable)
                    {
                        drillable.OnDrill(fxSpawnPoint.position, ThisSeamoth, out GameObject gameObject2);

                        if (!gameObject2)
                        {
                            StopEffects();
                        }

                        drillTarget = gameObject2;

                        if (fxControl.emitters[0].fxPS != null && !fxControl.emitters[0].fxPS.emission.enabled)
                        {
                            fxControl.Play(0);
                        }
                    }
                    else
                    {
                        LiveMixin liveMixin = hitObject.FindAncestor<LiveMixin>();

                        if (liveMixin)
                        {
                            bool flag = liveMixin.IsAlive();
                            liveMixin.TakeDamage(4f, pos, DamageType.Drill, null);
                            drillTarget = hitObject;
                        }

                        VFXSurface component2 = hitObject.GetComponent<VFXSurface>();

                        if (drillFXinstance == null)
                        {
                            drillFXinstance = VFXSurfaceTypeManager.main.Play(component2, vfxEventType, fxSpawnPoint.position, fxSpawnPoint.rotation, fxSpawnPoint);
                        }
                        else if (component2 != null && prevSurfaceType != component2.surfaceType)
                        {
                            VFXLateTimeParticles component3 = drillFXinstance.GetComponent<VFXLateTimeParticles>();
                            component3.Stop();
                            Destroy(drillFXinstance.gameObject, 1.6f);
                            drillFXinstance = VFXSurfaceTypeManager.main.Play(component2, vfxEventType, fxSpawnPoint.position, fxSpawnPoint.rotation, fxSpawnPoint);
                            prevSurfaceType = component2.surfaceType;
                        }

                        hitObject.SendMessage("BashHit", this, SendMessageOptions.DontRequireReceiver);
                    }
                }
                else
                {
                    StopEffects();
                }
            }
        }
            
        private void StopEffects()
        {
            if (drillFXinstance != null)
            {
                VFXLateTimeParticles component = drillFXinstance.GetComponent<VFXLateTimeParticles>();
                component.Stop();
                Destroy(drillFXinstance.gameObject, 1.6f);
                drillFXinstance = null;
            }

            if (fxControl.emitters[0].fxPS != null && fxControl.emitters[0].fxPS.emission.enabled)
            {
                fxControl.Stop(0);
            }
            loop.Stop();
            loopHit.Stop();
        }
        
        bool ISeamothArm.HasClaw()
        {
            return false;
        }

        bool ISeamothArm.HasDrill()
        {
            return true;
        }

        float ISeamothArm.GetEnergyCost()
        {
            return energyCost;
        }

        void ISeamothArm.SetRotation(Arm arm, bool isDocked)
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
                    if (arm == Arm.Right)
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
    }

}

