using UnityEngine;

namespace SlotExtender.Config
{
    public class ConsoleCommand : MonoBehaviour
    {
        public static ConsoleCommand Instance { get; private set; }

        public void Awake()
        {
            Instance = this;           
            DevConsole.RegisterConsoleCommand(this, "sxconfig", false, false);            
        }
        
        private void OnConsoleCommand_sxconfig(NotificationCenter.Notification n)
        {
            Bindings.InitWindow();
        }

        public static ConsoleCommand Load()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(ConsoleCommand)) as ConsoleCommand;

                if (Instance == null)
                {
                    GameObject sx_command = new GameObject().AddComponent<ConsoleCommand>().gameObject;
                    sx_command.name = "ConsoleCommand";
                    Instance = sx_command.GetComponent<ConsoleCommand>();
                }
            }

            return Instance;
        }
    }
}
