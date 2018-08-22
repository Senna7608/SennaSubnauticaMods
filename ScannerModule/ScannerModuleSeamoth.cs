using Common.Modules;
using UnityEngine;

namespace ScannerModule
{
    public class ScannerModuleSeamoth : MonoBehaviour
    {
        // Some code extracted with dnSpy from Assembly-CSharp.dll:ScannerTool 
        
        private EnergyMixin energyMixin;        
        public float powerConsumption = 0.5f;
        public const float scanDistance = 40f;               
        public FMOD_CustomLoopingEmitter scanSound;
        public FMODAsset completeSound;
        public Texture2D scanCircuitTex;
        public Color scanCircuitColor = Color.white;
        public Texture2D scanOrganicTex;
        public Color scanOrganicColor = Color.white;
        public VFXController fxControl;
        private ScanState stateLast;
        private ScanState stateCurrent;
        private float idleTimer;
        private Material scanMaterialCircuitFX;
        private Material scanMaterialOrganicFX;
        private VFXOverlayMaterial scanFX;
        public bool toggle;
        public bool isScanning;

        private SeaMoth seamoth;
        public GameObject scanBeam;
        Quaternion beamRotation;
        private Transform leftTorpedoSlot;

        public enum ScanState
        {
            None,
            Scan
        }

        public void Awake()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();

            if (!seamoth)
            {
                Destroy(this);
            }
        }        

        private void Start()
        {
            energyMixin = GetComponent<EnergyMixin>();
            ScannerTool scanner = Resources.Load<GameObject>("WorldEntities/Tools/Scanner").GetComponent<ScannerTool>();            

            scanSound = Instantiate(scanner.scanSound, gameObject.transform);            
            completeSound = Instantiate(scanner.completeSound, gameObject.transform);            

            scanCircuitTex = Instantiate(scanner.scanCircuitTex, gameObject.transform);
            scanOrganicTex = Instantiate(scanner.scanOrganicTex, gameObject.transform);           
            fxControl = Instantiate(scanner.fxControl, gameObject.transform);
            leftTorpedoSlot = seamoth.torpedoTubeLeft.transform;
            scanBeam = Instantiate(scanner.scanBeam, leftTorpedoSlot);            
            scanBeam.transform.localScale = new Vector3(1, 4, 1);
            
            //scanBeam.transform.localPosition = new Vector3(-0.7f, -0.5f, 1.9f);
           
            beamRotation = new Quaternion(-0.7683826f, 0.1253118f, 0.0448633f, 0.6259971f);
            scanBeam.transform.localRotation = beamRotation;
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
        }
        
        private void Update()
        {
            if (toggle && Player.main.inSeamoth)
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
            if (toggle)
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
                    Modules.SetProgressColor(Modules.Colors.White);
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

            if (state && PDAScanner.scanTarget.isValid)
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

