using Common.Helpers;
using UnityEngine;
using UWE;
using static Common.Helpers.GameHelper;

namespace ScannerModule
{
    public class ScannerModuleExosuit : MonoBehaviour
    {
        private int moduleCount = 0;
        private Exosuit thisExosuit;        
        private EnergyMixin energyMixin;
        private HandReticle handReticleMain;

        private FMOD_CustomLoopingEmitter scanSound;
        private FMODAsset completeSound;
        private FMODAsset scan_loop;

        private const float powerConsumption = 0.5f;
        private const float scanDistance = 50f;        
        private float idleTimer;        

        private bool isToggle;
        private bool isScanning;
        private bool isActive;
        private bool isPlayerInThisExosuit;

        private ScanState stateLast;
        private ScanState stateCurrent;        

        public void Awake()
        {            
            thisExosuit = GetComponent<Exosuit>();
            energyMixin = thisExosuit.GetComponent<EnergyMixin>();            
            handReticleMain = HandReticle.main;

            scan_loop = ScriptableObject.CreateInstance<FMODAsset>();
            scan_loop.name = "scan_loop";
            scan_loop.path = "event:/tools/scanner/scan_loop";
            scanSound = gameObject.AddComponent<FMOD_CustomLoopingEmitter>();
            scanSound.asset = scan_loop;

            completeSound = ScriptableObject.CreateInstance<FMODAsset>();
            completeSound.name = "scan_complete";
            completeSound.path = "event:/tools/scanner/scan_complete";

            thisExosuit.onToggle += OnToggle;
            thisExosuit.modules.onAddItem += OnAddItem;
            thisExosuit.modules.onRemoveItem += OnRemoveItem;

            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));

            moduleCount = thisExosuit.modules.GetCount(ScannerModulePrefab.TechTypeID);
        }

        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {
            if (newMotorMode == Player.MotorMode.Vehicle)
            {
                if (Player.main.currentMountedVehicle == thisExosuit)
                {
                    isPlayerInThisExosuit = true;
                    OnEnable();
                }
                else
                {
                    isPlayerInThisExosuit = false;
                    OnDisable();
                }
            }
            else
            {
                isPlayerInThisExosuit = false;
                OnDisable();
            }
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == ScannerModulePrefab.TechTypeID)
            {
                moduleCount--;
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            if (item.item.GetTechType() == ScannerModulePrefab.TechTypeID)
            {
                moduleCount++;
            }
        }

        private void OnToggle(int slotID, bool state)
        {
            if (thisExosuit.GetSlotBinding(slotID) == ScannerModulePrefab.TechTypeID)
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
            isActive = isPlayerInThisExosuit && Player.main.isPiloting && isToggle && moduleCount > 0;
        }

        public void OnDisable()
        {
            scanSound.Stop();
            isActive = false;
            isToggle = false;
            stateCurrent = ScanState.None;
            SetInteractColor(Color.white);
            SetProgressColor(Color.white);
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
                    SetProgressColor(ColorHelper.Orange);
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
                    PDASounds.queue.PlayIfFree(completeSound);                    
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
            thisExosuit.onToggle -= OnToggle;
            thisExosuit.modules.onAddItem -= OnAddItem;
            thisExosuit.modules.onRemoveItem -= OnRemoveItem;
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
        }
    }
}

