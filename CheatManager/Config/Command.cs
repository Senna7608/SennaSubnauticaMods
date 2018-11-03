using UnityEngine;

namespace CheatManager.Config
{
    public class CMhotkeysConsoleCommand : MonoBehaviour
    {        
        private void Awake()
        {
            DevConsole.RegisterConsoleCommand(this, "cmhotkeys", false, false);            
        }
        
        private void OnConsoleCommand_cmhotkeys(NotificationCenter.Notification n)
        {
            HotKeys.InitWindow();
        }        
    }
}
