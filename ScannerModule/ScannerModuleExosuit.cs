using Common;
using UnityEngine;
using UWE;

namespace ScannerModule
{
    public class ScannerModuleExosuit : MonoBehaviour
    {
        public ScannerModuleExosuit Instance { get; private set; }
        public int moduleSlotID { get; set; }
        private Exosuit thisExosuit { get; set; }
        private Player playerMain { get; set; }
        private EnergyMixin energyMixin { get; set; }
        private HandReticle handReticleMain { get; set; }

        private FMOD_CustomLoopingEmitter scanSound;
        private FMODAsset completeSoundAsset;
        private FMODAsset scanSoundAsset;
        
        private const float powerConsumption = 0.5f;
        private const float scanDistance = 50f;        
        private float idleTimer;        

        private bool isToggle;
        private bool isScanning;
        private bool isActive;
        private bool isPlayerInThisExosuit;

        private ScanState stateLast;
        private ScanState stateCurrent;

        public enum ScanState
        {
            None,
            Scan
        }

        public void Awake()
        {
            Instance = gameObject.GetComponent<ScannerModuleExosuit>();
            thisExosuit = Instance.GetComponent<Exosuit>();
            energyMixin = thisExosuit.GetComponent<EnergyMixin>();
            playerMain = Player.main;
            handReticleMain = HandReticle.main;

            isPlayerInThisExosuit = playerMain.GetVehicle() == thisExosuit ? true : false;
            scanSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            scanSoundAsset.path = "event:/tools/scanner/scan_loop";
            scanSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            scanSound.asset = scanSoundAsset;
            completeSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            completeSoundAsset.path = "event:/tools/scanner/scan_complete";            
        }

        public void Start()
        {
            thisExosuit.onToggle += OnToggle;
            thisExosuit.modules.onAddItem += OnAddItem;
            thisExosuit.modules.onRemoveItem += OnRemoveItem;
            playerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == ScannerModule.TechTypeID)
            {
                moduleSlotID = -1;
                Instance.enabled = false;
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            if (item.item.GetTechType() == ScannerModule.TechTypeID)
            {
                moduleSlotID = thisExosuit.GetSlotByItem(item) - 2;
                Instance.enabled = true;
            }
        }

        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (playerMain.GetVehicle() == thisExosuit)
                {
                    isPlayerInThisExosuit = true;
                    OnEnable();
                    return;
                }
                else
                {
                    isPlayerInThisExosuit = false;
                    OnDisable();
                    return;
                }
            }
            else
            {
                isPlayerInThisExosuit = false;
                OnDisable();
            }
        }

        private void OnToggle(int slotID, bool state)
        {
            if (thisExosuit.GetSlotBinding(slotID) == ScannerModule.TechTypeID)
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
            isActive = isPlayerInThisExosuit && playerMain.isPiloting && isToggle && moduleSlotID > -1;
        }

        public void OnDisable()
        {
            scanSound.Stop();
            isActive = false;
            isToggle = false;
            stateCurrent = ScanState.None;
            Modules.SetInteractColor(Modules.Colors.White);
            Modules.SetProgressColor(Modules.Colors.White);
        }

        public void Update()
        {
            if (isActive)
            {
                PDAScanner.Result result = Scan();
                
                if (!isScanning)
                {
                    if (result != PDAScanner.Result.Known)
                    {                        
                        handReticleMain.SetInteractText("AutoScan", "Active", false, false, HandReticle.Hand.None);
                        handReticleMain.SetIcon(HandReticle.IconType.Scan, 1.5f);
                    }
                }
                else
                {
                    handReticleMain.SetInteractText(PDAScanner.scanTarget.techType.AsString(false), true, HandReticle.Hand.None);
                    handReticleMain.SetIcon(HandReticle.IconType.Progress, 10f);
                    handReticleMain.progressText.text = Mathf.RoundToInt(PDAScanner.scanTarget.progress * 100f) + "%";
                    Modules.SetProgressColor(Modules.Colors.Orange);
                    handReticleMain.progressImage.fillAmount = Mathf.Clamp01(PDAScanner.scanTarget.progress);
                    handReticleMain.SetProgress(PDAScanner.scanTarget.progress);                    
                }

                if (idleTimer > 0f)
                {
                    idleTimer = Mathf.Max(0f, idleTimer - Time.deltaTime);
                }                
            }            
        }       

        private void LateUpdate()
        {
            if (isActive)
            {
                isScanning = stateCurrent == ScanState.Scan;                

                if (idleTimer <= 0f)
                {
                    OnHover();
                }                

                if (isScanning)
                {
                    scanSound.Play();
                }
                else
                {
                    scanSound.Stop();                    
                }
                stateLast = stateCurrent;
                stateCurrent = ScanState.None;
            }
        } 
                
        private PDAScanner.Result Scan()
        {
            if (stateCurrent != ScanState.None)
            {
                return PDAScanner.Result.None;
            }
            if (idleTimer > 0f)
            {
                return PDAScanner.Result.None;
            }

            PDAScanner.Result result = PDAScanner.Result.None;
            PDAScanner.ScanTarget scanTarget = PDAScanner.scanTarget;
            PDAScanner.UpdateTarget(scanDistance, false);

            if (scanTarget.isValid && energyMixin.charge > 0f)
            {
                result = PDAScanner.Scan();
                
                if (result == PDAScanner.Result.Scan)
                {
                    float amount = powerConsumption * Time.deltaTime;
                    energyMixin.ConsumeEnergy(amount);
                    stateCurrent = ScanState.Scan;
                    isScanning = true;
                }
                else if (result == PDAScanner.Result.Done || result == PDAScanner.Result.Researched)
                {                    
                    idleTimer = 0.5f;
                    PDASounds.queue.PlayIfFree(completeSoundAsset);                    
                }                
            }
            return result;
        }        

        private void OnHover()
        {
            if (energyMixin.charge <= 0f)
            {                
                return;
            }

            PDAScanner.ScanTarget scanTarget = PDAScanner.scanTarget;
            
            PDAScanner.UpdateTarget(scanDistance, false);

            if (!scanTarget.isValid)
            {                
                return;
            }

            PDAScanner.Result result = PDAScanner.CanScan();                    
        }

        private void OnDestroy()
        {
            playerMain.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            thisExosuit.onToggle -= OnToggle;
            thisExosuit.modules.onAddItem -= OnAddItem;
            thisExosuit.modules.onRemoveItem -= OnRemoveItem;
        }
    }
}

