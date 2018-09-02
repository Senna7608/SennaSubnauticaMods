using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using GUIHelper;
using UWE;

namespace CheatManager
{
    public class CheatManager : MonoBehaviour
    {
        public static CheatManager Instance { get; private set; }

        private static readonly KeyCode MainHotkey = KeyCode.F5;
        private static readonly KeyCode ToggleHotkey = KeyCode.F4;
        
        private static bool initStyles = false;

        private static Vector2 scrollPos = Vector2.zero;
        private static bool isActive;
        private static Rect windowRect = new Rect(Screen.width - 500, 0, 500, 762);
        private static readonly float space = 3;       

        private static readonly string[][] WarpData = WarpTargets.Targets;

        private string windowTitle;
        public static string prevCwPos = null;

        public static bool seamothCanFly = false;

        public Utils.MonitoredValue<bool> isSeaglideFast = new Utils.MonitoredValue<bool>();       

        public static float seamothSpeedMultiplier;
        public static float exosuitSpeedMultiplier;
        public static float cyclopsSpeedMultiplier;

        public static float playerPrevInfectionLevel = 0f;

        private int normalButtonID = -1;
        private int toggleButtonID = -1;
        private int daynightTabID = 4;
        private int categoriesTabID = 0;
        private static int vehicleSettingsID = -1;

        private int currentdaynightTab = 4;
        private int currentTab = 0;

        public static FMODAsset warpSound;

        public static string seamothName;
        public static string exosuitName;
        public static string cyclopsName;

        private static List<TechMatrix.TechTypeData>[] TechnologyMatrix;

        private static List<Tools.ButtonInfo> Buttons;
        private static List<Tools.ButtonInfo> toggleButtons;
        private static List<Tools.ButtonInfo> daynightTab;
        private static List<Tools.ButtonInfo> categoriesTab;
        private static List<Tools.ButtonInfo> vehicleSettings;

        private bool initToggleButtons = false;
                
        public static CheatManager Load()
        {
            Instance = FindObjectOfType(typeof(CheatManager)) as CheatManager;            

            if (Instance == null)
            {
                GameObject cheatmanager = new GameObject().AddComponent<CheatManager>().gameObject;
                cheatmanager.name = "CheatManager";
                Instance = cheatmanager.GetComponent<CheatManager>();
            }
            
            return Instance;
        }

        public void Awake()
        {            
            Instance = this;
            useGUILayout = false;
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string Version = fvi.FileVersion;
            windowTitle = $"CheatManager v.{Version}  {ToggleHotkey} Toggle Cursor, {MainHotkey} Toggle Window";

            warpSound = ScriptableObject.CreateInstance<FMODAsset>();
            warpSound.path = "event:/tools/gravcannon/fire";

            TechnologyMatrix = new List<TechMatrix.TechTypeData>[TechMatrix.techMatrix.Length];
            TechMatrix.InitTechMatrixList(ref TechnologyMatrix);
            if (Main.isExistsSMLHelperV2)
            {
                TechMatrix.IsExistsModdersTechTypes(ref TechnologyMatrix, TechMatrix.Known_Senna7608_TechTypes);
                TechMatrix.IsExistsModdersTechTypes(ref TechnologyMatrix, TechMatrix.Known_AHK1221_TechTypes);
                TechMatrix.IsExistsModdersTechTypes(ref TechnologyMatrix, TechMatrix.Known_PrimeSonic_TechTypes);
                TechMatrix.IsExistsModdersTechTypes(ref TechnologyMatrix, TechMatrix.Known_Kylinator25_TechTypes);
                TechMatrix.IsExistsModdersTechTypes(ref TechnologyMatrix, TechMatrix.Known_Coticvo_TechTypes);
            }
            else
            {
                Logger.Log("[CheatManager] Warning:\n'SMLHelper.V2' not found! Some functions are not available!", LogType.Warning);
            }
            TechMatrix.SortTechLists(ref TechnologyMatrix);

            Buttons = new List<Tools.ButtonInfo>();
            toggleButtons = new List<Tools.ButtonInfo>();
            daynightTab = new List<Tools.ButtonInfo>();
            categoriesTab = new List<Tools.ButtonInfo>();
            vehicleSettings = new List<Tools.ButtonInfo>();

            Tools.CreateButtonsList(ButtonText.Buttons, Tools.BUTTONTYPE.NORMAL_CENTER, ref Buttons);
            Tools.CreateButtonsList(ButtonText.ToggleButtons, Tools.BUTTONTYPE.TOGGLE_CENTER, ref toggleButtons);
            Tools.CreateButtonsList(ButtonText.DayNightTab, Tools.BUTTONTYPE.TAB_CENTER, ref daynightTab);
            Tools.CreateButtonsList(ButtonText.CategoriesTab, Tools.BUTTONTYPE.TAB_CENTER, ref categoriesTab);

            var searchSeaGlide = new TechMatrix.TechTypeSearch(TechType.Seaglide);
            string seaglideName = TechnologyMatrix[1][TechnologyMatrix[1].FindIndex(searchSeaGlide.EqualsWith)].Name;

            var searchSeamoth = new TechMatrix.TechTypeSearch(TechType.Seamoth);
            seamothName = TechnologyMatrix[0][TechnologyMatrix[0].FindIndex(searchSeamoth.EqualsWith)].Name;

            var searchExosuit = new TechMatrix.TechTypeSearch(TechType.Exosuit);
            exosuitName = TechnologyMatrix[0][TechnologyMatrix[0].FindIndex(searchExosuit.EqualsWith)].Name;

            var searchCyclops = new TechMatrix.TechTypeSearch(TechType.Cyclops);
            cyclopsName = TechnologyMatrix[0][TechnologyMatrix[0].FindIndex(searchCyclops.EqualsWith)].Name;

            string[] vehicleSetButtons = { $"{seamothName} Can Fly", $"{seaglideName} Speed Fast" };

            Tools.CreateButtonsList(vehicleSetButtons, Tools.BUTTONTYPE.TOGGLE_CENTER, ref vehicleSettings);

            daynightTab[4].Pressed = true;
            categoriesTab[0].Pressed = true;
            toggleButtons[17].Pressed = false;
            Buttons[7].Enabled = false;
            Buttons[7].Pressed = true;

            exosuitSpeedMultiplier = 1;
            cyclopsSpeedMultiplier = 1;
        }        

        public void OnDestroy()
        {
            Buttons = null;
            toggleButtons = null;
            daynightTab = null;
            categoriesTab = null;
            vehicleSettings = null;
            TechnologyMatrix = null;
            initToggleButtons = false;
            prevCwPos = null;
            warpSound = null;            
            isActive = false;
            seamothCanFly = false;            
            initStyles = false;            
        }

        public void Start()
        {
            isSeaglideFast.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(IsSeaglideFast));            
        }             

        private void IsSeaglideFast(Utils.MonitoredValue<bool> parms)
        {
            SeaglideOverDrive.Main.SetSeaglideSpeed();            
        }

        public void Update()
        {
            if (Player.main != null)
            {
                if (Input.GetKeyDown(MainHotkey))
                {
                    isActive = !isActive;
                }

                if (isActive)
                {
                    if (Input.GetKeyDown(ToggleHotkey))
                    {
                        UWE.Utils.lockCursor = !UWE.Utils.lockCursor;
                    }

                    if(!initToggleButtons)
                    {
                        ReadGameValues();
                        initToggleButtons = true;
                    }                   

                    if (normalButtonID != -1)
                    {
                        ButtonControl.NormalButtonControl(normalButtonID, ref Buttons, ref toggleButtons);                        
                    }

                    if (toggleButtonID != -1)
                    {
                        ButtonControl.ToggleButtonControl(toggleButtonID, ref toggleButtons);                        
                    }

                    if (daynightTabID != -1)
                    {
                        ButtonControl.DayNightButtonControl(daynightTabID, ref currentdaynightTab, ref daynightTab);
                    }

                    if (categoriesTabID != -1)
                    {
                        if (categoriesTabID != currentTab)
                        {
                            categoriesTab[currentTab].Pressed = false;
                            categoriesTab[categoriesTabID].Pressed = true;
                            currentTab = categoriesTabID;
                            scrollPos = Vector2.zero;
                        }
                    }

                    if (vehicleSettingsID != -1)
                    {
                        if (vehicleSettingsID == 0)
                        {
                            if (SeamothOverDrive.Main != null)
                            {
                                seamothCanFly = !seamothCanFly;
                                vehicleSettings[0].Pressed = seamothCanFly;
                            }
                            else
                            {
                                ErrorMessage.AddMessage("CheatManager Error!\nYou do not have a Seamoth!");
                            }
                        }

                        if (vehicleSettingsID == 1)
                        {
                            if (SeaglideOverDrive.Main != null)
                            {
                                isSeaglideFast.Update(!isSeaglideFast.value);
                                vehicleSettings[1].Pressed = isSeaglideFast.value;
                            }
                            else
                            {
                                ErrorMessage.AddMessage("CheatManager Error!\nYou do not have a Seaglide!");
                            }
                            
                        }
                    }
                }
                
                if (toggleButtons[18].Pressed)
                {
                    Player.main.infectedMixin.SetInfectedAmount(0f);
                }                                        
            }
        }       


        internal static void ReadGameValues()
        {            
            toggleButtons[0].Pressed = GameModeUtils.IsOptionActive(GameModeOption.NoSurvival);
            toggleButtons[1].Pressed = GameModeUtils.IsOptionActive(GameModeOption.NoBlueprints);
            toggleButtons[2].Pressed = GameModeUtils.RequiresSurvival();
            toggleButtons[3].Pressed = GameModeUtils.IsPermadeath();
            toggleButtons[4].Pressed = NoCostConsoleCommand.main.fastBuildCheat;
            toggleButtons[5].Pressed = NoCostConsoleCommand.main.fastScanCheat;
            toggleButtons[6].Pressed = NoCostConsoleCommand.main.fastGrowCheat;
            toggleButtons[7].Pressed = NoCostConsoleCommand.main.fastHatchCheat;
          //toggleButtons[8].Pressed = filterfast cheat
            toggleButtons[9].Pressed = GameModeUtils.IsOptionActive(GameModeOption.NoCost);
            toggleButtons[10].Pressed = GameModeUtils.IsCheatActive(GameModeOption.NoEnergy);
            toggleButtons[11].Pressed = GameModeUtils.IsOptionActive(GameModeOption.NoSurvival);
            toggleButtons[12].Pressed = GameModeUtils.IsOptionActive(GameModeOption.NoOxygen);
            toggleButtons[13].Pressed = GameModeUtils.IsOptionActive(GameModeOption.NoRadiation);
            toggleButtons[14].Pressed = GameModeUtils.IsInvisible();
          //toggleButtons[15].Pressed = shotgun cheat
            toggleButtons[16].Pressed = NoDamageConsoleCommand.main.GetNoDamageCheat();
          //toggleButtons[17].Pressed = alwaysDay cheat
          //toggleButtons[18].Pressed = noInfect cheat                      
            toggleButtons[19].Enabled = GameModeUtils.RequiresSurvival();
            vehicleSettings[0].Pressed = seamothCanFly;            
        }
        

        public static void ExecuteCommand(object message, object command)
        {
            if (message != null)
            {
                ErrorMessage.AddMessage(message.ToString());
            }

            if (command != null)
            {
                DevConsole.SendConsoleCommand(command.ToString());                
            }           
            
        }        
        
        public void OnGUI()
        {
            if (!isActive)
            {
                return;
            }

            if (!initStyles)
                initStyles = Tools.SetCustomStyles();

            Rect drawingRect = Tools.CreatePopupWindow(windowRect, windowTitle);

            float lastYcoord = drawingRect.y;
            float baseHeight = drawingRect.height;

            drawingRect.x += 5;
            drawingRect.y += space;
            drawingRect.width -= 10;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Commands:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight - drawingRect.y + 22;

            normalButtonID = Tools.CreateButtonsGrid(drawingRect, space, 4, Buttons, out lastYcoord);            

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Toggle Commands:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight - (lastYcoord + 22 + space);

            toggleButtonID = Tools.CreateButtonsGrid(drawingRect, space, 4, toggleButtons, out lastYcoord);            

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Day/Night Speed:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight;

            daynightTabID = Tools.CreateButtonsGrid(drawingRect, space, 6, daynightTab , out lastYcoord);

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Categories:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight;

            categoriesTabID = Tools.CreateButtonsGrid(drawingRect, space, 4, categoriesTab, out lastYcoord);                        

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Select Item in Category:");

            drawingRect.x += 150;

            GUI.Label(drawingRect, categoriesTab[currentTab].Name, Tools.Label);
            
            drawingRect.x = windowRect.x + 10;
            drawingRect.y = lastYcoord + 22 + (space * 2);
            drawingRect.width = drawingRect.width - 10;
            drawingRect.height = (baseHeight - drawingRect.y) + 20;            
            
            TabControl(currentTab, drawingRect);
            
        }       


        private static void TabControl(int category, Rect scrollRect)
        {
            int scrollItems;

            if (category == 19)
                scrollItems = WarpData.Length;            
            else           
                scrollItems = TechnologyMatrix[category].Count;

            float width = scrollRect.width;

            if (scrollItems > 10 && category != 0)
                width -= 20;

            if (scrollItems > 4 && category == 0)
            {
                scrollRect.height = 104;
                width -= 20;
            }

            scrollPos = GUI.BeginScrollView(scrollRect, scrollPos, new Rect(scrollRect.x, scrollRect.y, width, scrollItems * 26));

            string itemName, selectedTech;

            for (int i = 0; i < scrollItems; i++)
            {
                if (category == 19)
                {
                    itemName = WarpData[i][1];
                    selectedTech = WarpData[i][0];                    
                }
                else
                {                    
                    itemName = TechnologyMatrix[category][i].Name; 
                    selectedTech = TechnologyMatrix[category][i].TechType.ToString();                    
                }               

                if (GUI.Button(new Rect(scrollRect.x, scrollRect.y + (i * 26), width, 22), itemName, Tools.GetCustomStyle(false, Tools.BUTTONTYPE.NORMAL_LEFTALIGN)))                
                {
                    switch (category)
                    {
                        case 0:
                            if (!Player.main.IsInBase() && !Player.main.IsInSubmarine() && !Player.main.escapePod.value)
                            {
                                if (TechnologyMatrix[category][i].TechType == TechType.Cyclops)
                                    ExecuteCommand($"{itemName}  has spawned", "sub cyclops");
                                else
                                    ExecuteCommand($"{itemName}  has spawned", $"spawn {selectedTech}");
                                break;
                            }
                            ErrorMessage.AddMessage("CheatManager Error!\nVehicles cannot spawn inside Lifepod, Base or Submarine!");
                            break;                       
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:                       
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:                                                    
                            ExecuteCommand($"{itemName}  added to inventory", $"item {selectedTech}");
                            break;                        
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        
                        case 16:
                        case 17:
                            ExecuteCommand($"{itemName}  has spawned", $"spawn {selectedTech}");                             
                            break;
                        case 18:
                            ExecuteCommand($"Blueprint: {itemName} unlocked", $"unlock {selectedTech}");                            
                            break;
                        case 19:
                            Teleport(itemName, selectedTech);
                            Buttons[7].Enabled = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            GUI.EndScrollView();

            if (category == 0)
            {
                scrollRect.y += (4 * 26) + 2;

                GUI.Box(new Rect(scrollRect.x, scrollRect.y, scrollRect.width, 23), "Vehicle Settings:", Tools.Box);
                
                vehicleSettingsID = Tools.CreateButtonsGrid(new Rect(scrollRect.x - 5, scrollRect.y + 27, scrollRect.width + 10, 22), 2, 2, vehicleSettings, out float lastYcoord);
                
                GUI.Label(new Rect(scrollRect.x, scrollRect.y + 53, 250, 22), seamothName + " speed multiplier: " + string.Format("{0:#.##}", seamothSpeedMultiplier));
                
                seamothSpeedMultiplier = (GUI.HorizontalSlider(new Rect(scrollRect.x, scrollRect.y + 79, scrollRect.width, 10), seamothSpeedMultiplier, 1f, 5f));

                GUI.Label(new Rect(scrollRect.x, scrollRect.y + 93 , 250, 22), exosuitName + " speed multiplier: " + string.Format("{0:#.##}",exosuitSpeedMultiplier));
                exosuitSpeedMultiplier = GUI.HorizontalSlider(new Rect(scrollRect.x, scrollRect.y + 119, scrollRect.width, 10), exosuitSpeedMultiplier, 1f, 5f);

                GUI.Label(new Rect(scrollRect.x, scrollRect.y + 133, 250, 22), cyclopsName + " speed multiplier: " + string.Format("{0:#.##}", cyclopsSpeedMultiplier));
                cyclopsSpeedMultiplier = GUI.HorizontalSlider(new Rect(scrollRect.x, scrollRect.y + 159, scrollRect.width, 10), cyclopsSpeedMultiplier, 1.0F, 5.0F);
                
            }    
            
        }
        
        private static void Teleport(string name, string Vector3string)
        {
            Vector3 currentWorldPos = MainCamera.camera.transform.position;
            prevCwPos = string.Format("{0:D} {1:D} {2:D}", (int)currentWorldPos.x, (int)currentWorldPos.y, (int)currentWorldPos.z);
            if (IsPlayerInVehicle())
            {
                Vehicle vehicle = Player.main.GetVehicle();
                vehicle.TeleportVehicle(WarpTargets.ConvertStringPosToVector3(Vector3string), Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to: {name}\n({Vector3string})");
            }
            else
            {
                ExecuteCommand($"Player Warped to: {name}\n({Vector3string})", $"warp {Vector3string}");
            }

            Utils.PlayFMODAsset(warpSound, Player.main.transform, 20f);
        }

        internal static bool IsPlayerInVehicle()
        {
            if (Player.main.inSeamoth == true || Player.main.inExosuit == true)
            {
                return true;
            }

            return false;
        }

    }
}


