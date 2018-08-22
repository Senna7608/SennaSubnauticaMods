using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;
using System.Collections.Generic;
using System.Collections;

namespace Common.DebugHelper
{
    public static class DebugHelper
    {

        public static void DebugGameObject(string name, GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.LogError($"[DebugHelper] GameObject: {name} is NULL!");
                return;
            }

            bool noParent = false;

            if (gameObject.transform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugGameObject: {name}\n" +
                $"\nObject:\n" +
                $" type: {gameObject.GetType().ToString()}\n" +
                $" name: {gameObject.name}\n" +
                $" tag: {gameObject.tag}\n" +
                $" scene: {gameObject.scene}\n" +
                $" activeInHierarchy: {gameObject.activeInHierarchy}\n" +
                $" hideFlags: {gameObject.hideFlags}\n" +
                $" isStatic: {gameObject.isStatic}\n" +
                $" activeSelf: {gameObject.activeSelf}\n" +                
                $"\nTransform:\n" +
                $" transform: {gameObject.transform}\n" +
                $" parent: {(noParent ? "NULL" : gameObject.transform.parent.ToString())}"
                );
        }

        public static void DebugCollider(string name, Collider collider)
        {
            if (collider == null)
            {
                Debug.LogError($"[DebugHelper] Collider: {name} is NULL!");
                return;
            }

            bool noRigidbody = false;           

            if (collider.attachedRigidbody == null)
                noRigidbody = true;

            bool noParent = false;

            if (collider.transform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugCollider: {name}\n" +
                $"\nObject:\n" +
                $" type: {collider.GetType().ToString()}\n" +          
                $" name: {collider.name}\n" +                
                $" tag: {collider.tag}\n" +
                $" enabled: {collider.enabled}\n" +
                $" hideFlags: {collider.hideFlags}\n" +
                $" isTrigger: {collider.isTrigger}\n" +
                $" contactOffset: {collider.contactOffset}\n" +
                $" attachedRigidbody: {(noRigidbody ? "NULL" : collider.attachedRigidbody.ToString())}\n" +
                $" isKinematic: {(noRigidbody ? "NULL" : collider.attachedRigidbody.isKinematic.ToString())}\n" +
                $"\nTransform:\n" +
                $" transform: {collider.transform}\n" +
                $" parent: {(noParent? "NULL" : collider.transform.parent.ToString())}\n" +                
                $"\nMaterial:\n" +
                $" material: {collider.material}\n" +
                $" material.name: {collider.material.name}\n" +
                $" sharedMaterial: {collider.sharedMaterial}\n" +
                $" sharedMaterial.name: {collider.sharedMaterial.name}\n" +                
                $"\nBounds:\n" +
                $" bounds.size: {collider.bounds.size}\n" +
                $" bounds.center: {collider.bounds.center}\n" +
                $" bounds.extents: {collider.bounds.extents}\n" +
                $" bounds.max: {collider.bounds.max}\n" +
                $" bounds.min: {collider.bounds.min}"
                );
        }

        public static void DebugEquipment(string name, Equipment equipment)
        {
            if (equipment == null)
            {
                Debug.LogError($"[DebugHelper] Equipment: {name} is NULL!");
                return;
            }

            bool noParent = false;

            if (equipment.tr.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugEquipment: {name}\n" +
                $"\nObject:\n" +
                $" type: {equipment.GetType().ToString()}\n" +
                $" owner: {equipment.owner}\n" +                              
                $"\nTransform:\n" +
                $" transform: {equipment.tr}\n" +
                $" parent: {(noParent ? "NULL" : equipment.tr.parent.ToString())}"
                );
        }









    }
}
