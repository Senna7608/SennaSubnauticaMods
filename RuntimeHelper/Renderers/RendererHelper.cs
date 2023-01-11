using RuntimeHelper.Logger;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Renderers
{
    public static class RendererHelper
    {
        public static List<string> ShaderPropertyKeywords;
        private static Color transparent = new Color(0f, 0f, 0f, 0f);

        public static List<MaterialInfo> GetRendererInfo(this GameObject gameObject)
        {
            if (ShaderPropertyKeywords == null)
            {
                ShaderPropertyKeywords = ShaderHelper.CreateShaderPropertyList();
            }

            List<MaterialInfo> materialInfos = new List<MaterialInfo>();

            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (renderer == null)
            {
                renderer = gameObject.GetComponentInChildren<Renderer>(true);
            }
            else  if (renderer == null)
            {
                RHLogger.Error($"Object: {gameObject.name} has no renderers!");
            }

            List<string> keywords = new List<string>();

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material material = renderer.materials[i];

                materialInfos.Add(new MaterialInfo(material, i));

                int activeIndex = 0;

                keywords.Clear();

                material.GetTexturePropertyNames(keywords);

                for (int j = 0; j < keywords.Count; j++)
                {
                    string keyword = keywords[j];

                    if (material.HasProperty(keyword))
                    {
                        string value;

                        try
                        {
                            value = material.GetTexture(Shader.PropertyToID(keyword)).name;
                            materialInfos[i].ActiveShaders.Add(new ShaderInfo(material.shader.name, activeIndex, material.shader, keyword, value));
                            activeIndex++;
                        }
                        catch
                        {                            
                        }


                        for (int k = 0; k < ShaderPropertyKeywords.Count; k++)
                        {
                            string keyword2 = ShaderPropertyKeywords[j];

                            try
                            {
                                Color color = material. GetColor(keyword2);

                                if (color != transparent)
                                {
                                    materialInfos[i].ActiveShaders.Add(new ShaderInfo(material.shader.name, activeIndex, material.shader, keyword2, color.ToString()));
                                    activeIndex++;
                                }

                            }
                            catch
                            {
                            }
                        }
                    }
                }

                foreach (string item in material.shaderKeywords)
                {
                    materialInfos[i].ShaderKeywords.Add(item);
                }
            }            

            return materialInfos;
        }

        public static List<MaterialInfo> GetRendererInfo(Renderer renderer)
        {
            if (renderer == null)
            {
                throw new ArgumentException("[RuntimeHelper] *** ERROR! Renderer is NULL!");
            }

            if (ShaderPropertyKeywords == null)
            {
                ShaderPropertyKeywords = ShaderHelper.CreateShaderPropertyList();
            }

            List<MaterialInfo> materialInfos = new List<MaterialInfo>();

            List<string> keywords = new List<string>();

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material material = renderer.materials[i];

                materialInfos.Add(new MaterialInfo(material, i));

                int activeIndex = 0;

                keywords.Clear();

                material.GetTexturePropertyNames(keywords);

                for (int j = 0; j < keywords.Count; j++)
                {
                    string keyword = keywords[j];

                    if (material.HasProperty(keyword))
                    {
                        string textureName;

                        try
                        {
                            textureName = material.GetTexture(Shader.PropertyToID(keyword)).name;
                        }
                        catch
                        {
                            continue;
                        }

                        materialInfos[i].ActiveShaders.Add(new ShaderInfo(material.shader.name, activeIndex, material.shader, keyword, textureName));

                        activeIndex++;
                    }
                }

                foreach (string item in material.shaderKeywords)
                {
                    materialInfos[i].ShaderKeywords.Add(item);
                }
            }

            return materialInfos;
        }     









    }







}
