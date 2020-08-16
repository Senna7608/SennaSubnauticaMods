using UnityEngine;
using Common;

namespace CheatManager.Configuration
{
    public class CmConfigCommand : MonoBehaviour
    {
        public static CmConfigCommand Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
            DevConsole.RegisterConsoleCommand(this, "cmconfig", false, false);
            SNLogger.Log("CheatManager", "Information: Enter 'cmconfig' command for configuration window.");
        }

        private void OnConsoleCommand_cmconfig(NotificationCenter.Notification n)
        {
            ConfigUI.InitWindow();
        }

        public CmConfigCommand()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(CmConfigCommand)) as CmConfigCommand;

                if (Instance == null)
                {
                    GameObject cmconfig_command = new GameObject().AddComponent<CmConfigCommand>().gameObject;
                    cmconfig_command.name = "CmConfig";
                    Instance = cmconfig_command.GetComponent<CmConfigCommand>();
                }
            }
            else
            {
                Instance.Awake();
            }
        }
    }
}
