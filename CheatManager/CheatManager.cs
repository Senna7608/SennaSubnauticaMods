//#define DEBUG_PROGRAM
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common;
using CheatManager.Configuration;

namespace CheatManager
{
    public class CheatManager : MonoBehaviour
    {  
        internal CheatManager Instance { get; private set; }

        internal Player PlayerMain { get; private set; }

        internal ButtonControl buttonControl;
        internal ButtonText buttonText;
        internal TechnologyMatrix techMatrix;
        internal WarpTargets warpTargets;

        private bool initStyles = false;

        private Vector2 scrollPos = Vector2.zero;
        private bool isActive;
        private Rect windowRect = new Rect();
        private readonly float space = 3;

        //private readonly string[][] WarpData; = WarpTargets.Targets;

        private string windowTitle;
        internal string prevCwPos = null;

        internal bool seamothCanFly = false;

        internal Utils.MonitoredValue<bool> isSeaglideFast = new Utils.MonitoredValue<bool>();

        internal float seamothSpeedMultiplier;
        internal float exosuitSpeedMultiplier;
        internal float cyclopsSpeedMultiplier;

        internal float playerPrevInfectionLevel = 0f;

        private int normalButtonID = -1;
        private int toggleButtonID = -1;
        private int daynightTabID = 4;
        private int categoriesTabID = 0;
        private int vehicleSettingsID = -1;

        private int currentdaynightTab = 4;
        private int currentTab = 0;

        internal FMODAsset warpSound;

        internal string seamothName;
        internal string exosuitName;
        internal string cyclopsName;

        private List<TechnologyMatrix.TechTypeData>[] tMatrix;

        private List<GUIHelper.ButtonInfo> Buttons;
        private List<GUIHelper.ButtonInfo> toggleButtons;
        private List<GUIHelper.ButtonInfo> daynightTab;
        private List<GUIHelper.ButtonInfo> categoriesTab;
        private List<GUIHelper.ButtonInfo> vehicleSettings;

        internal bool initToggleButtons = false;

#if DEBUG_PROGRAM
            internal static float crTimer = 10;
#endif                
     
        public void Awake()
        {
            Instance = Main.Instance;
            useGUILayout = false;
            
            UpdateTitle();
            warpTargets = new WarpTargets();
            warpSound = ScriptableObject.CreateInstance<FMODAsset>();
            warpSound.path = "event:/tools/gravcannon/fire";

            techMatrix = new TechnologyMatrix();

            tMatrix = new List<TechnologyMatrix.TechTypeData>[techMatrix.baseTechMatrix.Length];

            techMatrix.InitTechMatrixList(ref tMatrix);

            if (Main.isExistsSMLHelperV2)
            {
                techMatrix.IsExistsModdersTechTypes(ref tMatrix, techMatrix.Known_Modded_TechTypes);
            }
            else
            {
                Main.logger.Log("[CheatManager] Warning:\n'SMLHelper.V2' not found! Some functions are not available!", LogType.Warning);
            }

            techMatrix.SortTechLists(ref tMatrix);

            Buttons = new List<GUIHelper.ButtonInfo>();
            toggleButtons = new List<GUIHelper.ButtonInfo>();
            daynightTab = new List<GUIHelper.ButtonInfo>();
            categoriesTab = new List<GUIHelper.ButtonInfo>();
            vehicleSettings = new List<GUIHelper.ButtonInfo>();

            buttonText = new ButtonText();

            GUIHelper.CreateButtonsGroup(buttonText.Buttons, GUIHelper.BUTTONTYPE.NORMAL_CENTER, ref Buttons);
            GUIHelper.CreateButtonsGroup(buttonText.ToggleButtons, GUIHelper.BUTTONTYPE.TOGGLE_CENTER, ref toggleButtons);
            GUIHelper.CreateButtonsGroup(buttonText.DayNightTab, GUIHelper.BUTTONTYPE.TAB_CENTER, ref daynightTab);
            GUIHelper.CreateButtonsGroup(buttonText.CategoriesTab, GUIHelper.BUTTONTYPE.TAB_CENTER, ref categoriesTab);

            var searchSeaGlide = new TechnologyMatrix.TechTypeSearch(TechType.Seaglide);
            string seaglideName = tMatrix[1][tMatrix[1].FindIndex(searchSeaGlide.EqualsWith)].Name;

            var searchSeamoth = new TechnologyMatrix.TechTypeSearch(TechType.Seamoth);
            seamothName = tMatrix[0][tMatrix[0].FindIndex(searchSeamoth.EqualsWith)].Name;

            var searchExosuit = new TechnologyMatrix.TechTypeSearch(TechType.Exosuit);
            exosuitName = tMatrix[0][tMatrix[0].FindIndex(searchExosuit.EqualsWith)].Name;

            var searchCyclops = new TechnologyMatrix.TechTypeSearch(TechType.Cyclops);
            cyclopsName = tMatrix[0][tMatrix[0].FindIndex(searchCyclops.EqualsWith)].Name;

            string[] vehicleSetButtons = { $"{seamothName} Can Fly", $"{seaglideName} Speed Fast" };

            GUIHelper.CreateButtonsGroup(vehicleSetButtons, GUIHelper.BUTTONTYPE.TOGGLE_CENTER, ref vehicleSettings);

            daynightTab[4].Pressed = true;
            categoriesTab[0].Pressed = true;
            toggleButtons[17].Pressed = false;
            Buttons[7].Enabled = false;
            Buttons[7].Pressed = true;

            exosuitSpeedMultiplier = 1;
            cyclopsSpeedMultiplier = 1;

            buttonControl = new ButtonControl();
        }        

        public void OnDestroy()
        {
            Buttons = null;
            toggleButtons = null;
            daynightTab = null;
            categoriesTab = null;
            vehicleSettings = null;
            tMatrix = null;
            initToggleButtons = false;
            prevCwPos = null;
            warpSound = null;            
            isActive = false;
            seamothCanFly = false;            
            initStyles = false;
            isSeaglideFast.changedEvent.RemoveHandler(this, IsSeaglideFast);
        }

        public void Start()
        {
            isSeaglideFast.changedEvent.AddHandler(this, new Event<Utils.MonitoredValue<bool>>.HandleFunction(IsSeaglideFast));
            

#if DEBUG_PROGRAM
            StartCoroutine(DebugProgram());
#endif
        }        

#if DEBUG_PROGRAM
        private IEnumerator DebugProgram()
        {
            yield return new WaitForSeconds(crTimer);
            print($"[CheatManager] Coroutine Debugger, recall every {crTimer} seconds\n");
            if (isActive)
                StartCoroutine(DebugProgram());
        }
#endif


        private void IsSeaglideFast(Utils.MonitoredValue<bool> parms)
        {
            SeaglideOverDrive.Instance.SetSeaglideSpeed();            
        }

        internal void UpdateTitle()
        {
            windowTitle = $"CheatManager v.{Config.VERSION}, {Config.KEYBINDINGS["ToggleWindow"]} Toggle Window, {Config.KEYBINDINGS["ToggleMouse"]} Toggle Mouse";
        }

        public void Update()
        {
            if (Player.main != null)
            {                
                if (Input.GetKeyDown(Config.KEYBINDINGS["ToggleWindow"]))
                {
                    isActive = !isActive;
                }

                if (isActive)
                {
                    if (Input.GetKeyDown(Config.KEYBINDINGS["ToggleMouse"]))
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
                        buttonControl.NormalButtonControl(normalButtonID, ref Buttons, ref toggleButtons);                        
                    }

                    if (toggleButtonID != -1)
                    {
                        buttonControl.ToggleButtonControl(toggleButtonID, ref toggleButtons);                        
                    }

                    if (daynightTabID != -1)
                    {
                        buttonControl.DayNightButtonControl(daynightTabID, ref currentdaynightTab, ref daynightTab);
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
                            seamothCanFly = !seamothCanFly;
                            vehicleSettings[0].Pressed = seamothCanFly;                            
                        }

                        if (vehicleSettingsID == 1)
                        {
                            if (SeaglideOverDrive.Instance != null)
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
                    PlayerMain.infectedMixin.SetInfectedAmount(0f);
                }                                        
            }
        }

        internal void ReadGameValues()
        {
            PlayerMain = Player.main;
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
        

        public void ExecuteCommand(object message, object command)
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
                return;

            if (!initStyles)
                initStyles = GUIHelper.InitGUIStyles();

            windowRect = GUIHelper.CreatePopupWindow(new Rect(Screen.width - (Screen.width / 4.8f), 0, Screen.width / 4.8f, Screen.height / 4 * 3), windowTitle);

            float lastYcoord = windowRect.y;
            float baseHeight = windowRect.height;

            windowRect.x += 5;
            windowRect.y += space;
            windowRect.width -= 10;
            windowRect.height = 22;

            GUI.Label(windowRect, "Commands:");

            normalButtonID = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x, windowRect.y + 22, windowRect.width, baseHeight), space, 4, Buttons, out lastYcoord);

            GUI.Label(new Rect(windowRect.x, lastYcoord + space, 150, 22), "Toggle Commands:");            

            toggleButtonID = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x, lastYcoord + space + 22, windowRect.width, baseHeight - (lastYcoord + 22 + space)), space, 4, toggleButtons, out lastYcoord);            

            GUI.Label(new Rect(windowRect.x, lastYcoord + space, 150, 22), "Day/Night Speed:");            

            daynightTabID = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x, lastYcoord + space + 22, windowRect.width, baseHeight), space, 6, daynightTab , out lastYcoord);            

            GUI.Label(new Rect(windowRect.x, lastYcoord + space, 100, 22), "Categories:");            

            categoriesTabID = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x, lastYcoord + space + 22, windowRect.width, baseHeight), space, 4, categoriesTab, out lastYcoord);

            GUI.Label(new Rect(windowRect.x, lastYcoord + space, 150, 22), "Select Item in Category:");            

            GUI.Label(new Rect(windowRect.x + 150, lastYcoord + space, 100, 22), categoriesTab[currentTab].Name, GUIHelper.Label);
            
            windowRect.x = windowRect.x + 5;
            windowRect.y = lastYcoord + 22 + (space * 2);
            windowRect.width = windowRect.width - 10;
            windowRect.height = (baseHeight - windowRect.y) + 20;            
            
            TabControl(currentTab);            
        }       


        private void TabControl(int category)
        {
            int scrollItems;

            if (category == 19)
                scrollItems = warpTargets.Targets.Length;            
            else           
                scrollItems = tMatrix[category].Count;            

            float width = windowRect.width;

            if (scrollItems > 10 && category != 0)
                width -= 20;

            if (scrollItems > 4 && category == 0)
            {
                windowRect.height = 104;
                width -= 20;
            }

            scrollPos = GUI.BeginScrollView(windowRect, scrollPos, new Rect(windowRect.x, windowRect.y, width, scrollItems * 26));

            string itemName, selectedTech;

            for (int i = 0; i < scrollItems; i++)
            {
                if (category == 19)
                {
                    itemName = warpTargets.Targets[i][1];
                    selectedTech = warpTargets.Targets[i][0];                    
                }                
                else
                {                    
                    itemName = tMatrix[category][i].Name; 
                    selectedTech = tMatrix[category][i].TechType.ToString();                    
                }               

                if (GUI.Button(new Rect(windowRect.x, windowRect.y + (i * 26), width, 22), itemName, GUIHelper.GetGUIStyle(null, GUIHelper.BUTTONTYPE.NORMAL_LEFTALIGN)))                
                {
                    switch (category)
                    {
                        case 0:
                            if (!PlayerMain.IsInBase() && !PlayerMain.IsInSubmarine() && !PlayerMain.escapePod.value)
                            {
                                if (tMatrix[category][i].TechType == TechType.Cyclops)
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
                windowRect.y += (4 * 26) + 2;

                GUI.Box(new Rect(windowRect.x, windowRect.y, windowRect.width, 23), "Vehicle Settings:", GUIHelper.Box);
                
                vehicleSettingsID = GUIHelper.CreateButtonsGrid(new Rect(windowRect.x - 5 , windowRect.y + 27, windowRect.width + 10, 22), space, 2, vehicleSettings, out float lastYcoord);
                
                GUI.Label(new Rect(windowRect.x, windowRect.y + 53, 250, 22), seamothName + " speed multiplier: " + string.Format("{0:#.##}", seamothSpeedMultiplier));
                
                seamothSpeedMultiplier = (GUI.HorizontalSlider(new Rect(windowRect.x, windowRect.y + 79, windowRect.width, 10), seamothSpeedMultiplier, 1f, 5f));

                GUI.Label(new Rect(windowRect.x, windowRect.y + 93 , 250, 22), exosuitName + " speed multiplier: " + string.Format("{0:#.##}",exosuitSpeedMultiplier));
                exosuitSpeedMultiplier = GUI.HorizontalSlider(new Rect(windowRect.x, windowRect.y + 119, windowRect.width, 10), exosuitSpeedMultiplier, 1f, 5f);

                GUI.Label(new Rect(windowRect.x, windowRect.y + 133, 250, 22), cyclopsName + " speed multiplier: " + string.Format("{0:#.##}", cyclopsSpeedMultiplier));
                cyclopsSpeedMultiplier = GUI.HorizontalSlider(new Rect(windowRect.x, windowRect.y + 159, windowRect.width, 10), cyclopsSpeedMultiplier, 1.0F, 5.0F);
                
            }    
            
        }
        
        private void Teleport(string name, string Vector3string)
        {
            Vector3 currentWorldPos = MainCamera.camera.transform.position;
            prevCwPos = string.Format("{0:D} {1:D} {2:D}", (int)currentWorldPos.x, (int)currentWorldPos.y, (int)currentWorldPos.z);
            if (IsPlayerInVehicle())
            {
                PlayerMain.GetVehicle().TeleportVehicle(warpTargets.ConvertStringPosToVector3(Vector3string), Quaternion.identity);
                PlayerMain.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to: {name}\n({Vector3string})");
            }
            else
            {
                ExecuteCommand($"Player Warped to: {name}\n({Vector3string})", $"warp {Vector3string}");
            }

            Utils.PlayFMODAsset(warpSound, PlayerMain.transform, 20f);
        }

        internal bool IsPlayerInVehicle()
        {
            if (PlayerMain.inSeamoth == true || PlayerMain.inExosuit == true)
            {
                return true;
            }

            return false;
        }

        internal void OverPower(bool enable)
        {
            Oxygen o2 = PlayerMain.GetComponent<OxygenManager>().GetComponent<Oxygen>();

            if (enable)
            {
                PlayerMain.GetComponent<Survival>().SetPrivateField("kUpdateHungerInterval", 10 / Main.OverPowerMultiplier);

                if (o2.isPlayer)
                {
                    o2.oxygenCapacity = 45 * Main.OverPowerMultiplier;
                }

                PlayerMain.liveMixin.data.maxHealth = 100 * Main.OverPowerMultiplier;
                PlayerMain.liveMixin.health = 100 * Main.OverPowerMultiplier;
            }
            else
            {
                PlayerMain.GetComponent<Survival>().SetPrivateField("kUpdateHungerInterval", 10f);

                if (o2.isPlayer)
                {
                    o2.oxygenCapacity = 45f;
                    o2.oxygenAvailable = 45f;
                }

                PlayerMain.liveMixin.data.maxHealth = 100f;
                PlayerMain.liveMixin.health = 100f;
            }
        }
    }
}


