using UnityEngine;

namespace ModdedArmsHelper.API.ArmHandlers
{
    /// <summary>
    /// The abstract class to inherit for handling the drill arm base mechanics.
    /// </summary>
    public abstract class DrillArmHandler : ArmHandler
    {

#pragma warning disable CS1591 //XML documentation

        public VFXController fxControl { get; private set; }
        public FMOD_CustomLoopingEmitter loop { get; private set; }
        public FMOD_CustomLoopingEmitter loopHit { get; private set; }        

        public VFXEventTypes vfxEventType { get; set; }
        
        public Transform fxSpawnPoint { get; private set; }
        public DamageType damageType { get; set; } = DamageType.Drill;
        public VFXSurfaceTypes prevSurfaceType { get; set; } = VFXSurfaceTypes.fallback;

        public float energyCost = 0.5f;
        public float attackDist = 4.8f;
        public float damage = 4f;
        public bool drilling;

        public ParticleSystem drillFXinstance;
        public GameObject drillTarget;
        public Quaternion smoothedDirection = Quaternion.identity;

#pragma warning restore CS1591 //XML documentation

        /// <summary>
        /// If you implement this method in your modded arm handler, the first line should be 'base.Awake()'
        /// </summary>
        public override void Awake()
        {            
            fxControl = GetComponent<VFXController>();
            vfxEventType = VFXEventTypes.exoDrill;
            fxSpawnPoint = ArmServices.main.objectHelper.FindDeepChild(gameObject, "FXSpawnPoint").transform;

            FMOD_CustomLoopingEmitter[] emitters = GetComponents<FMOD_CustomLoopingEmitter>();

            for (int i = 0; i < emitters.Length; i++)
            {
                if (emitters[i].asset.name.Equals("drill_loop"))
                {
                    loop = emitters[i];
                    loop.followParent = true;
                }

                if (emitters[i].asset.name.Equals("drill_hit_loop"))
                {
                    loopHit = emitters[i];
                    loopHit.followParent = true;
                }
            }
        }        
    }
}
