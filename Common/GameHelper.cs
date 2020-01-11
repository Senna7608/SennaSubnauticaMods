using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime;

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
            if (!Player.main)
                return false;

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

        public static void SetMeshMaterial(this GameObject model, Material newMaterial, int index, string newMaterialName)
        {
            Renderer renderer = model.GetComponent<Renderer>();

            Material[] materials = renderer.materials;

            materials.SetValue(newMaterial, index);

            materials[index].name = newMaterialName;

            renderer.materials = materials;
        }

        public static void ChangeObjectTexture(this GameObject model, int materialIndex, Texture2D mainTex = null, Texture2D illumTex = null, Texture2D bumpMap = null, Texture2D specTex = null)
        {
            Renderer renderer = model.GetComponent<Renderer>();

            if (materialIndex < 0 || materialIndex >= renderer.materials.Length)
            {                
                throw new ArgumentException("Material index is out of range!");
            }

            if (mainTex != null)
            {
                renderer.materials[materialIndex].SetTexture(ShaderPropertyID._MainTex, mainTex);
            }            

            if (illumTex != null)
            {
                renderer.materials[materialIndex].SetTexture(ShaderPropertyID._Illum, illumTex);
            }

            if (bumpMap != null)
            {
                renderer.materials[materialIndex].SetTexture(ShaderPropertyID._BumpMap, bumpMap);
            }

            if (specTex != null)
            {
                renderer.materials[materialIndex].SetTexture(ShaderPropertyID._SpecTex, specTex);
            }            
        }

        public static Texture2D GetTextureFromRenderer(Renderer renderer, string shaderPropertyKeyword)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.HasProperty(shaderPropertyKeyword))
                {
                    Texture2D texture = (Texture2D)material.GetTexture(Shader.PropertyToID(shaderPropertyKeyword));

                    return CreateRWTextureFromNonReadableTexture(texture);                    
                }
            }

            return null;
        }

        public static Texture2D CreateRWTextureFromNonReadableTexture(this Texture2D texture)
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

        public static void WriteTextureToPNG(this Texture2D texture, string filename)
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

        public static GameObject FindChildInMaxDepth(this GameObject gameObject, string name)
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

        public static GameObject GetRootGameObject(string sceneName, string gameObjectName)
        {
            Scene scene;

            GameObject[] rootObjects;

            try
            {
                scene = SceneManager.GetSceneByName(sceneName);
            }
            catch
            {
                return null;
            }

            rootObjects = scene.GetRootGameObjects();
            
            foreach (GameObject gameObject in rootObjects)
            {
                if (gameObject.name.Equals(gameObjectName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return gameObject;
                }
            }
            return null;
        }
        
        public static void DebugComponent(this Component component)
        {
            List<string> keywords = new List<string>();

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.IgnoreReturn;
                        
            keywords.Add("Properties:");

            foreach (PropertyInfo propertyInfo in component.GetType().GetProperties(bindingFlags))
            {
                keywords.Add($"{propertyInfo.Name}  [{propertyInfo.GetValue(component, bindingFlags, null, null, null).ToString()}]");
            }

            keywords.Add("Fields:");

            foreach (FieldInfo fieldInfo in component.GetType().GetFields(bindingFlags))
            {
                keywords.Add($"{fieldInfo.Name}  [{fieldInfo.GetValue(component).ToString()}]");
            }

            foreach (string key in keywords)
            {
                SNLogger.Log($"{key}");
            }
        }

        public static GameObject GetPrefabClone(this GameObject prefab)
        {
            bool isActive = prefab.activeSelf;

            if (isActive)
            {
                prefab.SetActive(false);
            }

            GameObject clone = UnityEngine.Object.Instantiate(prefab);
            Utils.ZeroTransform(clone.transform);            
            return clone;
        }

        public static GameObject GetPrefabClone(this GameObject prefab, Transform newParent)
        {
            bool isActive = prefab.activeSelf;

            if (isActive)
            {
                prefab.SetActive(false);
            }

            GameObject clone = UnityEngine.Object.Instantiate(prefab);
            clone.transform.SetParent(newParent, false);
            Utils.ZeroTransform(clone.transform);
            prefab.SetActive(isActive);
            return clone;
        }

        public static GameObject GetPrefabClone(this GameObject prefab, Transform newParent, bool setActive)
        {
            bool isActive = prefab.activeSelf;

            if (isActive)
            {
                prefab.SetActive(false);
            }

            GameObject clone = UnityEngine.Object.Instantiate(prefab);
            clone.SetActive(setActive);
            clone.transform.SetParent(newParent, false);
            Utils.ZeroTransform(clone.transform);
            prefab.SetActive(isActive);
            return clone;
        }

        public static GameObject GetPrefabClone(this GameObject prefab, Transform newParent, bool setActive, string newName)
        {
            bool isActive = prefab.activeSelf;

            if (isActive)
            {
                prefab.SetActive(false);
            }

            GameObject clone = UnityEngine.Object.Instantiate(prefab);
            clone.SetActive(setActive);
            clone.transform.SetParent(newParent, false);
            clone.name = newName;
            Utils.ZeroTransform(clone.transform);
            prefab.SetActive(isActive);            
            return clone;
        }

        public static T GetObjectClone<T>(this T uEobject) where T : UnityEngine.Object
        {
            return UnityEngine.Object.Instantiate(uEobject);            
        }

        public static T GetComponentClone<T>(this T uEcomponent, Transform newParent) where T : Component
        {
            T clone = UnityEngine.Object.Instantiate(uEcomponent);
            clone.transform.SetParent(newParent, false);
            Utils.ZeroTransform(clone.transform);
            return clone;
        }        

        public static GameObject CreateGameObject(string name, Transform newParent)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(newParent, false);
            Utils.ZeroTransform(newObject.transform);

            return newObject;
        }

        public static GameObject CreateGameObject(PrimitiveType primitiveType, string name, Transform newParent)
        {
            GameObject newObject = GameObject.CreatePrimitive(primitiveType);
            newObject.name = name;
            newObject.transform.SetParent(newParent, false);

            return newObject;
        }

        public static GameObject CreateGameObject(string name, Transform newParent, Vector3 localPos)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(newParent, false);
            Utils.ZeroTransform(newObject.transform);
            newObject.transform.localPosition = localPos;            

            return newObject;
        }

        public static GameObject CreateGameObject(PrimitiveType primitiveType, string name, Transform newParent, Vector3 localPos)
        {
            GameObject newObject = GameObject.CreatePrimitive(primitiveType);
            newObject.name = name;
            newObject.transform.SetParent(newParent, false);
            newObject.transform.localPosition = localPos;            

            return newObject;
        }

        public static GameObject CreateGameObject(string name, Transform newParent, Vector3 localPos, Vector3 localRot)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(newParent, false);
            newObject.transform.localPosition = localPos;
            newObject.transform.localRotation = Quaternion.Euler(localRot);

            return newObject;
        }
        
        public static GameObject CreateGameObject(PrimitiveType primitiveType, string name, Transform newParent, Vector3 localPos, Vector3 localRot)
        {
            GameObject newObject = GameObject.CreatePrimitive(primitiveType);
            newObject.name = name;
            newObject.transform.SetParent(newParent, false);
            newObject.transform.localPosition = localPos;
            newObject.transform.localRotation = Quaternion.Euler(localRot);

            return newObject;
        }

        public static int GetSlotIndex(Vehicle vehicle, TechType techType)
        {
            InventoryItem inventoryItem = null;

            for (int i = 0; i < vehicle.GetSlotCount(); i++)
            {
                try
                {
                    inventoryItem = vehicle.GetSlotItem(i);
                }
                catch
                {
                    continue;
                }

                if (inventoryItem != null && inventoryItem.item.GetTechType() == techType)
                {
                    return vehicle.GetType() == typeof(Exosuit) ? i - 2 : i;
                }
            }

            return -1;
        }

    }


    public class SelfDestruct : MonoBehaviour
    {
        private bool isRoot = false;

        private void Start()
        {
            isRoot = transform.parent == null ? true : false;           
        }

        private void Update()
        {
            if (isRoot && Vector3.Distance(transform.position, Vector3.zero) > 8000)
            {                
                Destroy(gameObject);                
            }
        }
    }

    


}
