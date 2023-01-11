﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using SMLHelper.V2.Utility;
using Common.GUIHelper;
using RuntimeHelper.FileHelper;
using System;
using RuntimeHelper.Renderers;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
    {        
        private bool showRendererWindow = false;        

        private List<GuiItem> guiItems_Renderer = new List<GuiItem>();

        private List<MaterialInfo> _materialInfos;

        private List<string> _materialInfo_ScrollItems = new List<string>();
        private List<string> usingSPIDkeywords = new List<string>();       

        private Vector2 scrollPos_Renderer = Vector2.zero;

        private string currentRenderer = string.Empty;
        
        private IDictionary<int, Material[]> undoDictionary = new Dictionary<int, Material[]>();

        private bool undo = false;

        private GuiItemEvent ScrollView_renderer_event;
        private int current_renderer_index = 0;

        private int materialIndex = -1;
        private int shaderIndex = -1;

        private List<string> textureKeywords = new List<string>()
        {
            "_Illum",
            "_MainTex",
            "_Normal",            
            "_BumpMap",            
            "_SpecTex"
        };


        private void RendererWindow_Awake()
        {           
            _materialInfos = selectedObject.GetRendererInfo();

            undo = IsUndoAvailable();

            RendererWindow_Refresh();
        }        
        
        private void RendererWindow_OnGUI()
        {
            if (!showRendererWindow)
                return;

            Rect windowrect = SNWindow.CreateWindow(new Rect(700, 732, 550, 348), "Renderer Window");

            ScrollView_renderer_event = SNScrollView.CreateScrollView(new Rect(windowrect.x + 5, windowrect.y, windowrect.width - 10, windowrect.height - 60), ref scrollPos_Renderer, ref guiItems_Renderer, currentRenderer, MESSAGE_TEXT[MESSAGES.RENDERER_WINDOW_INFOTEXT], 10);
          
            if (undo)
            {
                if (GUI.Button(new Rect(windowrect.x + 5, windowrect.y + (windowrect.height - 50), 60, 22), RendererWindow[0]))
                {
                    ResetMaterials();
                }
            }
                        
            Renderer renderer = (Renderer)objects[selected_component];

            if (GUI.Button(new Rect(windowrect.x + 70, windowrect.y + (windowrect.height - 50), 60, 22), renderer.enabled ? "Off" : "On"))
            {
                renderer.enabled = !renderer.enabled;
            }
            
        }

        private void RendererWindow_Update()
        {
            if (!showRendererWindow)
            {                
                return;
            }

            if (ScrollView_renderer_event.ItemID != -1 && ScrollView_renderer_event.MouseButton == 0)
            {
                current_renderer_index = ScrollView_renderer_event.ItemID;

                GetSelectedMaterialIndex(current_renderer_index);

                if (IsTextureItem(current_renderer_index))
                {
                    SystemFileRequester();
                }
            }
        }

        private void SystemFileRequester()
        {
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            ofn.filter = "All Files\0*.*\0\0";
            ofn.file = new string(new char[256]);
            ofn.maxFile = ofn.file.Length;
            ofn.fileTitle = new string(new char[64]);
            ofn.maxFileTitle = ofn.fileTitle.Length;
            ofn.initialDir = UnityEngine.Application.dataPath;
            ofn.title = "Select Texture Image";
            ofn.defExt = "PNG";
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
            //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

            if (DllTest.GetOpenFileName(ofn))
            {
                StartCoroutine(WaitToLoad(ofn.file));
                OutputWindow_Log(MESSAGE_TEXT[MESSAGES.SELECTED_FILE], ofn.file);
            }
        }

        private IEnumerator WaitToLoad(string fileName)
        {            
            Texture2D texture = ImageUtils.LoadTextureFromFile(fileName);

            yield return texture;

            string[] fileNameArray = fileName.Split('\\');

            texture.name = fileNameArray.GetLast();
            
            CreateNewMaterialForObject(texture);
        }
        
        private bool IsUndoAvailable()
        {
            Renderer renderer = selectedObject.GetComponent<Renderer>();

            return undoDictionary.Keys.Contains(renderer.GetInstanceID());            
        }


        private bool GetSelectedMaterialIndex(int scrollIndex)
        {
            string[] splittedItem = _materialInfo_ScrollItems[scrollIndex].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            if (int.TryParse(splittedItem[0], out materialIndex))
                return true;

            return false;
        }


        private bool IsTextureItem(int scrollIndex)
        {
            foreach (string keyword in textureKeywords)
            {
                if (_materialInfo_ScrollItems[scrollIndex].Contains(keyword))
                {
                    string[] splittedItem = _materialInfo_ScrollItems[scrollIndex].Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                    int.TryParse(splittedItem[1].ToString(), out int sIndex);
                    shaderIndex = sIndex;
                    return true;
                }
            }
            
            shaderIndex = -1;
            return false;            
        }
        
        private void ResetMaterials()
        {           

            Renderer renderer = selectedObject.GetComponent<Renderer>();

            int rendererID = renderer.GetInstanceID();

            Material[] materials = (Material[])undoDictionary[rendererID].Clone();      

            renderer.materials = materials;            

            OutputWindow_Log(MESSAGE_TEXT[MESSAGES.MATERIALS_TO_ORIGINAL]);

            RendererWindow_Awake();
        }


        private void CreateNewMaterialForObject(Texture2D newTexture)
        {
            selectedObject.SetActive(false);

            Renderer renderer = selectedObject.GetComponent<Renderer>();            
            
            string shaderKeyword = _materialInfos[materialIndex].ActiveShaders[shaderIndex].Keyword;
            string shaderName = _materialInfos[materialIndex].ActiveShaders[shaderIndex].Name;            

            Shader newShader = Shader.Find(shaderName);

            Material newMaterial = new Material(newShader)
            {
                hideFlags = HideFlags.HideAndDontSave,
                shaderKeywords = _materialInfos[materialIndex].Material.shaderKeywords
            };
            
            newMaterial.SetTexture(Shader.PropertyToID(shaderKeyword), newTexture);
            newMaterial.name = "newMaterial";

            Material[] materials = renderer.materials;

            int rendererID = renderer.GetInstanceID();

            if (!undoDictionary.Keys.Contains(rendererID))
            {
                undoDictionary.Add(objects[selected_component].GetInstanceID(), (Material[])materials.Clone());
            }
            
            foreach (ShaderInfo shaderInfo in _materialInfos[materialIndex].ActiveShaders)
            {
                if (shaderInfo.Keyword == shaderKeyword)
                {
                    continue;
                }
                else
                {
                    Texture texture = materials[materialIndex].GetTexture(Shader.PropertyToID(shaderInfo.Keyword));
                    newMaterial.SetTexture(Shader.PropertyToID(shaderInfo.Keyword), texture);
                }
            }

            materials.SetValue(newMaterial, materialIndex);
            
            renderer.materials = materials;            

            selectedObject.SetActive(true);

            OutputWindow_Log(MESSAGE_TEXT[MESSAGES.NEW_MATERIAL_CREATED]);

            RendererWindow_Awake();
        }        

        public void RendererWindow_Refresh()
        {
            showRendererWindow = false;            

            _materialInfo_ScrollItems.Clear();

            foreach (MaterialInfo materialInfo in _materialInfos)
            {
                _materialInfo_ScrollItems.Add($"[{materialInfo.Index}] : {materialInfo.Material.name}");

                foreach(ShaderInfo shaderInfo in materialInfo.ActiveShaders)
                {
                    _materialInfo_ScrollItems.Add($"[{materialInfo.Index}][{shaderInfo.Index}]: S: {shaderInfo.Name}, P: {shaderInfo.Keyword}, V: {shaderInfo.KeywordValue}");
                }

                _materialInfo_ScrollItems.Add($"[{materialInfo.Index}] Color: {materialInfo.Material.color}");                

                foreach (string shaderKeyword in materialInfo.ShaderKeywords)
                {
                    _materialInfo_ScrollItems.Add($"[{materialInfo.Index}] Shader Keywords: {shaderKeyword}");
                }                
            }
            
            guiItems_Renderer.SetScrollViewItems(_materialInfo_ScrollItems, 530f);

            showRendererWindow = true;
        }        
    }
}
