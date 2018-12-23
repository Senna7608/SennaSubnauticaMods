using System.Collections.Generic;
using Common;

namespace CheatManager
{
    internal static class ButtonControl
    {
        internal static void NormalButtonControl(int normalButtonID, ref List<GUIHelper.ButtonInfo> Buttons, ref List<GUIHelper.ButtonInfo> toggleButtons)
        {
            switch (normalButtonID)
            {
                case 0 when toggleButtons[17].Pressed == true:
                case 1 when toggleButtons[17].Pressed == true:
                    break;
                case 0:
                case 1:
                case 2:
                case 4:
                case 5:
                case 6:
                    CheatManager.ExecuteCommand("Send command to console: " + Buttons[normalButtonID].Name, Buttons[normalButtonID].Name);
                    break;

                case 3:
                    ErrorMessage.AddMessage("Inventory Cleared");
                    Inventory.main.container.Clear(false);                    
                    break;

                case 7:
                    CheatManager.ExecuteCommand("warp" + " to: " + CheatManager.prevCwPos, "warp " + CheatManager.prevCwPos);
                    Utils.PlayFMODAsset(CheatManager.warpSound, Player.main.transform, 20f);
                    CheatManager.prevCwPos = null;
                    Buttons[7].Enabled = false;
                    break;
            }

        }


        internal static void ToggleButtonControl (int toggleButtonID, ref List<GUIHelper.ButtonInfo> toggleButtons)
        {
            switch (toggleButtonID)
            {
                case 9 when toggleButtons[1].Pressed == true:
                    break;
                case 11 when toggleButtons[1].Pressed == true:
                case 11 when toggleButtons[0].Pressed == true:
                    break;
                case 12 when toggleButtons[1].Pressed == true:
                    break;
                case 13 when toggleButtons[1].Pressed == true:
                    break;
                case 14 when toggleButtons[1].Pressed == true:
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
                    CheatManager.ExecuteCommand("", toggleButtons[toggleButtonID].Name);
                    break;
                case 15:
                    toggleButtons[15].Pressed = !toggleButtons[15].Pressed;
                    CheatManager.ExecuteCommand(toggleButtons[15].Pressed ? "shotgun cheat is now True" : "shotgun cheat is now False", toggleButtons[toggleButtonID].Name);
                    break;
                case 8:
                    toggleButtons[8].Pressed = !toggleButtons[8].Pressed;
                    CheatManager.ExecuteCommand(toggleButtons[8].Pressed ? "filterFast cheat is now True" : "filterFast cheat is now False", toggleButtons[toggleButtonID].Name);
                    break;
                case 17:
                    toggleButtons[17].Pressed = !toggleButtons[17].Pressed;
                    if (toggleButtons[17].Pressed)
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
                    if (!toggleButtons[18].Pressed)
                    {
                        CheatManager.playerPrevInfectionLevel = Player.main.infectedMixin.GetInfectedAmount();
                    }
                    else
                    {
                        Player.main.infectedMixin.SetInfectedAmount(CheatManager.playerPrevInfectionLevel);
                    }

                    toggleButtons[18].Pressed = !toggleButtons[18].Pressed;
                    ErrorMessage.AddMessage(toggleButtons[18].Pressed ? "noInfect cheat is now True" : "noInfect cheat is now False");
                    break;

                case 19:
                    toggleButtons[19].Pressed = !toggleButtons[19].Pressed;
                    CheatManager.OverPower(toggleButtons[19].Pressed);                    
                    ErrorMessage.AddMessage(toggleButtons[19].Pressed ? "overPower cheat is now True" : "overPower cheat is now False");
                    break;  
            }

            CheatManager.ReadGameValues();
        }
       
        internal static readonly float[] DayNightSpeed = { 0.1f, 0.25f, 0.5f, 0.75f, 1f, 2f };

        internal static void DayNightButtonControl(int daynightTabID, ref int currentdaynightTab, ref List<GUIHelper.ButtonInfo> daynightTab)
        {
            if (daynightTabID != currentdaynightTab)
            {
                daynightTab[currentdaynightTab].Pressed = false;
                daynightTab[daynightTabID].Pressed = true;
                currentdaynightTab = daynightTabID;
                DevConsole.SendConsoleCommand("daynightspeed " + DayNightSpeed[daynightTabID]);
            }
        }        
    }
}
