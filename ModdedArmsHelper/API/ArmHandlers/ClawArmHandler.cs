using UnityEngine;

namespace ModdedArmsHelper.API.ArmHandlers
{
    /// <summary>
    /// The abstract class to inherit for handling the claw arm base mechanics.
    /// </summary>
    public abstract class ClawArmHandler : ArmHandler
    {
        /// <summary>
        /// This property is returning the <see cref="VFXController"/> component.
        /// </summary>
        public VFXController fxControl { get; private set; }

        /// <summary>
        /// This property is returning the 'hitTerrainSound'. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public FMODAsset hitTerrainSound { get; private set; }

        /// <summary>
        /// This property is returning the 'hitFishSound'. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public FMODAsset hitFishSound { get; private set; }

        /// <summary>
        /// This property is returning the 'pickupSound'. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public FMODAsset pickupSound { get; private set; }

        /// <summary>
        /// This property is returning the claw arm front. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public Transform front { get; private set; }

        /// <summary>
        /// This property can be set. See also <see cref="VFXEventTypes"/> enum.
        /// </summary>
        public VFXEventTypes vfxEventType { get; set; }

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float kGrabDistance = 4.8f;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float cooldownPunch = 1f;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float cooldownPickup = 1.533f;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float attackDist = 5.2f;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float damage = 50f;

        /// <summary>
        /// This field can be set. See also <see cref="DamageType"/> enum.
        /// </summary>
        public DamageType damageType = DamageType.Normal;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float timeUsed = float.NegativeInfinity;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float cooldownTime;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public bool shownNoRoomNotification = true;

        /// <summary>
        /// This field can be set. See also <see cref="ExosuitClawArm"/> class.
        /// </summary>
        public float energyCost = 0.5f;

        /// <summary>
        /// If you implement this method in your modded arm handler, the first line should be 'base.Awake()'
        /// </summary>
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
