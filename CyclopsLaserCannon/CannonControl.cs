using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common;
using System.Collections;
using SMLHelper.V2.Utility;
using static Common.Helpers.GraphicsHelper;
using System;

namespace CyclopsLaserCannonModule
{
    public partial class CannonControl : MonoBehaviour
    {
        public CannonControl Instance { get; private set; }

        public PowerRelay powerRelay
        {
            get
            {
                if (GameModeUtils.RequiresPower())
                {
                    return This_Cyclops_Root.GetComponent<PowerRelay>();
                }
                else
                {
                    return null;
                }
            }
        }

        public SubRoot subroot;
        public SubControl subcontrol;
        public CannonCamera camera_instance;
        
        private readonly TechType laserCannon = CannonPrefab.TechTypeID;        
        
        private readonly float fireRate = 0.8f;
        private float nextFire;        
        private const float maxLaserDistance = 100f;        
        private float targetDist;
        private Vector3[] right_right_beamPositions = new Vector3[2];
        private Vector3[] right_left_beamPositions = new Vector3[2];
        private Vector3[] left_right_beamPositions = new Vector3[2];
        private Vector3[] left_left_beamPositions = new Vector3[2];
        private GameObject targetGameobject;        
        private Vector3 targetPosition;
        private FMODAsset turret_fire;
        private FMOD_CustomEmitter turretLeftSound;
        private FMOD_CustomEmitter turretRightSound;
        public bool asyncOperationsComplete = false;

        public void Awake()
        {
            SNLogger.Debug("CannonControl: Awake started...");

            Instance = this;

            This_Cyclops_Root = gameObject;

            subroot = This_Cyclops_Root.GetComponent<SubRoot>();
            subcontrol = This_Cyclops_Root.GetComponent<SubControl>();

            subroot.upgradeConsole.modules.onEquip += OnEquip;
            subroot.upgradeConsole.modules.onUnequip += OnUnequip;
            
            Main.onConfigurationChanged.AddHandler(this, new Event<string>.HandleFunction(OnConfigurationChanged));

            CreateCannonCamera();
            CreateCannonButton();
            CreateUpgradeConsoleIcon();

            StartCoroutine(AwakeAsync());

            SNLogger.Debug("AwakeAsync started...");
        }        

        public IEnumerator AwakeAsync()
        {
            TaskResult<bool> loadAssetsResult = new TaskResult<bool>();
            CoroutineTask<bool> assetRequest = new CoroutineTask<bool>(LoadAssetsAsync(loadAssetsResult), loadAssetsResult);
            yield return assetRequest;

            if (!assetRequest.GetResult())
            {
                SNLogger.Error("LoadAssetsAsync failed!");
                yield break;
            }

            TaskResult<bool> rightResult = new TaskResult<bool>();
            CoroutineTask<bool> cannonRightRequest = new CoroutineTask<bool>(CreateCannonRightAsync(rightResult), rightResult);
            yield return cannonRightRequest;

            if (!cannonRightRequest.GetResult())
            {
                SNLogger.Error("CreateCannonRightAsync failed!");
                yield break;
            }

            TaskResult<bool> leftResult = new TaskResult<bool>();
            CoroutineTask<bool> cannonLeftRequest = new CoroutineTask<bool>(CreateCannonLeftAsync(leftResult), leftResult);
            yield return cannonLeftRequest;

            if (!cannonLeftRequest.GetResult())
            {
                SNLogger.Error("CreateCannonLeftAsync failed!");
                yield break;
            }

            turret_fire = ScriptableObject.CreateInstance<FMODAsset>();
            turret_fire.name = "turret_fire";
            turret_fire.path = "event:/sub/seamoth/torpedo_fire";

            turretLeftSound = cannon_left_rotation_point.AddComponent<FMOD_CustomEmitter>();
            turretLeftSound.asset = turret_fire;

            turretRightSound = cannon_right_rotation_point.AddComponent<FMOD_CustomEmitter>();
            turretRightSound.asset = turret_fire;

            SetOnlyHostile();
            SetLaserStrength();
            SetLaserSFXVolume();
            SetWarningMessage();

            SNLogger.Debug("AwakeAsync completed.");
            asyncOperationsComplete = true;
            StartCoroutine(LaserCannonSetActiveAsync(false));
            yield break;
        }

        public IEnumerator LoadAssetsAsync(IOut<bool> success)
        {
            Texture2D cannon_Button = ImageUtils.LoadTextureFromFile($"{Main.modFolder}/Assets/cannon_Button.png");

            Main.buttonSprite = Sprite.Create(cannon_Button, new Rect(0, 0, cannon_Button.width, cannon_Button.height), new Vector2(cannon_Button.width * 0.5f, cannon_Button.height * 0.5f));

            TaskResult<Material> materialResult = new TaskResult<Material>();
            CoroutineTask<Material> materialRequest = new CoroutineTask<Material>(GetResourceMaterialAsync("WorldEntities/Doodads/Precursor/PrecursorTeleporter.prefab", "precursor_interior_teleporter_02_01", 0, materialResult), materialResult);
            yield return materialRequest;

            if (materialRequest.GetResult() == null)
            {
                SNLogger.Error("GetResourceMaterialAsync failed!");
                success.Set(false);
                yield break;
            }

            Main.cannon_material = materialResult.Get();
            Main.cannon_material.name = "cannon_material";
            Main.isAssetsLoaded = true;

            success.Set(true);
            yield break;
        }

        public void Start()
        {
            SNLogger.Debug("Start started...");
            Player.main.playerModeChanged.AddHandler(this, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
            Player.main.currentSubChangedEvent.AddHandler(this, new Event<SubRoot>.HandleFunction(OnSubRootChanged));
        }      
                            
        public void OnDestroy()
        {
            Player.main.playerModeChanged.RemoveHandler(this, OnPlayerModeChanged);
            Player.main.currentSubChangedEvent.RemoveHandler(this, OnSubRootChanged);
            
            Destroy(cannon_base_right);
            Destroy(cannon_base_left);            
            Destroy(Button_Cannon);
            Destroy(Cannon_Camera);
            Destroy(CannonCamPosition);            
            Destroy(Instance);
        }        

        private void AddDamageToTarget(GameObject gameObject)
        {
            Vector3 position = gameObject.transform.position;           

            if (gameObject != null)
            {
                LiveMixin liveMixin = gameObject.GetComponent<LiveMixin>();
                if (!liveMixin)
                {
                    liveMixin = Utils.FindEnabledAncestorWithComponent<LiveMixin>(gameObject);
                }
                if (liveMixin)
                {                 
                    if (liveMixin.IsAlive())
                    {                        
                        liveMixin.TakeDamage(laserDamage, position, DamageType.Normal, null);                       
                    }                        
                }
                else
                {
                    if (gameObject.GetComponent<BreakableResource>() != null)
                    {
                        gameObject.SendMessage("BreakIntoResources", null, SendMessageOptions.DontRequireReceiver);                        
                    }                    
                }                
            }
        }        

        private void AddForceToTarget(Transform source, GameObject targetObject)
        {
            Rigidbody rb = targetObject.GetComponent<Rigidbody>();

            Vector3 forwardForce = source.forward;

            if (rb != null)
            {
                rb.AddForce(forwardForce * 20f, ForceMode.Impulse);
            }
            else
            {
                rb = targetObject.GetComponentInChildren<Rigidbody>();

                if (rb != null)
                {
                    rb.AddForce(forwardForce * 20f, ForceMode.Impulse);
                }
            }
        }

        private void CalculateBeamVectors(float targetDistance)
        {            
            Transform aimTransform = Player.main.camRoot.GetAimingTransform();

            targetPosition = aimTransform.position + targetDistance * aimTransform.forward;                       

            left_right_beamPositions[0] = cannon_left_tube_right.transform.position;            
            left_right_beamPositions[1] = targetPosition;

            left_left_beamPositions[0] = cannon_left_tube_left.transform.position;            
            left_left_beamPositions[1] = targetPosition;

            right_right_beamPositions[0] = cannon_right_tube_right.transform.position;
            right_right_beamPositions[1] = targetPosition;

            right_left_beamPositions[0] = cannon_right_tube_left.transform.position;
            right_left_beamPositions[1] = targetPosition;            
        }

        private void SetBeamTarget()
        {
            if (Targeting.GetTarget(This_Cyclops_Root, maxLaserDistance, out targetGameobject, out targetDist))
            {
                lock (targetGameobject)
                {
                    if (isOnlyHostile)
                    {
                        Targeting.GetRoot(targetGameobject, out TechType targetTechType, out GameObject examinedGameObject);

                        if (!validTargets.Contains(targetTechType))
                        {
                            CalculateBeamVectors(targetDist);
                            return;
                        }
                    }

                    CalculateBeamVectors(targetDist);

                    AddDamageToTarget(targetGameobject);

                    AddForceToTarget(Player.main.camRoot.GetAimingTransform(), targetGameobject);
                }
            }
            else
                CalculateBeamVectors(maxLaserDistance);
        }

        public void Update()
        {
            if (!asyncOperationsComplete)
            {
                return;
            }

            if (powerRelay != null)
            {
                if (powerRelay.GetPower() < powerRelay.GetMaxPower() * 0.2f)
                {
                    isLowPower = true;
                    isShoot = false;
                }
                else
                {
                    isLowPower = false;
                }
            }

            if (isModuleInserted && isActive && !isLowPower && isPiloting && camera_instance.usingCamera)
            {
                if (GameInput.GetButtonDown(GameInput.Button.LeftHand) && Time.time > nextFire)
                {
                    turretLeftSound.Play();
                    turretRightSound.Play();

                    nextFire = Time.time + fireRate;
                    isShoot = true;

                    if (powerRelay != null)
                    {
                        powerRelay.ConsumeEnergy(powerConsumption, out float num);
                    }
                }
                if (Time.time > nextFire)
                {
                    turretLeftSound.Stop();
                    turretRightSound.Stop();
                    isShoot = false;                    
                }                
            }
        }

        private void SetLaserState(bool value)
        {
            cannon_right_tube_right.SetActive(value);
            cannon_right_tube_left.SetActive(value);
            cannon_left_tube_right.SetActive(value);
            cannon_left_tube_left.SetActive(value);
        }

        public void LateUpdate()
        {
            if (isActive && isModuleInserted && isPiloting && camera_instance.usingCamera)
            {
                Vector3 rot = CannonCamPosition.transform.localRotation.eulerAngles;

                cannon_right_rotation_point.transform.localRotation = Quaternion.Euler(rot.x, -rot.y, rot.z);
                cannon_left_rotation_point.transform.localRotation = Quaternion.Euler(rot.x, -rot.y, rot.z);

                if (isShoot)
                {
                    SetBeamTarget();                    
                    
                    right_right.SetPositions(right_right_beamPositions);
                    right_left.SetPositions(right_left_beamPositions);
                    left_right.SetPositions(left_right_beamPositions);
                    left_left.SetPositions(left_left_beamPositions);

                    SetLaserState(true);               
                }
                else
                {
                    SetLaserState(false);
                }
            }            
        }        

        private static readonly List<TechType> validTargets = new List<TechType>
        {
            TechType.Crash,
            TechType.Stalker,
            TechType.Sandshark,
            TechType.BoneShark,
            TechType.Mesmer,
            TechType.Crabsnake,
            TechType.Warper,
            TechType.Biter,
            TechType.Shocker,
            TechType.Blighter,
            TechType.CrabSquid,
            TechType.LavaLizard,
            TechType.SpineEel,            
            TechType.LavaLarva,           
            TechType.Bleeder,
            TechType.Rockgrub,
            TechType.CaveCrawler,                     
            TechType.PrecursorDroid,           
            TechType.ReaperLeviathan,
            TechType.SeaDragon,           
            TechType.GhostLeviathanJuvenile,
            TechType.GhostLeviathan,
            TechType.LimestoneChunk,
            TechType.SandstoneChunk,
            TechType.BasaltChunk,
            TechType.ShaleChunk,
            TechType.SpikePlant
        };
    }
}

