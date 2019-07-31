using UnityEngine;
using UWE;
using static Common.Modules;
using static Common.GameHelper;

namespace ScannerModule
{
    public class ScannerModuleSeamoth : MonoBehaviour
    {
        public ScannerModuleSeamoth Instance { get; private set; }
        public int moduleSlotID { get; set; }
        private SeaMoth thisSeamoth { get; set; }
        private Player playerMain { get; set; }
        private EnergyMixin energyMixin { get; set; }
        private Transform leftTorpedoSlot;

        private GameObject scanBeam;
        private float powerConsumption = 0.5f;
        private const float scanDistance = 50f;

        private FMOD_CustomLoopingEmitter scanSound;
        private FMODAsset completeSound;
        private Texture scanCircuitTex;
        private Color scanCircuitColor = Color.white;
        private Texture scanOrganicTex;
        private Color scanOrganicColor = Color.white;

        private VFXController fxControl;
        private ScanState stateLast;
        private ScanState stateCurrent;
        private float idleTimer;
        private Material scanMaterialCircuitFX;
        private Material scanMaterialOrganicFX;        
        private VFXOverlayMaterial scanFX;

        private bool isToggle;
        private bool isScanning;
        private bool isActive;
        private bool isPlayerInThisSeamoth;

        public enum ScanState
        {
            None,
            Scan
        }

        public void Awake()
        {
            Instance = this;
            thisSeamoth = Instance.GetComponent<SeaMoth>();            
            leftTorpedoSlot = thisSeamoth.torpedoTubeLeft.transform;
            energyMixin = thisSeamoth.GetComponent<EnergyMixin>();
            playerMain = Player.main;

            isPlayerInThisSeamoth = playerMain.GetVehicle() == thisSeamoth ? true : false;
            var scannerPrefab = Resources.Load<GameObject>("WorldEntities/Tools/Scanner").GetComponent<ScannerTool>();
            //ScannerTool scannerPrefab = CraftData.InstantiateFromPrefab(TechType.Scanner, false).GetComponent<ScannerTool>();
            scanSound = Instantiate(scannerPrefab.scanSound, gameObject.transform);
            completeSound = Instantiate(scannerPrefab.completeSound, gameObject.transform);
            fxControl = Instantiate(scannerPrefab.fxControl, gameObject.transform);
            scanBeam = Instantiate(scannerPrefab.scanBeam, leftTorpedoSlot.transform);

            MeshRenderer[] renderers = scannerPrefab.GetComponentsInChildren<MeshRenderer>(true);                        
            Renderer instantiated_renderer = Instantiate(renderers[0]);
            scanCircuitTex = instantiated_renderer.materials[0].mainTexture;
            scanOrganicTex = instantiated_renderer.materials[2].mainTexture;            
            
            //Destroy(instantiated_renderer);            
            //Resources.UnloadAsset(scannerPrefab);
            
            scanBeam.transform.localScale = new Vector3(1, 4, 1);
            scanBeam.transform.localRotation = new Quaternion(-0.7683826f, 0.1253118f, 0.0448633f, 0.6259971f);
        }        
        
        private void Start()
        {
            SetFXActive(false);

            Shader shader = Shader.Find("FX/Scanning");

            if (shader != null)
            {
                scanMaterialCircuitFX = new Material(shader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                scanMaterialCircuitFX.SetTexture(ShaderPropertyID._MainTex, scanCircuitTex);
                scanMaterialCircuitFX.SetColor(ShaderPropertyID._Color, scanCircuitColor);
                scanMaterialOrganicFX = new Material(shader)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                scanMaterialOrganicFX.SetTexture(ShaderPropertyID._MainTex, scanOrganicTex);
                scanMaterialOrganicFX.SetColor(ShaderPropertyID._Color, scanOrganicColor);
            }

            thisSeamoth.onToggle += OnToggle;
            thisSeamoth.modules.onAddItem += OnAddItem;
            thisSeamoth.modules.onRemoveItem += OnRemoveItem;
            playerMain.playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
        }

        private void OnRemoveItem(InventoryItem item)
        {
            if (item.item.GetTechType() == ScannerModulePrefab.TechTypeID)
            {                
                moduleSlotID = -1;
                Instance.enabled = false;                
            }
        }

        private void OnAddItem(InventoryItem item)
        {
            if (item.item.GetTechType() == ScannerModulePrefab.TechTypeID)
            {
                moduleSlotID = thisSeamoth.GetSlotByItem(item);
                Instance.enabled = true;
            }
        }

        private void OnPlayerModeChanged(Player.Mode playerMode)
        {
            if (playerMode == Player.Mode.LockedPiloting)
            {
                if (playerMain.GetVehicle() == thisSeamoth)
                {
                    isPlayerInThisSeamoth = true;
                    OnEnable();
                    return;
                }
                else
                {
                    isPlayerInThisSeamoth = false;
                    OnDisable();
                    return;
                }
            }
            else
            {
                isPlayerInThisSeamoth = false;
                OnDisable();
            }                
        }


        private void OnToggle(int slotID, bool state)
        {
            if (thisSeamoth.GetSlotBinding(slotID) == ScannerModulePrefab.TechTypeID)
            {
                isToggle = state;

                if (isToggle)
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
            isActive = isPlayerInThisSeamoth && playerMain.isPiloting && isToggle && moduleSlotID > -1;
        }

        public void OnDisable()
        {
            isActive = false;
            isScanning = false;
            scanSound.Stop();
            SetFXActive(false);           
            stateCurrent = ScanState.None;
            SetProgressColor(Colors.White);
            SetInteractColor(Colors.White);
        }


        private void Update()
        {
            if (isActive)
            {
                if (GameInput.GetButtonDown(GameInput.Button.LeftHand) && !isScanning)
                {
                    isScanning = true;
                }

                if (GameInput.GetButtonUp(GameInput.Button.LeftHand))
                {                    
                    isScanning = false;                    
                }

                if (isScanning)
                {
                    Scan();                    
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
                bool flag = stateCurrent == ScanState.Scan;
                
                if (idleTimer <= 0f)
                {
                    OnHover();
                }

                SetFXActive(flag);                
                
                if (flag)
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

            if (scanTarget.isValid && energyMixin.charge > 0f)
            {
                result = PDAScanner.Scan();
                
                if (result == PDAScanner.Result.Scan)
                {
                    float amount = powerConsumption * Time.deltaTime;
                    energyMixin.ConsumeEnergy(amount);
                    stateCurrent = ScanState.Scan;
                }
                else if (result == PDAScanner.Result.Done || result == PDAScanner.Result.Researched)
                {                    
                    idleTimer = 0.5f;
                    PDASounds.queue.PlayIfFree(completeSound);
                    if (fxControl != null)
                    {
                        fxControl.Play(0);
                    }
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

            if (result == PDAScanner.Result.Scan)
            {
                HandReticle main = HandReticle.main;
                main.SetInteractText(scanTarget.techType.AsString(false), true, HandReticle.Hand.Left);
                main.SetIcon(HandReticle.IconType.Scan, 1.5f);

                if (stateCurrent == ScanState.Scan)
                {                    
                    main.SetIcon(HandReticle.IconType.Progress, 4f);
                    main.progressText.text = Mathf.RoundToInt(PDAScanner.scanTarget.progress * 100f) + "%";
                    SetProgressColor(Colors.Orange);                    
                    main.progressImage.fillAmount = Mathf.Clamp01(PDAScanner.scanTarget.progress);                    
                    main.SetProgress(PDAScanner.scanTarget.progress);
                }                
            }            
        }
        
        private void SetFXActive(bool state)
        {
            scanBeam.SetActive(state);

            if (isActive && state && PDAScanner.scanTarget.isValid)
            {                
                PlayScanFX();
            }
            else
            {
                StopScanFX();
            }
        }
        
        private void PlayScanFX()
        {
            PDAScanner.ScanTarget scanTarget = PDAScanner.scanTarget;

            if (scanTarget.isValid)
            {
                if (scanFX != null)
                {
                    if (scanFX.gameObject != scanTarget.gameObject)
                    {
                        StopScanFX();
                        scanFX = scanTarget.gameObject.AddComponent<VFXOverlayMaterial>();
                        bool flag = scanTarget.gameObject.GetComponent<Creature>() != null;
                        if (flag)
                        {
                            scanFX.ApplyOverlay(scanMaterialOrganicFX, "VFXOverlay: Scanning", false, null);
                        }
                        else
                        {
                            scanFX.ApplyOverlay(scanMaterialCircuitFX, "VFXOverlay: Scanning", false, null);
                        }
                    }
                }
                else
                {
                    scanFX = scanTarget.gameObject.AddComponent<VFXOverlayMaterial>();
                    bool flag2 = scanTarget.gameObject.GetComponent<Creature>() != null;
                    if (flag2)
                    {
                        scanFX.ApplyOverlay(scanMaterialOrganicFX, "VFXOverlay: Scanning", false, null);
                    }
                    else
                    {                        
                        scanFX.ApplyOverlay(scanMaterialCircuitFX, "VFXOverlay: Scanning", false, null);
                    }
                }
            }
        }
        
        private void StopScanFX()
        {
            if (scanFX != null)
            {
                scanFX.RemoveOverlay();
            }
        }
        
        private void OnDestroy()
        {
            if (scanFX != null)
                StopScanFX();

            playerMain.playerModeChanged.RemoveHandler(gameObject, OnPlayerModeChanged);
            thisSeamoth.onToggle -= OnToggle;
            thisSeamoth.modules.onAddItem -= OnAddItem;
            thisSeamoth.modules.onRemoveItem -= OnRemoveItem;
        } 
    }
}

