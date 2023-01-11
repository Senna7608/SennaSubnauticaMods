using UnityEngine;
using UWE;
using static Common.Helpers.GameHelper;

namespace RepairModule
{
    public class RepairModuleControl : MonoBehaviour
    {
        private Vehicle thisVehicle;
        private EnergyMixin energyMixin;
        private int moduleSlotID;
        private FMODAsset weld_loop;
        private FMOD_CustomLoopingEmitter weldSound;
        private HandReticle main = HandReticle.main;
        private const float powerConsumption = 5f;
        private float repairPerSec;
        private bool isToggle;
        private bool isActive;
        private bool isPlayerInThisVehicle;

        private float idleTimer = 3f;        

        public void Awake()
        {            
            thisVehicle = GetComponent<Vehicle>();            
            energyMixin = thisVehicle.GetComponent<EnergyMixin>();

            weld_loop = ScriptableObject.CreateInstance<FMODAsset>();
            weld_loop.name = "weld_loop";
            weld_loop.path = "event:/tools/welder/weld_loop";

            weldSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            weldSound.asset = weld_loop;

            repairPerSec = thisVehicle.liveMixin.maxHealth * 0.1f;

            thisVehicle.onToggle += OnToggle;
            thisVehicle.modules.onAddItem += OnAddItem;
            thisVehicle.modules.onRemoveItem += OnRemoveItem;            

            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));

            moduleSlotID = GetSlotIndex(thisVehicle, RepairModulePrefab.TechTypeID);
        }

        
        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {
            if (newMotorMode == Player.MotorMode.Vehicle)
            {
                if (Player.main.currentMountedVehicle == thisVehicle)
                {
                    isPlayerInThisVehicle = true;
                    OnEnable();
                }
                else
                {
                    isPlayerInThisVehicle = false;
                    OnDisable();
                }
            }
            else
            {
                isPlayerInThisVehicle = false;
                OnDisable();
            }
        }


        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == RepairModulePrefab.TechTypeID)
            {
                moduleSlotID = -1;                
            }
        }

        private void OnAddItem(InventoryItem item)
        {            
            if (item.item.GetTechType() == RepairModulePrefab.TechTypeID)
            {
                if (thisVehicle.GetType() == typeof(Exosuit))
                    moduleSlotID = thisVehicle.GetSlotByItem(item) - 2;
                else
                    moduleSlotID = thisVehicle.GetSlotByItem(item);                
            }
        }

        public void OnDestroy()
        {
            thisVehicle.onToggle -= OnToggle;
            thisVehicle.modules.onAddItem -= OnAddItem;
            thisVehicle.modules.onRemoveItem -= OnRemoveItem;
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
            SetInteractColor(Color.white);                      
        }
        
        private void OnToggle(int slotID, bool state)
        {
            if (thisVehicle.GetSlotBinding(slotID) == RepairModulePrefab.TechTypeID)
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

        private void OnEnable()
        {
            isActive = isPlayerInThisVehicle && Player.main.isPiloting && isToggle && moduleSlotID > -1;            
        }

        private void OnDisable()
        {           
            weldSound.Stop();
            isActive = false;                                
            SetInteractColor(Color.white);
            SetProgressColor(Color.white);            
        }        

        public void Update()
        {
            if (!isActive)
                return;

            else if (thisVehicle.liveMixin.health < thisVehicle.liveMixin.maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)                
            {                
                weldSound.Play();                    
                energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);                    
                main.SetIcon(HandReticle.IconType.Progress, 1.5f);
                thisVehicle.liveMixin.health += Time.deltaTime * repairPerSec;                    
                main.SetText(HandReticle.TextType.Hand, "Repairing...", false, GameInput.Button.None);

                if (thisVehicle.liveMixin.health < thisVehicle.liveMixin.maxHealth * 0.5f)                    
                {
                    SetProgressColor(Color.red);
                    SetInteractColor(Color.red);
                }
                else if (thisVehicle.liveMixin.health < thisVehicle.liveMixin.maxHealth * 0.75f && thisVehicle.liveMixin.health > thisVehicle.liveMixin.maxHealth * 0.5f)                    
                {
                    SetProgressColor(Color.yellow);
                    SetInteractColor(Color.yellow);
                }
                else if (thisVehicle.liveMixin.health > thisVehicle.liveMixin.maxHealth * 0.75f)                    
                {
                    SetProgressColor(Color.green);
                    SetInteractColor(Color.green);
                }

                main.SetProgress(thisVehicle.liveMixin.health / thisVehicle.liveMixin.maxHealth);                    

                if (thisVehicle.liveMixin.health >= thisVehicle.liveMixin.maxHealth)                    
                {
                    thisVehicle.liveMixin.health = thisVehicle.liveMixin.maxHealth;                    
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
                    SetInteractColor(Color.red);
                    main.SetText(HandReticle.TextType.Hand, "Warning!\nLow Power!", false, GameInput.Button.None);
                    main.SetText(HandReticle.TextType.HandSubscript, "Repair Module Disabled!", false, GameInput.Button.None);
                }
                else
                {                        
                    idleTimer = 3f;
                    thisVehicle.SlotKeyDown(moduleSlotID);
                    return;
                }
            }                
            else if (thisVehicle.liveMixin.health == thisVehicle.liveMixin.maxHealth)                
            {
                thisVehicle.SlotKeyDown(moduleSlotID);
                return;
            }            
        } 
    }
}

