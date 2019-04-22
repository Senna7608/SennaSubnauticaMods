using UnityEngine;
using Common;

namespace CheatManager.Configuration
{
    public class CmConfig : MonoBehaviour
    {
        public static CmConfig Instance { get; private set; }

        public void Awake()
        {
            Instance = this;
            DevConsole.RegisterConsoleCommand(this, "cmconfig", false, false);
            SNLogger.Log($"[{Config.PROGRAM_NAME}] Information: Enter 'cmconfig' command for configuration window.");
        }

        private void OnConsoleCommand_cmconfig(NotificationCenter.Notification n)
        {
            ConfigUI.InitWindow();
        }

        public static CmConfig Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(CmConfig)) as CmConfig;

                if (Instance == null)
                {
                    GameObject cmconfig_command = new GameObject().AddComponent<CmConfig>().gameObject;
                    cmconfig_command.name = "CmConfig";
                    Instance = cmconfig_command.GetComponent<CmConfig>();
                }
            }

            return Instance;
        }
    }
}
