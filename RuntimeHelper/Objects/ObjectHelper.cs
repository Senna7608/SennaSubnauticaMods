using RuntimeHelper.Visuals;
using System;
using UnityEngine;

namespace RuntimeHelper.Objects
{
    public static class ObjectHelper
    {
        public static void CopyObject(this GameObject gameObject, out GameObject tempObject)
        {
            tempObject = UnityEngine.Object.Instantiate(gameObject, null, false);

            foreach (Component component in tempObject.GetComponents<Component>())
            {
                Type componentType = component.GetType();

                if (componentType == typeof(DrawObjectBounds) || componentType == typeof(DrawColliderBounds))
                {
                    UnityEngine.Object.Destroy(component);
                }
            }            

            tempObject.SetActive(false);
        }

        public static void PasteObject(this GameObject tempObject, Transform parent)
        {
            GameObject newObject = UnityEngine.Object.Instantiate(tempObject, parent, false);
            newObject.name = "newPastedObject";
            newObject.transform.localPosition = Vector3.zero;
            newObject.transform.localRotation = Quaternion.identity;

            newObject.SetActive(true);

            UnityEngine.Object.Destroy(tempObject);
        }

       

        public static void RotateCustom(this Transform t, Vector3 angles)
        {            
            t.Rotate(angles.x, angles.y, angles.z);            
        }

        
       















    }
}
