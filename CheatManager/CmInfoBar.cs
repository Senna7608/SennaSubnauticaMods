using System.Text;
using UnityEngine;
using Common.GUIHelper;
using System;
using CheatManager.Configuration;

namespace CheatManager
{
    public class CmInfoBar : MonoBehaviour
    {
        public CmInfoBar Instance { get; private set; }

        private Rect windowRect = new Rect(0, 0, Screen.width - (Screen.width / CmConfig.ASPECT) - 2 , Screen.height / 45);
        private Rect drawRect;        
        private Int3 currentBatch = new Int3();
        private string currentBiome = "";
        private int day = 0;
        private string infoText = string.Empty;
        private float playerInfectionLevel = 0;
        private readonly string[] daynightstr = { "Day", "Night" };
        private int isDay;
        private Vector3 currentWorldPos = Vector3.zero;       
        private StringBuilder stringBuilder;
        private float temperature = 0;

        float FPS;
        float totalmem;
        float diffTotalmem;
        private float timeNextSample = -1f;
        private float timeNextUpdate = -1f;
        private long lastTotalMem;
        private long diffTotalMem;
        private float accumulatedFrameTime;
        private int numAccumulatedFrames;
        private int lastCollectionCount1;
        private int lastCollectionCount2;
        private float lastCollectionTime;
        private float timeBetweenCollections;
        private float avgFrameTime;
        private int numCollections;
        private int numFixedUpdates;
        private int numUpdates;
        private float avgFixedUpdatesPerFrame;

        private Vector3 playerMainLastPosition = Vector3.zero;
        private float speed;
        private Vector3 currVel;
        private float timeCount = 0.0f;

        private bool refreshFixedUpdate = false;

        public void Awake()
        {            
            Instance = this;
            useGUILayout = false;            
            DontDestroyOnLoad(this);
            drawRect = new Rect(windowRect.x + 5, windowRect.y, windowRect.width, windowRect.height);
            //isShow = true;
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
            if (!CmConfig.isInfoBarEnabled)
            {
                return;
            }

            UpdateFPS();

            if (Player.main != null && CmConfig.isInfoBarEnabled)
            {
                timeCount += Time.deltaTime;

                if (timeCount > 1.0f)
                {
                    if (WaterTemperatureSimulation.main != null)
                        temperature = WaterTemperatureSimulation.main.GetTemperature(Player.main.transform.position);

                    currentBiome = Player.main.GetBiomeString();
                    currentBatch = LargeWorldStreamer.main.GetContainingBatch(Player.main.transform.position);
                    isDay = DayNightCycle.main.IsDay() ? 0 : 1;
                    playerInfectionLevel = Player.main.infectedMixin.GetInfectedAmount() * 100f;
                    day = (int)DayNightCycle.main.GetDay();
                    timeCount = 0.0f;                    
                }
                
                currentWorldPos = Player.main.transform.position;

                stringBuilder.Remove(0, stringBuilder.Length);

                stringBuilder.AppendFormat($"Biome: {currentBiome}" +
                    $"   {string.Format("World Position: {0,3:N0}, {1,3:N0}, {2,3:N0}", currentWorldPos.x, currentWorldPos.y, currentWorldPos.z)}" +
                    $"   Batch: {currentBatch}" +
                    $"   Infection: {playerInfectionLevel}%" +
                    $"   Day: {day}" +
                    $"   Time of Day: {daynightstr[isDay]}" +
                    $"   Temp.: {Mathf.CeilToInt(temperature)} \u00B0C" +
                    $"   Speed: {(int)speed} km/h" +
                    $"   {string.Format("FPS: {0,3:N0}", FPS)}" +
                    $"   {string.Format("MEM: {0,3:N0} MB (+{1,6:N2} MB/s)", totalmem, diffTotalmem)}" +
                    $"   {string.Format("GC: {0,2:N0} ms (Total:{1,3})", timeBetweenCollections, numCollections)}" +
                    $"   Fixed Updates: ");
                
                if (refreshFixedUpdate)
                {
                    stringBuilder.Append(avgFixedUpdatesPerFrame.ToTwoDecimalString());
                }

                infoText = stringBuilder.ToString();
            }
            else
            {
                timeCount = 0.0f;

                stringBuilder.Remove(0, stringBuilder.Length);

                stringBuilder.AppendFormat(
                    $"   {string.Format("FPS: {0,3:N0}", FPS)}" +
                    $"   {string.Format("MEM: {0,3:N0} MB (+{1,6:N2} MB/s)", totalmem, diffTotalmem)}" +
                    $"   {string.Format("GC: {0,2:N0} ms (Total:{1,3})", timeBetweenCollections, numCollections)}" +
                    $"   Fixed Updates: ");

                if (refreshFixedUpdate)
                {
                    stringBuilder.Append(avgFixedUpdatesPerFrame.ToTwoDecimalString());
                }

                infoText = stringBuilder.ToString();
            }

        }

        public void OnGUI()
        {
            if (!CmConfig.isInfoBarEnabled)
            {
                return;
            }

            SNWindow.CreateWindow(windowRect, null);
            GUI.Label(drawRect, infoText, SNStyles.GetGuiItemStyle(GuiItemType.LABEL, textColor: GuiColor.Green, textAnchor: TextAnchor.MiddleLeft));
        }
        
        private void FixedUpdate()
        {
            if (!CmConfig.isInfoBarEnabled)
            {
                return;
            }

            if (Player.main != null)
            {                
                if (Player.main.GetVehicle() != null)
                {
                    speed = Player.main.GetVehicle().useRigidbody.velocity.magnitude * 3.6f;
                }
                else if (Player.main.currentSub != null && Player.main.currentSub.isCyclops)
                {
                    if (Player.main.isPiloting)
                    {
                        speed = Player.main.currentSub.rb.velocity.magnitude * 3.6f;
                    }
                    else
                    {
                        currVel = (Player.main.transform.position - playerMainLastPosition) / Time.fixedDeltaTime;
                        speed = currVel.magnitude * 3.6f;
                    }
                }
                else
                {
                    currVel = (Player.main.transform.position - playerMainLastPosition) / Time.fixedDeltaTime;
                    speed = currVel.magnitude * 3.6f;
                }

                playerMainLastPosition = Player.main.transform.position;
            }

            numFixedUpdates++;            
        }

        private void UpdateFPS()
        {
            numAccumulatedFrames++;
            numUpdates++;
            accumulatedFrameTime += Time.unscaledDeltaTime;
            
            if (Time.unscaledTime > timeNextSample)
            {
                SampleTotalMemory();
                timeNextSample = Time.unscaledTime + 1f;
                refreshFixedUpdate = true;
            }

            if (Time.unscaledTime > timeNextUpdate)
            {
                SampleFrameRate();
                refreshFixedUpdate = true;
                timeNextUpdate = Time.unscaledTime + 0.1f;
                if (numUpdates > 0)
                {
                    avgFixedUpdatesPerFrame = numFixedUpdates / (float)numUpdates;
                    numUpdates = 0;
                    numFixedUpdates = 0;
                }
            }

            int num = GC.CollectionCount(1);
            int num2 = GC.CollectionCount(2);

            if (num2 > lastCollectionCount2 || num > lastCollectionCount1)
            {
                float unscaledTime = Time.unscaledTime;
                timeBetweenCollections = unscaledTime - lastCollectionTime;
                lastCollectionTime = unscaledTime;
                numCollections++;
                refreshFixedUpdate = true;
            }

            lastCollectionCount1 = num;
            lastCollectionCount2 = num2;

            if (refreshFixedUpdate)
            {
                totalmem = lastTotalMem / 1048576f;
                diffTotalmem = diffTotalMem / 1048576f;
                FPS = 1f / avgFrameTime;                
            }
        }

        private void SampleTotalMemory()
        {
            long totalMemory = GC.GetTotalMemory(false);
            diffTotalMem = totalMemory - lastTotalMem;
            lastTotalMem = totalMemory;
        }

        private void SampleFrameRate()
        {
            avgFrameTime = accumulatedFrameTime / numAccumulatedFrames;
            numAccumulatedFrames = 0;
            accumulatedFrameTime = 0f;
        }        
        
    }
}
