using Common;
using UnityEngine;

namespace RuntimeHelper.Command
{
    public class RuntimeHelperCommand : MonoBehaviour
    {
        public static RuntimeHelperCommand Instance;

        public void Awake()
        {
            DontDestroyOnLoad(this);
            DevConsole.RegisterConsoleCommand(Instance, "runtimehelper", false, false);
        }        

        public void OnConsoleCommand_runtimehelper(NotificationCenter.Notification n)
        {            
            RuntimeHelper rh = new RuntimeHelper();
        }

        public RuntimeHelperCommand()
        {
            if (Instance.IsNull())
            {
                Instance = UnityEngine.Object.FindObjectOfType(typeof(RuntimeHelperCommand)) as RuntimeHelperCommand;

                if (Instance.IsNull())
                {
                    GameObject rh_command_go = new GameObject("rh_command_go");
                    Instance = rh_command_go.GetOrAddComponent<RuntimeHelperCommand>();
                }
            }            
        }
    }
}
