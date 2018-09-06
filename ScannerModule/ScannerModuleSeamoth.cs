using Common;
using UnityEngine;
using UWE;

namespace ScannerModule
{
    public class ScannerModuleSeamoth : MonoBehaviour
    {
        // Some code extracted with dnSpy from Assembly-CSharp.dll:ScannerTool 
        [AssertNotNull]
        private EnergyMixin energyMixin;        
        public float powerConsumption = 0.5f;
        public const float scanDistance = 50f;
        
        public FMOD_CustomLoopingEmitter scanSound;
        [AssertNotNull]
        public FMODAsset completeSound;
        public Texture scanCircuitTex;
        public Color scanCircuitColor = Color.white;
        public Texture scanOrganicTex;
        public Color scanOrganicColor = Color.white;
        [AssertNotNull]
        public VFXController fxControl;
        private ScanState stateLast;
        private ScanState stateCurrent;
        private float idleTimer;
        private Material scanMaterialCircuitFX;
        private Material scanMaterialOrganicFX;
        [AssertNotNull]
        private VFXOverlayMaterial scanFX;
        public bool toggle;
        public bool isScanning;
        public bool isActive;

        private SeaMoth seamoth;
        public GameObject scanBeam;        
        private Transform leftTorpedoSlot;

        public enum ScanState
        {
            None,
            Scan
        }

        public void Awake()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();
            leftTorpedoSlot = seamoth.torpedoTubeLeft.transform;
            energyMixin = seamoth.GetComponent<EnergyMixin>();

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

            seamoth.onToggle += OnToggle;
            Utils.GetLocalPlayerComp().playerModeChanged.AddHandler(gameObject, new Event<Player.Mode>.HandleFunction(OnPlayerModeChanged));
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
            if (seamoth.GetSlotBinding(slotID) == ScannerModule.TechTypeID)
            {
                toggle = state;

                if (toggle)
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
            isActive = Player.main.inSeamoth && toggle;
        }

        public void OnDisable()
        {
            isActive = false;
            isScanning = false;
            scanSound.Stop();
            SetFXActive(false);           
            stateCurrent = ScanState.None;
            Modules.SetProgressColor(Modules.Colors.White);
            Modules.SetInteractColor(Modules.Colors.White);
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
                    Modules.SetProgressColor(Modules.Colors.Orange);                    
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
            {
                StopScanFX();
            }
        } 
    }
}

