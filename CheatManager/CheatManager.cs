using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using UnityEngine;
using Common.MyGUI;
using SMLHelper.V2.Handlers;

namespace CheatManager
{
    public class CheatManager : MonoBehaviour
    {
        public static CheatManager _Instance { get; private set; }
        private static readonly KeyCode MainHotkey = KeyCode.F5;
        private static readonly KeyCode ToggleHotkey = KeyCode.F4;
        
        private static Vector2 scrollPos = Vector2.zero;
        private static bool isActive = new bool();
        private static Rect windowRect = new Rect(Screen.width - 500, 0, 500, 762);
        private static readonly float space = 3;
        private static readonly int buttonWidth = 115;
        private static readonly int gridWidth = (buttonWidth * 4) + 12;

        private static Vector3 currentWorldPos = Vector3.zero;
        
        public static string lastCheat;
        public static int lastCheatCode;
        public static bool overPower;
#if DEBUG
        private static Survival survival;
#endif
        private static readonly string[][] WarpData = WarpTargets.Targets;
        private static readonly string[] Categories = ButtonText.CategoriesTab;                      

        private static string windowTitle;
        private static string prevCwPos = null;
        private static bool[] isToggle = null;

        private static Vehicle vehicle;
        private static SubControl subControl;
        private static bool seamothCanFly;
        public static bool prevSeamothCanFly;

        private static bool seaGlideFastSpeed;

        private static float seamothSpeedMultiplier = 1;
        private static float exosuitSpeedMultiplier = 1;
        private static float cyclopsSpeedMultiplier = 1;

        public static float prevSeamothSpeedMultiplier = 1;
        public static float prevExosuitSpeedMultiplier = 1;
        public static float prevCyclopsSpeedMultiplier = 1;
        public static CyclopsMotorMode.CyclopsMotorModes currentCyclopsMotorMode;

        private static string seaGlideName;
        private static string seamothName;
        private static string exosuitName;
        private static string cyclopsName;

        public static float playerPrevInfectionLevel = 0f;

        private static int toggleButtonID = 1;
        private static int daynightTabID = 4;
        private static int currentdaynightTab = 4;
        private static int categoriesTabID = 0;
        private static int currentTab = 0;

        private static readonly float[] DayNightSpeed = { 0.1f, 0.25f, 0.5f, 0.75f, 1f, 2f };

        private static FMODAsset warpSound;

        private static string itemName;
        private static string selectedTech;
        private static int scrollItems;

        private static List<GUI_Tools.ButtonInfo> Buttons;
        private static List<GUI_Tools.ButtonInfo> toggleButtons;
        private static List<GUI_Tools.ButtonInfo> daynightTab;
        private static List<GUI_Tools.ButtonInfo> categoriesTab;

        private static List<TechType>[] TechnologyMatrix;

        public static void Load()
        {
            _Instance = null;

            if (_Instance == null)
            {
                _Instance = FindObjectOfType(typeof(CheatManager)) as CheatManager;
                if (_Instance == null)
                {
                    GameObject cheatmanager = new GameObject().AddComponent<CheatManager>().gameObject;                    
                    cheatmanager.name = "CheatManager";
                    _Instance = cheatmanager.GetComponent<CheatManager>();                   
                }
            }                       
        }
        
        public void Awake()
        {        
           DontDestroyOnLoad(gameObject);
           useGUILayout = false;
        }

        public void OnDestroy()
        {
            Buttons = null;
            toggleButtons = null;
            daynightTab = null;
            categoriesTab = null;
            TechnologyMatrix = null;
        }

        public void Start()
        {
            isToggle = new bool[ButtonText.ToggleButtons.Length];
            seaGlideName = Language.main.Get(TechTypeExtensions.AsString(TechType.Seaglide, false));
            seamothName = Language.main.Get(TechTypeExtensions.AsString(TechType.Seamoth, false));
            exosuitName = Language.main.Get(TechTypeExtensions.AsString(TechType.Exosuit, false));
            cyclopsName = Language.main.Get(TechTypeExtensions.AsString(TechType.Cyclops, false));
                        
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string Version = fvi.FileVersion;

            TechnologyMatrix = new List<TechType>[20];

            for (int i = 0; i < TechMatrix.techMatrix.Length; i++)
            {
                TechnologyMatrix[i] = new List<TechType>();

                for (int j = 0; j < TechMatrix.techMatrix[i].Length ; j++)
                {
                    TechnologyMatrix[i].Add(TechMatrix.techMatrix[i][j]);
                }
            }
            
            if (TechTypeHandler.TryGetModdedTechType("SeaMothMk2", out TechType SeamothMk2))
            {                
                Logger.Log("SeaMoth Mk2 found and added to TechMatrix!", LogType.Warning);
                TechnologyMatrix[0].Insert(1, SeamothMk2);
            }

            if (TechTypeHandler.TryGetModdedTechType("ExosuitMk2", out TechType ExosuitMk2))
            {
                Logger.Log("Exosuit Mk2 found and added to TechMatrix!", LogType.Warning);
                TechnologyMatrix[0].Insert(3, ExosuitMk2);
            }

            Buttons = new List<GUI_Tools.ButtonInfo>();
            toggleButtons = new List<GUI_Tools.ButtonInfo>();
            daynightTab = new List<GUI_Tools.ButtonInfo>();
            categoriesTab = new List<GUI_Tools.ButtonInfo>();

            GUI_Tools.CreateButtonsList(ButtonText.Buttons, GUI_Tools.BUTTONTYPE.NORMAL_CENTER, ref Buttons);
            GUI_Tools.CreateButtonsList(ButtonText.ToggleButtons, GUI_Tools.BUTTONTYPE.TOGGLE_CENTER, ref toggleButtons);
            GUI_Tools.CreateButtonsList(ButtonText.DayNightTab, GUI_Tools.BUTTONTYPE.TAB_CENTER, ref daynightTab);
            GUI_Tools.CreateButtonsList(ButtonText.CategoriesTab, GUI_Tools.BUTTONTYPE.TAB_CENTER, ref categoriesTab);

            daynightTab[4].Pressed = true;
            categoriesTab[0].Pressed = true;

            isToggle[17] = false;
            warpSound = ScriptableObject.CreateInstance<FMODAsset>();
            warpSound.path = "event:/tools/gravcannon/fire";            
            windowTitle = "CheatManager v." + Version + "   " + ToggleHotkey + " Toggle Cursor, " + MainHotkey + " Toggle Window";            
        }

       

        public void Update()
        {
            if (Player.main != null && IngameMenu.main != null)
            {
                if (isActive)
                {
                    if (Input.GetKeyDown(ToggleHotkey))
                    {
                        UWE.Utils.lockCursor = !UWE.Utils.lockCursor;
                    }

                    ReadGameValues();                     
                }
#if DEBUG
                
                if (survival == null)
                {
                    survival = Player.main.GetComponent<Survival>();
                }
                
#endif
                if (!IngameMenu.main.isActiveAndEnabled && Input.GetKeyDown(MainHotkey))
                {
                    isActive = !isActive;
                }

                if (isToggle[18])
                {
                    Player.main.infectedMixin.SetInfectedAmount(0f);
                }                
                
                if (Player.main.motorMode == Player.MotorMode.Seaglide && seaGlideFastSpeed == true)
                {
                    Player.main.playerController.activeController.acceleration = 60f;                    
                    Player.main.playerController.activeController.verticalMaxSpeed = 75f;
                }
                else
                {
                    Player.main.playerController.activeController.acceleration = 20f;
                    Player.main.playerController.activeController.verticalMaxSpeed = 5f;
                }
                
                if (IngameMenu.main.isActiveAndEnabled)
                {
                    isActive = false;                   
                }
                
                if (Player.main.isPiloting)
                {
                    if (Player.main.inSeamoth && seamothSpeedMultiplier != prevSeamothSpeedMultiplier || seamothCanFly != prevSeamothCanFly)
                    {
                        vehicle = Player.main.GetVehicle();

                        VehicleControl.SeamothControl(ref vehicle, seamothSpeedMultiplier, seamothCanFly);
                    }

                    if (Player.main.inExosuit && exosuitSpeedMultiplier != prevExosuitSpeedMultiplier)
                    {
                        vehicle = Player.main.GetVehicle();

                        VehicleControl.ExosuitControl(ref vehicle, exosuitSpeedMultiplier);
                    }

                    if (Player.main.IsInSubmarine())                       
                    {
                        subControl = Player.main.currentSub.gameObject.GetComponent<SubControl>();

                        if (cyclopsSpeedMultiplier != prevCyclopsSpeedMultiplier || currentCyclopsMotorMode != subControl.cyclopsMotorMode.cyclopsMotorMode)
                        {
                            VehicleControl.CyclopsControl(ref subControl, cyclopsSpeedMultiplier);
                        }
                    }
                               
                }
            }
        }
                

        private static void ReadGameValues()
        {
            currentWorldPos = MainCamera.camera.transform.position;
            isToggle[0] = GameModeUtils.IsOptionActive(GameModeOption.NoSurvival);
            isToggle[1] = GameModeUtils.IsOptionActive(GameModeOption.NoBlueprints);
            isToggle[2] = GameModeUtils.RequiresSurvival();
            isToggle[3] = GameModeUtils.IsPermadeath();
            isToggle[4] = NoCostConsoleCommand.main.fastBuildCheat;
            isToggle[5] = NoCostConsoleCommand.main.fastScanCheat;
            isToggle[6] = NoCostConsoleCommand.main.fastGrowCheat;
            isToggle[7] = NoCostConsoleCommand.main.fastHatchCheat;
            //isToggle[8] = filterfast cheat
            isToggle[9] = GameModeUtils.IsOptionActive(GameModeOption.NoCost);
            isToggle[10] = GameModeUtils.IsCheatActive(GameModeOption.NoEnergy);
            isToggle[11] = GameModeUtils.IsOptionActive(GameModeOption.NoSurvival);
            isToggle[12] = GameModeUtils.IsOptionActive(GameModeOption.NoOxygen);
            isToggle[13] = GameModeUtils.IsOptionActive(GameModeOption.NoRadiation);
            isToggle[14] = GameModeUtils.IsInvisible();
            //isToggle[15] = shotgun cheat
            isToggle[16] = NoDamageConsoleCommand.main.GetNoDamageCheat();
            //isToggle[17]  = alwaysDay cheat
            //isToggle[18]  = noInfect cheat

            for (int i = 0; i < isToggle.Length; i++)
            {
                if (isToggle[i])
                {
                    toggleButtons[i].Pressed = true;
                }
                else
                {
                    toggleButtons[i].Pressed = false;
                }
            }           

            if (prevCwPos == null)
            {
                Buttons[7].Enabled = false;
            }
            else
            {
                Buttons[7].Enabled = true;
                Buttons[7].Pressed = true;
            }

            if (daynightTabID != -1 && daynightTabID != currentdaynightTab)
            {
                for (int i = 0; i < daynightTab.Count; i++)
                {
                    if (i == daynightTabID)
                    {
                        daynightTab[i].Pressed = true;
                        currentdaynightTab = daynightTabID;
                        DevConsole.SendConsoleCommand("daynightspeed " + DayNightSpeed[daynightTabID]);
                    }
                    else
                    {
                        daynightTab[i].Pressed = false;
                    }
                }
            }

            if (categoriesTabID != -1 && categoriesTabID != currentTab)
            {
                for (int i = 0; i < categoriesTab.Count; i++)
                {
                    if (i == categoriesTabID)
                    {
                        categoriesTab[i].Pressed = true;
                        currentTab = categoriesTabID;
                    }
                    else
                    {
                        categoriesTab[i].Pressed = false;
                    }
                }
            }
#if DEBUG
            overPower = isToggle[19];
#endif
        }
        


        public static void ExecuteCommand(string message, string command, int code)
        {
            if (message != "")
            {
                ErrorMessage.AddMessage(message);
            }

            if (command != "")
            {
                DevConsole.SendConsoleCommand(command);
                lastCheat = command;
            }
            
            lastCheatCode = code;
        }

        private static bool initStyles = false;
        
        public void OnGUI()
        {
            if (!isActive)
            {
                return;
            }

            if (!initStyles)
                initStyles = GUI_Tools.SetCustomStyles();

            Rect drawingRect = GUI_Tools.CreatePopupWindow(windowRect, windowTitle);

            float lastYcoord = drawingRect.y;
            float baseHeight = drawingRect.height;

            drawingRect.x += 5;
            drawingRect.y += space;
            drawingRect.width -= 10;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Commands:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight - drawingRect.y + 22;

            int normalButtonID = GUI_Tools.CreateButtonsGrid(drawingRect, space, 4, Buttons, out lastYcoord);

            if (normalButtonID > -1)
            {
                switch (normalButtonID)
                {
                    case 0 when isToggle[17] == true:
                    case 1 when isToggle[17] == true:
                        break;
                    case 0:
                    case 1:
                    case 2:
                    case 4:
                    case 5:
                    case 6:
                        ExecuteCommand("Send command to console: " + Buttons[normalButtonID].Name, Buttons[normalButtonID].Name, normalButtonID);
                        break;

                    case 3:
                        ErrorMessage.AddMessage("Inventory Cleared");
                        Inventory.main.container.Clear(false);
                        lastCheat = Buttons[normalButtonID].Name;
                        lastCheatCode = normalButtonID;
                        break;

                    case 7:
                        ExecuteCommand("warp" + " to: " + prevCwPos, "warp " + prevCwPos, normalButtonID);
                        Utils.PlayFMODAsset(warpSound, Player.main.transform, 20f);
                        prevCwPos = null;
                        break;
                }                
            }

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Toggle Commands:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight - (lastYcoord + 22 + space);

            toggleButtonID = GUI_Tools.CreateButtonsGrid(drawingRect, space, 4, toggleButtons, out lastYcoord);

            if (toggleButtonID > -1)
            {
                ButtonControl.ToggleButtonControl(toggleButtonID, ref isToggle, ref toggleButtons, ref playerPrevInfectionLevel);                
            }

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Day/Night Speed:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight;

            daynightTabID = GUI_Tools.CreateButtonsGrid(drawingRect, space, 6, daynightTab , out lastYcoord);

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Categories:");

            drawingRect.y += 22;
            drawingRect.height = baseHeight;

            categoriesTabID = GUI_Tools.CreateButtonsGrid(drawingRect, space, 4, categoriesTab, out lastYcoord);                        

            drawingRect.y = lastYcoord + space;
            drawingRect.height = 22;

            GUI.Label(drawingRect, "Select Item in Category:");

            drawingRect.x += 150;

            GUI.Label(drawingRect, categoriesTab[currentTab].Name, GUI_Tools.Label);
            
            drawingRect.x = windowRect.x + 10;
            drawingRect.y = lastYcoord + 22 + (space * 2);
            drawingRect.width = drawingRect.width - 10;
            drawingRect.height = (baseHeight - drawingRect.y) + 20;            
            
            TabControl(currentTab, drawingRect);
            
        }       


        private static void TabControl(int category, Rect scrollRect)
        {
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

            for (int i = 0; i < scrollItems; i++)
            {
                if (category == 19)
                {
                    itemName = WarpData[i][1];
                    selectedTech = WarpData[i][0];                    
                }
                else
                {
                    itemName = Language.main.Get(TechTypeExtensions.AsString(TechnologyMatrix[category][i], false));
                    selectedTech = TechnologyMatrix[category][i].ToString();                    
                }
               

                if (GUI.Button(new Rect(scrollRect.x, scrollRect.y + (i * 26), width, 22), itemName, GUI_Tools.GetCustomStyle(false, GUI_Tools.BUTTONTYPE.NORMAL_LEFTALIGN)))                
                {
                    switch (category)
                    {
                        case 0:
                            if (TechnologyMatrix[category][i] == TechType.Cyclops)                           
                                ExecuteCommand(itemName + "  has spawned", "sub cyclops", (int)TechnologyMatrix[category][i]);                            
                            else                           
                                ExecuteCommand(itemName + "  has spawned", "spawn " + selectedTech, (int)TechnologyMatrix[category][i]);                                                                                   
                            break;

                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 12:
                        case 13:
                        case 14:
                        case 16:                            
                            ExecuteCommand(itemName + "  added to inventory", "item " + selectedTech, (int)TechnologyMatrix[category][i]);
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:                        
                        case 15:
                        case 17:
                            ExecuteCommand(itemName + "  has spawned", "spawn " + selectedTech, (int)TechnologyMatrix[category][i]);                            
                            break;
                        case 18:
                            ExecuteCommand("Blueprint:  " + itemName + "  unlocked", "unlock " + selectedTech, (int)TechnologyMatrix[category][i]);                            
                            break;
                        case 19:
                            prevCwPos = string.Format("{0:D} {1:D} {2:D}", (int)currentWorldPos.x, (int)currentWorldPos.y, (int)currentWorldPos.z);
                            ExecuteCommand("Player Warped to:  " + itemName + "\n(" + selectedTech + ")", "warp " + selectedTech, i);                            
                            Utils.PlayFMODAsset(warpSound, Player.main.transform, 20f);                            
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

                GUI.Box(new Rect(scrollRect.x, scrollRect.y, scrollRect.width, 23), "Vehicle Settings:", GUI_Tools.Box);
                
                if (GUI.Button(new Rect(scrollRect.x, scrollRect.y + 27, (scrollRect.width / 2) - 2, 22), seamothName + " Can Fly", seamothCanFly ? GUI_Tools.GetCustomStyle(true, GUI_Tools.BUTTONTYPE.TOGGLE_CENTER) : GUI_Tools.GetCustomStyle(false, GUI_Tools.BUTTONTYPE.TOGGLE_CENTER)))
                {
                    seamothCanFly = !seamothCanFly;                    
                }

                if (GUI.Button(new Rect(scrollRect.x + (scrollRect.width / 2) + 4f, scrollRect.y + 27, (scrollRect.width / 2) - 4f, 22), seaGlideName + " Speed: " + (seaGlideFastSpeed ? "Fast" : "Normal"), seaGlideFastSpeed ? GUI_Tools.GetCustomStyle(true, GUI_Tools.BUTTONTYPE.TOGGLE_CENTER) : GUI_Tools.GetCustomStyle(false, GUI_Tools.BUTTONTYPE.TOGGLE_CENTER)))
                {
                    seaGlideFastSpeed = !seaGlideFastSpeed;
                }                

                GUI.Label(new Rect(scrollRect.x, scrollRect.y + 53, 250, 22), seamothName + " speed multiplier: " + string.Format("{0:#.##}", seamothSpeedMultiplier));
                
                seamothSpeedMultiplier = GUI.HorizontalSlider(new Rect(scrollRect.x, scrollRect.y + 79, scrollRect.width, 10), seamothSpeedMultiplier, 1.0F, 5.0F);

                GUI.Label(new Rect(scrollRect.x, scrollRect.y + 93 , 250, 22), exosuitName + " speed multiplier: " + string.Format("{0:#.##}",exosuitSpeedMultiplier));
                exosuitSpeedMultiplier = GUI.HorizontalSlider(new Rect(scrollRect.x, scrollRect.y + 119, scrollRect.width, 10), exosuitSpeedMultiplier, 1.0F, 5.0F);

                GUI.Label(new Rect(scrollRect.x, scrollRect.y + 133, 250, 22), cyclopsName + " speed multiplier: " + string.Format("{0:#.##}", cyclopsSpeedMultiplier));
                cyclopsSpeedMultiplier = GUI.HorizontalSlider(new Rect(scrollRect.x, scrollRect.y + 159, scrollRect.width, 10), cyclopsSpeedMultiplier, 1.0F, 5.0F);
                
            }    
            
        }

              
    }
}


