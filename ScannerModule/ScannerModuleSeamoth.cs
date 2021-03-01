using UnityEngine;
using UWE;
using static Common.Helpers.GraphicsHelper;
using static Common.Helpers.GameHelper;
using Common;

namespace ScannerModule
{
    public enum ScanState
    {
        None,
        Scan
    }

    public class ScannerModuleSeamoth : MonoBehaviour
    {
        private int moduleCount = 0;
        private SeaMoth thisSeamoth;
        private EnergyMixin energyMixin;
        private Transform leftTorpedoSlot;

        private GameObject scanBeam;
        private float powerConsumption = 0.5f;
        private const float scanDistance = 30f;

        private FMOD_CustomLoopingEmitter scanSound;
        private FMODAsset completeSound;
        private FMODAsset scan_loop;
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

        public void Awake()
        {            
            thisSeamoth = GetComponent<SeaMoth>();            
            leftTorpedoSlot = thisSeamoth.torpedoTubeLeft.transform;
            energyMixin = thisSeamoth.GetComponent<EnergyMixin>();            

            GameObject scannerPrefab = CraftData.InstantiateFromPrefab(TechType.Scanner);

            scannerPrefab.SetActive(false);
            Utils.ZeroTransform(scannerPrefab.transform);

            ScannerTool scannerTool = scannerPrefab.GetComponent<ScannerTool>();
            
            GameObject ScannerModuleGO = new GameObject("ScannerModuleGO");
            ScannerModuleGO.transform.SetParent(leftTorpedoSlot.transform, false);
            Utils.ZeroTransform(ScannerModuleGO.transform);

            scan_loop = ScriptableObject.CreateInstance<FMODAsset>();
            scan_loop.name = "scan_loop";
            scan_loop.path = "event:/tools/scanner/scan_loop";
            scanSound = ScannerModuleGO.AddComponent<FMOD_CustomLoopingEmitter>();
            scanSound.asset = scan_loop;
            
            completeSound = ScriptableObject.CreateInstance<FMODAsset>();
            completeSound.name = "scan_complete";
            completeSound.path = "event:/tools/scanner/scan_complete";

            fxControl = Instantiate(scannerTool.fxControl) as VFXController;                      

            scanBeam = Main.objectHelper.GetGameObjectClone(scannerPrefab.FindChild("x_ScannerBeam"), ScannerModuleGO.transform, false);

            scanCircuitTex = CreateRWTextureFromNonReadableTexture(scannerTool.scanCircuitTex);            
            scanOrganicTex = CreateRWTextureFromNonReadableTexture(scannerTool.scanOrganicTex);

            scanCircuitColor = scannerTool.scanCircuitColor;
            scanOrganicColor = scannerTool.scanOrganicColor;

            scanBeam.transform.localPosition = new Vector3(0, 0, -0.37f);
            scanBeam.transform.localScale = new Vector3(1, 4.41f, 1.55f);
            scanBeam.transform.localRotation = Quaternion.Euler(280, 184, 185);                        

            Destroy(scannerPrefab);
            Destroy(ScannerModuleGO.FindChild("Scanner(Clone)(Clone)"));            
        
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

            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));

            moduleCount = thisSeamoth.modules.GetCount(ScannerModulePrefab.TechTypeID);            
        }

        private void OnPlayerMotorModeChanged(Player.MotorMode newMotorMode)
        {
            if (newMotorMode == Player.MotorMode.Vehicle)
            {
                if (Player.main.currentMountedVehicle == thisSeamoth)
                {                    
                    isPlayerInThisSeamoth = true;
                    OnEnable();
                }
                else
                {
                    isPlayerInThisSeamoth = false;
                    OnDisable();
                }
            }
            else
            {
                isPlayerInThisSeamoth = false;
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
            isActive = isPlayerInThisSeamoth && Player.main.isPiloting && isToggle && moduleCount > 0;
        }

        public void OnDisable()
        {
            isActive = false;
            isScanning = false;
            scanSound.Stop();
            SetFXActive(false);           
            stateCurrent = ScanState.None;
            SetProgressColor(Color.white);
            SetInteractColor(Color.white);
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
                    SetProgressColor(Color.green);                    
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
            
            thisSeamoth.onToggle -= OnToggle;
            thisSeamoth.modules.onAddItem -= OnAddItem;
            thisSeamoth.modules.onRemoveItem -= OnRemoveItem;
            Player.main.playerMotorModeChanged.RemoveHandler(this, OnPlayerMotorModeChanged);
        }
        
        /*
        private void DestroyFakeScanners()
        {
            try
            {
                GameObject trash = gameObject.FindChild("Scanner(Clone)");

                if (trash)
                {
                    Debug.Log($"[ScannerModule] Found fake scanner gameobject in seamoth hierarchy with ID: [{trash.GetInstanceID()}]");
                    Debug.Log("and destroying it...");
                    DestroyImmediate(trash);                    
                }
            }
            catch
            {
                return;
            }
        }
        */

    }
}

