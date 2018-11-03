using UnityEngine;

namespace SlotExtender.Config
{
    public class SlotExtenderConsoleCommand : MonoBehaviour
    {        
        private void Awake()
        {
            DevConsole.RegisterConsoleCommand(this, "slotextender", false, false);            
        }
        
        private void OnConsoleCommand_slotextender(NotificationCenter.Notification n)
        {
            HotKeys.InitWindow();
        }        
    }
}
