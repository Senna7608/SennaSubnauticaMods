using Common;
using UnityEngine;
using UWE;

namespace RepairModule
{
    public class RepairModuleControl : MonoBehaviour
    {        
        public Vehicle thisVehicle;
        private EnergyMixin energyMixin;
        private FMODAsset weldSoundAsset;
        private FMOD_CustomLoopingEmitter weldSound;

        HandReticle main = HandReticle.main;

        private const float powerConsumption = 5f;
        private float repairPerSec;
        private bool toggle;
        private bool isActive;
        public int moduleSlotID;        
        private float idleTimer = 3f;

        private RepairMode currentMode = RepairMode.None;

        private enum RepairMode
        {
            None,
            Repair,
            Disabled
        };

        public void Awake()
        {
            thisVehicle = gameObject.GetComponent<Vehicle>();            
            energyMixin = thisVehicle.GetComponent<EnergyMixin>();
            weldSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            weldSoundAsset.path = "event:/tools/welder/weld_loop";
            weldSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            weldSound.asset = weldSoundAsset;
            repairPerSec = thisVehicle.liveMixin.maxHealth * 0.1f;                      
        }        

        public void Start()
        {            
            thisVehicle.onToggle += OnToggle;
            Player.main.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }       

        public void OnDestroy()
        {
            thisVehicle.onToggle -= OnToggle;
            Player.main.playerModeChanged.RemoveHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
            Destroy(this);
        }
                     
        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                thisVehicle = Player.main.GetVehicle();
                OnEnable();
            }
            else
            {
                OnDisable();
            }
        }

        private void OnToggle(int slotID, bool state)
        {
            if (thisVehicle.GetSlotBinding(slotID) == RepairModule.TechTypeID)
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
            isActive = Player.main.GetMode() == Player.Mode.LockedPiloting && toggle;            
        }

        public void OnDisable()
        {           
            weldSound.Stop();
            isActive = false;
            toggle = false;
            currentMode = RepairMode.Disabled;            
            Modules.SetInteractColor(Modules.Colors.White);
            Modules.SetProgressColor(Modules.Colors.White);            
        }
          

        public void Update()
        {
            if (isActive)
            {
                if (thisVehicle.liveMixin.health < thisVehicle.liveMixin.maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)                
                {
                    currentMode = RepairMode.Repair;
                    weldSound.Play();
                    
                    energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);                    
                    
                    main.SetIcon(HandReticle.IconType.Progress, 1.5f);
                    thisVehicle.liveMixin.health += Time.deltaTime * repairPerSec;                    
                    main.SetInteractText("Repairing...", false, HandReticle.Hand.None);

                    if (thisVehicle.liveMixin.health < thisVehicle.liveMixin.maxHealth * 0.5f)                    
                    {
                        Modules.SetProgressColor(Modules.Colors.Red);
                        Modules.SetInteractColor(Modules.Colors.Red);
                    }
                    else if (thisVehicle.liveMixin.health < thisVehicle.liveMixin.maxHealth * 0.75f && thisVehicle.liveMixin.health > thisVehicle.liveMixin.maxHealth * 0.5f)                    
                    {
                        Modules.SetProgressColor(Modules.Colors.Yellow);
                        Modules.SetInteractColor(Modules.Colors.Yellow);
                    }
                    else if (thisVehicle.liveMixin.health > thisVehicle.liveMixin.maxHealth * 0.75f)                    
                    {
                        Modules.SetProgressColor(Modules.Colors.Green);
                        Modules.SetInteractColor(Modules.Colors.Green);
                    }

                    main.SetProgress(thisVehicle.liveMixin.health / thisVehicle.liveMixin.maxHealth);                    

                    if (thisVehicle.liveMixin.health >= thisVehicle.liveMixin.maxHealth)                    
                    {
                        thisVehicle.liveMixin.health = thisVehicle.liveMixin.maxHealth;                        
                        currentMode = RepairMode.None;
                        thisVehicle.SlotKeyDown(moduleSlotID);
                        return;
                    }                    
                }

                else if (energyMixin.charge <= energyMixin.capacity * 0.1f)
                {
                    if (idleTimer > 0f)
                    {
                        weldSound.Stop();
                        idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                        Modules.SetInteractColor(Modules.Colors.Red);
                        main.SetInteractText("Warning!\nLow Power!", "Repair Module Disabled!", false, false, HandReticle.Hand.None);
                    }
                    else
                    {                        
                        idleTimer = 3f;
                        thisVehicle.SlotKeyDown(moduleSlotID);
                        return;
                    }
                }

                if (currentMode == RepairMode.None || currentMode == RepairMode.Disabled && thisVehicle.liveMixin.health == thisVehicle.liveMixin.maxHealth)                
                {
                    thisVehicle.SlotKeyDown(moduleSlotID);
                    return;
                }
            }
            else if (currentMode == RepairMode.Repair)
            {
                thisVehicle.SlotKeyDown(moduleSlotID);
            }

        } 
    }
}

