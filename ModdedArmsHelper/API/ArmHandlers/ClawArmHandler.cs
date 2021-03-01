using UnityEngine;

namespace ModdedArmsHelper.API.ArmHandlers
{
    public abstract class ClawArmHandler : ArmHandler
    {        
        public VFXController fxControl { get; private set; }
        public FMODAsset hitTerrainSound { get; private set; }
        public FMODAsset hitFishSound { get; private set; }
        public FMODAsset pickupSound { get; private set; }
        public Transform front { get; private set; }

        public VFXEventTypes vfxEventType { get; set; }        

        public float kGrabDistance = 4.8f;
        public float cooldownPunch = 1f;
        public float cooldownPickup = 1.533f;
        public float attackDist = 5.2f;
        public float damage = 50f;
        public DamageType damageType = DamageType.Normal;
        public float timeUsed = float.NegativeInfinity;
        public float cooldownTime;
        public bool shownNoRoomNotification = true;
        public float energyCost = 0.5f;

        public override void Awake()
        {            
            fxControl = GetComponent<VFXController>();
            vfxEventType = VFXEventTypes.impact;

            foreach (FMODAsset asset in GetComponents<FMODAsset>())
            {
                if (asset.name == "claw_hit_terrain")
                    hitTerrainSound = asset;

                if (asset.name == "claw_hit_fish")
                    hitFishSound = asset;

                if (asset.name == "claw_pickup")
                    pickupSound = asset;
            }

            front = ArmServices.main.objectHelper.FindDeepChild(gameObject, "wrist").transform;
        }
    }
}
