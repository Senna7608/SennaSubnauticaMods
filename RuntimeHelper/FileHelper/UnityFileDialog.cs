using Common.GUIHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RuntimeHelper.FileHelper
{
    //need a Unity based file dialog for MAC compatibility
    public class UnityFileDialog : MonoBehaviour
    {
        public UnityFileDialog Instance { get; private set; }
        private GuiItemEvent Scroll_retval;
        private int current_index = 0;
        private List<GuiItem> guiItems = new List<GuiItem>();
        private Vector2 scrollpos = Vector2.zero;
        private static Rect windowRect = new Rect(550, 30, 250, 400);
        private Rect drawRect;
        private int dirs, files;

        DirectoryInfo directoryInfo;
        DirectoryInfo[] directoryInfos;
        List<string> dirNames = new List<string>();

        public UnityFileDialog()
        {            
            if (Instance == null)
            {
                Instance = FindObjectOfType(typeof(UnityFileDialog)) as UnityFileDialog;

                if (Instance == null)
                {
                    GameObject fileDialog = new GameObject("RH_FileDialog");
                    Instance = fileDialog.AddComponent<UnityFileDialog>();                    
                }
            }           
        }

        public void Awake()
        {
            drawRect = SNWindow.InitWindowRect(windowRect, true);

            GetDirectoryStructure(-1);

        }
                

        private void OnGUI()
        {
            SNWindow.CreateWindow(windowRect, "File Dialog");           

            Scroll_retval = SNScrollView.CreateScrollView(new Rect(drawRect.x + 5, drawRect.y, drawRect.width - 10, 168), ref scrollpos, ref guiItems, "Current Directory:", directoryInfo.Name, 7);

            if (Scroll_retval.ItemID != -1)
            {
                current_index = Scroll_retval.ItemID;
            }
        }


        private void Update()
        {
            if (Event.current.clickCount == 2 && current_index != -1)
            {                
                GetDirectoryStructure(current_index);
            }

        }
        
        private void GetDirectoryStructure(int index)
        {           

            if (index == -1)
            {
                directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
            }
            else if (index == 0)
            {
                if (!directoryInfos[index].Name.Equals(directoryInfo.Root.Name))
                    directoryInfo = directoryInfo.Parent;
            }            
            else if (index > 0 )
            {
                if (directoryInfos[index - 1] != directoryInfo.Root)
                {
                    directoryInfo = directoryInfos[index -1];
                }
            }


            directoryInfos = directoryInfo.GetDirectories();

            dirNames.Clear();

            dirs = 1;
            
            if (directoryInfo.Parent.Name.Equals(directoryInfo.Root.Name))
            {
                dirNames.Add($"{directoryInfo.Parent.Name} <Root>");
            }
            else
            {
                dirNames.Add($"{directoryInfo.Parent.Name} <Parent>");
            }

            foreach (DirectoryInfo dirInfo in directoryInfos)
            {
                dirNames.Add($"{dirInfo.Name} <Dir>");
                dirs++;
            }

            FileInfo[] fileInfos = directoryInfo.GetFiles();

            files = 0;

            foreach (FileInfo fileInfo in fileInfos)
            {
                dirNames.Add(fileInfo.Name);
                files++;
            }


            guiItems.SetScrollViewItems(dirNames, windowRect.width - 20);

            current_index = -1;
        }



    }
}
