using System;
using UnityEngine;

namespace RepairModule
{
    public class RepairModuleExosuit : MonoBehaviour
    {        
        private Exosuit exosuit;
        private EnergyMixin energyMixin;
        private FMODASRPlayer weldSound;
        HandReticle main = HandReticle.main;

        private const float powerConsumption = 5f;
        private float repairPerSec;
        public bool toggle;
        public int slotID;
        private float maxHealth;
        private float idleTimer = 5f;

        public void Awake()
        {
            exosuit = gameObject.GetComponent<Exosuit>();
            repairPerSec = exosuit.liveMixin.maxHealth * 0.1f;
            maxHealth = exosuit.liveMixin.maxHealth;
        }

        private void Start()
        {            
            energyMixin = exosuit.GetComponent<EnergyMixin>();
            var welder = Resources.Load<GameObject>("WorldEntities/Tools/Welder").GetComponent<Welder>();
            weldSound = Instantiate(welder.weldSound, gameObject.transform);            
        }             

        private void ResetProgressColor()
        {
            main.progressText.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
            main.progressImage.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        }

        private void Update()
        {
            if (toggle)
            {
                if (exosuit.liveMixin.health < exosuit.liveMixin.maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)
                {
                    weldSound.Play();
                    float amount = powerConsumption * Time.deltaTime;
                    energyMixin.ConsumeEnergy(amount);                    
                    
                    main.SetIcon(HandReticle.IconType.Progress, 1.5f);
                    exosuit.liveMixin.health += Time.deltaTime * repairPerSec;
                    main.SetInteractText("Repairing...", false, HandReticle.Hand.None);

                    if (exosuit.liveMixin.health < maxHealth * 0.5f)
                    {
                        main.progressText.color = new Color32(255, 0, 0, byte.MaxValue);
                        main.progressImage.color = new Color32(255, 0, 0, byte.MaxValue);
                    }
                    else if (exosuit.liveMixin.health < maxHealth * 0.75f && exosuit.liveMixin.health > maxHealth * 0.5f)
                    {
                        main.progressText.color = new Color32(255, 255, 0, byte.MaxValue);
                        main.progressImage.color = new Color32(255, 255, 0, byte.MaxValue);
                    }
                    else if (exosuit.liveMixin.health > maxHealth * 0.75f)
                    {
                        main.progressText.color = new Color32(0, 255, 0, byte.MaxValue);
                        main.progressImage.color = new Color32(0, 255, 0, byte.MaxValue);
                    }
                    
                    main.SetProgress(exosuit.liveMixin.health / maxHealth);

                    if (exosuit.liveMixin.health > maxHealth)
                    {
                        weldSound.Stop();                        
                        exosuit.liveMixin.health = maxHealth;                        
                        toggle = false;
                        ResetProgressColor();
                        return;
                    }                    
                }

                else if (energyMixin.charge <= energyMixin.capacity * 0.2f)
                {
                    if (idleTimer > 0f)
                    {
                        weldSound.Stop();
                        idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                        main.SetInteractText("Warning! Low Power!", "Repair Module Disabled!", false, false, HandReticle.Hand.None);
                    }
                    else
                    {
                        toggle = false;
                        exosuit.SlotKeyDown(slotID);
                        idleTimer = 5;
                        ResetProgressColor();
                        return;
                    }
                }

                else
                {
                    toggle = false;
                    exosuit.SlotKeyDown(slotID);
                    idleTimer = 5;
                    ResetProgressColor();
                    return;
                }

            }
            
        }        
    }
}

