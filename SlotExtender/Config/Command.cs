using UnityEngine;

namespace SlotExtender.Config
{
    public class SxConfig : MonoBehaviour
    {
        public static SxConfig Instance { get; private set; }

        public void Awake()
        {
            Instance = this;           
            DevConsole.RegisterConsoleCommand(this, "sxconfig", false, false);
            Debug.Log("[SlotExtender] Information: Enter 'sxconfig' command for configuration window.");
        }
        
        private void OnConsoleCommand_sxconfig(NotificationCenter.Notification n)
        {
            Bindings.InitWindow();
        }

        public static SxConfig Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(SxConfig)) as SxConfig;

                if (Instance == null)
                {
                    GameObject sxconfig_command = new GameObject().AddComponent<SxConfig>().gameObject;
                    sxconfig_command.name = "SxConfig";
                    Instance = sxconfig_command.GetComponent<SxConfig>();
                }
            }

            return Instance;
        }
    }
}
