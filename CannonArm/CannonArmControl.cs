using Common;
using UnityEngine;
using UWE;
using Common.DebugHelper;

namespace CannonArm
{
    public class CannonArmControl : MonoBehaviour
    {
        public CannonArmControl Instance { get; private set; }
        public GameObject ArmGameObject { get; set; }

        public int moduleSlotID { get; set; }
        private Exosuit thisExosuit { get; set; }
        private EnergyMixin energyMixin { get; set; }
        private Player playerMain { get; set; }        

        //private FMODAsset weldSoundAsset;
        //private FMOD_CustomLoopingEmitter weldSound;
        private HandReticle main = HandReticle.main;
        private const float powerConsumption = 5f;
        //private float repairPerSec;
        private bool isToggle;
        private bool isActive;
        private bool isPlayerInThisVehicle;
        private float idleTimer = 3f;

        //------------------------------------------------------

        private const float cooldownTime = 5f;
        private const float cooldownInterval = 1f;
        [AssertNotNull]
        public Transform siloFirst;
        [AssertNotNull]
        public Transform siloSecond;
        [AssertNotNull]
        public GameObject visualTorpedoFirst;
        [AssertNotNull]
        public GameObject visualTorpedoSecond;
        [AssertNotNull]
        public GameObject visualTorpedoReload;
        public Animator animator;
        public FMODAsset fireSound;
        public FMODAsset torpedoDisarmed;
        private ItemsContainer container;
        private float timeFirstShot = float.NegativeInfinity;
        private float timeSecondShot = float.NegativeInfinity;





        public void Awake()
        {
            Instance = gameObject.GetComponent<CannonArmControl>();
            
            

            thisExosuit = Instance.GetComponent<Exosuit>();            
            energyMixin = thisExosuit.GetComponent<EnergyMixin>();            
            playerMain = Player.main;            

            isPlayerInThisVehicle = playerMain.GetVehicle() == thisExosuit ? true : false;

            //weldSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            //weldSoundAsset.path = "event:/tools/welder/weld_loop";
            //weldSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            //weldSound.asset = weldSoundAsset;
            //repairPerSec = thisVehicle.liveMixin.maxHealth * 0.1f;
                    
               
        }

        public void Start()
        {                       
            thisExosuit.modules.onAddItem += OnAddItem;
            thisExosuit.modules.onRemoveItem += OnRemoveItem;
            playerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == CannonArm.TechTypeID)
            {
                moduleSlotID = -1;
                Instance.enabled = false;
            }
        }

        private void OnAddItem(InventoryItem item)
        {            
            if (item.item.GetTechType() == CannonArm.TechTypeID)
            {                
                moduleSlotID = thisExosuit.GetSlotByItem(item);

                Instance.enabled = true;
            }
        }

        public void OnDestroy()
        {            
            thisExosuit.modules.onAddItem -= OnAddItem;
            thisExosuit.modules.onRemoveItem -= OnRemoveItem;
            playerMain.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            Modules.SetInteractColor(Modules.Colors.White);
            Destroy(Instance);            
        }

        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (playerMain.GetVehicle() == thisExosuit)
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
               

        private void OnEnable()
        {
            isActive = isPlayerInThisVehicle && playerMain.isPiloting && moduleSlotID > -1;            
        }

        private void OnDisable()
        {           
            //weldSound.Stop();
            isActive = false;                                
            Modules.SetInteractColor(Modules.Colors.White);
            Modules.SetProgressColor(Modules.Colors.White);            
        }        

        /*
        public void Update()
        {
            if (!isActive)
                return;

            else if (thisExosuit.liveMixin.health < thisExosuit.liveMixin.maxHealth && energyMixin.charge > energyMixin.capacity * 0.1f)                
            {                
                //weldSound.Play();                    
                energyMixin.ConsumeEnergy(powerConsumption * Time.deltaTime);                    
                main.SetIcon(HandReticle.IconType.Progress, 1.5f);
                thisExosuit.liveMixin.health += Time.deltaTime * repairPerSec;                    
                main.SetInteractText("Repairing...", false, HandReticle.Hand.None);

                if (thisExosuit.liveMixin.health < thisExosuit.liveMixin.maxHealth * 0.5f)                    
                {
                    Modules.SetProgressColor(Modules.Colors.Red);
                    Modules.SetInteractColor(Modules.Colors.Red);
                }
                else if (thisExosuit.liveMixin.health < thisExosuit.liveMixin.maxHealth * 0.75f && thisExosuit.liveMixin.health > thisExosuit.liveMixin.maxHealth * 0.5f)                    
                {
                    Modules.SetProgressColor(Modules.Colors.Yellow);
                    Modules.SetInteractColor(Modules.Colors.Yellow);
                }
                else if (thisExosuit.liveMixin.health > thisExosuit.liveMixin.maxHealth * 0.75f)                    
                {
                    Modules.SetProgressColor(Modules.Colors.Green);
                    Modules.SetInteractColor(Modules.Colors.Green);
                }

                main.SetProgress(thisExosuit.liveMixin.health / thisExosuit.liveMixin.maxHealth);                    

                if (thisExosuit.liveMixin.health >= thisExosuit.liveMixin.maxHealth)                    
                {
                    thisExosuit.liveMixin.health = thisExosuit.liveMixin.maxHealth;                    
                    thisExosuit.SlotKeyDown(moduleSlotID);
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
                    thisExosuit.SlotKeyDown(moduleSlotID);
                    return;
                }
            }                
            else if (thisExosuit.liveMixin.health == thisExosuit.liveMixin.maxHealth)                
            {
                thisExosuit.SlotKeyDown(moduleSlotID);
                return;
            }            
        }
       

        public GameObject GetGameObject()
        {
            return Instance.GetComponent<CannonArm>().GetGameObject();
        }

        void IExosuitArm.SetSide(Exosuit.Arm arm)
        {
            Exosuit componentInParent = GetComponentInParent<Exosuit>();
            if (container != null)
            {
                container.onAddItem -= OnAddItem;
                container.onRemoveItem -= OnRemoveItem;
            }
            if (arm == Exosuit.Arm.Right)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                container = componentInParent.GetStorageInSlot(componentInParent.GetSlotIndex("ExosuitArmRight"), TechType.ExosuitTorpedoArmModule);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                container = componentInParent.GetStorageInSlot(componentInParent.GetSlotIndex("ExosuitArmLeft"), TechType.ExosuitTorpedoArmModule);
            }
            if (container != null)
            {
                container.onAddItem += OnAddItem;
                container.onRemoveItem += OnRemoveItem;
            }
            UpdateVisuals();
        }


        bool IExosuitArm.OnUseDown(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, true);
        }

        bool IExosuitArm.OnUseHeld(out float cooldownDuration)
        {
            return TryShoot(out cooldownDuration, false);
        }

        bool IExosuitArm.OnUseUp(out float cooldownDuration)
        {
            animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return true;
        }

        bool IExosuitArm.OnAltDown()
        {
            return false;
        }

        void IExosuitArm.Update(ref Quaternion aimDirection)
        {
        }

        void IExosuitArm.Reset()
        {
            animator.SetBool("use_tool", false);
        }

        private bool TryShoot(out float cooldownDuration, bool verbose)
        {
            Exosuit componentInParent = GetComponentInParent<Exosuit>();
            TorpedoType[] torpedoTypes = componentInParent.torpedoTypes;
            TorpedoType torpedoType = null;
            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                if (container.Contains(torpedoTypes[i].techType))
                {
                    torpedoType = torpedoTypes[i];
                    break;
                }
            }
            float num = Mathf.Clamp(Time.time - timeFirstShot, 0f, 5f);
            float num2 = Mathf.Clamp(Time.time - timeSecondShot, 0f, 5f);
            float b = 5f - num;
            float b2 = 5f - num2;
            float num3 = Mathf.Min(num, num2);
            if (num3 < 1f)
            {
                cooldownDuration = 0f;
                return false;
            }
            if (num >= 5f)
            {
                if (Shoot(torpedoType, siloFirst, verbose))
                {
                    timeFirstShot = Time.time;
                    cooldownDuration = Mathf.Max(1f, b2);
                    return true;
                }
            }
            else
            {
                if (num2 < 5f)
                {
                    cooldownDuration = 0f;
                    return false;
                }
                if (Shoot(torpedoType, siloSecond, verbose))
                {
                    timeSecondShot = Time.time;
                    cooldownDuration = Mathf.Max(1f, b);
                    return true;
                }
            }
            animator.SetBool("use_tool", false);
            cooldownDuration = 0f;
            return false;
        }

        private bool Shoot(TorpedoType torpedoType, Transform siloTransform, bool verbose)
        {
            if (Vehicle.TorpedoShot(container, torpedoType, siloTransform))
            {
                Utils.PlayFMODAsset(fireSound, siloTransform, 20f);
                animator.SetBool("use_tool", true);
                if (container.count == 0)
                {
                    Utils.PlayFMODAsset(torpedoDisarmed, transform, 1f);
                }
                return true;
            }
            if (verbose)
            {
                ErrorMessage.AddError(Language.main.Get("VehicleTorpedoNoAmmo"));
            }
            return false;
        }

        private void UpdateVisuals()
        {
            Exosuit componentInParent = GetComponentInParent<Exosuit>();
            int num = 0;
            TorpedoType[] torpedoTypes = componentInParent.torpedoTypes;
            for (int i = 0; i < torpedoTypes.Length; i++)
            {
                num += container.GetCount(torpedoTypes[i].techType);
            }
            visualTorpedoReload.SetActive(num >= 3);
            visualTorpedoSecond.SetActive(num >= 2);
            visualTorpedoFirst.SetActive(num >= 1);
        }*/
    }
}

