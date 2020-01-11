using UnityEngine;
using Debuggers.UE._Structures;

namespace Debuggers.UE._Object._Component
{
    public static class _Transform
    {
        public static void DebugTransform(this Transform transform, string prefixString = null)
        {
            if (prefixString == null)
            {
                DLog.Log($"\n[Transform] ({transform.name})");
                prefixString = "this";
            }

            DLog.Log($"{prefixString}.childCount: {transform.childCount}");
            DLog.Log($"{prefixString}.eulerAngles: {transform.eulerAngles}");
            DLog.Log($"{prefixString}.forward: {transform.forward}");
            DLog.Log($"{prefixString}.gameObject: {transform.gameObject}");
            DLog.Log($"{prefixString}.hasChanged: {transform.hasChanged}");
            DLog.Log($"{prefixString}.hideFlags: {transform.hideFlags}");
            DLog.Log($"{prefixString}.hierarchyCapacity: {transform.hierarchyCapacity}");
            DLog.Log($"{prefixString}.hierarchyCount: {transform.hierarchyCount}");
            DLog.Log($"{prefixString}.localEulerAngles: {transform.localEulerAngles}");
            DLog.Log($"{prefixString}.localPosition: {transform.localPosition}");
            DLog.Log($"{prefixString}.localRotation: {transform.localRotation}");
            DLog.Log($"{prefixString}.localScale: {transform.localScale}");

            transform.localToWorldMatrix.DebugMatrix4x4($"{prefixString}.localToWorldMatrix");

            DLog.Log($"{prefixString}.lossyScale: {transform.lossyScale}");
            DLog.Log($"{prefixString}.name: {transform.name}");

            if (transform.parent == null)
            {
                DLog.Log($"{prefixString}.parent: NULL");
            }
            else
            {
                DLog.Log($"{prefixString}.parent: {transform.parent}");
            }

            DLog.Log($"{prefixString}.position: {transform.position}");
            DLog.Log($"{prefixString}.right: {transform.right}");
            DLog.Log($"{prefixString}.root: {transform.root}");
            DLog.Log($"{prefixString}.rotation: {transform.rotation}");
            DLog.Log($"{prefixString}.tag: {transform.tag}");
            DLog.Log($"{prefixString}.transform: {transform.transform}");
            DLog.Log($"{prefixString}.up: {transform.up}");

            transform.worldToLocalMatrix.DebugMatrix4x4($"{prefixString}.worldToLocalMatrix");
        }
    }
}
