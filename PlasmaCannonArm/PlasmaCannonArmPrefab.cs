using System.Collections.Generic;
using UnityEngine;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using ModdedArmsHelper.API;
using System.Collections;
using SMLExpander;

namespace PlasmaCannonArm
{
    internal class PlasmaCannonArmPrefab : CraftableModdedArm
    {
        internal PlasmaCannonArmPrefab()
            : base(
                  techTypeName: "ExosuitPlasmaCannonArm",
                  friendlyName: "P.R.A.W.N Plasma Cannon Arm",
                  description: "Allows P.R.A.W.N suit to firing small plasma bullets.",
                  armType: ArmType.ExosuitArm,
                  armTemplate: ArmTemplate.TorpedoArm,
                  requiredForUnlock: TechType.ExosuitTorpedoArmModule,
                  fragment: null
                  )
        {
        }

        protected override RegisterArmRequest RegisterArm()
        {
            return new RegisterArmRequest(this, new PlasmaCannonArmModdingRequest());
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }

        protected override void SetCustomLanguageText()
        {
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile($"{Main.modFolder}/Assets/PlasmaCannonArm.png");
        }

        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.TitaniumIngot, 1),
                    new Ingredient(TechType.PrecursorIonCrystal, 1),                    
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.ComputerChip, 1)
                })
            };
        }

        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {            
            GameObject model = PrefabClone.transform.Find("model").gameObject;

            Object.DestroyImmediate(PrefabClone.FindChild("GameObject"));
            Object.DestroyImmediate(PrefabClone.GetComponent<SeamothStorageContainer>());                
            Object.DestroyImmediate(PrefabClone.transform.Find("model/exosuit_rig_armLeft:exosuit_torpedoLauncher_geo").gameObject);

            GameObject colliderGO = PrefabClone.FindChild("collider");

            BoxCollider boxCollider = colliderGO.AddComponent<BoxCollider>();

            boxCollider.size = new Vector3(1.87f, 0.48f, 0.69f);
            boxCollider.center = new Vector3(0.01f, 0.01f, 0.0f);
            Object.DestroyImmediate(colliderGO.GetComponent<CapsuleCollider>());

            GameObject PlasmaArm = Object.Instantiate(Main.assetBundle.LoadAsset<GameObject>("PlasmaArm"), model.transform);
            PlasmaArm.name = "exosuit_plasma_arm";
            PlasmaArm.transform.localPosition = new Vector3(-0.02f, -0.15f, -0.20f);
            PlasmaArm.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);
            PlasmaArm.transform.localRotation = Quaternion.Euler(0, 90, 0);

            Object.DestroyImmediate(PlasmaArm.FindChild("SFX"));

            Renderer laserArmRenderer = PlasmaArm.GetComponent<Renderer>();                

            Texture2D _MainTex = Main.assetBundle.LoadAsset<Texture2D>("PlasmaArm_MainTex");
            Texture2D _Illum = Main.assetBundle.LoadAsset<Texture2D>("PlasmaArm_Illum");
            Texture2D _BumpMap = Main.assetBundle.LoadAsset<Texture2D>("PlasmaArm_BumpMap");            

            Shader marmoShader = Shader.Find("MarmosetUBER");

            foreach (Material material in laserArmRenderer.materials)
            {
                material.name = "plasmaArm";                
                material.shader = marmoShader;                
                material.EnableKeyword("MARMO_EMISSION");                
                material.EnableKeyword("_ZWRITE_ON");
                material.SetTexture(Shader.PropertyToID("_MainTex"), _MainTex);
                material.SetTexture(Shader.PropertyToID("_BumpMap"), _BumpMap);                
                material.SetTexture(Shader.PropertyToID("_Illum"), _Illum);
            }

            success.Set(true);
            yield break;
        }        
    }
}
