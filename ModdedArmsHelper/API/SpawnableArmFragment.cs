using Common.Helpers.SMLHelpers;
using System.Collections.Generic;
using UnityEngine;

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
            LargeWorldEntity.CellLevel cellLevel = LargeWorldEntity.CellLevel.Medium,
            float scanTime = 3,
            int totalFragments = 3
            )
            : base(

            techTypeName,
            friendlyName,
            template: ArmFragmentTypes[fragmentTemplate],
            slotType: EntitySlot.Type.Medium,
            prefabZUp: false,
            cellLevel: cellLevel,
            localScale: new Vector3(0.8f, 0.8f, 0.8f),
            scanTime: scanTime,
            totalFragments: totalFragments,
            destroyAfterScan: true
            )
        {
            ArmFragmentTemplate = fragmentTemplate;
        }

        protected override void ModifyGameObject()
        {
            if (ArmFragmentTemplate == ArmTemplate.ClawArm)
            {
                Main.graphics.ArmsTemplateCache.TryGetValue(ArmTemplate.ClawArm, out GameObject armPrefab);

                SkinnedMeshRenderer smr = armPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
                Mesh clawMesh = smr.sharedMesh;

                MeshFilter mf = GameObjectClone.GetComponentInChildren<MeshFilter>();
                mf.sharedMesh = Object.Instantiate(clawMesh);                

                MeshRenderer mr = GameObjectClone.GetComponentInChildren<MeshRenderer>();
                mr.materials = (Material[])smr.materials.Clone();
            }

            GameObjectClone.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            PostModify();
        }

        protected abstract void PostModify();
    }
}
