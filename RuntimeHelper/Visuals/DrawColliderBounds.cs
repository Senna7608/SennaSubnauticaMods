using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Visuals
{
    public class DrawColliderBounds : MonoBehaviour
    {
        public int cInstanceID;        
        public Collider collider;        
        public ColliderInfo colliderInfo;        
        private List<GameObject> lineContainers = new List<GameObject>();

        public void Init()
        {
            if (lineContainers.Count > 0)
            {
                lineContainers.DestroyContainers();
            }

            switch (colliderInfo.ColliderType)
            {
                case ColliderType.BoxCollider:
                    gameObject.CreateLineContainers(ref lineContainers, ContainerType.Box, 0.008f, Color.red, false);
                    break;
                case ColliderType.CapsuleCollider:
                    gameObject.CreateLineContainers(ref lineContainers, ContainerType.Capsule, 0.004f, Color.red, false);
                    break;
                case ColliderType.SphereCollider:
                    gameObject.CreateLineContainers(ref lineContainers, ContainerType.Sphere, 0.004f, Color.red, false);
                    break;
            }
        }

        public void LateUpdate()
        {
            if (lineContainers.Count == 0)
                return;

                collider.SetColliderInfo(ref colliderInfo);
            
            switch (colliderInfo.ColliderType)
            {
                case ColliderType.BoxCollider:
                    gameObject.DrawBoxColliderBounds(ref lineContainers, colliderInfo);
                    break;
                case ColliderType.CapsuleCollider:
                    gameObject.DrawCapsuleColliderBounds(ref lineContainers, colliderInfo);
                    break;
                case ColliderType.SphereCollider:
                    gameObject.DrawSphereColliderBounds(ref lineContainers, colliderInfo);
                    break;
            }            
        }        
    }
}
