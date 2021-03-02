using UnityEngine;

namespace ModdedArmsHelper.API.ArmHandlers
{
    public abstract class SeamothGrapplingArmHandler : ArmHandler
    {        
        public VFXGrapplingRope rope { get; private set; }
        public SeamothGrapplingHook hook { get; private set; }
        public FMOD_CustomLoopingEmitter grapplingLoopSound { get; private set; }
        public FMODAsset shootSound { get; private set; }
        public Transform front { get; private set; }
        public WorldForces worldForces { get; private set; }
        public GameObject hookPrefab { get; private set; }

        public DamageType damageType { get; set; } = DamageType.Collide;

        public float energyCost = 0.5f;        
        public float maxDistance = 50f;
        public float damage = 5f;
        public float seamothGrapplingAccel = 25f;
        public float targetGrapplingAccel = 400f;
        public Vector3 grapplingStartPos = Vector3.zero;

        public override void Awake()
        {
            front = ArmServices.main.objectHelper.FindDeepChild(gameObject, "hook").transform;            
            hookPrefab = Instantiate(Main.graphics.HookPrefab);

            DestroyImmediate(hookPrefab.GetComponent<GrapplingHook>());
            hook = hookPrefab.AddComponent<SeamothGrapplingHook>();
            hook.transform.parent = front;
            hook.transform.localPosition = Vector3.zero;
            hook.transform.localRotation = Quaternion.identity;
            hook.transform.localScale = new Vector3(1f, 1f, 1f);            

            rope = FindObjectOfType(typeof(VFXGrapplingRope)) as VFXGrapplingRope;
            rope.origin = front.parent;
            rope.attachPoint = hook.transform;

            grapplingLoopSound = GetComponent<FMOD_CustomLoopingEmitter>();

            shootSound = ScriptableObject.CreateInstance<FMODAsset>();
            shootSound.path = "event:/sub/exo/hook_shoot";
            shootSound.name = "hook_shoot";
        }

        public override void Start()
        {
            worldForces = vehicle.GetComponent<WorldForces>();
        }

    }
}
