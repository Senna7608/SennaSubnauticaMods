using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Internal;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace Common.DebugHelper
{   
    public static class DebugHelper
    {
        private static readonly string FILENAME = $"{Environment.CurrentDirectory}\\DebugHelper_";        

        public static void Write(string logMessage) => Debug.Log(logMessage);        

        public static void DebugGameObject(GameObject gameObject, bool fullDebug = false)
        {
            if (gameObject == null)
                return;

            FileStream fileStream = new FileStream($"{FILENAME}{gameObject.name}.txt", FileMode.CreateNew, FileAccess.Write);

            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                DebugGameObject(gameObject, streamWriter, fullDebug: fullDebug);
            }
        }              

        public static void DebugGameObject(GameObject gameObject, StreamWriter streamWriter, string space = "", bool fullDebug = false)
        {
            if (gameObject == null)
                return;          

            streamWriter.WriteLine(space + $"Name: [{gameObject}]");
            streamWriter.WriteLine(space + "{");
            streamWriter.WriteLine(space + " Components:");
            streamWriter.WriteLine(space + " {");

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                streamWriter.WriteLine(space + $"   [{component.GetType().ToString()}]");
            }
            streamWriter.WriteLine(space + " }");

            DebugTransform(gameObject.transform, streamWriter, space, fullDebug);
           
            streamWriter.WriteLine(space + $"  Child:");
            streamWriter.WriteLine(space + "   {");            

            foreach (Transform child in gameObject.transform)
            {
                DebugGameObject(child.gameObject, streamWriter, space + "    ", fullDebug);               
            }
            streamWriter.WriteLine(space + "   }");
            streamWriter.WriteLine(space + "}");            
        }


        private static void DebugTransform(Transform transform, StreamWriter streamWriter, string space, bool fullDebug = false)
        {
            streamWriter.WriteLine(space + "  Transform:");
            streamWriter.WriteLine(space + "  {");
            streamWriter.WriteLine(space + $"   name: [{transform}]");            
            streamWriter.WriteLine(space + $"   parent: [{transform.parent}]");
            streamWriter.WriteLine(space + $"   root: [{transform.root}]");
            streamWriter.WriteLine(space + $"   childCount: [{transform.childCount}]");

            if (fullDebug)
            {
                streamWriter.WriteLine(space);
                streamWriter.WriteLine(space + "  Vector3:");
                streamWriter.WriteLine(space + $"   position: {transform.position}");
                streamWriter.WriteLine(space + $"   localPosition: {transform.localPosition}");
                streamWriter.WriteLine(space + $"   right: {transform.right}");
                streamWriter.WriteLine(space + $"   up: {transform.up}");
                streamWriter.WriteLine(space + $"   eulerAngles: {transform.eulerAngles}");
                streamWriter.WriteLine(space + $"   localEulerAngles: {transform.localEulerAngles}");
                streamWriter.WriteLine(space + $"   forward: {transform.forward}");
                streamWriter.WriteLine(space + $"   localScale: {transform.localScale}");
                streamWriter.WriteLine(space + $"   lossyScale: {transform.lossyScale}");
                streamWriter.WriteLine(space);
                streamWriter.WriteLine(space + "  Quaternion:");
                streamWriter.WriteLine(space + $"   rotation: {transform.rotation}");
                streamWriter.WriteLine(space + $"   localRotation: {transform.localRotation}");
                streamWriter.WriteLine(space);
                streamWriter.WriteLine(space + "  Other:");                
                streamWriter.WriteLine(space + $"   hasChanged: [{transform.hasChanged}]");
                streamWriter.WriteLine(space + $"   hideFlags: [{transform.hideFlags}]");
                streamWriter.WriteLine(space + $"   hierarchyCapacity: [{transform.hierarchyCapacity}]");
                streamWriter.WriteLine(space + $"   hierarchyCount: [{transform.hierarchyCount}]");
            }
            streamWriter.WriteLine(space + "  }");
        }



        public static void DebugVehicle(string instance, string methodName, Vehicle vehicle)
        {
            Write("[DebugHelper]\n" +
                $"Instance: {instance}\n" +                
                $"Call from: {methodName}()\n" +
                $"Object class: {vehicle.GetType()}\n" +
                $"Object name: {vehicle.name}");            
        }

        
        public static void DebugCollider(Collider collider)
        {
            if (collider == null)
            {
                Write($"[DebugHelper] DebugCollider() parameter is NULL!");
                return;
            }

            bool noRigidbody = false;           

            if (collider.attachedRigidbody == null)
                noRigidbody = true;

            bool noParent = false;

            if (collider.transform.parent == null)
                noParent = true;

            Write($"[DebugHelper] DebugCollider()\n" +

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

        public static void DebugEquipment(Equipment equipment)
        {
            if (equipment == null)
            {
                Write($"[DebugHelper] DebugEquipment() parameter is NULL!");
                return;
            }

            bool noParent = false;

            if (equipment.tr.parent == null)
                noParent = true;

            Write($"[DebugHelper] DebugEquipment()\n" +

                $"\nObject:\n" +
                $" type: {equipment.GetType().ToString()}\n" +
                $" owner: {equipment.owner}\n" +
                
                $"\nTransform:\n" +
                $" transform: {equipment.tr}\n" +
                $" parent: {(noParent ? "NULL" : equipment.tr.parent.ToString())}"
                );
        }


        public static void DebugRectTransform(RectTransform rectTransform)
        {
            if (rectTransform == null)
            {
                Write($"[DebugHelper] DebugRectTransform() parameter is NULL!");
                return;
            }

            bool noParent = false;

            if (rectTransform.parent == null)
                noParent = true;

            Write($"[DebugHelper] DebugRectTransform()\n" +

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
        
        public static void DebugRigidbody(Rigidbody rb)
        {
            if (rb == null)
            {
                Write($"[DebugHelper] DebugRigidbody() parameter is NULL!");
                return;
            }

            bool noParent = false;

            if (rb.transform.parent == null)
                noParent = true;

            Write($"[DebugHelper] DebugRigidbody()\n" +

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
