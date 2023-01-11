using ModdedArmsHelper.API.ArmHandlers;
using UnityEngine;

namespace APIBasedExosuitArms.Handlers
{
    public class APIBasedDrillArmHandler : DrillArmHandler, IExosuitArm
    {
        public override void Start()
        {
            smoothedDirection = exosuit.transform.rotation;
        }

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
            drilling = true;
            loop.Play();
            cooldownDuration = 0f;
            drillTarget = null;
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
            drilling = false;
            StopEffects();
            cooldownDuration = 0f;
            return true;
        }

        bool IExosuitArm.OnAltDown()
        {
            return false;
        }

        void IExosuitArm.Update(ref Quaternion aimDirection)
        {
            if (drillTarget != null)
            {
                Quaternion b = Quaternion.LookRotation(Vector3.Normalize(UWE.Utils.GetEncapsulatedAABB(drillTarget, -1).center - MainCamera.camera.transform.position), Vector3.up);
                smoothedDirection = Quaternion.Lerp(smoothedDirection, b, Time.deltaTime * 6f);
                aimDirection = smoothedDirection;
            }
            
            smoothedDirection = Quaternion.Lerp(smoothedDirection, aimDirection, Time.deltaTime * 15f);
            aimDirection = smoothedDirection;            
        }

        void IExosuitArm.ResetArm()
        {
            animator.SetBool("use_tool", false);
            drilling = false;
            StopEffects();
        }        

        public void OnHit()
        {
            if (exosuit.CanPilot() && exosuit.GetPilotingMode())
            {
                Vector3 pos = Vector3.zero;
                GameObject hitObject = null;
                drillTarget = null;

                UWE.Utils.TraceFPSTargetPosition(exosuit.gameObject, attackDist, ref hitObject, ref pos, true);

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
                    var drillable = hitObject.FindAncestor<Drillable>();

                    loopHit.Play();

                    if (drillable)
                    {
                        drillable.OnDrill(fxSpawnPoint.position, exosuit, out GameObject gameObject2);

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
    }
}
