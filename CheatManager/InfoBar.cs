using System.Text;
using UnityEngine;
using Common.MyGUI;

namespace CheatManager
{
    public class InfoBar : MonoBehaviour
    {
        private Rect windowRect = new Rect(0, Screen.height-120, 450, 120);
        public static InfoBar _Instance = null;
        public static bool isShow;

        private Int3 currentBatch = new Int3();
        private static string currentBiome = "";
        private static int day = 0;
        private static float playerInfectionLevel = 0;
        private static string[] daynightstr = { "Day", "Night" };
        private static int isDay;
        private static Vector3 currentWorldPos = new Vector3();       
        private static StringBuilder stringBuilder;       

        public void Awake()
        {
            _Instance = this;
            useGUILayout = false;
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

                if (DayNightCycle.main.IsDay())
                    isDay = 0;
                else
                    isDay = 1;

                stringBuilder.Remove(0, stringBuilder.Length);
                
                stringBuilder.AppendFormat("Last used command: {0} Code: [{1}]\nCurrent Biome: {2}\nCurrent World Position: {4}\nCurrent Batch: {5}\nPlayer infection level: {6}%\nDay: {3}\nDay/Night: {7}",
                                                   CheatManager.lastCheat,
                                                   CheatManager.lastCheatCode,
                                                   currentBiome,
                                                   day,
                                                   currentWorldPos,
                                                   currentBatch,
                                                   playerInfectionLevel,
                                                   daynightstr[isDay]
                                                   );
            }            
            
        }

        public void OnGUI()
        {

            if (!isShow)
            {
                return;
            }           

            GUI_Tools.CreatePopupWindow(windowRect, null);            

            GUI.Label(windowRect, stringBuilder.ToString());
        }
        

        public static InfoBar Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType(typeof(InfoBar)) as InfoBar;
                    if (_Instance == null)
                    {
                        GameObject infobar = new GameObject();
                        infobar.AddComponent<InfoBar>();
                        infobar.name = "CheatManager.InfoBar";
                        _Instance = infobar.GetComponent(typeof(InfoBar)) as InfoBar;
                        Instance.Awake();
                    }

                }

                return _Instance;
            }
        }

        public static void InitInfoBar(bool show)
        {
            Instance.Awake();
            isShow = show;
        }

    }
}
