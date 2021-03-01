using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Common.Helpers
{
    public class ObjectHelper
    {
        public T GetObjectClone<T>(T uEobject) where T : Object
        {
            return Object.Instantiate(uEobject);
        }
                
        public GameObject FindDeepChild(Transform parent, string childName)
        {
            Queue<Transform> queue = new Queue<Transform>();

            queue.Enqueue(parent);

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();

                if (c.name == childName)
                {
                    return c.gameObject;
                }

                foreach (Transform t in c)
                {
                    queue.Enqueue(t);
                }
            }
            return null;
        }

        public GameObject FindDeepChild(GameObject parent, string childName)
        {
            Queue<Transform> queue = new Queue<Transform>();

            queue.Enqueue(parent.transform);

            while (queue.Count > 0)
            {
                var c = queue.Dequeue();

                if (c.name == childName)
                {
                    return c.gameObject;
                }

                foreach (Transform t in c)
                {
                    queue.Enqueue(t);
                }
            }
            return null;
        }        

        public GameObject GetRootGameObject(string sceneName, string startsWith)
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
                if (gameObject.name.StartsWith(startsWith))
                {
                    return gameObject;
                }
            }
            return null;
        }

        public GameObject GetGameObjectClone(GameObject go)
        {
            bool isActive = go.activeSelf;

            if (isActive)
            {
                go.SetActive(false);
            }

            GameObject clone = Object.Instantiate(go);
            Utils.ZeroTransform(clone.transform);
            return clone;
        }

        public GameObject GetGameObjectClone(GameObject go, Transform newParent)
        {
            bool isActive = go.activeSelf;

            if (isActive)
            {
                go.SetActive(false);
            }

            GameObject clone = Object.Instantiate(go);
            clone.transform.SetParent(newParent, false);
            Utils.ZeroTransform(clone.transform);
            go.SetActive(isActive);
            return clone;
        }

        public GameObject GetGameObjectClone(GameObject go, Transform newParent, bool setActive)
        {
            bool isActive = go.activeSelf;

            if (isActive)
            {
                go.SetActive(false);
            }

            GameObject clone = Object.Instantiate(go);
            clone.SetActive(setActive);
            clone.transform.SetParent(newParent, false);
            Utils.ZeroTransform(clone.transform);
            go.SetActive(isActive);
            return clone;
        }

        public GameObject GetGameObjectClone(GameObject go, Transform newParent, bool setActive, string newName)
        {
            bool isActive = go.activeSelf;

            if (isActive)
            {
                go.SetActive(false);
            }

            GameObject clone = Object.Instantiate(go);
            clone.SetActive(setActive);
            clone.transform.SetParent(newParent, false);
            clone.name = newName;
            Utils.ZeroTransform(clone.transform);
            go.SetActive(isActive);
            return clone;
        }

        public void GetGameObjectClone(ref GameObject go, Transform newParent, bool setActive, string newName, out GameObject clone)
        {
            bool isActive = go.activeSelf;

            if (isActive)
            {
                go.SetActive(false);
            }

            clone = Object.Instantiate(go);
            clone.SetActive(setActive);
            clone.transform.SetParent(newParent, false);
            clone.name = newName;
            Utils.ZeroTransform(clone.transform);
            go.SetActive(isActive);
        }

        public GameObject GetModelCloneFromPrefab(TechType techType, string model, Transform newParent, bool setActive, string newName)
        {
            GameObject clone = null;

            try
            {
                clone = FindDeepChild(Object.Instantiate(CraftData.GetPrefabForTechType(techType)), model);
            }
            catch
            {
                return null;
            }

            clone.SetActive(setActive);
            clone.transform.SetParent(newParent, false);
            clone.name = newName;
            Utils.ZeroTransform(clone.transform);
            return clone;
        }

        public GameObject CreateGameObject(string name)
        {
            GameObject newObject = new GameObject(name);
            Utils.ZeroTransform(newObject.transform);
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(parent, false);
            Utils.ZeroTransform(newObject.transform);
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent, bool active)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(parent, false);
            Utils.ZeroTransform(newObject.transform);
            newObject.SetActive(active);
            return newObject;
        }

        public GameObject CreateGameObject(PrimitiveType primitiveType, string name, Transform parent)
        {
            GameObject newObject = GameObject.CreatePrimitive(primitiveType);
            newObject.name = name;
            newObject.transform.SetParent(parent, false);
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent, Vector3 localPos)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(parent, false);
            Utils.ZeroTransform(newObject.transform);
            newObject.transform.localPosition = localPos;
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent, Vector3 localPos, PrimitiveType primitiveType)
        {
            GameObject newObject = GameObject.CreatePrimitive(primitiveType);
            newObject.name = name;
            newObject.transform.SetParent(parent, false);
            newObject.transform.localPosition = localPos;
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent, Vector3 localPos, Vector3 localRot)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(parent, false);
            newObject.transform.localPosition = localPos;
            newObject.transform.localRotation = Quaternion.Euler(localRot);
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent, Vector3 localPos, Vector3 localRot, Vector3 localScale)
        {
            GameObject newObject = new GameObject(name);
            newObject.transform.SetParent(parent, false);
            newObject.transform.localPosition = localPos;
            newObject.transform.localRotation = Quaternion.Euler(localRot);
            newObject.transform.localScale = localScale;
            return newObject;
        }

        public GameObject CreateGameObject(string name, Transform parent, Vector3 localPos, Vector3 localRot, PrimitiveType primitiveType)
        {
            GameObject newObject = GameObject.CreatePrimitive(primitiveType);
            newObject.name = name;
            newObject.transform.SetParent(parent, false);
            newObject.transform.localPosition = localPos;
            newObject.transform.localRotation = Quaternion.Euler(localRot);
            return newObject;
        }
    }
}
