using System.Collections.Generic;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Common;
using FMODUnity;

namespace AncientSword
{
    internal class SwordPrefab : CraftableModItem
    {
        public static TechType TechTypeID { get; private set; }

        internal SwordPrefab()
            : base(nameID: "AncientSword",
                  iconFileName: "AncientSword",
                  iconTechType: TechType.None,
                  friendlyName: "Ancient Sword",
                  description: "An ancient sword from Earth.\nFound in an ancient Precursor facility.",
                  template: TechType.None,
                  fabricatorType: CraftTree.Type.Fabricator,
                  fabricatorTab: new string[] { "Personal", "Tools", "AncientSword" },
                  requiredAnalysis: TechType.None,
                  groupForPDA: TechGroup.Personal,
                  categoryForPDA: TechCategory.Tools,
                  equipmentType: EquipmentType.Hand,
                  quickSlotType: QuickSlotType.Passive,
                  backgroundType: CraftData.BackgroundType.Normal,
                  itemSize: new Vector2int(1,1),                  
                  gamerResourceFileName: "worldentities/doodads/precursor/prison/relics/alien_relic_08"
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
                Ingredients = new List<Ingredient>(new Ingredient[4]
                {
                    new Ingredient(TechType.Titanium, 3),
                    new Ingredient(TechType.Copper, 2),
                    new Ingredient(TechType.Nickel, 2),
                    new Ingredient(TechType.Diamond, 2)
                })
            };
        }        
        
        public override GameObject GetGameObject()
        {
            base.GetGameObject();            
            
            GameObject modelGO = _GameObject.FindChild("alien_relic_08_world_rot").FindChild("alien_relic_08_hlpr").
                       FindChild("alien_relic_08_ctrl").FindChild("alien_relic_08");

            modelGO.transform.SetParent(_GameObject.transform, false);
            modelGO.name = "sword_model";
            modelGO.transform.localPosition = new Vector3(0.06f, 0.27f, 0.05f);
            modelGO.transform.localRotation = Quaternion.Euler(350f, 265f, 172f);
            modelGO.transform.localScale = new Vector3(0.63f, 0.63f, 0.63f);

            GameObject colliderGO = new GameObject("collider_container");
            colliderGO.transform.SetParent(_GameObject.transform, false);
            colliderGO.transform.localPosition = new Vector3(0f, 0.13f, 0.02f);
            colliderGO.transform.localScale = new Vector3(1f, 1f, 1f);
            colliderGO.transform.localRotation = Quaternion.Euler(7f, 358f, 355f);

            BoxCollider boxCollider = colliderGO.GetOrAddComponent<BoxCollider>();
            boxCollider.size = new Vector3(0.13f, 0.83f, 0.05f);
            boxCollider.center = new Vector3(0f, 0.12f, 0f);

            Object.DestroyImmediate(_GameObject.FindChild("Cube"));
            Object.DestroyImmediate(_GameObject.FindChild("alien_relic_08_world_rot"));
            Object.DestroyImmediate(_GameObject.GetComponent<ImmuneToPropulsioncannon>());
            Object.DestroyImmediate(_GameObject.GetComponent<CapsuleCollider>());
            Object.DestroyImmediate(_GameObject.GetComponent<EntityTag>());

            _GameObject.GetOrAddComponent<PrefabIdentifier>().ClassId = ClassID;
            _GameObject.GetOrAddComponent<TechTag>().type = TechType;
            _GameObject.GetOrAddComponent<Pickupable>().isPickupable = true;
            _GameObject.GetOrAddComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;

            _GameObject.AddIfNeedComponent<VFXSurface>();
            _GameObject.AddIfNeedComponent<EcoTarget>();
            _GameObject.AddIfNeedComponent<FMOD_CustomEmitter>();
            _GameObject.AddIfNeedComponent<StudioEventEmitter>();

            SkyApplier skyApplier = _GameObject.GetOrAddComponent<SkyApplier>();
            skyApplier.renderers = _GameObject.GetComponentsInChildren<MeshRenderer>();
            skyApplier.anchorSky = Skies.Auto;

            Rigidbody rigidbody = _GameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.useGravity = false;

            WorldForces worldForces = _GameObject.GetOrAddComponent<WorldForces>();
            worldForces.underwaterGravity = 1f;
            worldForces.handleGravity = true;
            worldForces.aboveWaterDrag = 1f;
            worldForces.underwaterDrag = 0.00001f;
            worldForces.handleDrag = true;
            worldForces.useRigidbody = rigidbody;

            VFXFabricating vfxFabricating = modelGO.GetOrAddComponent<VFXFabricating>();
            vfxFabricating.localMinY = -0.4f;
            vfxFabricating.localMaxY = 0.2f;
            vfxFabricating.posOffset = new Vector3(-0.054f, 0f, -0.06f);
            vfxFabricating.eulerOffset = new Vector3(0f, 0f, 90f);
            vfxFabricating.scaleFactor = 1f;

            AncientSword component = _GameObject.GetOrAddComponent<AncientSword>();

            Knife knife = Resources.Load<GameObject>("WorldEntities/Tools/Knife").GetComponent<Knife>();

            component.mainCollider = boxCollider;
            component.socket = PlayerTool.Socket.RightHand;
            component.ikAimRightArm = true;
            component.attackSound = Object.Instantiate(knife.attackSound, _GameObject.transform);
            component.underwaterMissSound = Object.Instantiate(knife.underwaterMissSound, _GameObject.transform);
            component.surfaceMissSound = Object.Instantiate(knife.surfaceMissSound, _GameObject.transform);

            return _GameObject;
        }        
    }
}
