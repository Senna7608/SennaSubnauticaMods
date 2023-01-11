using Common;
using UnityEngine;

namespace CheatManager.NewCommands
{
    public class NoInfectConsoleCommand : MonoBehaviour
    {
        public static NoInfectConsoleCommand main;
        private bool NoInfectCheat;
        private float playerPrevInfectionLevel = 0f;               

        public void Awake()
        {
            main = this;            
            DevConsole.RegisterConsoleCommand(this, "noinfect");
            SNLogger.Log("Console command 'noinfect' registered.");
        }

        private void OnConsoleCommand_noinfect(NotificationCenter.Notification n)
        {
            NoInfectCheat = !NoInfectCheat;
            SetNoInfectCheat();
            ErrorMessage.AddMessage($"noinfect cheat is now {NoInfectCheat}");
        }

        public bool GetNoInfectCheat()
        {
            return NoInfectCheat;
        }

        public void SetNoInfectCheat()
        {
            if (NoInfectCheat)
            {
                playerPrevInfectionLevel = Player.main.infectedMixin.GetInfectedAmount();
            }
            else
            {
                Player.main.infectedMixin.SetInfectedAmount(playerPrevInfectionLevel);
            }
        }

        private void Update()
        {
            if (NoInfectCheat)
            {
                Player.main.infectedMixin.SetInfectedAmount(0f);
            }
        }        
    }
}
