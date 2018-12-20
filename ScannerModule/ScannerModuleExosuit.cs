using Common;
using UnityEngine;
using UWE;

namespace ScannerModule
{
    public class ScannerModuleExosuit : MonoBehaviour
    {        
       
        private EnergyMixin energyMixin;        
        public FMOD_CustomLoopingEmitter scanSound;        
        public FMODAsset completeSoundAsset;        
        public FMODAsset scanSoundAsset;       
        private Exosuit exosuit;

        public const float powerConsumption = 0.5f;
        public const float scanDistance = 50f;        
        private bool isScanning = false;
        public bool toggle; 
        private ScanState stateLast;
        private ScanState stateCurrent;
        private float idleTimer;
        private bool isActive;
        
        HandReticle main = HandReticle.main;

        public enum ScanState
        {
            None,
            Scan
        }

        public void Awake()
        {
            exosuit = gameObject.GetComponent<Exosuit>();
            energyMixin = exosuit.GetComponent<EnergyMixin>();
            scanSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            scanSoundAsset.path = "event:/tools/scanner/scan_loop";
            scanSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            scanSound.asset = scanSoundAsset;
            completeSoundAsset = ScriptableObject.CreateInstance<FMODAsset>();
            completeSoundAsset.path = "event:/tools/scanner/scan_complete";            
        }

        public void Start()
        {
            exosuit.onToggle += OnToggle;
            Player.main.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
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
            if (exosuit.GetSlotBinding(slotID) == ScannerModule.TechTypeID)
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

            scanSound.Stop();
            isActive = false;
            toggle = false;
            stateCurrent = ScanState.None;
            Modules.SetInteractColor(Modules.Colors.White);
            Modules.SetProgressColor(Modules.Colors.White);
        }

        private void Update()
        {
            if (isActive)
            {
                PDAScanner.Result result = Scan();
                
                if (!isScanning)
                {
                    if (result != PDAScanner.Result.Known)
                    {                        
                        main.SetInteractText("AutoScan", "Active", false, false, HandReticle.Hand.None);
                        main.SetIcon(HandReticle.IconType.Scan, 1.5f);
                    }
                }
                else
                {
                    main.SetInteractText(PDAScanner.scanTarget.techType.AsString(false), true, HandReticle.Hand.None);
                    main.SetIcon(HandReticle.IconType.Progress, 10f);
                    main.progressText.text = Mathf.RoundToInt(PDAScanner.scanTarget.progress * 100f) + "%";
                    Modules.SetProgressColor(Modules.Colors.Orange);
                    main.progressImage.fillAmount = Mathf.Clamp01(PDAScanner.scanTarget.progress);
                    main.SetProgress(PDAScanner.scanTarget.progress);                    
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
            Player.main.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            exosuit.onToggle -= OnToggle;
        }
    }
}

