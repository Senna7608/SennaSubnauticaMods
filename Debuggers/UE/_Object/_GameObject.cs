using UnityEngine;
using Debuggers.UE._Object._Component;
using Debuggers.UE._Structures;

namespace Debuggers.UE._Object
{
    public static class _GameObject
    {
        public static void DebugGameObject(this GameObject gameObject, string prefixString = null, bool debugChilds = false)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[GameObject] ({gameObject.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.activeInHierarchy: {gameObject.activeInHierarchy}");
            DLog.Log($"{prefixString}.activeSelf: {gameObject.activeSelf}");
            DLog.Log($"{prefixString}.hideFlags: {gameObject.hideFlags}");
            DLog.Log($"{prefixString}.isStatic: {gameObject.isStatic}");
            DLog.Log($"{prefixString}.layer: {gameObject.layer}");
            DLog.Log($"{prefixString}.name: {gameObject.name}");
            gameObject.scene.DebugScene($"{prefixString}.scene");
            DLog.Log($"{prefixString}.tag: {gameObject.tag}");
            DLog.Log($"{prefixString}.transform: {gameObject.transform.name}");

            DLog.Log($"Components of this GameObject: {gameObject.name}");

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                DLog.Log($"{prefixString}.{component.GetType()}");
            }

            if (debugChilds)
                gameObject.DebugChildObjects();
        }

        private static void DebugChildObjects(this GameObject gameObject)
        {
            Transform transform = gameObject.transform;             

            for(int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.DebugGameObject(child.name, false);
            }
        }
    }
}
