using Common;
using UnityEngine;
using UWE;

namespace RepairModule
{
    public class RepairModuleControl : MonoBehaviour
    {
        public RepairModuleControl Instance { get; private set; }
        public int moduleSlotID { get; set; }
        private Vehicle thisVehicle { get; set; }
        private EnergyMixin energyMixin { get; set; }
        private Player playerMain { get; set; }

        private FMODAsset weldSoundAsset;
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
            Instance = gameObject.GetComponent<RepairModuleControl>();
            thisVehicle = Instance.GetComponent<Vehicle>();            
            energyMixin = thisVehicle.GetComponent<EnergyMixin>();            
            playerMain = Player.main;

            isPlayerInThisVehicle = playerMain.GetVehicle() == thisVehicle ? true : false;
            weldSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            weldSoundAsset.path = "event:/tools/welder/weld_loop";
            weldSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            weldSound.asset = weldSoundAsset;
            repairPerSec = thisVehicle.liveMixin.maxHealth * 0.1f;            
        }

        public void Start()
        {            
            thisVehicle.onToggle += OnToggle;            
            thisVehicle.modules.onAddItem += OnAddItem;
            thisVehicle.modules.onRemoveItem += OnRemoveItem;
            playerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == RepairModule.TechTypeID)
            {
                moduleSlotID = -1;
                Instance.enabled = false;
            }
        }

        private void OnAddItem(InventoryItem item)
        {            
            if (item.item.GetTechType() == RepairModule.TechTypeID)
            {
                if (thisVehicle.GetType() == typeof(Exosuit))
                    moduleSlotID = thisVehicle.GetSlotByItem(item) - 2;
                else
                    moduleSlotID = thisVehicle.GetSlotByItem(item);

                Instance.enabled = true;
            }
        }

        public void OnDestroy()
        {
            thisVehicle.onToggle -= OnToggle;
            thisVehicle.modules.onAddItem -= OnAddItem;
            thisVehicle.modules.onRemoveItem -= OnRemoveItem;
            playerMain.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            Modules.SetInteractColor(Modules.Colors.White);
            Destroy(Instance);            
        }

        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (playerMain.GetVehicle() == thisVehicle)
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
            if (thisVehicle.GetSlotBinding(slotID) == RepairModule.TechTypeID)
            {
                isToggle = state;
                
                if (state)
                {
                    OnEnable();
                    return;
                }
                else
                    OnDisable();
            }            
        }

        private void OnEnable()
        {
            isActive = isPlayerInThisVehicle && playerMain.isPiloting && isToggle && moduleSlotID > -1;            
        }

        private void OnDisable()
        {           
            weldSound.Stop();
            isActive = false;                                
            Modules.SetInteractColor(Modules.Colors.White);
            Modules.SetProgressColor(Modules.Colors.White);            
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
            else if (thisVehicle.liveMixin.health == thisVehicle.liveMixin.maxHealth)                
            {
                thisVehicle.SlotKeyDown(moduleSlotID);
                return;
            }            
        } 
    }
}

