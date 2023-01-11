using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using FMODUnity;
using SMLExpander;
using SMLHelper.V2.Utility;
using System.Collections;
using UWE;
using Common;

namespace AncientSword
{
    internal class SwordPrefab : ModPrefab_Craftable
    {
        public static TechType TechTypeID { get; private set; }

        internal SwordPrefab()
            : base(techTypeName: "AncientSword",
                  friendlyName: "Ancient Sword",
                  description: "An ancient sword from Earth.\nFound in an ancient Precursor facility.",
                  template: TechType.None,
                  gamerResourceFileName: "WorldEntities/Doodads/Precursor/Prison/Relics/Alien_relic_08.prefab",              
                  requiredAnalysis: TechType.HeatBlade,
                  groupForPDA: TechGroup.Personal,
                  categoryForPDA: TechCategory.Tools,
                  equipmentType: EquipmentType.Hand,
                  quickSlotType: QuickSlotType.Passive,
                  backgroundType: CraftData.BackgroundType.Normal,
                  itemSize: new Vector2int(1,1),                  
                  fragment: null
                  )
        {
        }

        protected override void PrePatch()
        {
            TechTypeID = TechType;            
        }                
        
        protected override TechData GetRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.Titanium, 3),
                    new Ingredient(TechType.Copper, 2),
                    new Ingredient(TechType.Nickel, 2),
                    new Ingredient(TechType.Diamond, 2)
                })
            };
        }

        protected override EncyData GetEncyclopediaData()
        {
            return null;
        }        
        
        protected override IEnumerator ModifyGameObjectAsync(IOut<bool> success)
        {
            GameObject modelGO = GameObjectClone.transform.Find("alien_relic_08_world_rot/alien_relic_08_hlpr/alien_relic_08_ctrl/alien_relic_08").gameObject;
            
            modelGO.transform.SetParent(GameObjectClone.transform, false);
            modelGO.name = "sword_model";
            modelGO.transform.localPosition = new Vector3(0.06f, 0.27f, 0.05f);
            modelGO.transform.localRotation = Quaternion.Euler(350f, 265f, 172f);
            modelGO.transform.localScale = new Vector3(0.63f, 0.63f, 0.63f);

            GameObject colliderGO = new GameObject("collider_container");
            colliderGO.transform.SetParent(GameObjectClone.transform, false);
            colliderGO.transform.localPosition = new Vector3(0f, 0.13f, 0.02f);
            colliderGO.transform.localScale = new Vector3(1f, 1f, 1f);
            colliderGO.transform.localRotation = Quaternion.Euler(7f, 358f, 355f);

            BoxCollider boxCollider = colliderGO.EnsureComponent<BoxCollider>();
            boxCollider.size = new Vector3(0.13f, 0.83f, 0.05f);
            boxCollider.center = new Vector3(0f, 0.12f, 0f);

            Object.DestroyImmediate(GameObjectClone.FindChild("Cube"));
            Object.DestroyImmediate(GameObjectClone.FindChild("alien_relic_08_world_rot"));
            Object.DestroyImmediate(GameObjectClone.GetComponent<ImmuneToPropulsioncannon>());
            Object.DestroyImmediate(GameObjectClone.GetComponent<CapsuleCollider>());
            Object.DestroyImmediate(GameObjectClone.GetComponent<EntityTag>());

            GameObjectClone.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
            GameObjectClone.EnsureComponent<TechTag>().type = TechType;
            GameObjectClone.EnsureComponent<Pickupable>().isPickupable = true;
            GameObjectClone.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;

            GameObjectClone.EnsureComponent<VFXSurface>();
            GameObjectClone.EnsureComponent<EcoTarget>();
            GameObjectClone.EnsureComponent<FMOD_CustomEmitter>();
            GameObjectClone.EnsureComponent<StudioEventEmitter>();

            SkyApplier skyApplier = GameObjectClone.EnsureComponent<SkyApplier>();
            skyApplier.renderers = GameObjectClone.GetComponentsInChildren<MeshRenderer>();
            skyApplier.anchorSky = Skies.Auto;

            Rigidbody rigidbody = GameObjectClone.EnsureComponent<Rigidbody>();
            rigidbody.useGravity = false;

            WorldForces worldForces = GameObjectClone.EnsureComponent<WorldForces>();
            worldForces.underwaterGravity = 1f;
            worldForces.handleGravity = true;
            worldForces.aboveWaterDrag = 1f;
            worldForces.underwaterDrag = 0.00001f;
            worldForces.handleDrag = true;
            worldForces.useRigidbody = rigidbody;

            VFXFabricating vfxFabricating = modelGO.EnsureComponent<VFXFabricating>();
            vfxFabricating.localMinY = -0.4f;
            vfxFabricating.localMaxY = 0.2f;
            vfxFabricating.posOffset = new Vector3(-0.054f, 0f, -0.06f);
            vfxFabricating.eulerOffset = new Vector3(0f, 0f, 90f);
            vfxFabricating.scaleFactor = 1f;

            AncientSwordHandler component = GameObjectClone.EnsureComponent<AncientSwordHandler>();

            //Knife knife = Resources.Load<GameObject>("WorldEntities/Tools/Knife").GetComponent<Knife>();

            IPrefabRequest request = PrefabDatabase.GetPrefabForFilenameAsync("WorldEntities/Tools/Knife.prefab");

            yield return request;

            if (!request.TryGetPrefab(out GameObject prefab))
            {
                SNLogger.Error("Cannot load [Knife] prefab!");
                yield break;
            }

            Knife knife = prefab.GetComponent<Knife>();

            component.mainCollider = boxCollider;
            component.socket = PlayerTool.Socket.RightHand;
            component.ikAimRightArm = true;

            component.attackSound = Object.Instantiate(knife.attackSound, GameObjectClone.transform);
            component.underwaterMissSound = Object.Instantiate(knife.underwaterMissSound, GameObjectClone.transform);
            component.surfaceMissSound = Object.Instantiate(knife.surfaceMissSound, GameObjectClone.transform);

            success.Set(true);
            yield break;
        }

        protected override CrafTreeTypesData GetCraftTreeTypesData()
        {
            return new CrafTreeTypesData()
            {
                TreeTypes = new List<CraftTreeType>()
                {
                    new CraftTreeType(CraftTree.Type.Fabricator, new string[] { "Personal", "Tools" } )
                }
            };
        }

        protected override TabNode GetTabNodeData()
        {
            return null;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile($"{Main.modFolder}/Assets/AncientSword.png");
        }
    }
}
