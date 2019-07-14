using System.Collections.Generic;
using UnityEngine;
using UWE;
using static Common.Modules;
using static Common.GameHelper;
using UnityEngine.Rendering;

namespace LaserCannon
{
    public class LaserCannonControl : MonoBehaviour
    {
        public LaserCannonControl Instance { get; private set; }
        public int moduleSlotID { get; set; }

        private SeaMoth thisSeamoth { get; set; }
        private EnergyMixin energyMixin { get; set; }
        private Player playerMain { get; set; }
        private PDA PdaMain { get; set; }

        private FMODAsset shootSound;
        private FMOD_CustomLoopingEmitter loopingEmitter;

        private GameObject laserRight, laserLeft;
        private LineRenderer rightBeam, leftBeam;
        private Vector3[] right_beamPositions = new Vector3[2];
        private Vector3[] left_beamPositions = new Vector3[2];

        private float powerConsumption = 1f;
        private float laserDamage = 1f;
        private const float maxLaserDistance = 70f;
        private float idleTimer = 3f;        
        
        private float targetDist;
        private Vector3 targetPosition;

        private GameObject targetGameobject;        

        private bool isToggle;
        private bool isShoot;
        private bool isRepeat;
        private bool isActive;
        private bool isPlayerInThisVehicle;
        private bool isOnlyHostile;

        private string lowPower_title;
        private string lowPower_message;

        public void Start()
        {
            Instance = gameObject.GetComponent<LaserCannonControl>();
            thisSeamoth = Instance.GetComponent<SeaMoth>();
            energyMixin = thisSeamoth.GetComponent<EnergyMixin>();
            playerMain = Player.main;
            PdaMain = playerMain.GetPDA();

            isPlayerInThisVehicle = playerMain.GetVehicle() == thisSeamoth ? true : false;

            GameObject repulsionCannonPrefab = Resources.Load<GameObject>("WorldEntities/Tools/RepulsionCannon");
            repulsionCannonPrefab.SetActive(false);

            shootSound = Instantiate(repulsionCannonPrefab.GetComponent<RepulsionCannon>().shootSound, thisSeamoth.transform, false);
            Destroy(repulsionCannonPrefab);

            loopingEmitter = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            loopingEmitter.asset = shootSound;

            GameObject powerTransmitterPrefab = Resources.Load<GameObject>("Submarine/Build/PowerTransmitter");
            
            GameObject laserBeam = Instantiate(powerTransmitterPrefab.GetComponent<PowerFX>().vfxPrefab, null, false);
            laserBeam.SetActive(false);

            LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.positionCount = 2;
            lineRenderer.receiveShadows = false;
            lineRenderer.loop = false;           

            laserRight = Instantiate(laserBeam, thisSeamoth.torpedoTubeRight, false);
            laserRight.name = "laserRight";
            laserRight.transform.localPosition = Vector3.zero;
            laserRight.transform.localRotation = Quaternion.identity;
            rightBeam = laserRight.GetComponent<LineRenderer>();                      

            laserLeft = Instantiate(laserBeam, thisSeamoth.torpedoTubeLeft, false);
            laserLeft.name = "laserLeft";
            laserLeft.transform.localPosition = Vector3.zero;
            laserLeft.transform.localRotation = Quaternion.identity;
            leftBeam = laserLeft.GetComponent<LineRenderer>();            

            Destroy(laserBeam);
            Destroy(powerTransmitterPrefab);            

            SetBeamColor();
            ShootOnlyHostile();
            SetLaserStrength();
            SetWarningMessage();            
       
            thisSeamoth.onToggle += OnToggle;
            thisSeamoth.modules.onAddItem += OnAddItem;
            thisSeamoth.modules.onRemoveItem += OnRemoveItem;
            playerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));            
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == LaserCannonPrefab.TechTypeID)
            {
                moduleSlotID = -1;
                OnDisable();
                Instance.enabled = false;
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            if (item.item.GetTechType() == LaserCannonPrefab.TechTypeID)
            {               
                moduleSlotID = thisSeamoth.GetSlotByItem(item);
                OnEnable();
                Instance.enabled = true;
            }
        }

        public void OnDestroy()
        {
            thisSeamoth.onToggle -= OnToggle;
            thisSeamoth.modules.onAddItem -= OnAddItem;
            thisSeamoth.modules.onRemoveItem -= OnRemoveItem;
            playerMain.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            SetInteractColor(Colors.White);
            Destroy(Instance);
        }

        public void SetBeamColor()
        {
            rightBeam.material.color = Colors.ColorArray[LaserCannonConfig.beamColor];
            leftBeam.material.color = Colors.ColorArray[LaserCannonConfig.beamColor];            
        }

        public void ShootOnlyHostile()
        {
            isOnlyHostile = bool.Parse(LaserCannonConfig.program_settings["OnlyHostile"].ToString());
        }

        public void SetLaserStrength()
        {
            laserDamage = float.Parse(LaserCannonConfig.program_settings["Damage"].ToString()) * 0.1f;
            powerConsumption = 1 + laserDamage * 1f;            
        }

        public void SetWarningMessage()
        {
            lowPower_title = LaserCannonConfig.language_settings[LaserCannonConfig.SECTION_LANGUAGE[17]];
            lowPower_message = LaserCannonConfig.language_settings[LaserCannonConfig.SECTION_LANGUAGE[18]];
        }

        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (playerMain.GetVehicle() == thisSeamoth)
                {
                    isPlayerInThisVehicle = true;
                    OnEnable();
                    return;
                }
                else
                {
                    isPlayerInThisVehicle = false;
                    OnDisable();
                    return;
                }
            }
            else
            {
                isPlayerInThisVehicle = false;
                OnDisable();
            }
        }

        private void OnToggle(int slotID, bool state)
        {
            if (thisSeamoth.GetSlotBinding(slotID) == LaserCannonPrefab.TechTypeID)
            {
                isToggle = state;

                if (state)
                {                    
                    OnEnable();
                }
                else
                {                    
                    OnDisable();
                }
            }
        }

        private void SetLaserState(bool value)
        {
            laserRight.SetActive(value);
            laserLeft.SetActive(value);
        }

        public void OnEnable()
        {            
            isActive = isPlayerInThisVehicle && playerMain.isPiloting && isToggle && moduleSlotID > -1;
        }

        public void OnDisable()
        {
            SetLaserState(false);
            loopingEmitter.Stop();
            isActive = false;
            isShoot = false;
            isRepeat = false;
            SetInteractColor(Colors.White);
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

        private void SetBeamTarget()
        {
            if (Targeting.GetTarget(thisSeamoth.gameObject, maxLaserDistance, out targetGameobject, out targetDist))
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

                    AddForceToTarget();
                }
            }
            else
                CalculateBeamVectors(maxLaserDistance);
        }


        private void CalculateBeamVectors(float targetDistance)
        {
            Transform aimTransform = Player.main.camRoot.GetAimingTransform();

            targetPosition = aimTransform.position + targetDistance * aimTransform.forward;

            right_beamPositions[0] = laserRight.transform.position;
            right_beamPositions[1] = targetPosition;

            left_beamPositions[0] = laserLeft.transform.position;
            left_beamPositions[1] = targetPosition;            
        }
                
                
        public void Update()
        {
            if (isActive)
            {
                if (IngameMenu.main.isActiveAndEnabled || PdaMain.state == PDA.State.Opening)
                {
                    thisSeamoth.SlotKeyDown(moduleSlotID);                    
                }
                if (energyMixin.charge < energyMixin.capacity * 0.1f)
                {
                    if (idleTimer > 0f)
                    {
                        isToggle = false;
                        isShoot = false;
                        isRepeat = false;
                        idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                        SetInteractColor(Colors.Red);
                        HandReticle.main.SetInteractText(lowPower_title, lowPower_message, false, false, HandReticle.Hand.None);                        
                    }
                    else
                    {
                        idleTimer = 3;
                        thisSeamoth.SlotKeyDown(moduleSlotID);
                    }
                }

                if (isToggle)
                {
                    if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
                    {
                        isShoot = true;
                    }
                    if (GameInput.GetButtonUp(GameInput.Button.LeftHand))
                    {
                        isShoot = false;
                        SetLaserState(false);
                    }
                    if (isShoot)
                    {
                        RepeatCycle(Time.time, 0.4f);
                    }
                }
            }
        }

        public void LateUpdate()
        {            
            if (isActive)
            {
                if (isShoot && isRepeat)
                {
                    SetBeamTarget();

                    rightBeam.SetPositions(right_beamPositions);                    
                    leftBeam.SetPositions(left_beamPositions);                    

                    SetLaserState(true);

                    loopingEmitter.Play();                                       
                    energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);                    
                }
                else
                {
                    loopingEmitter.Stop();
                    SetLaserState(false);
                }
            }           
        }
        
        private void RepeatCycle(float value, float length)
        {
            float x = Mathf.Repeat(value, length);
           
            if (x < 0.3f && x > 0f)
                isRepeat = true;
            else
                isRepeat = false;
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

