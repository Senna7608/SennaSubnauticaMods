using Common;
using SMLExpander;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS1591 //XML documentation

namespace ModdedArmsHelper.API
{
    public abstract class SpawnableArmFragment : ModPrefab_Fragment
    {
        private static readonly Dictionary<ArmTemplate, TechType> ArmFragmentTypes = new Dictionary<ArmTemplate, TechType>()
        {
            { ArmTemplate.ClawArm, TechType.ExosuitDrillArmFragment },
            { ArmTemplate.DrillArm, TechType.ExosuitDrillArmFragment },
            { ArmTemplate.GrapplingArm, TechType.ExosuitGrapplingArmFragment },
            { ArmTemplate.PropulsionArm, TechType.ExosuitPropulsionArmFragment },
            { ArmTemplate.TorpedoArm, TechType.ExosuitTorpedoArmFragment },
        };

        public ArmTemplate ArmFragmentTemplate { get; private set; }

        protected SpawnableArmFragment(
            string techTypeName,
            string friendlyName,
            ArmTemplate fragmentTemplate,
            string prefabFilePath,            
            float scanTime = 3,
            int totalFragments = 3
            )
            : base(

            techTypeName,
            friendlyName,
            template: ArmFragmentTypes[fragmentTemplate],
            prefabFilePath: prefabFilePath,
            slotType: EntitySlot.Type.Medium,
            prefabZUp: false,
            cellLevel: LargeWorldEntity.CellLevel.Medium,
            localScale: new Vector3(0.8f, 0.8f, 0.8f),
            scanTime: scanTime,
            totalFragments: totalFragments,
            destroyAfterScan: true
            )
        {
            ArmFragmentTemplate = fragmentTemplate;
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {
            if (ArmFragmentTemplate == ArmTemplate.ClawArm)
            {
                Main.armsGraphics.ArmsTemplateCache.TryGetValue(ArmTemplate.ClawArm, out GameObject armPrefab);

                SkinnedMeshRenderer smr = armPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
                Mesh clawMesh = smr.sharedMesh;

                MeshFilter mf = GameObjectClone.GetComponentInChildren<MeshFilter>();
                mf.sharedMesh = Object.Instantiate(clawMesh);                

                MeshRenderer mr = GameObjectClone.GetComponentInChildren<MeshRenderer>();
                mr.materials = (Material[])smr.materials.Clone();
            }

            GameObjectClone.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            TaskResult<bool> modifyResult = new TaskResult<bool>();
            CoroutineTask<bool> modifyRequest = new CoroutineTask<bool>(PostModifyGameObjectAsync(modifyResult), modifyResult);
            yield return modifyRequest;
            if (!modifyRequest.GetResult())
            {
                SNLogger.Error("PostModifyGameObjectAsync failed!");
                yield break;
            }

            success.Set(true);
            yield break;
        }

        protected abstract IEnumerator PostModifyGameObjectAsync(IOut<bool> success);
    }
}
