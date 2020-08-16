/*
using Common;
using UnityEngine;

namespace SeamothArms.ArmPrefabs
{
    public class SeamothClawArmPrefabFixer : MonoBehaviour
    {
        private bool isFixed = false;

        private void Start()
        {
            //safety switch
            if (isFixed)
            {
                return;
            }

            var exosuit = Resources.Load<GameObject>("worldentities/tools/exosuit").GetComponent<Exosuit>();

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

            MeshFilter mf = gameObject.GetComponentInChildren<MeshFilter>();
            mf.sharedMesh = Instantiate(clawMesh);
            mf.sharedMesh.name = "seamoth_hand_geo";

            MeshRenderer mr = gameObject.GetComponentInChildren<MeshRenderer>();
            mr.materials = (Material[])smr.materials.Clone();

            Main.graphics.objectHelper.FindDeepChild(gameObject, "exosuit_rig_armLeft:exosuit_drill_geo").name = "seamoth_claw_arm_geo";

            Destroy(gameObject.GetComponentInChildren<CapsuleCollider>());

            BoxCollider bc_1 = gameObject.FindChild("collider").AddComponent<BoxCollider>();

            bc_1.size = new Vector3(1.29f, 0.33f, 0.42f);
            bc_1.center = new Vector3(-0.53f, 0f, 0.04f);

            GameObject collider2 = new GameObject("collider2");
            collider2.transform.SetParent(gameObject.transform, false);
            collider2.transform.localPosition = new Vector3(-1.88f, 0.07f, 0.50f);
            collider2.transform.localRotation = Quaternion.Euler(0, 34, 0);

            BoxCollider bc_2 = collider2.AddComponent<BoxCollider>();
            bc_2.size = new Vector3(1.06f, 0.23f, 0.31f);
            bc_2.center = new Vector3(0, -0.08f, 0);

            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

            isFixed = true;
        }

    }
}
*/