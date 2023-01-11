using Common;

namespace RuntimeHelper.Command
{
    public class RuntimeHelperCommand : ConsoleCommand
    {
        public override bool AvailableInStartScreen => true;

        public override bool AvailableInGame => true;

        public override void RegisterCommand()
        {
            DevConsole.RegisterConsoleCommand(this, "rhelper", false, false);
            SNLogger.Log("Console command 'rhelper' registered.");
        }

        public void OnConsoleCommand_rhelper(NotificationCenter.Notification n)
        {            
            RuntimeHelperManager rh = new RuntimeHelperManager();
        }           
    }
}
