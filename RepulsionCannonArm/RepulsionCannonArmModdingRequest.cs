using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using Common.Helpers;

namespace RepulsionCannonArm
{
    public class RepulsionCannonArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<RepulsionCannonArmHandler>();
        }        

        public ISeamothArm GetSeamothArmHandler(GameObject clonedArm)
        {
            return null;
        }

        public void SetUpArm(GameObject clonedArm, ModdedArmsHelper.API.SetupHelper graphicsHelper)
        {
            GameObject elbow = ArmServices.main.objectHelper.FindDeepChild(clonedArm, "elbow");

            Object.DestroyImmediate(clonedArm.FindChild("xPropulsionCannon_Beams"));
            Object.DestroyImmediate(clonedArm.FindChild("PropulsionCannonGrabbedEffect"));
            Object.DestroyImmediate(clonedArm.GetComponent<ExosuitPropulsionArm>());
            Object.DestroyImmediate(clonedArm.GetComponent<PropulsionCannon>());            

            GameObject repulsion = elbow.FindChild("propulsion");
            repulsion.name = "repulsion";

            GameObject ArmRig = ArmServices.main.objectHelper.FindDeepChild(clonedArm, "ArmRig");
            GameObject exosuit_repulsion_geo = ArmRig.FindChild("ArmModel");            

            Common.Helpers.GraphicsHelper.ChangeObjectTexture(exosuit_repulsion_geo, 2, illumTex: Main.illumTex);

            GameObject RepulsionCannonClone = ArmServices.main.objectHelper.GetGameObjectClone(Resources.Load<GameObject>("WorldEntities/Tools/RepulsionCannon"), clonedArm.transform);
            RepulsionCannonClone.name = "RepulsionCannon";
            /*
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<Pickupable>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<EnergyMixin>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<Rigidbody>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<PrefabIdentifier>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<LargeWorldEntity>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<FPModel>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<TechTag>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<WorldForces>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<CapsuleCollider>());
            Object.DestroyImmediate(RepulsionCannonClone.GetComponent<SkyApplier>());
            
            Object.DestroyImmediate(RepulsionCannonClone.transform.Find("1st Person Model").gameObject);
            Object.DestroyImmediate(RepulsionCannonClone.transform.Find("3rd Person Model").gameObject);
            Object.DestroyImmediate(RepulsionCannonClone.transform.Find("BatterySlot").gameObject);
            */
            Object.DestroyImmediate(clonedArm.GetComponent<VFXController>());

            RepulsionCannon repulsionCannon = RepulsionCannonClone.GetComponent<RepulsionCannon>();

            repulsionCannon.fxControl.CopyComponent(clonedArm);                    
            
            VFXController armFX = clonedArm.GetComponent<VFXController>();            

            Transform muzzle = ArmServices.main.objectHelper.FindDeepChild(clonedArm, "muzzle").transform;
            
            for (int i = 0; i < armFX.emitters.Length; i++)
            {
                VFXController.VFXEmitter emitter = armFX.emitters[i];

                emitter.parentTransform = muzzle;

                if (emitter.fx != null)
                {
                    foreach (ParticleSystem ps in emitter.fx.GetComponentsInChildren<ParticleSystem>(true))
                    {
                        ParticleSystem.MainModule psMain = ps.main;

                        ParticleSystem.MinMaxCurve startSize = psMain.startSize;

                        if (startSize.mode == ParticleSystemCurveMode.TwoConstants)
                        {
                            startSize.constant = 1.5f;
                            startSize.constantMax = 1.5f;
                            startSize.constantMin = 1.5f;
                            psMain.startSize = startSize;
                        }
                    }
                }
            }

            GameObject bubblesFX = ArmServices.main.objectHelper.GetGameObjectClone(repulsionCannon.bubblesFX, clonedArm.transform, false);
            bubblesFX.name = "xRepulsionCannon_Bubbles";

            Object.DestroyImmediate(RepulsionCannonClone);
        }
    }    
}
