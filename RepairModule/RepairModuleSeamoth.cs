using Common;
using UnityEngine;
using UWE;

namespace RepairModule
{
    public class RepairModuleSeamoth : MonoBehaviour
    {
        [AssertNotNull]
        private SeaMoth seamoth;
        [AssertNotNull]
        private EnergyMixin energyMixin;
        [AssertNotNull]
        private FMODASRPlayer weldSound;
        private HandReticle main = HandReticle.main;
        private const float powerConsumption = 5f;
        private float repairPerSec;
        private float maxHealth;
        private float idleTimer = 3f;
        private bool toggle;
        public bool isActive;
        public int slotID;
        private RepairMode currentMode = RepairMode.None;

        private enum RepairMode
        {
            None,            
            Repair,
            Disabled
        };

        public void Awake()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();
            energyMixin = seamoth.GetComponent<EnergyMixin>();
            var welderPrefab = Resources.Load<GameObject>("WorldEntities/Tools/Welder").GetComponent<Welder>();
            //var welderPrefab = CraftData.InstantiateFromPrefab(TechType.Welder, false).GetComponent<Welder>();
            weldSound = Instantiate(welderPrefab.weldSound, gameObject.transform);
            Destroy(welderPrefab);

            repairPerSec = seamoth.liveMixin.maxHealth * 0.1f;
            maxHealth = seamoth.liveMixin.maxHealth;
        }        

        private void Start()
        {
            seamoth.onToggle += OnToggle;
            Utils.GetLocalPlayerComp().playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
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
            if (seamoth.GetSlotBinding(slotID) == RepairModule.TechTypeID)
            {
                toggle = state;                              

                if (toggle)
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
            isActive = Player.main.inSeamoth && toggle;
        }

        public void OnDisable()
        {
            weldSound.Stop();
            isActive = false;
            currentMode = RepairMode.Disabled;
            Modules.SetProgressColor(Modules.Colors.White);
            Modules.SetInteractColor(Modules.Colors.White);
        }

        public void Update()
        {
            if (isActive)
            {
                if (seamoth.liveMixin.health < maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)
                {                    
                    currentMode = RepairMode.Repair;
                    weldSound.Play();

                    energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);
                    
                    main.SetIcon(HandReticle.IconType.Progress, 1.5f);                    
                    seamoth.liveMixin.health += Time.deltaTime * repairPerSec;
                    main.SetInteractText("Repairing...", false, HandReticle.Hand.None);

                    if (seamoth.liveMixin.health < maxHealth * 0.5f)
                    {
                        Modules.SetProgressColor(Modules.Colors.Red);
                        Modules.SetInteractColor(Modules.Colors.Red);
                    }
                    else if (seamoth.liveMixin.health < maxHealth * 0.75f && seamoth.liveMixin.health > maxHealth * 0.5f)
                    {
                        Modules.SetProgressColor(Modules.Colors.Yellow);
                        Modules.SetInteractColor(Modules.Colors.Yellow);
                    }
                    else if (seamoth.liveMixin.health > maxHealth * 0.75f)
                    {
                        Modules.SetProgressColor(Modules.Colors.Green);
                        Modules.SetInteractColor(Modules.Colors.Green);
                    }

                    main.SetProgress(seamoth.liveMixin.health / maxHealth);

                    if (seamoth.liveMixin.health >= seamoth.liveMixin.maxHealth)
                    {                        
                        seamoth.liveMixin.health = seamoth.liveMixin.maxHealth;
                        currentMode = RepairMode.None;
                        seamoth.SlotKeyDown(slotID);
                    }                    
                }

                else if (energyMixin.charge <= energyMixin.capacity * 0.1f)
                {                    
                    if (idleTimer > 0f)
                    {
                        weldSound.Stop();
                        idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                        main.SetInteractText("Warning!\nLow Power!", "Repair Module Disabled!", false, false, HandReticle.Hand.None);
                        Modules.SetInteractColor(Modules.Colors.Red);
                    }
                    else
                    {                        
                        idleTimer = 3;                        
                        seamoth.SlotKeyDown(slotID);                        
                    }
                }

                if (currentMode == RepairMode.None || currentMode == RepairMode.Disabled && seamoth.liveMixin.health == maxHealth)
                {                                     
                    seamoth.SlotKeyDown(slotID);
                    return;
                }
            }
            else if (currentMode == RepairMode.Repair)
            {                
                seamoth.SlotKeyDown(slotID);
            }            
        }        
    }
}

