using Common;

namespace RuntimeHelper.Command
{
    public class RuntimeHelperCommand : ConsoleCommand
    {
        public override bool AvailableInStartScreen => true;

        public override bool AvailableInGame => true;

        public override void RegisterCommand()
        {
            DevConsole.RegisterConsoleCommand(this, "runtimehelper", false, false);
        }

        public void OnConsoleCommand_runtimehelper(NotificationCenter.Notification n)
        {            
            RuntimeHelper rh = new RuntimeHelper();
        }           
    }
}
