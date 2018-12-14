using System.Collections.Generic;
using Common;
using UnityEngine;
using UWE;

namespace LaserCannon
{
    public class LaserCannon_Seamoth : MonoBehaviour
    {
        public static LaserCannon_Seamoth Main { get; private set; }

        
        public SeaMoth seamoth;
        private EnergyMixin energyMixin; 
        private FMODAsset shootSound;
        private FMOD_CustomLoopingEmitter loopingEmitter;
        private GameObject laserBeam;
        private LineRenderer lineRenderer;
       
        private float powerConsumption = 1f;
        private float laserDamage = 1f;
        private const float maxLaserDistance = 70f;

        private float idleTimer = 3f;
        private bool toggle;
        public int slotID;
        private bool shoot = false;
        private bool repeat;
        private bool laserCannonEnabled;
        private float targetDist;
        private Vector3[] beamPositions = new Vector3[3];       
        private GameObject targetGameobject;        
        private Color beamcolor = Modules.Colors.Default;
        private bool onlyHostile = false; 

        public void Awake()
        {
            Main = this;

            seamoth = gameObject.GetComponent<SeaMoth>();
            energyMixin = seamoth.GetComponent<EnergyMixin>();
            var repulsionCannonPrefab = Resources.Load<GameObject>("WorldEntities/Tools/RepulsionCannon").GetComponent<RepulsionCannon>();
            //RepulsionCannon repulsionCannonPrefab = CraftData.InstantiateFromPrefab(TechType.RepulsionCannon, false).GetComponent<RepulsionCannon>();
            shootSound = Instantiate(repulsionCannonPrefab.shootSound, seamoth.transform);
            //Destroy(repulsionCannonPrefab);

            loopingEmitter = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            loopingEmitter.asset = shootSound;

            var powerRelayPrefab = Resources.Load<GameObject>("Submarine/Build/PowerTransmitter").GetComponent<PowerFX>();
            //PowerFX powerRelayPrefab = CraftData.InstantiateFromPrefab(TechType.PowerTransmitter, false).GetComponent<PowerFX>();
            laserBeam = Instantiate(powerRelayPrefab.vfxPrefab, seamoth.transform);
            laserBeam.SetActive(false);
            //Destroy(powerRelayPrefab);

            lineRenderer = laserBeam.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.4f;
            lineRenderer.endWidth = 0.4f;
            lineRenderer.receiveShadows = false;
            lineRenderer.loop = false;
            
            SetBeamColor();
        }

        private void Start()
        { 
            seamoth.onToggle += OnToggle;
            Player.main.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));            
        }

        public void OnDestroy()
        {
            seamoth.onToggle -= OnToggle;
            Player.main.playerModeChanged.RemoveHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
            Destroy(this);
        }

        public void SetBeamColor()
        {
            beamcolor = Modules.Colors.ColorArray[Config.beamColor];            
        }

        public void ShootOnlyHostile()
        {
            onlyHostile = bool.Parse(Config.program_settings["OnlyHostile"].ToString());
        }

        public void SetLaserStrength()
        {
            laserDamage = float.Parse(Config.program_settings["Damage"].ToString()) * 0.1f;
            powerConsumption = 1 + laserDamage * 1f;            
        }

        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {                
                OnEnable();
            }
            else
            {
                OnDisable();
            }
        }

        private void OnToggle(int slotID, bool state)
        {
            if (seamoth.GetSlotBinding(slotID) == LaserCannon.TechTypeID)
            {
                toggle = state;

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
            laserCannonEnabled = Player.main.inSeamoth && toggle;
        }

        public void OnDisable()
        {            
            laserBeam.SetActive(false);
            loopingEmitter.Stop();
            laserCannonEnabled = false;
            shoot = false;
            repeat = false;
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
            Targeting.GetTarget(seamoth.gameObject, maxLaserDistance, out targetGameobject, out targetDist);
            
            if (targetDist == 0f)
            {
                return MainCamera.camera.transform.position + maxLaserDistance * MainCamera.camera.transform.forward;
            }
            else
            {
                if (onlyHostile)
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
            if (laserCannonEnabled)
            { 
                if (energyMixin.charge < energyMixin.capacity * 0.1f)
                {
                    if (idleTimer > 0f)
                    {
                        toggle = false;
                        shoot = false;
                        repeat = false;
                        idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                        HandReticle.main.SetInteractText("Warning!\nLow Power!", "Laser Cannon Disabled!", false, false, HandReticle.Hand.None);
                        Modules.SetInteractColor(Modules.Colors.Red);
                    }
                    else
                    {
                        idleTimer = 3;
                        seamoth.SlotKeyDown(slotID);
                    }
                }

                if (toggle)
                {
                    if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
                    {
                        shoot = true;
                    }
                    if (GameInput.GetButtonUp(GameInput.Button.LeftHand))
                    {
                        shoot = false;
                        laserBeam.SetActive(false);
                    }
                    if (shoot)
                    {
                        RepeatCycle(Time.time, 0.4f);
                    }
                }
            }                   
        }
        
        public void LateUpdate()
        {
            if (laserCannonEnabled)
            {
                if (shoot && repeat)
                {                    
                    beamPositions[0] = seamoth.torpedoTubeLeft.transform.position;
                    beamPositions[1] = CalculateLaserBeam();
                    beamPositions[2] = seamoth.torpedoTubeRight.transform.position;                    
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
                repeat = true;
            else
                repeat = false;
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

