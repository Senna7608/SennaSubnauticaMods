using System.Collections.Generic;
using UnityEngine;
using UWE;
using static Common.Modules;
using static Common.GameHelper;
using Common;
using System.Collections;
using MoreCyclopsUpgrades.API.Upgrades;
using MoreCyclopsUpgrades.API;

namespace CyclopsLaserCannonModule
{
    public partial class CannonControl : MonoBehaviour
    {
        public CannonControl Instance { get; private set; }           

        public PowerRelay powerRelay;
        public SubRoot subroot;
        public SubControl subcontrol;
        public CannonCamera camera_instance;        

        private AudioSource audioSource;
        private readonly TechType laserCannon = Main.techTypeID;        
        
        private readonly float fireRate = 0.5f;
        private float nextFire;        
        private const float maxLaserDistance = 100f;        
        private float targetDist;
        private Vector3[] right_right_beamPositions = new Vector3[2];
        private Vector3[] right_left_beamPositions = new Vector3[2];
        private Vector3[] left_right_beamPositions = new Vector3[2];
        private Vector3[] left_left_beamPositions = new Vector3[2];
        private GameObject targetGameobject;        
        private Vector3 targetPosition;

        private UpgradeHandler upgradeHandler;
        

        public void Start()
        {                       
            Instance = this;            

            Main.onConfigurationChanged.AddHandler(this, new Event<string>.HandleFunction(OnConfigurationChanged));

            This_Cyclops_Root = gameObject;
            
            subroot = gameObject.GetComponent<SubRoot>();
            subcontrol = subroot.GetComponentInParent<SubControl>();            
            powerRelay = subcontrol.powerRelay;            

            CreateCannonCamera();
            CreateCannonButton();
            CreateCannonRight();
            CreateCannonLeft();

            LaserCannonSetActive(false);

            GameObject laser_sound = Instantiate(Main.assetBundle.LoadAsset<GameObject>("turret_sound"), CannonCamPosition.transform);
            audioSource = laser_sound.GetComponent<AudioSource>();           
            
            SetOnlyHostile();
            SetLaserStrength();
            SetLaserSFXVolume();
            SetWarningMessage();                        
            
            Player.main.playerModeChanged.AddHandler(this, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
            Player.main.currentSubChangedEvent.AddHandler(this, new Event<SubRoot>.HandleFunction(OnSubRootChanged));

            StartCoroutine(GetMCUHandler());            
        }        

        private IEnumerator GetMCUHandler()
        {
            SNLogger.Log($"[CyclopsLaserCannonModule] GetMCUHandler coroutine started for this Cyclops: {This_Cyclops_Root.GetInstanceID()}");

            while (upgradeHandler == null)
            {
                upgradeHandler = MCUServices.Find.CyclopsUpgradeHandler(subroot, Main.techTypeID);
                SNLogger.Log($"[CyclopsLaserCannonModule] MCU UpgradeHandler is not ready for this Cyclops: {This_Cyclops_Root.GetInstanceID()}");
                yield return null;
            }

            SNLogger.Log($"[CyclopsLaserCannonModule] MCU UpgradeHandler is ready for this Cyclops: {This_Cyclops_Root.GetInstanceID()}");

            upgradeHandler.OnFinishedUpgrades = OnFinishedUpgrades;
            upgradeHandler.OnClearUpgrades = OnClearUpgrades;

            OnFirstTimeCheckModuleIsExists();

            SNLogger.Log($"[CyclopsLaserCannonModule] GetMCUHandler coroutine stopped for this Cyclops: {This_Cyclops_Root.GetInstanceID()}");

            yield break;
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

        /*
        private void AddForceToTarget()
        {
            Rigidbody rb = targetGameobject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(targetPosition.normalized * 20f, ForceMode.Impulse);
            }
            else
            {
                rb = targetGameobject.GetComponentInChildren<Rigidbody>();

                if (rb != null)
                {
                    rb.AddForce(targetPosition.normalized * 20f, ForceMode.Impulse);
                }
            }
        }
        */

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
            if (powerRelay.GetPower() < powerRelay.GetMaxPower() * 0.2f)
            {               
                isLowPower = true;
                isShoot = false;
            }
            else
            {
                isLowPower = false;
            }

            if (isModuleInserted && isActive && !isLowPower && isPiloting && camera_instance.usingCamera)
            {
                if (GameInput.GetButtonDown(GameInput.Button.LeftHand) && Time.time > nextFire)
                {                    
                    nextFire = Time.time + fireRate;
                    isShoot = true;
                    audioSource.Play();
                    powerRelay.ConsumeEnergy(powerConsumption, out float num);                    
                }
                if (Time.time > nextFire)
                {
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

