using System.Text;
using UnityEngine;
using Common;

namespace CheatManager
{
    internal class InfoBar : MonoBehaviour
    {
        public static InfoBar Instance { get; private set; }
        private Rect windowRect = new Rect(400, 0, Screen.width - 905, 23);        
        public static bool isShow;
        private Int3 currentBatch = new Int3();
        private static string currentBiome = "";
        private static int day = 0;
        private static float playerInfectionLevel = 0;
        private static readonly string[] daynightstr = { "Day", "Night" };
        private static int isDay;
        private static Vector3 currentWorldPos = new Vector3();       
        private static StringBuilder stringBuilder;
               
        public void Awake()
        {
            useGUILayout = false;
            Instance = this;
            DontDestroyOnLoad(this);            
        }

        public void Start()
        {
            stringBuilder = new StringBuilder();
        }

        public void OnDestroy()
        {
            stringBuilder = null;
        }

        public void Update()
        {
            if (Player.main != null && isShow)
            {
                currentBiome = Player.main.GetBiomeString();
                currentBatch = LargeWorldStreamer.main.GetContainingBatch(MainCamera.camera.transform.position);
                currentWorldPos = MainCamera.camera.transform.position;
                day = (int)DayNightCycle.main.GetDay();
                playerInfectionLevel = Player.main.infectedMixin.GetInfectedAmount() * 100f;

                isDay = DayNightCycle.main.IsDay()? 0 : 1;

                stringBuilder.Remove(0, stringBuilder.Length);
                
                stringBuilder.AppendFormat($"Current Biome: {currentBiome}" +
                    $"  Current World Position: {currentWorldPos}" +
                    $"    Current Batch: {currentBatch}" +
                    $"    Player infection level: {playerInfectionLevel}%" +
                    $"    Day: {day}" +
                    $"    Day/Night: {daynightstr[isDay]}");
            }            
            
        }

        public void OnGUI()
        {

            if (!isShow)
            {
                return;
            }

            GUIHelper.CreatePopupWindow(windowRect, null);
            GUI.contentColor = Color.green;            
            GUI.Label(new Rect(windowRect.x + 5, windowRect.y, windowRect.width, windowRect.height), stringBuilder.ToString());
        }        

        internal static void InitInfoBar(bool show)
        {
            Instance = Load();
            isShow = show;
        }


        public static InfoBar Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(InfoBar)) as InfoBar;

                if (Instance == null)
                {
                    GameObject infobar = new GameObject().AddComponent<InfoBar>().gameObject;
                    infobar.name = "CheatManager.InfoBar";
                    Instance = infobar.GetComponent<InfoBar>();                    
                }
            }

            return Instance;
        }

    }
}
