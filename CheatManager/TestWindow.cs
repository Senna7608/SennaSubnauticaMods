#if DEBUG
// Only for debugging and testing new functions
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using System.Text;
using UWE;

namespace CheatManager
{
    
    public class TestWindow : MonoBehaviour
    {
        private Rect windowRect = new Rect(0, 100, 500, 300);
        public static TestWindow _Instance = null;
        public static bool isShow;
        private static readonly KeyCode TestOnHotkey = KeyCode.F9;
        private static readonly KeyCode TestOffHotkey = KeyCode.F10;
       
        
        private static float playerHealth;        
        private static float playerMaxHealth;
        private static float o2Current;
        private static float o2Available;
        private static bool hasOxygenTank;

        

        private static StringBuilder stringBuilder = new StringBuilder();
        private static string targetName = "";
        Survival component = null;
        
        private static List<string> AtlasNames = new List<string>();
        private static List<string> names = new List<string>();
        private static Dictionary<string,string> AtlasItems = new Dictionary<string, string>();

        public void Awake()
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }        

        public static void GetAllAtlasName()
        {
            Atlas[] array = Resources.LoadAll<Atlas>("Atlases/");

            foreach (Atlas item in array)
            {
                AtlasNames.Add(item.name);
            }            
        }             

        public void Start()
        {
          

        }

        public void Update()
        {
           
            if (Player.main != null)
            {
                playerHealth = Player.main.liveMixin.health;
                playerMaxHealth = Player.main.liveMixin.maxHealth;
                Player.main.oxygenMgr.GetTotal(out o2Available, out o2Current);
                hasOxygenTank = Player.main.oxygenMgr.HasOxygenTank();
                
                if (component == null)
                {
                    component = Player.main.GetComponent<Survival>();
                }
                
                //OxygenManager component3 = Player.main.GetComponent<OxygenManager>(); 
                
                if (Input.GetKeyDown(TestOnHotkey))
                {
                    Player.main.liveMixin.data.maxHealth = 200f;
                    Player.main.liveMixin.health = 200f;
                    component.food = 200f;
                    component.water = 200f;
                    
                }

                if (Input.GetKeyDown(TestOffHotkey))
                {
                    Player.main.liveMixin.data.maxHealth = 100f;
                    component.food = 100f;
                    component.water = 100f;
                }
                
                //TraceCrossHair();
            }            
            
        }

        private void TraceCrossHair()
        {
            

            //Vector3 position = default(Vector3);
            GameObject gameObject = null;
            //TraceFPSTargetPosition(GameObject ignoreObj, float maxDist, ref GameObject closestObj, ref Vector3 position, bool includeUseableTriggers = true)
            //TraceForTarget(Vector3 startPos, Vector3 direction, GameObject ignoreObj, float maxDist, ref GameObject closestObj, ref Vector3 position, bool includeTriggers = false)
            //UWE.Utils.TraceForTarget(currentWorldPos, main , base.gameObject, 100f, ref gameObject, ref position, true);
            //UWE.Utils.TraceFPSTargetPosition(base.gameObject, 3000f, ref gameObject, ref position, true);
            

            if (gameObject != null)
            {
                targetName = gameObject.name.ToString();
            }
        }

        public void OnGUI()
        {

            if (!isShow)
            {
                return;
            }

            windowRect = GUILayout.Window(gameObject.GetInstanceID(), windowRect, WindowControl, "","Box");            
        }


        private void WindowControl(int windowID)
        {
            stringBuilder.AppendFormat("Player Health: {0}\n", playerHealth);
            stringBuilder.AppendFormat("Player Max Health: {0}\n", playerMaxHealth);
            stringBuilder.AppendFormat("Player Food: {0}\n", component.food);
            stringBuilder.AppendFormat("Player Water: {0}\n", component.water);
            stringBuilder.AppendFormat("Player Has Oxygen Tank: {0}\n", hasOxygenTank.ToString());
            stringBuilder.AppendFormat("Player O2: {0}/{1}\n", o2Current, o2Available);
            stringBuilder.AppendFormat("Crosshair current: {0}\n", targetName);
            
            GUILayout.Label(stringBuilder.ToString());

            stringBuilder.Remove(0, stringBuilder.Length);
        }
        
        public static TestWindow Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType(typeof(TestWindow)) as TestWindow;
                    if (_Instance == null)
                    {
                        GameObject testwindow = new GameObject();
                        testwindow.AddComponent<TestWindow>();
                        testwindow.name = "CheatManager.TestWindow";
                        _Instance = testwindow.GetComponent(typeof(TestWindow)) as TestWindow;
                        Instance.Awake();
                    }

                }

                return _Instance;
            }
        }

        public static void InitTestWindow(bool show)
        {
            Instance.Awake();
            isShow = show;
        }
    }
}
#endif