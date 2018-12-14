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
        public static void DebugVehicle(string instance, string methodName, Vehicle vehicle)
        {
            Debug.Log("[DebugHelper]\n" +
                $"Instance: {instance}\n" +                
                $"Call from: {methodName}()\n" +
                $"Object class: {vehicle.GetType()}\n" +
                $"Object name: {vehicle.name}");            
        }


        public static void DebugGameObject(string name, GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.LogError($"[DebugHelper] GameObject: [{name}] is NULL!");
                return;
            }

            bool noParent = false;

            if (gameObject.transform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugGameObject: [{name}]\n" +

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
                Debug.LogError($"[DebugHelper] Collider: [{name}] is NULL!");
                return;
            }

            bool noRigidbody = false;           

            if (collider.attachedRigidbody == null)
                noRigidbody = true;

            bool noParent = false;

            if (collider.transform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugCollider: [{name}]\n" +

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
                Debug.LogError($"[DebugHelper] Equipment: [{name}] is NULL!");
                return;
            }

            bool noParent = false;

            if (equipment.tr.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugEquipment: [{name}]\n" +

                $"\nObject:\n" +
                $" type: {equipment.GetType().ToString()}\n" +
                $" owner: {equipment.owner}\n" +
                
                $"\nTransform:\n" +
                $" transform: {equipment.tr}\n" +
                $" parent: {(noParent ? "NULL" : equipment.tr.parent.ToString())}"
                );
        }


        public static void DebugRectTransform(string name, RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                Debug.LogError($"[DebugHelper] RectTransform: [{name}] is NULL!");
                return;
            }

            bool noParent = false;

            if (rectTransform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugRectTransform: [{name}]\n" +

                $"\nObject:\n" +
                $" type: {rectTransform.GetType().ToString()}\n" +                

                $"\nTransform:\n" +
                $" transform: {rectTransform.transform}\n" +
                $" parent: {(noParent ? "NULL" : rectTransform.parent.ToString())}\n" +                
                $" root: {rectTransform.root}\n" + 

                $"\nVector2:\n" +
                $" anchoredPosition: {rectTransform.anchoredPosition}\n" +
                $" anchorMax: {rectTransform.anchorMax}\n" +
                $" anchorMin: {rectTransform.anchorMin}\n" +
                $" offsetMax: {rectTransform.offsetMax}\n" +
                $" offsetMin: {rectTransform.offsetMin}\n" +
                $" pivot: {rectTransform.pivot}\n" +
                $" sizeDelta: {rectTransform.sizeDelta}\n" +

                $"\nVector3:\n" +
                $" position: {rectTransform.position}\n" +
                $" localPosition: {rectTransform.localPosition}\n" +
                $" right: {rectTransform.right}\n" +
                $" up: {rectTransform.up}\n" +
                $" anchoredPosition3D: {rectTransform.anchoredPosition3D}\n" +
                $" eulerAngles: {rectTransform.eulerAngles}\n" +
                $" localEulerAngles: {rectTransform.localEulerAngles}\n" +
                $" forward: {rectTransform.forward}\n" +
                $" localScale: {rectTransform.localScale}\n" +
                $" lossyScale: {rectTransform.lossyScale}\n" +

                $"\nQuaternion:\n" +
                $" rotation: {rectTransform.rotation}\n" +
                $" localRotation: {rectTransform.localRotation}\n" +

                $"\nOther:\n" +
                $" childCount: {rectTransform.childCount}\n" +
                $" hasChanged: {rectTransform.hasChanged}\n" +
                $" hideFlags: {rectTransform.hideFlags}\n" +
                $" hierarchyCapacity: {rectTransform.hierarchyCapacity}\n" +
                $" hierarchyCount: {rectTransform.hierarchyCount}\n"
                );
        }

        public static void DebugTransform(string name, Transform transform)
        {
            if (transform == null)
            {
                Debug.LogError($"[DebugHelper] transform: [{name}] is NULL!");
                return;
            }

            bool noParent = false;

            if (transform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugTransform: [{name}]\n" +

                $"\nObject:\n" +
                $" type: {transform.GetType().ToString()}\n" +

                $"\nTransform:\n" +
                $" transform: {transform.transform}\n" +
                $" parent: {(noParent ? "NULL" : transform.parent.ToString())}\n" +
                $" root: {transform.root}\n" +          

                $"\nVector3:\n" +
                $" position: {transform.position}\n" +
                $" localPosition: {transform.localPosition}\n" +                
                $" right: {transform.right}\n" +
                $" up: {transform.up}\n" +                
                $" eulerAngles: {transform.eulerAngles}\n" +
                $" localEulerAngles: {transform.localEulerAngles}\n" +
                $" forward: {transform.forward}\n" +
                $" localScale: {transform.localScale}\n" +
                $" lossyScale: {transform.lossyScale}\n" +

                $"\nQuaternion:\n" +
                $" rotation: {transform.rotation}\n" +
                $" localRotation: {transform.localRotation}\n" +

                $"\nOther:\n" +
                $" childCount: {transform.childCount}\n" +
                $" hasChanged: {transform.hasChanged}\n" +
                $" hideFlags: {transform.hideFlags}\n" +
                $" hierarchyCapacity: {transform.hierarchyCapacity}\n" +
                $" hierarchyCount: {transform.hierarchyCount}\n"
                );
        }


        public static void DebugRigidbody(string name, Rigidbody rb)
        {
            if (rb == null)
            {
                Debug.LogError($"[DebugHelper] Rigidbody: [{name}] is NULL!");
                return;
            }

            bool noParent = false;

            if (rb.transform.parent == null)
                noParent = true;

            Debug.Log($"[DebugHelper] DebugRigidbody: [{name}]\n" +

                $"\nObject:\n" +
                $" type: {rb.GetType().ToString()}\n" +

                $"\nTransform:\n" +
                $" transform: {rb.transform}\n" +
                $" parent: {(noParent ? "NULL" : rb.transform.parent.ToString())}\n" +
                $" root: {rb.transform.root}\n" +

                $" angularDrag: {rb.angularDrag}\n" +
                $" angularVelocity: {rb.angularVelocity}\n" +
                $" centerOfMass: {rb.centerOfMass}\n" +
                $" collisionDetectionMode: {rb.collisionDetectionMode}\n" +
                $" constraints: {rb.constraints}\n" +
                $" detectCollisions: {rb.detectCollisions}\n" +
                $" drag: {rb.drag}\n" +
                $" freezeRotation: {rb.freezeRotation}\n" +                               
                $" inertiaTensor: {rb.inertiaTensor}\n" +
                $" inertiaTensorRotation: {rb.inertiaTensorRotation}\n" +                
                $" interpolation: {rb.interpolation}\n" +
                $" isKinematic: {rb.isKinematic}\n" +
                $" mass: {rb.mass}\n" +
                $" maxAngularVelocity: {rb.maxAngularVelocity}\n" +
                $" maxDepenetrationVelocity: {rb.maxDepenetrationVelocity}\n" +
                $" useGravity: {rb.useGravity}\n" +
                $" velocity: {rb.velocity}\n" +
                $" worldCenterOfMass: {rb.worldCenterOfMass}\n"
                );
        }

    }
}
