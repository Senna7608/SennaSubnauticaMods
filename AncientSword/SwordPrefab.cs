using SMLHelper.V2.Assets;
using UnityEngine;
using FMODUnity;
using Common;

namespace AncientSword
{
    internal class SwordPrefab : ModPrefab
    {
        public SwordPrefab(string classId, string prefabFileName, TechType techType = TechType.None) : base(classId, prefabFileName, techType)
        {
            ClassID = classId;
            PrefabFileName = prefabFileName;
            TechType = techType;
        }
        
        public override GameObject GetGameObject()
        {            
            GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("worldentities/doodads/precursor/prison/relics/alien_relic_08"));
            
            GameObject modelGO = gameObject.FindChild("alien_relic_08_world_rot").FindChild("alien_relic_08_hlpr").
                       FindChild("alien_relic_08_ctrl").FindChild("alien_relic_08");

            modelGO.transform.SetParent(gameObject.transform);
            modelGO.name = "sword_model";
            modelGO.transform.localPosition = new Vector3(0.06f, 0.27f, 0.05f);
            modelGO.transform.localRotation = Quaternion.Euler(350f, 265f, 172f);
            modelGO.transform.localScale = new Vector3(0.63f, 0.63f, 0.63f);

            GameObject colliderGO = new GameObject("collider_container");
            colliderGO.transform.SetParent(gameObject.transform);
            colliderGO.transform.localPosition = new Vector3(0f, 0f, 0.01f);
            colliderGO.transform.localScale = new Vector3(1f, 1f, 1f);
            colliderGO.transform.localRotation = Quaternion.Euler(7f, 358f, 355f);

            BoxCollider boxCollider = colliderGO.AddOrGetComponent<BoxCollider>();
            boxCollider.size = new Vector3(0.13f, 0.83f, 0.05f);
            boxCollider.center = new Vector3(0f, 0.12f, 0f);            

            Object.DestroyImmediate(gameObject.FindChild("Cube"));
            Object.DestroyImmediate(gameObject.FindChild("alien_relic_08_world_rot"));
            Object.DestroyImmediate(gameObject.GetComponent<ImmuneToPropulsioncannon>());
            Object.DestroyImmediate(gameObject.GetComponent<CapsuleCollider>());
            Object.DestroyImmediate(gameObject.GetComponent<EntityTag>());            

            gameObject.AddOrGetComponent<PrefabIdentifier>().ClassId = ClassID;
            gameObject.AddOrGetComponent<TechTag>().type = TechType;            
            gameObject.AddOrGetComponent<Pickupable>().isPickupable = true;            
            gameObject.AddOrGetComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Near;

            gameObject.AddIfNeedComponent<VFXSurface>();
            gameObject.AddIfNeedComponent<EcoTarget>();
            gameObject.AddIfNeedComponent<FMOD_CustomEmitter>();
            gameObject.AddIfNeedComponent<StudioEventEmitter>();

            SkyApplier skyApplier = gameObject.AddOrGetComponent<SkyApplier>();
            skyApplier.renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            skyApplier.anchorSky = Skies.Auto;
            
            Rigidbody rigidbody = gameObject.AddOrGetComponent<Rigidbody>();                     
            rigidbody.useGravity = false;                    

            WorldForces worldForces = gameObject.AddOrGetComponent<WorldForces>();
            worldForces.underwaterGravity = 1f;
            worldForces.handleGravity = true;
            worldForces.aboveWaterDrag = 1f;
            worldForces.underwaterDrag = 0.00001f;
            worldForces.handleDrag = true;
            worldForces.useRigidbody = rigidbody;           

            VFXFabricating vfxFabricating = modelGO.AddOrGetComponent<VFXFabricating>();
            vfxFabricating.localMinY = -0.4f;
            vfxFabricating.localMaxY = 0.2f; 
            vfxFabricating.posOffset = new Vector3(-0.054f, 0f, -0.06f);
            vfxFabricating.eulerOffset = new Vector3(0f, 0f, 90f);
            vfxFabricating.scaleFactor = 1f;

            AncientSword component = gameObject.AddOrGetComponent<AncientSword>();

            Knife knife = Resources.Load<GameObject>("WorldEntities/Tools/Knife").GetComponent<Knife>();
            
            component.mainCollider = boxCollider;
            component.socket = PlayerTool.Socket.RightHand;            
            component.ikAimRightArm = true;
            component.attackSound = Object.Instantiate(knife.attackSound, gameObject.transform);
            component.underwaterMissSound = Object.Instantiate(knife.underwaterMissSound, gameObject.transform);
            component.surfaceMissSound = Object.Instantiate(knife.surfaceMissSound, gameObject.transform);            

            return gameObject;
        }        
    }
}
