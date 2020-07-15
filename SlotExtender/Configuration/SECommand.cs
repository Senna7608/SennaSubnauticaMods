using Common;

namespace SlotExtender.Configuration
{
    public class SECommand : ConsoleCommand
    {
        public override bool AvailableInStartScreen => true;
        public override bool AvailableInGame => false;

        public void Awake()
        {
            SNLogger.Log("SlotExtender", "Information: Enter 'seconfig' command in main menu for configuration window.");
        }

        public override void RegisterCommand()
        {
            DevConsole.RegisterConsoleCommand(this, "seconfig", false, false);            
        }

        public void OnConsoleCommand_seconfig(NotificationCenter.Notification n)
        {
            SEConfigUI configUI = new SEConfigUI();
        }        
    }
}
