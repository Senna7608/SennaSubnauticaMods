using Common.GUIHelper;
using RuntimeHelper.Logger;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
    {
        private GuiItemEvent ScrollView_materialList_event;
        private int current_materialList_index = 0;
        private List<GuiItem> guiItems_materialList = new List<GuiItem>();
        private Vector2 scrollpos_materialList = Vector2.zero;
        private static Rect MaterialWindow_Rect = new Rect(550, 30, 300, 500);
        private Rect materialWindow_drawRect;
        private List<Material> MATERIALS = new List<Material>();
        private List<string> materialList = new List<string>();
        private bool isMaterialEmpty;
             

        private void RefreshMaterialList()
        {
            current_materialList_index = 0;

            materialList.Clear();

            foreach (Material item in MATERIALS)
            {
                materialList.Add(item.name);
            }

            if (materialList.Count == 0)
            {
                materialList.Add("<Empty>");

                isMaterialEmpty = true;
            }
            else
            {
                isMaterialEmpty = false;
            }

            guiItems_materialList.SetScrollViewItems(materialList, MaterialWindow_Rect.width - 20);
            guiItems_materialList.SetStateInverseTAB(current_materialList_index);
        }

        private void MaterialWindow_Awake()
        {
            RefreshMaterialList();

            materialWindow_drawRect = SNWindow.InitWindowRect(MaterialWindow_Rect, true);
        }


        private void MaterialWindow_OnGUI()
        {
            SNWindow.CreateWindow(MaterialWindow_Rect, "Resources: materials");

            ScrollView_materialList_event = SNScrollView.CreateScrollView(new Rect(materialWindow_drawRect.x + 5, materialWindow_drawRect.y, materialWindow_drawRect.width - 10, 80), ref scrollpos_materialList, ref guiItems_materialList, "Selected:", materialList[current_materialList_index], 5);

            if (GUI.Button(new Rect(materialWindow_drawRect.x + 5, materialWindow_drawRect.y + 446, 40, 22), FMODWindow[0], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isMaterialEmpty)
                {
                    FindMaterialAssets();
                    ListVFXPoolPrefabs();                   
                }
                else
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_GET_FMODS_IS_READY], LogType.Warning);
                }
            }

            /*
            if (GUI.Button(new Rect(materialWindow_drawRect.x + 50, materialWindow_drawRect.y + 152, 60, 22), FMODWindow[1], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isMaterialEmpty)
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_PLAY_FMOD_ASSET], LogType.Warning);
                }
                else
                {
                    PlayFMODAsset(MATERIALS[current_materialList_index]);
                }
            }

            if (GUI.Button(new Rect(materialWindow_drawRect.x + 115, materialWindow_drawRect.y + 152, 60, 22), FMODWindow[2], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (isMaterialEmpty)
                {
                    OutputWindow_Log(WARNING_TEXT[WARNINGS.CANNOT_STOP_FMOD_ASSET], LogType.Warning);
                }
                else
                {
                    customEmitter.Stop();
                }
            }

            if (GUI.Button(new Rect(materialWindow_drawRect.x + 180, materialWindow_drawRect.y + 152, 60, 22), FMODWindow[3], SNStyles.GetGuiItemStyle(GuiItemType.NORMALBUTTON, GuiColor.Gray)))
            {
                if (!isMaterialEmpty)
                {
                    FMODAsset currentAsset = MATERIALS[current_materialList_index];

                    OutputWindow_Log(MESSAGE_TEXT[MESSAGES.FMOD_ASSET_PRINTED], LogType.Log, currentAsset.name, currentAsset.path);
                }

            }
            */
        }

        private void MaterialWindow_Update()
        {
            if (ScrollView_materialList_event.ItemID != -1 && ScrollView_materialList_event.MouseButton == 0)
            {
                current_materialList_index = ScrollView_materialList_event.ItemID;
            }
        }

        private void FindMaterialAssets()
        {
            MATERIALS.Clear();

            Material[] materials = Resources.LoadAll<Material>("Materials/");

            foreach (Material material in materials)
            {
                MATERIALS.Add(material);

                RHLogger.Debug($"Material: {material.name}, mainTexture: {material.mainTexture?.name}");
            }

            MATERIALS.Sort(delegate (Material a, Material b)
            {
                return a.name.CompareTo(b.name);
            });

            RefreshMaterialList();
        }       
        
        private void ListVFXPoolPrefabs()
        {
            if (VFXPool.main == null)
                return;

            foreach (VFXPool.FX effect in VFXPool.main.effects)
            {
                RHLogger.Debug($"VFX effect: {effect.prefab.name}");
            }
        }        
    }
}
