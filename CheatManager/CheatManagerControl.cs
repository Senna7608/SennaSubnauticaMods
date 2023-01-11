using System.Collections.Generic;
using UnityEngine;
using UWE;
using Common;
using Common.GUIHelper;
using CheatManager.Configuration;
using CheatManager.NewCommands;
using Common.Helpers;
using System.Linq;

namespace CheatManager
{
    public partial class CheatManagerControl : MonoBehaviour
    {
        public ObjectHelper objectHelper = new ObjectHelper();
                
        internal ButtonText buttonText;
        internal TechnologyMatrix techMatrix;        
        private Vector2 scrollPos;

        private static Rect windowRect = new Rect(Screen.width - (Screen.width / CmConfig.ASPECT), 0, Screen.width / CmConfig.ASPECT, (Screen.height / 4 * 3) - 2);
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
        internal List<GuiItem> warpExtras = new List<GuiItem>();

        internal bool isActive;
        internal bool isDirty = false;
        internal bool initToggleButtons = false;

        internal IntVector prevCwPos;
        internal string seamothName;
        internal string exosuitName;
        internal string cyclopsName;

        internal float seamothSpeedMultiplier;
        internal float exosuitSpeedMultiplier;
        internal float cyclopsSpeedMultiplier;        

        private const int SPACE = 4;
        private const int ITEMSIZE = 23;
        private const int SLIDERHEIGHT = 34;
        private const int MAXSHOWITEMS = 4;

        private string windowTitle;

        private GuiItemEvent CommandsGroup = new GuiItemEvent(-1, -1, false);
        private GuiItemEvent ToggleCommandsGroup = new GuiItemEvent(-1, -1, false);
        private GuiItemEvent DayNightGroup = new GuiItemEvent(-1, -1, false);
        private GuiItemEvent CategoriesGroup = new GuiItemEvent(-1, -1, false);
        private GuiItemEvent ScrollViewGroup = new GuiItemEvent(-1, -1, false);
        private GuiItemEvent VehicleSettingsGroup = new GuiItemEvent(-1, -1, false);
        private GuiItemEvent WarpExtrasGroup = new GuiItemEvent(-1, -1, false);

        private int currentdaynightTab = 4;
        private int currentTab = 0;
        private bool filterFast;
        
        public void Awake()
        {
            SNLogger.Trace("Awake started.");

            DontDestroyOnLoad(this);
            useGUILayout = false;

            gameObject.AddComponent<AlwaysDayConsoleCommand>();
            gameObject.AddComponent<OverPowerConsoleCommand>();
            gameObject.AddComponent<NoInfectConsoleCommand>();

            SNLogger.Debug("Console commands added.");

            UpdateTitle();
            
            warpSound = ScriptableObject.CreateInstance<FMODAsset>();
            warpSound.path = "event:/tools/gravcannon/fire";

            SNLogger.Debug("Warpsound created.");

            techMatrix = new TechnologyMatrix();
            tMatrix = new List<TechTypeData>[techMatrix.baseTechMatrix.Count];
            techMatrix.InitTechMatrixList(ref tMatrix);

            SNLogger.Debug("Base Tech matrix created.");

            
            if (CmConfig.Section_userWarpTargets.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in CmConfig.Section_userWarpTargets)
                {
                    WarpTargets_User.Add(new IntVector(kvp.Key), kvp.Value);
                }

                SNLogger.Debug("User warp targets added.");
            }            

            techMatrix.GetModdedTechTypes(ref tMatrix);

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

                if (tMatrix[i].Count * (ITEMSIZE + SPACE) > scrollRect.height)
                    width -= 20;                
                
                scrollItemRects[i] = SNWindow.SetGridItemsRect(new Rect(0, 0, width, tMatrix[i].Count * (ITEMSIZE + SPACE)), 1, tMatrix[i].Count, ITEMSIZE, SPACE, 2, false, false, true);
            }

            int warpCounts = WarpTargets_Internal.Count + WarpTargets_User.Count;

            scrollItemRects[tMatrix.Length] = SNWindow.SetGridItemsRect(new Rect(0, 0, drawRect.width - 20, warpCounts * (ITEMSIZE + SPACE)), 1, warpCounts, ITEMSIZE, SPACE, 2, false, false, true);
            
            scrollItemsList = new List<GuiItem>[tMatrix.Length + 1];
            
            for (int i = 0; i < tMatrix.Length; i++)
            {
                scrollItemsList[i] = new List<GuiItem>();
                CreateTechGroup(tMatrix[i], scrollItemRects[i], GuiItemType.NORMALBUTTON, ref scrollItemsList[i], new GuiItemColor(GuiColor.Gray, GuiColor.Green, GuiColor.White),  GuiItemState.NORMAL, true, FontStyle.Normal, TextAnchor.MiddleLeft);                
            }
            
            scrollItemsList[tMatrix.Length] = new List<GuiItem>();
            AddListToGroup(GetWarpTargetNames(), scrollItemRects[tMatrix.Length], GuiItemType.NORMALBUTTON, ref scrollItemsList[tMatrix.Length], new GuiItemColor(GuiColor.Gray, GuiColor.Green, GuiColor.White), GuiItemState.NORMAL, true, FontStyle.Normal, TextAnchor.MiddleLeft);
            
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

            string[] warpExtrasButtons = { "Add current position to list", "Remove selected from list" };
            scrollRectheight = 11 * (scrollItemsList[0][0].Rect.height + 2);
            y = scrollRect.y + scrollRectheight + SPACE + 2;
            List<Rect> warpExtrasRects = new Rect(drawRect.x, y, drawRect.width, drawRect.height).SetGridItemsRect(2, 1, ITEMSIZE, SPACE, SPACE, false, false);
            warpExtras.CreateGuiItemsGroup(warpExtrasButtons, warpExtrasRects, GuiItemType.NORMALBUTTON, new GuiItemColor());

            commands[(int)Commands.BackWarp].Enabled = false;
            commands[(int)Commands.BackWarp].State = GuiItemState.PRESSED;

            daynightTab.SetStateInverseTAB(4);
            categoriesTab[0].State = GuiItemState.PRESSED;

            seamothSpeedMultiplier = 1;
            exosuitSpeedMultiplier = 1;
            cyclopsSpeedMultiplier = 1;

            SNLogger.Trace("Awake completed.");
        }

        public void AddListToGroup(List<string> names, List<Rect> rects, GuiItemType type, ref List<GuiItem> guiItems, GuiItemColor itemColor,
                                               GuiItemState state = GuiItemState.NORMAL, bool enabled = true, FontStyle fontStyle = FontStyle.Normal,
                                               TextAnchor textAnchor = TextAnchor.MiddleCenter)
        {            
            for (int i = 0; i < names.Count; i++)
            {
                guiItems.Add(new GuiItem()
                {
                    Name = names[i],
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
            CmConfig.Write();
            commands = null;            
            toggleCommands = null;
            daynightTab = null;
            categoriesTab = null;
            vehicleSettings = null;
            tMatrix = null;
            initToggleButtons = false;            
            warpSound = null;            
            isActive = false;            
            onConsoleCommandEntered.RemoveHandler(this, OnConsoleCommandEntered);
            onFilterFastChanged.RemoveHandler(this, OnFilterFastChanged);
        }

        public void Start()
        {                       
            onConsoleCommandEntered.AddHandler(this, new Event<string>.HandleFunction(OnConsoleCommandEntered));
            onFilterFastChanged.AddHandler(this, new Event<bool>.HandleFunction(OnFilterFastChanged));
        }        

        private void OnFilterFastChanged(bool enabled)
        {
            filterFast = enabled;                        
        }

        private void OnConsoleCommandEntered(string command)
        {            
            UpdateButtonsState();                     
        }

        internal void UpdateTitle()
        {
            windowTitle = $"CheatManager v.{CmConfig.PROGRAM_VERSION}, {CmConfig.KEYBINDINGS["ToggleWindow"]} Toggle Window, {CmConfig.KEYBINDINGS["ToggleMouse"]} Toggle Mouse";
        }

        public void Update()
        {
            if (Player.main != null)
            {
                if (!initToggleButtons && !WaitScreen.IsWaiting)
                {
                    SetToggleButtons();
                    initToggleButtons = true;
                    UpdateButtonsState();
                }

                if (Input.GetKeyDown(CmConfig.KEYBINDINGS["ToggleWindow"]))
                {
                    isActive = !isActive;
                }

                if (!isActive)
                    return;
                
                if (Input.GetKeyDown(CmConfig.KEYBINDINGS["ToggleMouse"]))
                {
                    UWE.Utils.lockCursor = !UWE.Utils.lockCursor;
                }                
                
                if (CommandsGroup.ItemID != -1 && CommandsGroup.MouseButton == 0)
                {
                    NormalButtonControl(CommandsGroup.ItemID, ref commands, ref toggleCommands);
                }

                if (ToggleCommandsGroup.ItemID != -1 && ToggleCommandsGroup.MouseButton == 0)
                {
                    ToggleButtonControl(ToggleCommandsGroup.ItemID, ref toggleCommands);
                }

                if (DayNightGroup.ItemID != -1 && DayNightGroup.MouseButton == 0)
                {
                    DayNightButtonControl(DayNightGroup.ItemID, ref currentdaynightTab, ref daynightTab);
                }

                if (CategoriesGroup.ItemID != -1 && CategoriesGroup.MouseButton == 0)
                {
                    if (CategoriesGroup.ItemID != currentTab)
                    {
                        //categoriesTab[currentTab].State = SNGUI.SetStateInverse(categoriesTab[currentTab].State);
                        //categoriesTab[categoriesTabID].State = SNGUI.SetStateInverse(categoriesTab[categoriesTabID].State);
                        currentTab = CategoriesGroup.ItemID;
                        scrollPos = Vector2.zero;
                    }
                }

                if (ScrollViewGroup.ItemID != -1)
                {
                    if (ScrollViewGroup.MouseButton == 0)
                    {
                        ScrollViewControl(currentTab, ScrollViewGroup.ItemID, ref scrollItemsList[currentTab], ref tMatrix, ref commands);
                    }
                    else if (currentTab == 19 && ScrollViewGroup.MouseButton == 1)
                    {
                        if (ScrollViewGroup.ItemID < WarpTargets_Internal.Count)
                        {
                            ErrorMessage.AddMessage($"CheatManager Warning!\nInternal warp points cannot be deleted!");
                            return;
                        }

                        scrollItemsList[currentTab].UnmarkAll();
                        scrollItemsList[currentTab][ScrollViewGroup.ItemID].SetStateInverse();
                    }
                }

                if (VehicleSettingsGroup.ItemID != -1 && VehicleSettingsGroup.MouseButton == 0)
                {
                    if (VehicleSettingsGroup.ItemID == 0)
                    {
                        isSeamothCanFly.Update(!isSeamothCanFly.value);
                        vehicleSettings[0].State = SNGUI.ConvertBoolToState(isSeamothCanFly.value);
                    }

                    if (VehicleSettingsGroup.ItemID == 1)
                    {
                        isSeaglideFast.Update(!isSeaglideFast.value);
                        vehicleSettings[1].State = SNGUI.ConvertBoolToState(isSeaglideFast.value);
                    }
                }

                if (WarpExtrasGroup.ItemID != -1 && WarpExtrasGroup.MouseButton == 0)
                {
                    if (WarpExtrasGroup.ItemID == 0)
                    {
                        IntVector position = new IntVector(Player.main.transform.position);

                        if (IsPositionWithinRange(position, out string nearestWarpName))
                        {
                            ErrorMessage.AddMessage($"CheatManager Warning!\nPosition cannot be added to the Warp list\nbecause it is very close to:\n{nearestWarpName} warp point!");
                        }
                        else
                        {
                            AddToList(position);
                        }
                    }

                    if (WarpExtrasGroup.ItemID == 1)
                    {
                        int item = scrollItemsList[currentTab].GetMarkedItem();

                        if (item == -1)
                        {
                            ErrorMessage.AddMessage("CheatManager Error!\nNo item selected from the user Warp list!");
                            return;
                        }

                        isDirty = true;

                        int userIndex = item - WarpTargets_Internal.Count;

                        IntVector intVector = WarpTargets_User.Keys.ElementAt(userIndex);


                        //WarpTargets_User.TryGetValue(intVector, out string value);

                        //print($"item: {item}, userIndex: {userIndex}, Internal.Count: {WarpTargets_Internal.Count}, User.Count: {WarpTargets_User.Count}");
                        //print($"Key: {intVector}, Value: {value}");
                        //print($"scrollItemsList[currentTab].Count: {scrollItemsList[currentTab].Count}");

                        RemoveFormList(intVector);

                        scrollItemsList[currentTab].RemoveGuiItemFromGroup(item);

                        isDirty = false;
                    }
                }

            }
        }
        

        private void SetToggleButtons()
        {
            foreach (KeyValuePair<string, string> kvp in CmConfig.Section_toggleButtons)
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
            if (!isActive || isDirty)
                return;

            SNWindow.CreateWindow(windowRect, windowTitle);

            CommandsGroup = commands.DrawGuiItemsGroup();
            ToggleCommandsGroup = toggleCommands.DrawGuiItemsGroup();
            DayNightGroup = daynightTab.DrawGuiItemsGroup();
            CategoriesGroup = categoriesTab.DrawGuiItemsGroup();
            
            if (currentTab == 0)
            {
                ScrollViewGroup = SNScrollView.CreateScrollView(scrollRect, ref scrollPos, ref scrollItemsList[currentTab], "Select Item in Category:", categoriesTab[currentTab].Name, MAXSHOWITEMS);

                VehicleSettingsGroup = vehicleSettings.DrawGuiItemsGroup();
                
                SNHorizontalSlider.CreateHorizontalSlider(sliders[0].Rect, ref seamothSpeedMultiplier, 1f, 5f, sliders[0].Name, sliders[0].OnChangedEvent);
                SNHorizontalSlider.CreateHorizontalSlider(sliders[1].Rect, ref exosuitSpeedMultiplier, 1f, 5f, sliders[1].Name, sliders[1].OnChangedEvent);
                SNHorizontalSlider.CreateHorizontalSlider(sliders[2].Rect, ref cyclopsSpeedMultiplier, 1f, 5f, sliders[2].Name, sliders[2].OnChangedEvent);
            }
            else if (currentTab == 19)
            {
                ScrollViewGroup = SNScrollView.CreateScrollView(scrollRect, ref scrollPos, ref scrollItemsList[currentTab], "Select Item in Category:", categoriesTab[currentTab].Name, 10);

                WarpExtrasGroup = warpExtras.DrawGuiItemsGroup();
            }
            else
            {
                ScrollViewGroup = SNScrollView.CreateScrollView(scrollRect, ref scrollPos, ref scrollItemsList[currentTab], "Select Item in Category:", categoriesTab[currentTab].Name);
            }
        }

        public void ExecuteCommand(object message, object command)
        {
            if (message != null)
            {
                ErrorMessage.AddMessage(message.ToString());
            }

            if (command != null)
            {
                SNLogger.Log((string)command);
                DevConsole.SendConsoleCommand(command.ToString());
            }
        }

        public bool IsPlayerInVehicle()
        {
            return Player.main.inSeamoth || Player.main.inExosuit ? true : false;
        }
    }
}


