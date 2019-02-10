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
            DontDestroyOnLoad(this);
            DevConsole.RegisterConsoleCommand(this, "alwaysday", false, false);            
        }

        private void OnConsoleCommand_alwaysday(NotificationCenter.Notification n)
        {
            AlwaysDayCheat = !AlwaysDayCheat;
            SetAlwaysDayCheat();
            ErrorMessage.AddMessage($"alwaysday cheat is now {AlwaysDayCheat}");
        }

        public bool GetAlwaysDayCheat()
        {
            return AlwaysDayCheat;
        }

        public void SetAlwaysDayCheat()
        {
            if (AlwaysDayCheat)
            {                
                DayNightCycle.main.sunRiseTime = 70F;
                DayNightCycle.main.sunSetTime = 200F;
            }
            else
            {                
                DayNightCycle.main.sunRiseTime = 0.125F;
                DayNightCycle.main.sunSetTime = 0.875F;
            }
        }
    }
}
