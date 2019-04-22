//#define DEBUG_PROGRAM
using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common;
using Common.GUIHelper;
using static Common.GameHelper;
using CheatManager.Configuration;
using CheatManager.NewCommands;
using System;

namespace CheatManager
{
    public class CheatManager : MonoBehaviour
    {  
        internal CheatManager Instance { get; private set; }
        internal static WarpTargets warpTargets = new WarpTargets();
        internal ButtonControl buttonControl;
        internal ButtonText buttonText;
        internal TechnologyMatrix techMatrix;        
        private Vector2 scrollPos;

        private static Rect windowRect = new Rect(Screen.width - (Screen.width / Config.ASPECT), 0, Screen.width / Config.ASPECT, (Screen.height / 4 * 3) - 2);
        private Rect drawRect;
        private Rect scrollRect;

        internal FMODAsset warpSound;

        internal Utils.MonitoredValue<bool> isSeaglideFast = new Utils.MonitoredValue<bool>();
        internal Utils.MonitoredValue<bool> isSeamothCanFly = new Utils.MonitoredValue<bool>();
        internal Event<string> onConsoleCommandEntered = new Event<string>();
        internal Event<bool> onFilterFastChanged = new Event<bool>();
        internal Event<object> onSeamothSpeedValueChanged = new Event<object>();
        internal Event<object> onExosuitSpeedValueChanged = new Event<object>();
        internal Event<object> onCyclopsSpeedValueChanged = new Event<object>();

        internal List<TechTypeData>[] tMatrix;
        internal List<GuiItem>[] scrollItemsList;        

        internal List<GuiItem> commands = new List<GuiItem>();
        internal List<GuiItem> toggleCommands = new List<GuiItem>();
        internal List<GuiItem> daynightTab = new List<GuiItem>();
        internal List<GuiItem> categoriesTab = new List<GuiItem>();
        internal List<GuiItem> vehicleSettings = new List<GuiItem>();
        internal List<GuiItem> sliders = new List<GuiItem>();

        internal bool isActive;        
        internal bool initToggleButtons = false;
               
        internal string prevCwPos = null;
        internal string seamothName;
        internal string exosuitName;
        internal string cyclopsName;

        internal float seamothSpeedMultiplier;
        internal float exosuitSpeedMultiplier;
        internal float cyclopsSpeedMultiplier;        

        private const int SPACE = 5;
        private const int ITEMSIZE = 24;
        private const int SLIDERHEIGHT = 34;
        private const int MAXSHOWITEMS = 4;

        private string windowTitle;
        private int normalButtonID = -1;
        private int toggleButtonID = -1;
        private int daynightTabID = 4;
        private int categoriesTabID = 0;
        private int scrollviewID = -1;
        private int vehicleSettingsID = -1;
        private int currentdaynightTab = 4;
        private int currentTab = 0;
        private bool filterFast;

#if DEBUG_PROGRAM
            internal static float crTimer = 10;
#endif

        public void Awake()
        {
            Instance = this;
            useGUILayout = false;
#if DEBUG
            isActive = true;
#endif
            UpdateTitle();            
            warpSound = ScriptableObject.CreateInstance<FMODAsset>();
            warpSound.path = "event:/tools/gravcannon/fire";

            techMatrix = new TechnologyMatrix();
            tMatrix = new List<TechTypeData>[techMatrix.baseTechMatrix.Count];
            techMatrix.InitTechMatrixList(ref tMatrix);           

            
            if (Main.isExistsSMLHelperV2)
            {
                techMatrix.IsExistsModdersTechTypes(ref tMatrix, techMatrix.Known_Modded_TechTypes);
            }
            else
            {
                SNLogger.Log($"[{Config.PROGRAM_NAME}] Warning: 'SMLHelper.V2' not found! Some functions are not available!");
            }
            
            techMatrix.SortTechLists(ref tMatrix);
            
            buttonText = new ButtonText();                        

            drawRect = SNWindow.InitWindowRect(windowRect, true);

            List<Rect> commandRects = drawRect.SetGridItemsRect( 4, 2, ITEMSIZE, SPACE, SPACE, true, true);
            commands.CreateGuiItemsGroup(buttonText.Buttons, commandRects, GuiItemType.NORMALBUTTON, new GuiItemColor());
            commands.SetGuiItemsGroupLabel("Commands", commandRects.GetLast(), new GuiItemColor(GuiColor.White));

            List<Rect> toggleCommandRects = new Rect(drawRect.x, SNWindow.GetNextYPos(ref commandRects), drawRect.width, drawRect.height).SetGridItemsRect( 4, 5, ITEMSIZE, SPACE, SPACE, true, true);
            toggleCommands.CreateGuiItemsGroup(buttonText.ToggleButtons, toggleCommandRects, GuiItemType.TOGGLEBUTTON, new GuiItemColor(GuiColor.Red, GuiColor.Green));
            toggleCommands.SetGuiItemsGroupLabel("Toggle Commands", toggleCommandRects.GetLast(), new GuiItemColor(GuiColor.White));

            List<Rect> daynightTabrects = new Rect(drawRect.x, SNWindow.GetNextYPos(ref toggleCommandRects), drawRect.width, drawRect.height).SetGridItemsRect( 6, 1, ITEMSIZE, SPACE, SPACE, true, true);
            daynightTab.CreateGuiItemsGroup(buttonText.DayNightTab, daynightTabrects, GuiItemType.TAB, new GuiItemColor());
            daynightTab.SetGuiItemsGroupLabel("Day/Night Speed:", daynightTabrects.GetLast(), new GuiItemColor(GuiColor.White));

            List<Rect> categoriesTabrects = new Rect(drawRect.x, SNWindow.GetNextYPos(ref daynightTabrects), drawRect.width, drawRect.height).SetGridItemsRect( 4, 5, ITEMSIZE, SPACE, SPACE, true, true);
            categoriesTab.CreateGuiItemsGroup(buttonText.CategoriesTab, categoriesTabrects, GuiItemType.TAB, new GuiItemColor(GuiColor.Gray, GuiColor.Green, GuiColor.White));
            categoriesTab.SetGuiItemsGroupLabel("Categories:", categoriesTabrects.GetLast(), new GuiItemColor(GuiColor.White));            

            float nextYpos = SNWindow.GetNextYPos(ref categoriesTabrects);
            scrollRect = new Rect(drawRect.x + SPACE, nextYpos, drawRect.width - (SPACE * 2), drawRect.height - nextYpos);

            List<Rect>[] scrollItemRects = new List<Rect>[tMatrix.Length + 1];

            for (int i = 0; i < tMatrix.Length; i++)
            {
                float width = drawRect.width;

                if (i == 0 && tMatrix[0].Count > MAXSHOWITEMS)
                    width -= 20;

                if (tMatrix[i].Count * 26 > scrollRect.height)
                    width -= 20;                
                
                scrollItemRects[i] = SNWindow.SetGridItemsRect(new Rect(0, 0, width, tMatrix[i].Count * (ITEMSIZE + SPACE)), 1, tMatrix[i].Count, ITEMSIZE, SPACE, 2, false, false, true);
            }

            scrollItemRects[tMatrix.Length] = SNWindow.SetGridItemsRect(new Rect(0, 0, drawRect.width - 20, warpTargets.Targets.Count * (ITEMSIZE + SPACE)), 1, warpTargets.Targets.Count, ITEMSIZE, SPACE, 2, false, false, true);
            
            scrollItemsList = new List<GuiItem>[tMatrix.Length + 1];
            
            for (int i = 0; i < tMatrix.Length; i++)
            {
                scrollItemsList[i] = new List<GuiItem>();
                CreateTechGroup(tMatrix[i], scrollItemRects[i], GuiItemType.NORMALBUTTON, ref scrollItemsList[i], new GuiItemColor(GuiColor.Gray, GuiColor.Green, GuiColor.White),  GuiItemState.NORMAL, true, FontStyle.Normal, TextAnchor.MiddleLeft);                
            }
            
            scrollItemsList[tMatrix.Length] = new List<GuiItem>();
            AddListToGroup(warpTargets.Targets, scrollItemRects[tMatrix.Length], GuiItemType.NORMALBUTTON, ref scrollItemsList[tMatrix.Length], new GuiItemColor(GuiColor.Gray, GuiColor.Green, GuiColor.White), GuiItemState.NORMAL, true, FontStyle.Normal, TextAnchor.MiddleLeft);
            
            var searchSeaGlide = new TechnologyMatrix.TechTypeSearch(TechType.Seaglide);
            string seaglideName = tMatrix[1][tMatrix[1].FindIndex(searchSeaGlide.EqualsWith)].Name;

            var searchSeamoth = new TechnologyMatrix.TechTypeSearch(TechType.Seamoth);
            seamothName = tMatrix[0][tMatrix[0].FindIndex(searchSeamoth.EqualsWith)].Name;

            var searchExosuit = new TechnologyMatrix.TechTypeSearch(TechType.Exosuit);
            exosuitName = tMatrix[0][tMatrix[0].FindIndex(searchExosuit.EqualsWith)].Name;

            var searchCyclops = new TechnologyMatrix.TechTypeSearch(TechType.Cyclops);
            cyclopsName = tMatrix[0][tMatrix[0].FindIndex(searchCyclops.EqualsWith)].Name;

            string[] vehicleSetButtons = { $"{seamothName} Can Fly", $"{seaglideName} Speed Fast" };

            float scrollRectheight = 5 * (scrollItemsList[0][0].Rect.height + 2);            
            float y = scrollRect.y + scrollRectheight + SPACE;            

            List<Rect> vehicleSettingsRects = new Rect(drawRect.x, y, drawRect.width, drawRect.height).SetGridItemsRect( 2, 1, ITEMSIZE, SPACE, SPACE, false, true);
            vehicleSettings.CreateGuiItemsGroup(vehicleSetButtons, vehicleSettingsRects, GuiItemType.TOGGLEBUTTON, new GuiItemColor(GuiColor.Red, GuiColor.Green));
            vehicleSettings.SetGuiItemsGroupLabel("Vehicle settings:", vehicleSettingsRects.GetLast(), new GuiItemColor(GuiColor.White));

            string[] sliderLabels = { $"{seamothName} speed multiplier:", $"{exosuitName} speed multiplier:", $"{cyclopsName} speed multiplier:" };

            List<Rect> slidersRects = new Rect(drawRect.x, SNWindow.GetNextYPos(ref vehicleSettingsRects), drawRect.width, drawRect.height).SetGridItemsRect( 1, 3, SLIDERHEIGHT, SPACE, SPACE, false, false);
            sliders.CreateGuiItemsGroup(sliderLabels, slidersRects, GuiItemType.HORIZONTALSLIDER, new GuiItemColor());

            sliders[0].OnChangedEvent = onSeamothSpeedValueChanged;
            sliders[1].OnChangedEvent = onExosuitSpeedValueChanged;
            sliders[2].OnChangedEvent = onCyclopsSpeedValueChanged;

            commands[(int)Commands.BackWarp].Enabled = false;
            commands[(int)Commands.BackWarp].State = GuiItemState.PRESSED;
            
            daynightTab[4].State = GuiItemState.PRESSED;
            categoriesTab[0].State = GuiItemState.PRESSED;

            seamothSpeedMultiplier = 1;
            exosuitSpeedMultiplier = 1;
            cyclopsSpeedMultiplier = 1;
            
            buttonControl = new ButtonControl();            
        }

        public void AddListToGroup(List<string[]> names, List<Rect> rects, GuiItemType type, ref List<GuiItem> guiItems, GuiItemColor itemColor,
                                               GuiItemState state = GuiItemState.NORMAL, bool enabled = true, FontStyle fontStyle = FontStyle.Normal,
                                               TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {            
            for (int i = 0; i < names.Count; i++)
            {
                guiItems.Add(new GuiItem()
                {
                    Name = names[i][1],
                    Type = type,
                    Enabled = enabled,
                    Rect = rects[i],
                    ItemColor = itemColor,
                    State = state,
                    FontStyle = fontStyle,
                    TextAnchor = textAnchor
                });
            }            
        }
        
        public void CreateTechGroup(List<TechTypeData> techTypeDatas, List<Rect> rects, GuiItemType type, ref List<GuiItem> guiItems, GuiItemColor itemColor,
                                               GuiItemState state = GuiItemState.NORMAL, bool enabled = true, FontStyle fontStyle = FontStyle.Normal,
                                               TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {
            guiItems.Clear();
                       
            for (int i = 0; i < techTypeDatas.Count; i++)
            {
                guiItems.Add(new GuiItem()
                {
                    Name = techTypeDatas[i].Name,
                    Type = type,
                    Enabled = enabled,
                    Rect = rects[i],
                    ItemColor = itemColor,
                    State = state,
                    FontStyle = fontStyle,
                    TextAnchor = textAnchor
                });
            }
        }
     
        public void OnDestroy()
        {            
            commands = null;            
            toggleCommands = null;
            daynightTab = null;
            categoriesTab = null;
            vehicleSettings = null;
            tMatrix = null;
            initToggleButtons = false;
            prevCwPos = null;
            warpSound = null;            
            isActive = false;            
            onConsoleCommandEntered.RemoveHandler(this, OnConsoleCommandEntered);
            onFilterFastChanged.RemoveHandler(this, OnFilterFastChanged);
        }

        public void Start()
        {                       
            onConsoleCommandEntered.AddHandler(this, new Event<string>.HandleFunction(OnConsoleCommandEntered));
            onFilterFastChanged.AddHandler(this, new Event<bool>.HandleFunction(OnFilterFastChanged));

#if DEBUG_PROGRAM
            StartCoroutine(DebugProgram());
#endif
        }        

        private void OnFilterFastChanged(bool enabled)
        {
            filterFast = enabled;                        
        }

        private void OnConsoleCommandEntered(string command)
        {            
            UpdateButtonsState();                     
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

                if (!isActive)
                    return;
                
                if (Input.GetKeyDown(Config.KEYBINDINGS["ToggleMouse"]))
                {
                    UWE.Utils.lockCursor = !UWE.Utils.lockCursor;
                }

                if(!initToggleButtons && !uGUI.main.loading.IsLoading)
                {
                    SetToggleButtons();                                                
                    initToggleButtons = true;
                    UpdateButtonsState();
                }
                if (normalButtonID != -1)
                {                        
                    buttonControl.NormalButtonControl(normalButtonID, ref commands, ref toggleCommands);
                }

                if (toggleButtonID != -1)
                {                        
                    buttonControl.ToggleButtonControl(toggleButtonID, ref toggleCommands);
                }

                if (daynightTabID != -1)
                {
                    buttonControl.DayNightButtonControl(daynightTabID, ref currentdaynightTab, ref daynightTab);
                }

                if (categoriesTabID != -1)
                {
                    if (categoriesTabID != currentTab)
                    {
                        //categoriesTab[currentTab].State = SNGUI.SetStateInverse(categoriesTab[currentTab].State);
                        //categoriesTab[categoriesTabID].State = SNGUI.SetStateInverse(categoriesTab[categoriesTabID].State);
                        currentTab = categoriesTabID;
                        scrollPos = Vector2.zero;
                    }
                }

                if (scrollviewID != -1)
                {
                    buttonControl.ScrollViewControl(currentTab, ref scrollviewID, ref scrollItemsList[currentTab], ref tMatrix, ref commands);
                }

                if (vehicleSettingsID != -1)
                {
                    if (vehicleSettingsID == 0)
                    {                        
                        isSeamothCanFly.Update(!isSeamothCanFly.value);
                        vehicleSettings[0].State = SNGUI.ConvertBoolToState(isSeamothCanFly.value);                
                    }

                    if (vehicleSettingsID == 1)
                    {                        
                        isSeaglideFast.Update(!isSeaglideFast.value);
                        vehicleSettings[1].State = SNGUI.ConvertBoolToState(isSeaglideFast.value);                                                
                    }
                }                                                                     
            }
        }
        

        private void SetToggleButtons()
        {
            foreach (KeyValuePair<string, string> kvp in Config.Section_toggleButtons)
            {
                bool.TryParse(kvp.Value, out bool result);

                if (result)
                {
                    ExecuteCommand("", kvp.Key);
                }
            }
        }
        

        internal void UpdateButtonsState()
        {
            toggleCommands[(int)ToggleCommands.freedom].State = SNGUI.ConvertBoolToState(GameModeUtils.IsOptionActive(GameModeOption.NoSurvival));
            toggleCommands[(int)ToggleCommands.creative].State = SNGUI.ConvertBoolToState(GameModeUtils.IsOptionActive(GameModeOption.NoBlueprints));
            toggleCommands[(int)ToggleCommands.survival].State = SNGUI.ConvertBoolToState(GameModeUtils.RequiresSurvival());
            toggleCommands[(int)ToggleCommands.hardcore].State = SNGUI.ConvertBoolToState(GameModeUtils.IsPermadeath());
            toggleCommands[(int)ToggleCommands.fastbuild].State = SNGUI.ConvertBoolToState(NoCostConsoleCommand.main.fastBuildCheat);
            toggleCommands[(int)ToggleCommands.fastscan].State = SNGUI.ConvertBoolToState(NoCostConsoleCommand.main.fastScanCheat);
            toggleCommands[(int)ToggleCommands.fastgrow].State = SNGUI.ConvertBoolToState(NoCostConsoleCommand.main.fastGrowCheat);
            toggleCommands[(int)ToggleCommands.fasthatch].State = SNGUI.ConvertBoolToState(NoCostConsoleCommand.main.fastHatchCheat);
            toggleCommands[(int)ToggleCommands.filterfast].State = SNGUI.ConvertBoolToState(filterFast);            
            toggleCommands[(int)ToggleCommands.nocost].State = SNGUI.ConvertBoolToState(GameModeUtils.IsOptionActive(GameModeOption.NoCost));
            toggleCommands[(int)ToggleCommands.noenergy].State = SNGUI.ConvertBoolToState(GameModeUtils.IsCheatActive(GameModeOption.NoEnergy));
            toggleCommands[(int)ToggleCommands.nosurvival].State = SNGUI.ConvertBoolToState(GameModeUtils.IsOptionActive(GameModeOption.NoSurvival));
            toggleCommands[(int)ToggleCommands.oxygen].State = SNGUI.ConvertBoolToState(GameModeUtils.IsOptionActive(GameModeOption.NoOxygen));
            toggleCommands[(int)ToggleCommands.radiation].State = SNGUI.ConvertBoolToState(GameModeUtils.IsOptionActive(GameModeOption.NoRadiation));
            toggleCommands[(int)ToggleCommands.invisible].State = SNGUI.ConvertBoolToState(GameModeUtils.IsInvisible());
            //toggleCommands[(int)ToggleCommands.shotgun].State = shotgun cheat
            toggleCommands[(int)ToggleCommands.nodamage].State = SNGUI.ConvertBoolToState(NoDamageConsoleCommand.main.GetNoDamageCheat());
            toggleCommands[(int)ToggleCommands.noinfect].State = SNGUI.ConvertBoolToState(NoInfectConsoleCommand.main.GetNoInfectCheat());
            toggleCommands[(int)ToggleCommands.alwaysday].State = SNGUI.ConvertBoolToState(AlwaysDayConsoleCommand.main.GetAlwaysDayCheat());
            toggleCommands[(int)ToggleCommands.overpower].Enabled = GameModeUtils.RequiresSurvival();

            if (toggleCommands[(int)ToggleCommands.overpower].Enabled)
                toggleCommands[(int)ToggleCommands.overpower].State = SNGUI.ConvertBoolToState(OverPowerConsoleCommand.main.GetOverPowerCheat());

            vehicleSettings[0].State = SNGUI.ConvertBoolToState(isSeamothCanFly.value);
            vehicleSettings[1].State = SNGUI.ConvertBoolToState(isSeaglideFast.value);
        }               
        
        public void OnGUI()
        {
            if (!isActive)
                return;            

            SNWindow.CreateWindow(windowRect, windowTitle);

            normalButtonID = commands.DrawGuiItemsGroup();
            toggleButtonID = toggleCommands.DrawGuiItemsGroup();
            daynightTabID = daynightTab.DrawGuiItemsGroup();
            categoriesTabID = categoriesTab.DrawGuiItemsGroup();
            
            if (currentTab == 0)
            {
                scrollviewID = SNScrollView.CreateScrollView(scrollRect, ref scrollPos, ref scrollItemsList[currentTab], "Select Item in Category:", categoriesTab[currentTab].Name, MAXSHOWITEMS);

                vehicleSettingsID = vehicleSettings.DrawGuiItemsGroup();
                
                SNHorizontalSlider.CreateHorizontalSlider(sliders[0].Rect, ref seamothSpeedMultiplier, 1f, 5f, sliders[0].Name, sliders[0].OnChangedEvent);
                SNHorizontalSlider.CreateHorizontalSlider(sliders[1].Rect, ref exosuitSpeedMultiplier, 1f, 5f, sliders[1].Name, sliders[1].OnChangedEvent);
                SNHorizontalSlider.CreateHorizontalSlider(sliders[2].Rect, ref cyclopsSpeedMultiplier, 1f, 5f, sliders[2].Name, sliders[2].OnChangedEvent);
            }
            else
            {
                scrollviewID = SNScrollView.CreateScrollView(scrollRect, ref scrollPos, ref scrollItemsList[currentTab], "Select Item in Category:", categoriesTab[currentTab].Name);
            }
        }        
    }
}


