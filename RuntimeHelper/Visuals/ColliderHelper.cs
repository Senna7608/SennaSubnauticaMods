using System;
using System.Collections.Generic;
using UnityEngine;
using RuntimeHelper.Logger;
using Common;
using RuntimeHelper.Components;

namespace RuntimeHelper.Visuals
{
    public static class ColliderHelper
    {     
        public static bool IsExistsCollider(this GameObject gameObject)
        {
            return gameObject.GetComponent<Collider>() ? true : false;
        }

        public static ColliderType GetColliderType(this Collider collider)
        {
            if (!collider)
            {                             
                return ColliderType.None;
            }

            Type thisCollider = collider.GetType();

            if (thisCollider == typeof(BoxCollider))
            {                
                return ColliderType.BoxCollider;                
            }
            else if (thisCollider == typeof(CapsuleCollider))
            {               
                return ColliderType.CapsuleCollider;                
            }
            else if (thisCollider == typeof(SphereCollider))
            {                
                return ColliderType.SphereCollider;                
            }
            else
            {
                Main.Instance.OutputWindow_Log($"Unsupported collider: [{thisCollider}]", LogType.Warning);

                return ColliderType.None;
            }
        }

        public static void SetColliderInfo(this Collider collider, ref ColliderInfo colliderBase)
        {
            switch (collider.GetColliderType())
            {
                case ColliderType.BoxCollider:                    
                    BoxCollider bc = (BoxCollider)collider;
                    colliderBase.ColliderType = ColliderType.BoxCollider;
                    colliderBase.Size = bc.size;
                    colliderBase.Center = bc.center;                    
                    break;

                case ColliderType.CapsuleCollider:
                    CapsuleCollider cc = (CapsuleCollider)collider;
                    colliderBase.ColliderType = ColliderType.CapsuleCollider;
                    colliderBase.Radius = cc.radius;
                    colliderBase.Center = cc.center;
                    colliderBase.Direction = cc.direction;
                    colliderBase.Height = cc.height;                   
                    break;

                case ColliderType.SphereCollider:
                    SphereCollider sc = (SphereCollider)collider;
                    colliderBase.ColliderType = ColliderType.SphereCollider;
                    colliderBase.Radius = sc.radius;
                    colliderBase.Center = sc.center;                    
                    break;
            }
        }
                     
        public static void ResetCollider(this GameObject gameObject, ColliderInfo colliderBase, int ID)
        {
            switch (colliderBase.ColliderType)
            {
                case ColliderType.BoxCollider:
                    BoxCollider bc = gameObject.FindComponentWithID<BoxCollider>(ID);
                    bc.size = colliderBase.Size;
                    bc.center = colliderBase.Center;                                        
                    break;

                case ColliderType.CapsuleCollider:
                    CapsuleCollider cc = gameObject.FindComponentWithID<CapsuleCollider>(ID);
                    cc.radius = colliderBase.Radius;
                    cc.center = colliderBase.Center;
                    cc.direction = colliderBase.Direction;
                    cc.height = colliderBase.Height;                                        
                    break;

                case ColliderType.SphereCollider:
                    SphereCollider sc = gameObject.FindComponentWithID<SphereCollider>(ID);
                    sc.radius = colliderBase.Radius;
                    sc.center = colliderBase.Center;                                       
                    break;
            }            
        }

        public static void DrawBoxColliderBounds(this GameObject gameObject, ref List<GameObject> lineContainers, ColliderInfo colliderInfo)
        {            
            lineContainers.SetLineWidth(Mathf.Max(colliderInfo.Size.x, colliderInfo.Size.y, colliderInfo.Size.z));

            lineContainers.DrawBox(colliderInfo.Center, colliderInfo.Size);
        }

        public static void DrawSphereColliderBounds(this GameObject gameObject, ref List<GameObject> lineContainers, ColliderInfo colliderInfo)
        {
            lineContainers.SetLineWidth(colliderInfo.Radius);

            lineContainers[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            lineContainers[1].transform.localRotation = Quaternion.Euler(90f, 0, 0);
            lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 90f);

            foreach (GameObject lineContainer in lineContainers)
            {
                lineContainer.transform.localPosition = colliderInfo.Center;
                lineContainer.DrawCircle(colliderInfo.Radius, 360);
            }                  
        }        
        
        public static void DrawCapsuleColliderBounds(this GameObject gameObject, ref List<GameObject> lineContainers, ColliderInfo cInfo)
        {
            lineContainers.SetLineWidth(cInfo.Radius);            

            int angleX = 360;
            int angleY = 360;
            int angleZ = 360;
            
            for (int i = 0; i < lineContainers.Count; i++)
            {
                lineContainers[i].transform.localPosition = cInfo.Center;
            }
            
            if (cInfo.Radius * 2.0f < cInfo.Height)
            {
                for (int i = 3; i < 10; i++)
                {
                    lineContainers[i].EnableContainer();
                }

                float modify = (cInfo.Height * 0.5f) - cInfo.Radius;

                switch (cInfo.Direction)
                {
                    case 0:
                        angleY = 180;
                        angleZ = 180;

                        Vector3 axisXpositive = new Vector3(cInfo.Center.x + modify, cInfo.Center.y, cInfo.Center.z);
                        Vector3 axisXnegative = new Vector3(cInfo.Center.x - modify, cInfo.Center.y, cInfo.Center.z);

                        lineContainers[0].transform.localPosition = axisXpositive;
                        lineContainers[1].transform.localPosition = axisXpositive;
                        lineContainers[2].transform.localPosition = axisXpositive;

                        lineContainers[0].transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        lineContainers[1].transform.localRotation = Quaternion.Euler(-90f, -90f, 90f);
                        lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 0);

                        lineContainers[3].transform.localPosition = axisXnegative;
                        lineContainers[4].transform.localPosition = axisXnegative;
                        lineContainers[5].transform.localPosition = axisXnegative;

                        lineContainers[3].transform.localRotation = Quaternion.Euler(0, 0, 90f);
                        lineContainers[4].transform.localRotation = Quaternion.Euler(0, 180f, 0);
                        lineContainers[5].transform.localRotation = Quaternion.Euler(90f, 180f, 0);

                        float distance = Vector3.Distance(axisXpositive, axisXnegative);

                        lineContainers[6].DrawLine(new Vector3(distance * 0.5f, -cInfo.Radius, 0), new Vector3(distance * -0.5f, -cInfo.Radius, 0));
                        lineContainers[7].DrawLine(new Vector3(distance * 0.5f, cInfo.Radius, 0), new Vector3(distance * -0.5f, cInfo.Radius, 0));
                        lineContainers[8].DrawLine(new Vector3(distance * 0.5f, 0, -cInfo.Radius), new Vector3(distance * -0.5f, 0, -cInfo.Radius));
                        lineContainers[9].DrawLine(new Vector3(distance * 0.5f, 0, cInfo.Radius), new Vector3(distance * -0.5f, 0, cInfo.Radius));

                        break;

                    case 1:
                        angleX = 180;
                        angleZ = 180;

                        Vector3 axisYpositive = new Vector3(cInfo.Center.x, cInfo.Center.y + modify, cInfo.Center.z);
                        Vector3 axisYnegative = new Vector3(cInfo.Center.x, cInfo.Center.y - modify, cInfo.Center.z);

                        lineContainers[0].transform.localPosition = axisYpositive;
                        lineContainers[1].transform.localPosition = axisYpositive;
                        lineContainers[2].transform.localPosition = axisYpositive;

                        lineContainers[0].transform.localRotation = Quaternion.Euler(180f, -90f, -90f);
                        lineContainers[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 90);

                        lineContainers[3].transform.localPosition = axisYnegative;
                        lineContainers[4].transform.localPosition = axisYnegative;
                        lineContainers[5].transform.localPosition = axisYnegative;

                        lineContainers[3].transform.localRotation = Quaternion.Euler(180f, 90f, 90f);
                        lineContainers[4].transform.localRotation = Quaternion.Euler(0, 0, 0);
                        lineContainers[5].transform.localRotation = Quaternion.Euler(0, 0, -90);

                        distance = Vector3.Distance(axisYpositive, axisYnegative);

                        lineContainers[6].DrawLine(new Vector3(-cInfo.Radius, distance * 0.5f, 0), new Vector3(-cInfo.Radius, distance * -0.5f, 0));
                        lineContainers[7].DrawLine(new Vector3(cInfo.Radius, distance * 0.5f, 0), new Vector3(cInfo.Radius, distance * -0.5f, 0));
                        lineContainers[8].DrawLine(new Vector3(0, distance * 0.5f, -cInfo.Radius), new Vector3(0, distance * -0.5f, -cInfo.Radius));
                        lineContainers[9].DrawLine(new Vector3(0, distance * 0.5f, cInfo.Radius), new Vector3(0, distance * -0.5f, cInfo.Radius));

                        break;

                    case 2:
                        angleX = 180;
                        angleY = 180;

                        Vector3 axisZpositive = new Vector3(cInfo.Center.x, cInfo.Center.y, cInfo.Center.z + modify);
                        Vector3 axisZnegative = new Vector3(cInfo.Center.x, cInfo.Center.y, cInfo.Center.z - modify);

                        lineContainers[0].transform.localPosition = axisZpositive;
                        lineContainers[1].transform.localPosition = axisZpositive;
                        lineContainers[2].transform.localPosition = axisZpositive;

                        lineContainers[0].transform.localRotation = Quaternion.Euler(90f, 0, 90f);
                        lineContainers[1].transform.localRotation = Quaternion.Euler(0, 90f, 180f);
                        lineContainers[2].transform.localRotation = Quaternion.Euler(90f, 0, 0);

                        lineContainers[3].transform.localPosition = axisZnegative;
                        lineContainers[4].transform.localPosition = axisZnegative;
                        lineContainers[5].transform.localPosition = axisZnegative;

                        lineContainers[3].transform.localRotation = Quaternion.Euler(-90f, 0, 90f);
                        lineContainers[4].transform.localRotation = Quaternion.Euler(0, 90f, 0);
                        lineContainers[5].transform.localRotation = Quaternion.Euler(90f, 0, 180f);
                        
                        distance = Vector3.Distance(axisZpositive, axisZnegative);

                        lineContainers[6].DrawLine(new Vector3(-cInfo.Radius, 0, distance * 0.5f), new Vector3(-cInfo.Radius, 0, distance * -0.5f));
                        lineContainers[7].DrawLine(new Vector3(cInfo.Radius, 0, distance * 0.5f), new Vector3(cInfo.Radius, 0, distance * -0.5f));
                        lineContainers[8].DrawLine(new Vector3(0, -cInfo.Radius, distance * 0.5f), new Vector3(0, -cInfo.Radius, distance * -0.5f));
                        lineContainers[9].DrawLine(new Vector3(0, cInfo.Radius, distance * 0.5f), new Vector3(0, cInfo.Radius, distance * -0.5f));

                        break;
                }

                lineContainers[3].DrawCircle(cInfo.Radius, angleX);
                lineContainers[4].DrawCircle(cInfo.Radius, angleY);
                lineContainers[5].DrawCircle(cInfo.Radius, angleZ);
            }
            else
            {
                for (int i = 3; i < 10; i++)
                {
                    lineContainers[i].DisableContainer();
                }                

                lineContainers[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
                lineContainers[1].transform.localRotation = Quaternion.Euler(90f, 0, 0);
                lineContainers[2].transform.localRotation = Quaternion.Euler(0, 0, 90f);
            }

            lineContainers[0].DrawCircle(cInfo.Radius, angleX);
            lineContainers[1].DrawCircle(cInfo.Radius, angleY);
            lineContainers[2].DrawCircle(cInfo.Radius, angleZ);            
        }             
    }
}
