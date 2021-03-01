using SeamothArms.API;
using SMLHelper.V2.Crafting;
using System.Collections.Generic;
using UnityEngine;

namespace SeamothArmAPITest
{
    public class NewClawArm : CraftableSeamothArm
    {
        public static TechType TechTypeID { get; private set; }

        internal NewClawArm()
            : base(techTypeName: "APIBasedSeamothClawArmModule",
                  friendlyName: "New API Based Seamoth Claw Arm",
                  description: "Allows Seamoth to use API Based Arms.",
                  iconFileName: null,
                  prefabForClone: ArmTemplate.ClawArm,
                  requiredForUnlock: TechType.Exosuit
                  )
        {
        }

        public override void Patch()
        {
            base.Patch();
            TechTypeID = TechType;
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                {
                    new Ingredient(TechType.TitaniumIngot, 2)                    
                })
            };
        }

        public override GameObject GetGameObject()
        {
            base.GetGameObject();

            var exosuit = ArmServices.main.ExosuitResource;

            GameObject armPrefab = null;

            for (int i = 0; i < exosuit.armPrefabs.Length; i++)
            {
                if (exosuit.armPrefabs[i].techType == TechType.ExosuitClawArmModule)
                {
                    armPrefab = exosuit.armPrefabs[i].prefab;
                    break;
                }
            }

            SkinnedMeshRenderer smr = armPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
            Mesh clawMesh = smr.sharedMesh;

            MeshFilter mf = PrefabClone.GetComponentInChildren<MeshFilter>();
            mf.sharedMesh = UnityEngine.Object.Instantiate(clawMesh);
            mf.sharedMesh.name = "seamoth_hand_geo";

            MeshRenderer mr = PrefabClone.GetComponentInChildren<MeshRenderer>();
            mr.materials = (Material[])smr.materials.Clone();

            PrefabClone.transform.Find("model/exosuit_rig_armLeft:exosuit_drill_geo").gameObject.name = "seamoth_claw_arm_geo";

            UnityEngine.Object.Destroy(PrefabClone.GetComponentInChildren<CapsuleCollider>());

            BoxCollider bc_1 = PrefabClone.FindChild("collider").AddComponent<BoxCollider>();

            bc_1.size = new Vector3(1.29f, 0.33f, 0.42f);
            bc_1.center = new Vector3(-0.53f, 0f, 0.04f);

            GameObject collider2 = new GameObject("collider2");
            collider2.transform.SetParent(PrefabClone.transform, false);
            collider2.transform.localPosition = new Vector3(-1.88f, 0.07f, 0.50f);
            collider2.transform.localRotation = Quaternion.Euler(0, 34, 0);

            BoxCollider bc_2 = collider2.AddComponent<BoxCollider>();
            bc_2.size = new Vector3(1.06f, 0.23f, 0.31f);
            bc_2.center = new Vector3(0, -0.08f, 0);

            PrefabClone.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);            

            return PrefabClone;
        }
    }
}
