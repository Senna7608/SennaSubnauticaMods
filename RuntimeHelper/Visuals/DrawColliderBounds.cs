using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Visuals
{
    public class DrawColliderBounds : MonoBehaviour
    {        
        public TransformInfo transformBase;
        public ColliderInfo colliderBase;
        public int cInstanceID;
        public IDictionary<int, ColliderInfo> ColliderBases = new Dictionary<int, ColliderInfo>();

        private List<GameObject> lineContainers = new List<GameObject>();
        private GameObject pointer;

        private void Awake()
        {            
            transformBase = new TransformInfo(transform.parent);

            gameObject.CreatePointerLine(PointerType.Collider);

            pointer = gameObject.FindChild("RH_POINTER_LINE");

            Main.AllVisuals.Add(gameObject);
        }

        public void IsDraw(bool value)
        {
            gameObject.SetActive(value);

            if (!value)
                return;

            switch (colliderBase.ColliderType)
            {
                case ColliderType.BoxCollider:
                    gameObject.DrawBoxColliderBounds(ref lineContainers, colliderBase);
                    break;
                case ColliderType.CapsuleCollider:
                    gameObject.DrawCapsuleColliderBounds(ref lineContainers, colliderBase);
                    break;
                case ColliderType.SphereCollider:
                    gameObject.DrawSphereColliderBounds(ref lineContainers, colliderBase);
                    break;
            }

            pointer.transform.position = transform.TransformPoint(colliderBase.Center);
        }

        public void SetColliderBase(Collider collider)
        {
            cInstanceID = collider.GetInstanceID();

            collider.SetColliderInfo(ref colliderBase);

            if (!ColliderBases.Keys.Contains(cInstanceID))
            {               
                ColliderBases.Add(cInstanceID, new ColliderInfo(colliderBase));
                Main.Instance.OutputWindow_Log($"Collider instance: [{cInstanceID}] added to dictionary. Type: {colliderBase.ColliderType.ToString()}");
            }         

            lineContainers.DestroyContainers();

            switch (colliderBase.ColliderType)
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
        
    }
}
