using UnityEngine;
using UnityEngine.UI;
using UWE;

namespace ScannerModule
{
    public class ScannerModuleComponent : MonoBehaviour
    {
        // Some code extracted with dnSpy from Assembly-CSharp.dll:ScannerTool 
        
        private EnergyMixin energyMixin;
        
        public float powerConsumption = 0.2f;
        public const float scanDistance = 18f;               
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
        
        public void Awake()
        {
            seamoth = gameObject.GetComponent<SeaMoth>();            
        }        

        public enum ScanState
        {
            None,
            Scan
        }

        public static readonly string[] slotIDs = new string[]
        {
            "SeamothModule1",
            "SeamothModule2",
            "SeamothModule3",
            "SeamothModule4"
        };


        public static GameObject TraceForTarget(float distance, float sphereRadius = 0.2f, bool preferSphereHits = false)
        {
            if (UWE.Utils.TraceForFPSTarget(Player.main.gameObject, distance, sphereRadius, out GameObject go, out float num, preferSphereHits))
            {
                return UWE.Utils.GetEntityRoot(go);
            }
            return null;
        }


        private void Start()
        {
            energyMixin = GetComponent<EnergyMixin>();            
            var scanner = Resources.Load<GameObject>("WorldEntities/Tools/Scanner").GetComponent<ScannerTool>();

            scanSound = scanner.scanSound;            
            completeSound = scanner.completeSound;
            
            scanCircuitTex = scanner.scanCircuitTex;
            scanOrganicTex = scanner.scanOrganicTex;
            fxControl = scanner.fxControl;

            
            scanBeam = Instantiate(scanner.scanBeam);
            scanBeam.transform.SetParent(seamoth.gameObject.transform, false);
            scanBeam.transform.localScale = new Vector3(1, 4, 1);
            scanBeam.transform.localPosition = new Vector3(-0.7f, -0.5f, 1.9f);
           
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

        
        private void OnDisable()
        {
            scanSound.Stop();
        }

        
        private void Update()
        {
            if (toggle)
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
            /*
            if (Input.GetKeyDown(KeyCode.G))
            {
                scanBeam.transform.localRotation = beamRotation;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"RESET: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");

            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                scanBeam.gameObject.SetActive(true);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;                
                Vector3 scale = scanBeam.transform.localScale;                
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
                
            }


            if (Input.GetKeyDown(KeyCode.Z))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x + 0.1f, quaternion.y, quaternion.z, quaternion.w);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x - 0.1f, quaternion.y, quaternion.z, quaternion.w);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x, quaternion.y + 0.1f, quaternion.z, quaternion.w);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x, quaternion.y - 0.1f, quaternion.z, quaternion.w);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z + 0.1f, quaternion.w);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z - 0.1f, quaternion.w);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w + 0.1f);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                scanBeam.transform.localRotation = new Quaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w - 0.1f);
                vector3 = scanBeam.transform.localPosition;
                quaternion = scanBeam.transform.localRotation;
                Debug.Log($"position: x: {vector3.x} y: {vector3.y} z: {vector3.z}");
                Debug.Log($"rotation: x: {quaternion.x} y: {quaternion.y} z: {quaternion.z} w: {quaternion.w}");
            }
            */
        }

        //private Vector3 vector3;
        //private Quaternion quaternion;

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
                    //main.progressText.color = new Color32(159, byte.MaxValue, byte.MaxValue, byte.MaxValue);
                    //main.progressImage.fillAmount = Mathf.Clamp01(PDAScanner.scanTarget.progress);
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

