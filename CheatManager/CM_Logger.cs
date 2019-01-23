#define DEBUG_GAMELOG
#define AUTOSCROLL
//#define MAXMESSAGE_INFINITY

using System.Collections.Generic;
using UnityEngine;
using Common.GUIHelper;
using CheatManager.Configuration;

namespace CheatManager
{
    public class CM_Logger : MonoBehaviour
    {
        public CM_Logger Instance { get; private set; }
        private Rect windowRect;       
        
        private Vector2 scrollPos = Vector2.zero;
        private float contentHeight = 0;
        private float drawingPos;
        private List<LOG> logMessage = new List<LOG>();
        private int messageCount = 0;

        private bool show = false;
        private string inputField = string.Empty;

#if MAXMESSAGE_INFINITY
        private static readonly int MAXLOG = int.MaxValue;
#else
        private readonly int MAXLOG = 100; 
#endif
        //private static List<string> history = new List<string>();

        //private static int historyIndex = 0;
        
        private struct LOG
        {
            public string message;
            public string stackTrace;
            public LogType type;
        }
        
        private readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
        {
            { LogType.Error, Color.magenta },
            { LogType.Assert, Color.blue },
            { LogType.Warning, Color.yellow },            
            { LogType.Log, Color.green },
            { LogType.Exception, Color.red },            

        };        
         
        public CM_Logger()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(CM_Logger)) as CM_Logger;

                if (Instance == null)
                {
                    GameObject cm_logger = new GameObject().AddComponent<CM_Logger>().gameObject;
                    cm_logger.name = "CM_Logger";
                    Instance = cm_logger.GetComponent<CM_Logger>();                    
                }
            }            
        }
        
        public void Awake()
        {
#if DEBUG
            show = true;
#endif          
            Instance = this;            
            DontDestroyOnLoad(this);            
            useGUILayout = false;
            Application.logMessageReceived += HandleLog;            
        }        

        public void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
            logMessage.Clear();
            Destroy(this);
        }

        void OnGUI()
        {             
            if (!show)
            {
                return;
            }

            windowRect = SNWindow.CreateWindow(new Rect(Screen.width - (Screen.width / 4.8f), Screen.height - (Screen.height / 4), Screen.width / 4.8f, Screen.height / 4), $"CheatManager Console (Press {Config.KEYBINDINGS["ToggleConsole"]} to toggle)", true, true);

            Rect scrollRect = new Rect(windowRect.x, windowRect.y + 5, windowRect.width - 5, windowRect.height - 37);

            scrollPos = GUI.BeginScrollView(scrollRect, scrollPos, new Rect(scrollRect.x, scrollRect.y, scrollRect.width - 40, drawingPos - scrollRect.y));
            
            for (int i = 0; i < logMessage.Count; i++)
            {
                if (i == 0)
                {
                    drawingPos = scrollRect.y;
                }                
               
                GUIStyle style = GUI.skin.GetStyle("Label");

                style.alignment = TextAnchor.MiddleLeft;
                style.wordWrap = true;                

                contentHeight = style.CalcHeight(new GUIContent(logMessage[i].message), scrollRect.width - 40);                
                
                GUI.contentColor = logTypeColors[logMessage[i].type];                
                
                GUI.Label(new Rect(scrollRect.x + 5, drawingPos, 15, 21), "> ");
                
                GUI.Label(new Rect(scrollRect.x + 20, drawingPos, scrollRect.width - 40, contentHeight), logMessage[i].message);
                
                drawingPos += contentHeight + 1;

                if (logMessage[i].stackTrace != "")
                {                    
                    contentHeight = style.CalcHeight(new GUIContent(logMessage[i].stackTrace), scrollRect.width - 40);
                    
                    GUI.Label(new Rect(scrollRect.x + 20, drawingPos, scrollRect.width - 40, contentHeight), logMessage[i].stackTrace);
                    drawingPos += contentHeight + 1;
                }
            }

#if AUTOSCROLL
            if (messageCount != logMessage.Count)
            {
                scrollPos.y += Mathf.Infinity;
                messageCount = logMessage.Count;
            }
#endif
            GUI.EndScrollView();

            GUI.contentColor = Color.white;

            /*
            if (Event.current.Equals(Event.KeyboardEvent("return")) && inputField != "")
            {
                history.Add(inputField);
                historyIndex = history.Count;
                Log(inputField);                
                DevConsole.SendConsoleCommand(inputField);
                inputField = "";
            }
            
            if (Event.current.Equals(Event.KeyboardEvent("up")))
            {
                if (history.Count > 0 && historyIndex >= 1)
                {
                    historyIndex--;
                    inputField = history[historyIndex];
                }
            }

            if (Event.current.Equals(Event.KeyboardEvent("down")))
            {
                if (history.Count > 0 && historyIndex < history.Count - 1)
                {
                    historyIndex++;
                    inputField = history[historyIndex];
                }
            }
            
            inputField = GUI.TextField(new Rect(scrollRect.x + 5, scrollRect.y + scrollRect.height + 5, 300, 22), inputField);
            */

            if (GUI.Button(new Rect(windowRect.x + 5, windowRect.y + windowRect.height - 27, windowRect.width - 10, 22),"Clear Window"))
            {
                logMessage.Clear();
                drawingPos = scrollRect.y;
            }
        }            
            
        public void Update()
        {
            if (Input.GetKeyDown(Config.KEYBINDINGS["ToggleConsole"]))
            {
                show = !show;                
            }
        }        

        private void Write(string message)
        {
            logMessage.Add(new LOG()
            {
                message = message,
                stackTrace = "",
                type = LogType.Log,
            });
            
            if (logMessage.Count == MAXLOG)
            {
                logMessage.RemoveAt(0);
                messageCount--;
            }
        }

        private void Write(string message, LogType type)
        {
            logMessage.Add(new LOG()
            {
                message = message,
                stackTrace = "",
                type = type,
            });

            if (logMessage.Count == MAXLOG)
            {
                logMessage.RemoveAt(0);
                messageCount--;
            }
        }        

        private void Write(string message, LogType type, params object[] arg)
        {
            logMessage.Add(new LOG()
            {
                message = string.Format(message, arg),
                stackTrace = "",
                type = type,
            });

            if (logMessage.Count == MAXLOG)
            {
                logMessage.RemoveAt(0);
                messageCount--;
            }
        }

        private void Write(string message, string stacktrace, LogType type)
        {            
            if (stacktrace != "")
            {
                string temp;
                temp = "<<STACKTRACE>>\n" + stacktrace;
                stacktrace = temp;
            }

            logMessage.Add(new LOG()
            {
                message = message,
                stackTrace = stacktrace,
                type = type,
            });

            if (logMessage.Count == MAXLOG)
            {
                logMessage.RemoveAt(0);
                messageCount--;
            }

        }

        public void Log(string message) => Instance.Write(message);

        public void Log(string message, LogType type) => Instance.Write(message, type);

        public void Log(string message, LogType type, params object[] arg) => Instance.Write(message, type, arg);

        public void HandleLog(string message, string stacktrace, LogType type) => Instance.Write(message, stacktrace, type);
    }
}