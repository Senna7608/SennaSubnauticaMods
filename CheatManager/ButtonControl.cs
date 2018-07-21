using System.Collections.Generic;
using Common.MyGUI;

namespace CheatManager
{
    internal static class ButtonControl
    {

        public static void ToggleButtonControl (int toggleButtonID, ref bool[] isToggle, ref List<GUI_Tools.ButtonInfo> toggleButtons, ref float playerPrevInfectionLevel)
        {
            switch (toggleButtonID)
            {
                case 9 when isToggle[1] == true:
                    break;
                case 11 when isToggle[1] == true:
                case 11 when isToggle[0] == true:
                    break;
                case 12 when isToggle[1] == true:
                    break;
                case 13 when isToggle[1] == true:
                    break;
                case 14 when isToggle[1] == true:
                    break;
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 16:
                    CheatManager.ExecuteCommand("", toggleButtons[toggleButtonID].Name, toggleButtonID);
                    break;
                case 15:
                    isToggle[15] = !isToggle[15];
                    CheatManager.ExecuteCommand(isToggle[15] ? "shotgun cheat is now True" : "shotgun cheat is now False", toggleButtons[toggleButtonID].Name, toggleButtonID);
                    break;
                case 8:
                    isToggle[8] = !isToggle[8];
                    CheatManager.ExecuteCommand(isToggle[8] ? "filterFast cheat is now True" : "filterFast cheat is now False", toggleButtons[toggleButtonID].Name, toggleButtonID);
                    break;
                case 17:
                    isToggle[17] = !isToggle[17];
                    if (isToggle[17])
                    {
                        ErrorMessage.AddMessage("alwaysDay cheat is now True");
                        DayNightCycle.main.sunRiseTime = 70F;
                        DayNightCycle.main.sunSetTime = 200F;
                    }

                    else
                    {
                        ErrorMessage.AddMessage("alwaysDay cheat is now False");
                        DayNightCycle.main.sunRiseTime = 0.125F;
                        DayNightCycle.main.sunSetTime = 0.875F;
                    }
                    break;
                case 18:
                    if (!isToggle[18])
                    {
                        playerPrevInfectionLevel = Player.main.infectedMixin.GetInfectedAmount();
                    }
                    else
                    {
                        Player.main.infectedMixin.SetInfectedAmount(playerPrevInfectionLevel);
                    }

                    isToggle[18] = !isToggle[18];
                    ErrorMessage.AddMessage(isToggle[18] ? "noInfect cheat is now True" : "noInfect cheat is now False");
                    break;
#if DEBUG
                case 19:
                    isToggle[19] = !isToggle[19];
                    if (isToggle[19])
                    {
                        Player.main.liveMixin.data.maxHealth = 999f;
                        Player.main.liveMixin.health = 999f;
                        survival.food = 999f;
                        survival.water = 999f;
                    }
                    else
                    {
                        Player.main.liveMixin.data.maxHealth = 100f;
                        Player.main.liveMixin.health = 100f;
                        survival.food = 100f;
                        survival.water = 100f;
                    }
                    break;
#endif
            }

        }













    }
}
