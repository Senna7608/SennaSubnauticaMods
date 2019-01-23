using System.Collections.Generic;
using Common;
using UnityEngine;
using UWE;

namespace LaserCannon
{
    public class LaserCannon_Seamoth : MonoBehaviour
    {
        public LaserCannon_Seamoth Instance { get; private set; }
        public int moduleSlotID { get; set; }

        private SeaMoth thisSeamoth { get; set; }
        private EnergyMixin energyMixin { get; set; }
        private Player playerMain { get; set; }
        private PDA PdaMain { get; set; }

        private FMODAsset shootSound;
        private FMOD_CustomLoopingEmitter loopingEmitter;
        private GameObject laserBeam;
        private LineRenderer lineRenderer;
       
        private float powerConsumption = 1f;
        private float laserDamage = 1f;
        private const float maxLaserDistance = 70f;
        private float idleTimer = 3f;        
        
        private float targetDist;
        private Vector3[] beamPositions = new Vector3[3];
        private GameObject targetGameobject;
        private Color beamcolor = Modules.Colors.Default;

        private bool isToggle;
        private bool isShoot;
        private bool isRepeat;
        private bool isActive;
        private bool isPlayerInThisVehicle;
        private bool isOnlyHostile;

        private string lowPower_title;
        private string lowPower_message;

        public void Awake()
        {
            Instance = gameObject.GetComponent<LaserCannon_Seamoth>();
            thisSeamoth = Instance.GetComponent<SeaMoth>();
            energyMixin = thisSeamoth.GetComponent<EnergyMixin>();
            playerMain = Player.main;
            PdaMain = playerMain.GetPDA();

            isPlayerInThisVehicle = playerMain.GetVehicle() == thisSeamoth ? true : false;

            var repulsionCannonPrefab = Resources.Load<GameObject>("WorldEntities/Tools/RepulsionCannon").GetComponent<RepulsionCannon>();
            //RepulsionCannon repulsionCannonPrefab = CraftData.InstantiateFromPrefab(TechType.RepulsionCannon, false).GetComponent<RepulsionCannon>();
            shootSound = Instantiate(repulsionCannonPrefab.shootSound, thisSeamoth.transform);
            //Destroy(repulsionCannonPrefab);

            loopingEmitter = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            loopingEmitter.asset = shootSound;

            var powerRelayPrefab = Resources.Load<GameObject>("Submarine/Build/PowerTransmitter").GetComponent<PowerFX>();
            //PowerFX powerRelayPrefab = CraftData.InstantiateFromPrefab(TechType.PowerTransmitter, false).GetComponent<PowerFX>();
            laserBeam = Instantiate(powerRelayPrefab.vfxPrefab, thisSeamoth.transform);
            laserBeam.SetActive(false);
            //Destroy(powerRelayPrefab);

            lineRenderer = laserBeam.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.4f;
            lineRenderer.endWidth = 0.4f;
            lineRenderer.receiveShadows = false;
            lineRenderer.loop = false;
            
            SetBeamColor();
            ShootOnlyHostile();
            SetLaserStrength();
            SetWarningMessage();            
        }

        public void Start()
        {
            thisSeamoth.onToggle += OnToggle;
            thisSeamoth.modules.onAddItem += OnAddItem;
            thisSeamoth.modules.onRemoveItem += OnRemoveItem;
            playerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));            
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == LaserCannon.TechTypeID)
            {
                moduleSlotID = -1;
                OnDisable();
                Instance.enabled = false;
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            if (item.item.GetTechType() == LaserCannon.TechTypeID)
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
            Modules.SetInteractColor(Modules.Colors.White);
            Destroy(Instance);
        }

        public void SetBeamColor()
        {
            beamcolor = Modules.Colors.ColorArray[Config.beamColor];            
        }

        public void ShootOnlyHostile()
        {
            isOnlyHostile = bool.Parse(Config.program_settings["OnlyHostile"].ToString());
        }

        public void SetLaserStrength()
        {
            laserDamage = float.Parse(Config.program_settings["Damage"].ToString()) * 0.1f;
            powerConsumption = 1 + laserDamage * 1f;            
        }

        public void SetWarningMessage()
        {
            lowPower_title = Config.language_settings[Config.SECTION_LANGUAGE[17]];
            lowPower_message = Config.language_settings[Config.SECTION_LANGUAGE[18]];
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
            if (thisSeamoth.GetSlotBinding(slotID) == LaserCannon.TechTypeID)
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

        public void OnEnable()
        {            
            isActive = isPlayerInThisVehicle && playerMain.isPiloting && isToggle && moduleSlotID > -1;
        }

        public void OnDisable()
        {
            laserBeam.SetActive(false);
            loopingEmitter.Stop();
            isActive = false;
            isShoot = false;
            isRepeat = false;
            Modules.SetInteractColor(Modules.Colors.White);
        }

        private void AddDamage(GameObject gameObject)
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
                        liveMixin.TakeDamage(laserDamage, position, DamageType.Explosive, null);
                        WorldForces.AddExplosion(position, DayNightCycle.main.timePassed, 5f, 4f);
                    }                        
                }
                else
                {
                    if (gameObject.GetComponent<BreakableResource>() != null)
                    {
                        gameObject.SendMessage("BreakIntoResources", null, SendMessageOptions.DontRequireReceiver);
                        WorldForces.AddExplosion(position, DayNightCycle.main.timePassed, 5f, 4f);
                    }                    
                }
            }
        }

        private Vector3 CalculateLaserBeam()
        {
            Targeting.GetTarget(thisSeamoth.gameObject, maxLaserDistance, out targetGameobject, out targetDist);
            
            if (targetDist == 0f)
            {
                return MainCamera.camera.transform.position + maxLaserDistance * MainCamera.camera.transform.forward;
            }
            else
            {
                if (isOnlyHostile)
                {
                    Targeting.GetRoot(targetGameobject, out TechType targetTechType, out GameObject examinedGameObject);

                    if (!validTargets.Contains(targetTechType))
                    {
                        return MainCamera.camera.transform.position + targetDist * MainCamera.camera.transform.forward;
                    }
                }
                
                AddDamage(targetGameobject);
                return MainCamera.camera.transform.position + targetDist * MainCamera.camera.transform.forward;                
            }
        }
                
        public void Update()
        {
            if (isActive)
            {
                if (isActive && IngameMenu.main.isActiveAndEnabled || isActive && PdaMain.state == PDA.State.Opening)
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
                        Modules.SetInteractColor(Modules.Colors.Red);
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
                        laserBeam.SetActive(false);
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
                    beamPositions[0] = thisSeamoth.torpedoTubeLeft.transform.position;
                    beamPositions[1] = CalculateLaserBeam();
                    beamPositions[2] = thisSeamoth.torpedoTubeRight.transform.position;                    
                    lineRenderer.positionCount = beamPositions.Length;                    
                    lineRenderer.SetPositions(beamPositions);
                    lineRenderer.material.color = Color.Lerp(beamcolor, Color.clear, 0.1f);
                    laserBeam.SetActive(true);                    
                    loopingEmitter.Play();                                       
                    energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);                    
                }
                else
                {
                    loopingEmitter.Stop();
                    laserBeam.SetActive(false);
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

