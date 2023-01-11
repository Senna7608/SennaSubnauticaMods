using Common;
using UnityEngine;

namespace CheatManager.NewCommands
{
    public class AlwaysDayConsoleCommand : MonoBehaviour
    {
        public static AlwaysDayConsoleCommand main;

        private bool AlwaysDayCheat;        

        public void Awake()
        {
            main = this;
            DevConsole.RegisterConsoleCommand(this, "alwaysday");
            SNLogger.Log("Console command 'alwaysday' registered.");
        }

        private void OnConsoleCommand_alwaysday(NotificationCenter.Notification n)
        {            
            SetAlwaysDayCheat(!AlwaysDayCheat);            
        }

        public bool GetAlwaysDayCheat() => AlwaysDayCheat;

        public void SetAlwaysDayCheat(bool value)
        {
            if (value)
            {                
                DayNightCycle.main.sunRiseTime = 70F;
                DayNightCycle.main.sunSetTime = 200F;
            }
            else
            {                
                DayNightCycle.main.sunRiseTime = 0.125F;
                DayNightCycle.main.sunSetTime = 0.875F;
            }

            AlwaysDayCheat = value;

            ErrorMessage.AddMessage($"alwaysday cheat is now {AlwaysDayCheat}");
        }        
    }
}
