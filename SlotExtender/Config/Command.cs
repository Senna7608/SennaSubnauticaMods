using UnityEngine;

namespace SlotExtender.Config
{
    public class SEConfig : MonoBehaviour
    {
        public static SEConfig Instance { get; private set; }

        public void Awake()
        {
            Instance = this;           
            DevConsole.RegisterConsoleCommand(this, "seconfig", false, false);
            Debug.Log("[SlotExtender] Information: Enter 'seconfig' command for configuration window.");
        }
        
        private void OnConsoleCommand_seconfig(NotificationCenter.Notification n)
        {
            ConfigUI.InitWindow();
        }

        public static SEConfig Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(SEConfig)) as SEConfig;

                if (Instance == null)
                {
                    GameObject sxconfig_command = new GameObject().AddComponent<SEConfig>().gameObject;
                    sxconfig_command.name = "SEConfig";
                    Instance = sxconfig_command.GetComponent<SEConfig>();
                }
            }

            return Instance;
        }
    }
}
