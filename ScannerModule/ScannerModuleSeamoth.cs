using UnityEngine;
using UWE;
using static Common.Helpers.GameHelper;
using Common;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;

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

            CoroutineHost.StartCoroutine(InitializeScannerAsync());            

            thisSeamoth.onToggle += OnToggle;
            thisSeamoth.modules.onAddItem += OnAddItem;
            thisSeamoth.modules.onRemoveItem += OnRemoveItem;            

            Player.main.playerMotorModeChanged.AddHandler(this, new Event<Player.MotorMode>.HandleFunction(OnPlayerMotorModeChanged));

            moduleCount = thisSeamoth.modules.GetCount(ScannerModulePrefab.TechTypeID);            
        }

        private IEnumerator InitializeScannerAsync()
        {
            CoroutineTask<GameObject> request = CraftData.GetPrefabForTechTypeAsync(TechType.Scanner);
            yield return request;

            GameObject result = request.GetResult();

            if (result == null)
            {
                SNLogger.Error($"{TechType.Scanner} : Cannot instantiate prefab from TechType!");
                yield break;
            }

            GameObject scannerPrefab = UWE.Utils.InstantiateDeactivated(result, leftTorpedoSlot.transform, Vector3.zero, Quaternion.identity);
                                    
            ScannerTool scannerTool = scannerPrefab.GetComponent<ScannerTool>();

            GameObject ScannerModuleGO = new GameObject("ScannerModuleGO");
            ScannerModuleGO.transform.SetParent(leftTorpedoSlot.transform, false);
            Utils.ZeroTransform(ScannerModuleGO.transform);
            ScannerModuleGO.SetActive(false);
           
            scan_loop = ScriptableObject.CreateInstance<FMODAsset>();
            scan_loop.name = "scan_loop";
            scan_loop.path = "event:/tools/scanner/scan_loop";
            scanSound = ScannerModuleGO.AddComponent<FMOD_CustomLoopingEmitter>();
            scanSound.asset = scan_loop;            
            
            completeSound = ScriptableObject.CreateInstance<FMODAsset>();
            completeSound.name = "scan_complete";
            completeSound.path = "event:/tools/scanner/scan_complete";
            
            scanBeam = scannerPrefab.FindChild("x_ScannerBeam");
            scanBeam.transform.SetParent(ScannerModuleGO.transform);                  

            AsyncOperationHandle<Texture2D> loadRequest_01 = AddressablesUtility.LoadAsync<Texture2D>("Assets/Textures/vfx/circuit_tile_01.tga");
            yield return loadRequest_01;

            if (loadRequest_01.Status == AsyncOperationStatus.Failed)
            {
                SNLogger.Error("Cannot find Texture2D in Resources folder at path 'Assets/Textures/vfx/circuit_tile_01.tga'");
                yield break;
            }

            scanCircuitTex = loadRequest_01.Result;

            AsyncOperationHandle<Texture2D> loadRequest_02 = AddressablesUtility.LoadAsync<Texture2D>("Assets/Textures/vfx/voronoi_tile_02.tga");
            yield return loadRequest_02;

            if (loadRequest_02.Status == AsyncOperationStatus.Failed)
            {
                SNLogger.Error("Cannot find Texture2D in Resources folder at path 'Assets/Textures/vfx/voronoi_tile_02.tga'");
                yield break;
            }

            scanOrganicTex = loadRequest_02.Result;             

            scanBeam.transform.localPosition = new Vector3(0, 0, -0.37f);
            scanBeam.transform.localScale = new Vector3(1, 4.41f, 1.55f);
            scanBeam.transform.localRotation = Quaternion.Euler(280, 184, 185);

            DestroyImmediate(scannerPrefab);            

            SetFXActive(false);

            Shader scannerToolScanning = ShaderManager.preloadedShaders.scannerToolScanning;

            if (scannerToolScanning != null)
            {
                scanMaterialCircuitFX = new Material(scannerToolScanning)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                scanMaterialCircuitFX.SetTexture(ShaderPropertyID._MainTex, scanCircuitTex);
                scanMaterialCircuitFX.SetColor(ShaderPropertyID._Color, scanCircuitColor);
                scanMaterialOrganicFX = new Material(scannerToolScanning)
                {
                    hideFlags = HideFlags.HideAndDontSave
                };
                scanMaterialOrganicFX.SetTexture(ShaderPropertyID._MainTex, scanOrganicTex);
                scanMaterialOrganicFX.SetColor(ShaderPropertyID._Color, scanOrganicColor);
            }
            
            ScannerModuleGO.SetActive(true);

            yield break;
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
                main.SetText(HandReticle.TextType.Hand, scanTarget.techType.AsString(false), true);
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

                        if (scanTarget.gameObject.GetComponent<Creature>() != null)
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

                    if (scanTarget.gameObject.GetComponent<Creature>() != null)
                    {
                        scanFX.ApplyOverlay(scanMaterialOrganicFX, "VFXOverlay: Scanning", false, null);
                    }
                    else
                    {                        
                        scanFX.ApplyOverlay(scanMaterialCircuitFX, "VFXOverlay: Scanning", false, null);
                    }
                }

                
                float value = 1f;

                if (!MiscSettings.flashes)
                {
                    value = 0.1f;
                }

                scanMaterialCircuitFX.SetFloat(ShaderPropertyID._TimeScale, value);
                scanMaterialOrganicFX.SetFloat(ShaderPropertyID._TimeScale, value);
                
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
    }
}

