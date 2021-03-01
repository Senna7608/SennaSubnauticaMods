using ModdedArmsHelper.API;
using ModdedArmsHelper.API.ArmHandlers;
using ModdedArmsHelper.API.Interfaces;
using UnityEngine;

namespace SeamothArms.InternalArmHandlers
{
    internal class InternalDrillArmHandler : DrillArmHandler, ISeamothArm
    {
        public override void Start()
        {
        }

        GameObject ISeamothArm.GetGameObject()
        {                       
            return gameObject;
        }

        public void SetSide(SeamothArm arm)
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

        bool ISeamothArm.OnUseDown(out float cooldownDuration)
        {
            Animator.SetBool("use_tool", true);
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
            Animator.SetBool("use_tool", false);
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
            if (Seamoth.docked && !isResetted)
            {
                Animator.SetBool("use_tool", false);
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
            Animator.SetBool("use_tool", false);
            drilling = false;
            StopEffects();
        }            
        
        public void OnHit()
        {
#if DEBUG
            if (true)
#else
            if (Seamoth.CanPilot() && Seamoth.GetPilotingMode())
#endif
            {
                Vector3 pos = Vector3.zero;
                GameObject hitObject = null;
                drillTarget = null;

                UWE.Utils.TraceFPSTargetPosition(Seamoth.gameObject, attackDist, ref hitObject, ref pos, true);

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
                        drillable.OnDrill(fxSpawnPoint.position, Seamoth, out GameObject gameObject2);

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

        bool ISeamothArm.HasPropCannon()
        {
            return false;
        }

        float ISeamothArm.GetEnergyCost()
        {
            return energyCost;
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
    }
}