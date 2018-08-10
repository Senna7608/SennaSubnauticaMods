using UnityEngine;

namespace RepairModule
{
    public class RepairModuleSeamoth : MonoBehaviour
    {      
        public SeaMoth seamoth;
        private EnergyMixin energyMixin;
        public FMODASRPlayer weldSound;
        private HandReticle main = HandReticle.main;
        private const float powerConsumption = 5f;
        private float repairPerSec;
        private float maxHealth;
        private float idleTimer = 5f;
        public bool toggle;
        public int slotID;        

        public void Awake()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();
            repairPerSec = seamoth.liveMixin.maxHealth * 0.1f;
            maxHealth = seamoth.liveMixin.maxHealth;
        }        

        private void Start()
        {
            energyMixin = seamoth.GetComponent<EnergyMixin>();
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
                if (seamoth.liveMixin.health < seamoth.liveMixin.maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)
                {
                    weldSound.Play();
                    float amount = powerConsumption * Time.deltaTime;
                    energyMixin.ConsumeEnergy(amount);
                    
                    main.SetIcon(HandReticle.IconType.Progress, 1.5f);                    
                    seamoth.liveMixin.health += Time.deltaTime * repairPerSec;
                    main.SetInteractText("Repairing...", false, HandReticle.Hand.None);

                    if (seamoth.liveMixin.health < maxHealth * 0.5f)
                    {
                        main.progressText.color = new Color32(255, 0, 0, byte.MaxValue);
                        main.progressImage.color = new Color32(255, 0, 0, byte.MaxValue);
                    }
                    else if (seamoth.liveMixin.health < maxHealth * 0.75f && seamoth.liveMixin.health > maxHealth * 0.5f)
                    {
                        main.progressText.color = new Color32(255, 255, 0, byte.MaxValue);
                        main.progressImage.color = new Color32(255, 255, 0, byte.MaxValue);
                    }
                    else if (seamoth.liveMixin.health > maxHealth * 0.75f)
                    {
                        main.progressText.color = new Color32(0, 255, 0, byte.MaxValue);
                        main.progressImage.color = new Color32(0, 255, 0, byte.MaxValue);
                    }

                    main.SetProgress(seamoth.liveMixin.health / maxHealth);

                    if (seamoth.liveMixin.health > seamoth.liveMixin.maxHealth)
                    {
                        weldSound.Stop();
                        seamoth.liveMixin.health = seamoth.liveMixin.maxHealth;
                        seamoth.SlotKeyDown(slotID);
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
                        seamoth.SlotKeyDown(slotID);
                        idleTimer = 5;
                        ResetProgressColor();
                        return;
                    }
                }
                else
                {
                    toggle = false;
                    seamoth.SlotKeyDown(slotID);
                    idleTimer = 5;
                    ResetProgressColor();
                    return;
                }

            }
            
        }        
    }
}

