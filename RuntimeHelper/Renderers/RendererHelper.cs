using RuntimeHelper.Logger;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Renderers
{
    public static class RendererHelper
    {
        public static List<string> ShaderPropertyKeywords;

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
                $"Object: {gameObject.name} has no renderers!".RH_Error();
            }

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material material = renderer.materials[i];

                materialInfos.Add(new MaterialInfo(material, i));

                int activeIndex = 0;

                for (int j = 0; j < ShaderPropertyKeywords.Count; j++)
                {
                    string keyword = ShaderPropertyKeywords[j];

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

                        materialInfos[i].ActiveShaders.Add(new ShaderInfo(material.shader.name, activeIndex, keyword, textureName));

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

            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material material = renderer.materials[i];

                materialInfos.Add(new MaterialInfo(material, i));

                int activeIndex = 0;

                for (int j = 0; j < ShaderPropertyKeywords.Count; j++)
                {
                    string keyword = ShaderPropertyKeywords[j];

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

                        materialInfos[i].ActiveShaders.Add(new ShaderInfo(material.shader.name, activeIndex, keyword, textureName));

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
