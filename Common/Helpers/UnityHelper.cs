using System;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS1591

namespace Common.Helpers
{
    public static class UnityHelper
    {
        public static bool IsRoot(this Transform transform)
        {
            return transform.parent == null;
        }

        public static bool IsRoot(this GameObject gameObject)
        {
            return gameObject.transform.parent == null;
        }        

        public static void PrefabCleaner(this GameObject rootGo, List<Type> componentList, List<string> childList, bool isWhiteList = false)
        {
            if (componentList == null)
            {
                throw new ArgumentException("*** Component list cannot be null!");
            }

            foreach (Component component in rootGo.GetComponentsInChildren<Component>(true))
            {
                Type componentType = component.GetType();

                if (componentType == typeof(Transform))
                    continue;

                if (componentType == typeof(RectTransform))
                    continue;

                bool containsComponent = componentList.Contains(componentType);

                if (isWhiteList)
                {
                    if (containsComponent)
                    {
                        continue;
                    }
                    else
                    {
                        UnityEngine.Object.DestroyImmediate(component);
                    }
                }
                else
                {
                    if (containsComponent)
                    {
                        UnityEngine.Object.DestroyImmediate(component);
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            SNLogger.Debug("PrefabCleaner: components cleared");

            if (childList == null)
                return;

            List<Transform> markedForDestroy = new List<Transform>();

            foreach (Transform transform in rootGo.GetComponentsInChildren<Transform>(true))
            {
                SNLogger.Debug($"Current Transform name: {transform?.name}");
                
                if (transform == rootGo.transform)
                {
                    continue;
                }

                bool containsTransform = childList.Contains(transform?.name);                

                if (isWhiteList)
                {
                    if (containsTransform)
                    {
                        continue;
                    }
                    else
                    {
                        markedForDestroy.Add(transform);
                        SNLogger.Debug($"Whitelist: {transform.name} added to markedForDestroy list.");
                    }
                }
                else
                {
                    if (containsTransform)
                    {
                        markedForDestroy.Add(transform);
                        SNLogger.Debug($"Blacklist: {transform.name} added to markedForDestroy list.");
                    }
                    else
                    {
                        continue;
                    }
                }                
            }

            foreach (Transform tr in markedForDestroy)
            {
              UnityEngine.Object.DestroyImmediate(tr?.gameObject);                
            }
        }

        public static void CleanObject(this GameObject gameObject)
        {
            foreach (Component component in gameObject.GetComponents<Component>())
            {
                Type componentType = component.GetType();

                if (componentType == typeof(Transform))
                    continue;
                if (componentType == typeof(Renderer))
                    continue;
                if (componentType == typeof(Mesh))
                    continue;
                if (componentType == typeof(Shader))
                    continue;

                UnityEngine.Object.DestroyImmediate(component);
            }
        }


        public static string GetUeObjectShortType(this UnityEngine.Object ueObject)
        {
            return ueObject.GetType().ToString().Split('.').GetLast();
        }

        public static string GetPath(this Transform current)
        {
            if (current.parent == null)
                return current.name;
            return current.parent.GetPath() + "/" + current.name;
        }
    }
}

