using Common.GUIHelper;
using RuntimeHelper.Logger;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
    {
        private GuiItemEvent ScrollView_fmodlist_event;
        private int current_fmodlist_index = 0;
        private List<GuiItem> guiItems_fmodlist = new List<GuiItem>();
        private Vector2 scrollpos_fmodlist = Vector2.zero;
        private static Rect FmodWindow_Rect = new Rect(300, 506, 248, 206);
        private Rect fmodWindow_drawRect;
        private List<FMODAsset> FMOD_ASSETS = new List<FMODAsset>();
        private List<string> fmodList = new List<string>();
        private bool isFmodEmpty;

        FMOD_CustomEmitter customEmitter;
        

        private void RefreshFMODList()
        {
            current_fmodlist_index = 0;

            fmodList.Clear();

            foreach (FMODAsset item in FMOD_ASSETS)
            {
                fmodList.Add(item.name);
            }

            if (fmodList.Count == 0)
            {
                fmodList.Add("<Empty>");

                isFmodEmpty = true;
            }
            else
            {
                isFmodEmpty = false;
            }

            guiItems_fmodlist.SetScrollViewItems(fmodList, FmodWindow_Rect.width - 20);
            guiItems_fmodlist.SetStateInverseTAB(current_fmodlist_index);
        }

        private void FMODWindow_Awake()
        {
            GameObject fmodPlayer = new GameObject("fmodPlayer");
            fmodPlayer.transform.SetParent(MainCamera.camera.transform, false);
            Utils.ZeroTransform(fmodPlayer.transform);

            customEmitter = fmodPlayer.AddComponent<FMOD_CustomEmitter>();

            RefreshFMODList();

            fmodWindow_drawRect = SNWindow.InitWindowRect(FmodWindow_Rect, true);
        }


        private void FMODWindow_OnGUI()
        {
            SNWindow.CreateWindow(FmodWindow_Rect, "FMOD assets window");

            ScrollView_fmodlist_event = SNScrollView.CreateScrollView(new Rect(fmodWindow_drawRect.x + 5, fmodWindow_drawRect.y, fmodWindow_drawRect.width - 10, 80), ref scrollpos_fmodlist, ref guiItems_fmodlist, "Selected:", fmodList[current_fmodlist_index], 5);

            if (GUI.Button(new Rect(fmodWindow_drawRect.x + 5, fmodWindow_drawRect.y + 152, 40, 22), FMODWindow[0], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isFmodEmpty)
                {
                    FindFMODAssets();
                    ListPrefabDatabase();
                }
                else
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_GET_FMODS_IS_READY], LogType.Warning);
                }
            }

            if (GUI.Button(new Rect(fmodWindow_drawRect.x + 50, fmodWindow_drawRect.y + 152, 60, 22), FMODWindow[1], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isFmodEmpty)
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_PLAY_FMOD_ASSET], LogType.Warning);
                }
                else
                {
                    PlayFMODAsset(FMOD_ASSETS[current_fmodlist_index]);
                }
            }

            if (GUI.Button(new Rect(fmodWindow_drawRect.x + 115, fmodWindow_drawRect.y + 152, 60, 22), FMODWindow[2], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isFmodEmpty)
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_STOP_FMOD_ASSET], LogType.Warning);
                }
                else
                {
                    customEmitter.Stop();
                }
            }

            if (GUI.Button(new Rect(fmodWindow_drawRect.x + 180, fmodWindow_drawRect.y + 152, 60, 22), FMODWindow[3], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (!isFmodEmpty)
                {
                    FMODAsset currentAsset = FMOD_ASSETS[current_fmodlist_index];

                    OutputWindow_Log(MESSAGE_TEXT[MESSAGES.FMOD_ASSET_PRINTED], LogType.Log, currentAsset.name, currentAsset.path);                    
                }

            }
        }

        private void FMODWindow_Update()
        {
            if (ScrollView_fmodlist_event.ItemID != -1 && ScrollView_fmodlist_event.MouseButton == 0)
            {
                current_fmodlist_index = ScrollView_fmodlist_event.ItemID;
            }
        }

        private void FindFMODAssets()
        {
            FMOD_ASSETS.Clear();

            FMODAsset[] fmods = Resources.FindObjectsOfTypeAll<FMODAsset>();

            foreach (FMODAsset fmod in fmods)
            {
                FMOD_ASSETS.Add(fmod);

                //RHZLogger.RHZ_Log($"Asset: {fmod.name}, path: {fmod.path}");
            }

            FMOD_ASSETS.Sort(delegate (FMODAsset a, FMODAsset b)
            {
                return a.name.CompareTo(b.name);
            });

            RefreshFMODList();
        }

        public void PlayFMODAsset(FMODAsset asset)
        {
            if (asset != null && asset.path != "")
            {

                FMODUWE.PlayOneShot(asset, Vector3.zero);
                /*
                if (customEmitter.playing)
                {
                    customEmitter.Stop();
                }

                customEmitter.SetAsset(asset);

                customEmitter.Play();
                */
            }
        }

        public void ListPrefabDatabase()
        {
            RHLogger.Log("Listing PrefabdataBase prefabFiles Dictionary:");

            foreach (KeyValuePair<string, string> kvp in PrefabDatabase.prefabFiles)
            {
                RHLogger.Log($"prefabFile: {kvp.Value}");
            }
        }
    }
}
