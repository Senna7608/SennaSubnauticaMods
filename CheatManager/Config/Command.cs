using UnityEngine;

namespace CheatManager.Config
{
    public class ConsoleCommand : MonoBehaviour
    {        
        private void Awake()
        {
            DevConsole.RegisterConsoleCommand(this, "cmconfig", false, false);            
        }
        
        private void OnConsoleCommand_cmconfig(NotificationCenter.Notification n)
        {
            Bindings.InitWindow();
        }        
    }
}
