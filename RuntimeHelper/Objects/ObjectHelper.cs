using RuntimeHelper.Visuals;
using System;
using UnityEngine;

namespace RuntimeHelper.Objects
{
    public static class ObjectHelper
    {
        public static int pasteCount = 0;

        public static void CopyObject(this GameObject gameObject, out GameObject tempObject)
        {
            gameObject.SetActive(false);

            tempObject = UnityEngine.Object.Instantiate(gameObject, null, false);

            tempObject.SetActive(false);

            var dob = tempObject.GetComponentInChildren<DrawObjectBounds>(true);
            dob.gameObject.SetActive(false);            

            UnityEngine.Object.Destroy(dob.gameObject);
            
            gameObject.SetActive(true);
        }

        public static GameObject PasteObject(this GameObject tempObject, Transform parent)
        {
            GameObject newObject = UnityEngine.Object.Instantiate(tempObject, parent, false);
            newObject.name = $"newPastedObject_{pasteCount}";
            /*
            Mesh mesh = tempObject.GetComponent<Mesh>();

            Bounds bounds = mesh.bounds;

            Vector3[] corners = 
            */

            newObject.transform.localPosition = new Vector3(0, tempObject.transform.localPosition.y, 0);
            newObject.transform.localRotation = tempObject.transform.localRotation;

            newObject.SetActive(true);
            
            UnityEngine.Object.Destroy(tempObject);

            pasteCount++;

            return newObject;
        }       

    }
}
