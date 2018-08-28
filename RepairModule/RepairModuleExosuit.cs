using Common.Modules;
using UnityEngine;
using System.Collections.Generic;

namespace RepairModule
{
    public class RepairModuleExosuit : MonoBehaviour
    {   
        [AssertNotNull]
        private Exosuit exosuit;
        [AssertNotNull]
        private EnergyMixin energyMixin;
        [AssertNotNull]
        private FMODASRPlayer weldSound;
        HandReticle main = HandReticle.main;

        private const float powerConsumption = 5f;
        private float repairPerSec;
        private bool toggle;
        private bool isActive;
        public int moduleSlotID;
        private float maxHealth;
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
            exosuit = gameObject.GetComponent<Exosuit>();
            energyMixin = exosuit.GetComponent<EnergyMixin>();
            var welderPrefab = Resources.Load<GameObject>("WorldEntities/Tools/Welder").GetComponent<Welder>();
            //var welderPrefab = CraftData.InstantiateFromPrefab(TechType.Welder, false).GetComponent<Welder>();
            weldSound = Instantiate(welderPrefab.weldSound, gameObject.transform);
            Destroy(welderPrefab);
            repairPerSec = exosuit.liveMixin.maxHealth * 0.1f;
            maxHealth = exosuit.liveMixin.maxHealth;
        }        

        public void Start()
        {
            exosuit.onToggle += OnToggle;           
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
            if (exosuit.GetSlotBinding(slotID) == RepairModule.TechTypeID)
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
            isActive = Player.main.inExosuit && toggle;
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
                if (exosuit.liveMixin.health < maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)                
                {
                    currentMode = RepairMode.Repair;
                    weldSound.Play();
                    
                    energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);                    
                    
                    main.SetIcon(HandReticle.IconType.Progress, 1.5f);
                    exosuit.liveMixin.health += Time.deltaTime * repairPerSec;                    
                    main.SetInteractText("Repairing...", false, HandReticle.Hand.None);

                    if (exosuit.liveMixin.health < maxHealth * 0.5f)                    
                    {
                        Modules.SetProgressColor(Modules.Colors.Red);
                        Modules.SetInteractColor(Modules.Colors.Red);

                    }
                    else if (exosuit.liveMixin.health < maxHealth * 0.75f && exosuit.liveMixin.health > maxHealth * 0.5f)                    
                    {
                        Modules.SetProgressColor(Modules.Colors.Yellow);
                        Modules.SetInteractColor(Modules.Colors.Yellow);
                    }
                    else if (exosuit.liveMixin.health > maxHealth * 0.75f)                    
                    {
                        Modules.SetProgressColor(Modules.Colors.Green);
                        Modules.SetInteractColor(Modules.Colors.Green);
                    }

                    main.SetProgress(exosuit.liveMixin.health / maxHealth);                    

                    if (exosuit.liveMixin.health >= maxHealth)                    
                    {                        
                        exosuit.liveMixin.health = maxHealth;                        
                        currentMode = RepairMode.None;
                        OnToggle(moduleSlotID, false);
                        return;
                    }                    
                }

                else if (energyMixin.charge <= energyMixin.capacity * 0.1f)
                {
                    if (idleTimer > 0f)
                    {
                        weldSound.Stop();
                        idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                        main.SetInteractText("Warning!\nLow Power!", "Repair Module Disabled!", false, false, HandReticle.Hand.None);
                    }
                    else
                    {                        
                        idleTimer = 3f;
                        OnToggle(moduleSlotID, false);
                        return;
                    }
                }

                if (currentMode == RepairMode.None || currentMode == RepairMode.Disabled && exosuit.liveMixin.health == exosuit.liveMixin.maxHealth)                
                {
                    //OnToggle(moduleSlotID, false);
                    
                    return;
                }
            }
            else if (currentMode == RepairMode.Repair)
            {
                OnToggle(moduleSlotID, false);
            }

        }        
    }
}

