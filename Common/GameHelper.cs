using System;
using System.IO;
using UnityEngine;

namespace Common
{
    public static class GameHelper
    {
        public static void EnableConsole()
        {
            DevConsole.disableConsole = false;
        }

        public static void ExecuteCommand(object message, object command)
        {
            if (message != null)
            {
                ErrorMessage.AddMessage(message.ToString());
            }

            if (command != null)
            {
                SNLogger.Log((string)command);
                DevConsole.SendConsoleCommand(command.ToString());
            }
        }

        public static bool IsPlayerInVehicle()
        {
            return Player.main.inSeamoth || Player.main.inExosuit ? true : false;
        }

        public static Vector3 ConvertStringPosToVector3(string target)
        {
            string[] numbers = target.Split(' ');

            return numbers.Length != 3 ? Vector3.zero : new Vector3(float.Parse(numbers[0]), float.Parse(numbers[1]), float.Parse(numbers[2]));
        }
               
        public static string Teleport(string targetName, string vector3string)
        {
            Vector3 currentWorldPos = Player.main.transform.position;

            string prevCwPos = string.Format("{0:D} {1:D} {2:D}", (int)currentWorldPos.x, (int)currentWorldPos.y, (int)currentWorldPos.z);

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(ConvertStringPosToVector3(vector3string), Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to: {targetName}\n({vector3string})");
            }
            else
            {
                Player.main.SetPosition(ConvertStringPosToVector3(vector3string));
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to: {targetName}\n({vector3string})");
            }

            return prevCwPos;
        }

        public static Vector3 Teleport(string targetName, Vector3 targetPos)
        {
            Vector3 currentWorldPos = Player.main.transform.position;            

            if (IsPlayerInVehicle())
            {
                Player.main.GetVehicle().TeleportVehicle(targetPos, Quaternion.identity);
                Player.main.CompleteTeleportation();
                ErrorMessage.AddMessage($"Vehicle and Player Warped to: {targetName}\n({targetPos.ToString()})");
            }
            else
            {
                Player.main.SetPosition(targetPos);
                Player.main.OnPlayerPositionCheat();
                ErrorMessage.AddMessage($"Player Warped to: {targetName}\n({targetPos.ToString()})");
            }

            return currentWorldPos;
        }

        public static void SetProgressColor(Color color)
        {
            HandReticle.main.progressText.color = color;
            HandReticle.main.progressImage.color = color;
        }

        public static void SetInteractColor(Color color, bool isSetSecondary = true)
        {
            HandReticle.main.interactPrimaryText.color = color;

            if (isSetSecondary)
                HandReticle.main.interactSecondaryText.color = color;
        }
        
        public class TechTypeData : IComparable<TechTypeData>
        {
            public TechType TechType { get; set; }
            public string Name { get; set; }

            public TechTypeData(TechType techType, string name)
            {
                TechType = techType;
                Name = name;
            }

            public int CompareTo(TechTypeData other)
            {
                return string.Compare(Name, other.Name);
            }

            public string GetTechName()
            {
                return Name;
            }
        }
        
        public static Material GetResourceMaterial(string gameObjectPath, string meshName, int materialIndex)
        {
            GameObject gameObject = Resources.Load<GameObject>(gameObjectPath);            

            foreach (MeshRenderer meshRenderer in gameObject.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (meshRenderer.name.Equals(meshName, StringComparison.CurrentCultureIgnoreCase))
                {
                    Material material = UnityEngine.Object.Instantiate(meshRenderer.materials[materialIndex]) as Material;

                    UnityEngine.Object.Destroy(gameObject);

                    return material;
                }               
            } 

            return null;
        }

        public static void SetMeshMaterial(this GameObject model, Material newMaterial, int index)
        {
            Renderer renderer = model.GetComponent<Renderer>();

            Material[] materials = renderer.materials;

            materials.SetValue(newMaterial, index);

            renderer.materials = materials;            
        }

        public static void CreateNewMaterialForObject(this GameObject model, string name, int materialIndex, Texture2D mainTex, Texture2D illumTex, Texture2D bumpMap, Texture2D specTex)
        {
            Renderer renderer = model.GetComponent<Renderer>();

            Shader newShader = renderer.material.shader;

            Material newMaterial = new Material(newShader)
            {
                name = name,
                hideFlags = HideFlags.HideAndDontSave,
                shaderKeywords = renderer.material.shaderKeywords
            };

            if (mainTex != null)
                newMaterial.SetTexture(ShaderPropertyID._MainTex, mainTex);

            if (illumTex != null)
                newMaterial.SetTexture(ShaderPropertyID._Illum, illumTex);

            if (bumpMap != null)
                newMaterial.SetTexture(ShaderPropertyID._BumpMap, bumpMap);

            if (specTex != null)
                newMaterial.SetTexture(ShaderPropertyID._SpecTex, specTex);

            Material[] materials = renderer.materials;

            materials.SetValue(newMaterial, materialIndex);

            renderer.materials = materials;            
        }

        public static Texture2D GetTextureFromRenderer(Renderer renderer, string shaderPropertyKeyword)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.HasProperty(shaderPropertyKeyword))
                {
                    Texture2D texture = (Texture2D)material.GetTexture(Shader.PropertyToID(shaderPropertyKeyword));

                    return CreateTextureFromNonReadableShaderTexture(texture);                    
                }
            }

            return null;
        }

        private static Texture2D CreateTextureFromNonReadableShaderTexture(Texture2D texture)
        {            
            RenderTexture tmp = RenderTexture.GetTemporary(texture.width, texture.height, 32, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);                        
            
            Graphics.Blit(texture, tmp);            
            
            RenderTexture previous = RenderTexture.active;
                    
            RenderTexture.active = tmp;            
            
            Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false, true);
            
            newTexture.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);

            newTexture.Apply();
            
            RenderTexture.active = previous;
            
            RenderTexture.ReleaseTemporary(tmp);

            return newTexture;            
        }

        public static void WriteTextureToPNGFile(Texture2D texture, string filename)
        {
            byte[] textureData = texture.EncodeToPNG();

            string pngPath = Path.Combine(Environment.CurrentDirectory, $"{filename}.png");            

            File.WriteAllBytes(pngPath, textureData);
        }


        public static void DebugObjectTransforms(this Transform transform, string space = "")
        {
            Console.WriteLine($"{space}--{transform.name}");            

            foreach (Transform child in transform)
            {
                child.DebugObjectTransforms(space + "   |");
            }            
        }

        public static GameObject FindChildWithMaxDepth(this GameObject gameObject, string name)
        {
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>(true))
            {
                if (child.name.Equals(name))
                {                    
                    return child.gameObject;
                }                
            }

            return null;
        }
    }
}
