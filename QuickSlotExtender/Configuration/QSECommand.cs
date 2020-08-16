using UnityEngine;
using Common;

namespace QuickSlotExtender.Configuration
{
    public class QSECommand : MonoBehaviour
    {
        private const string Message = "Information: Enter 'qseconfig' command for configuration window.";

        public QSECommand Instance { get; private set; }
        
        public void Awake()
        {
            Instance = this;           
            DevConsole.RegisterConsoleCommand(this, "qseconfig", false, false);
            SNLogger.Log("QuickSlotExtender", Message);
        }
        
        private void OnConsoleCommand_qseconfig(NotificationCenter.Notification n)
        {
            QSEConfigUI configUI = new QSEConfigUI();
        }

        public QSECommand()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(QSECommand)) as QSECommand;

                if (Instance == null)
                {
                    GameObject qsez_command = new GameObject("QSECommand");
                    Instance = qsez_command.AddComponent<QSECommand>();
                }
            }
            else
            {
                Instance.Awake();
            }
        }
    }   
}
