using Common;
using UnityEngine;

namespace RuntimeHelper.Command
{
    public class RuntimeHelperCommand : MonoBehaviour
    {
        public static RuntimeHelperCommand Instance;

        public void Awake()
        {            
            DevConsole.RegisterConsoleCommand(Instance, "runtimehelper", false, false);
        }        

        public void OnConsoleCommand_runtimehelper(NotificationCenter.Notification n)
        {            
            RuntimeHelper rh = new RuntimeHelper();
        }

        public RuntimeHelperCommand()
        {
            if (!Instance)
            {
                Instance = FindObjectOfType(typeof(RuntimeHelperCommand)) as RuntimeHelperCommand;

                if (!Instance)
                {
                    GameObject rh_command_go = new GameObject("rh_command_go");
                    Instance = rh_command_go.AddComponent<RuntimeHelperCommand>();
                }
            }            
        }
    }
}
