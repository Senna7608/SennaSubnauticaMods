using UnityEngine;
using Common;

namespace SlotExtender.Configuration
{
    public class SECommand : MonoBehaviour
    {
        private const string Message = "Information: Enter 'seconfig' command for configuration window.";

        public SECommand Instance { get; private set; }
        
        public void Awake()
        {
            Instance = this;           
            DevConsole.RegisterConsoleCommand(this, "seconfig", false, false);
            SNLogger.Log($"[{SEConfig.PROGRAM_NAME}] {Message}");
        }
        
        private void OnConsoleCommand_seconfig(NotificationCenter.Notification n)
        {
            SEConfigUI configUI = new SEConfigUI();
        }

        public SECommand ()
        {
            if (Instance.IsNull())
            {
                Instance = FindObjectOfType(typeof(SECommand)) as SECommand;

                if (Instance.IsNull())
                {
                    GameObject se_command = new GameObject("SECommand");
                    Instance = se_command.GetOrAddComponent<SECommand>();                    
                }
            }            
        }
    }
}
