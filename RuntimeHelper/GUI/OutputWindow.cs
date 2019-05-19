using System.Collections.Generic;
using UnityEngine;
using Common.GUIHelper;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {        
        private static GUIStyle logStyle;        

        private static Rect OutputWindow_Rect = new Rect(0, 992, 698, 86);        
        private Rect OutputWindow_drawRect, OutputWindow_scrollRect;

        private float scrollWidth;
        private Vector2 scrollPos = Vector2.zero;
        private float contentHeight = 0;
        private float drawingPos;

        private List<LOG> logMessage = new List<LOG>();
        private int messageCount = 0;

        private readonly int MAXLOG = 100; 
     
        private struct LOG
        {
            public string message;            
            public LogType type;
        }

        private readonly Dictionary<LogType, GuiColor> logTypeColors = new Dictionary<LogType, GuiColor>()
        {
            { LogType.Error, GuiColor.Magenta },
            { LogType.Assert, GuiColor.Blue },
            { LogType.Warning, GuiColor.Yellow },
            { LogType.Log, GuiColor.Green },
            { LogType.Exception, GuiColor.Red },
        };
                
        
        private void OutputWindow_Awake()
        {
            OutputWindow_drawRect = SNWindow.InitWindowRect(OutputWindow_Rect);
            OutputWindow_scrollRect = new Rect(OutputWindow_drawRect.x, OutputWindow_drawRect.y + 5, OutputWindow_drawRect.width - 5, OutputWindow_drawRect.height - 5);
            scrollWidth = OutputWindow_scrollRect.width - 42;                      
        }        

        private void OutputWindow_OnGUI()
        {                                    
            logStyle = SNStyles.GetGuiItemStyle(GuiItemType.LABEL, GuiColor.Green, TextAnchor.MiddleLeft, wordWrap: true);            

            SNWindow.CreateWindow(OutputWindow_Rect, "Output Window", false, false);            

            scrollPos = GUI.BeginScrollView(OutputWindow_scrollRect, scrollPos, new Rect(OutputWindow_scrollRect.x, OutputWindow_scrollRect.y, scrollWidth, drawingPos - OutputWindow_scrollRect.y));

            for (int i = 0; i < logMessage.Count; i++)            
            {               
                if (i == 0)
                {
                    drawingPos = OutputWindow_scrollRect.y;
                }
                
                contentHeight = logStyle.CalcHeight(new GUIContent(logMessage[i].message), scrollWidth);                

                logStyle.normal.textColor = SNStyles.GetGuiColor(logTypeColors[logMessage[i].type]);                

                GUI.Label(new Rect(OutputWindow_scrollRect.x + 5, drawingPos, 15, 21), "> ", logStyle);

                GUI.Label(new Rect(OutputWindow_scrollRect.x + 20, drawingPos, scrollWidth, contentHeight), logMessage[i].message, logStyle);                

                drawingPos += contentHeight + 1;                
            }

            if (messageCount != logMessage.Count)
            {
                scrollPos.y += Mathf.Infinity;
                messageCount = logMessage.Count;
            }

            GUI.EndScrollView();            
        }                

        private void Write(string message)
        {
            logMessage.Add(new LOG()            
            {
                message = message,                
                type = LogType.Log,
            });

            if (logMessage.Count == MAXLOG)
            {
                RemoveFirstLogEntry();
            }
        }

        private void Write(string message, LogType type)
        {
            logMessage.Add(new LOG()            
            {
                message = message,                
                type = type,
            });

            if (logMessage.Count == MAXLOG)
            {
                RemoveFirstLogEntry();
            }
        }        

        private void Write(string message, LogType type, params object[] arg)
        {
            logMessage.Add(new LOG()            
            {
                message = string.Format(message, arg),                
                type = type,
            });

            if (logMessage.Count == MAXLOG)
            {
                RemoveFirstLogEntry();
            }
        }        

        private void RemoveFirstLogEntry()
        {
            logMessage.RemoveAt(0);
            messageCount--;
        }

        public void OutputWindow_Log(string message) => Write(message);

        public void OutputWindow_Log(string message, LogType type) => Write(message, type);

        public void OutputWindow_Log(string message, LogType type, params object[] arg) => Write(message, type, arg);       
    }
}