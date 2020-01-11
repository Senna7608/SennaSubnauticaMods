using Common;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Visuals
{
    public class DrawColliderControl : MonoBehaviour
    {
        public IDictionary<int, ColliderDraw> ColliderBases = new Dictionary<int, ColliderDraw>();        

        public GameObject baseGameObject;

        private GameObject pointer;

        private int childCount = 0;

        public void Awake()
        {
            baseGameObject = GetComponentInParent<DrawObjectBounds>().transform.parent.gameObject;
                       
            pointer = gameObject.CreatePointerLine(PointerType.Collider);           

            foreach (Collider collider in baseGameObject.GetComponents<Collider>())
            {
                int cInstanceID = collider.GetInstanceID();

                GameObject childContainer = new GameObject($"childContainer_{childCount}");

                childContainer.transform.SetParent(transform, false);
                childContainer.transform.localPosition = Vector3.zero;
                childContainer.transform.localRotation = Quaternion.identity;

                var dcb = childContainer.GetOrAddComponent<DrawColliderBounds>();

                dcb.cInstanceID = cInstanceID;                
                dcb.collider = collider;
                collider.SetColliderInfo(ref dcb.colliderInfo);   
                dcb.Init();

                ColliderBases.Add(cInstanceID, new ColliderDraw()
                {
                    Container = childContainer,
                    ColliderBase = new ColliderInfo(collider),
                    DCB = dcb                    
                });

                Main.Instance.OutputWindow_Log($"Collider drawing instance added to this gameobject: [{baseGameObject.name}], collider ID:[{cInstanceID}], Type: [{dcb.colliderInfo.ColliderType}]");

                childCount++;
            }
        }

        public void DrawSelectedCollider(int ID, bool draw)
        {
            foreach (ColliderDraw cd in ColliderBases.Values)
            {
                cd.Container.SetActive(false);
            }

            if (ColliderBases.Keys.Contains(ID))
            {
                ColliderBases[ID].Container.SetActive(draw);
                
                pointer.transform.position = transform.TransformPoint(ColliderBases[ID].DCB.colliderInfo.Center);
            }
        }

        public void DrawAllCollider(bool draw)
        {
            foreach (ColliderDraw cd in ColliderBases.Values)
            {
                cd.Container.SetActive(draw);
            }
        }

        public void Rescan()
        {
            foreach (Collider collider in baseGameObject.GetComponents<Collider>())
            {
                int cInstanceID = collider.GetInstanceID();

                if (!ColliderBases.Keys.Contains(cInstanceID))
                {
                    GameObject childContainer = new GameObject($"childContainer_{childCount}");

                    childContainer.transform.SetParent(transform, false);
                    childContainer.transform.localPosition = Vector3.zero;
                    childContainer.transform.localRotation = Quaternion.identity;

                    var dcb = childContainer.GetOrAddComponent<DrawColliderBounds>();

                    dcb.cInstanceID = cInstanceID;
                    dcb.collider = collider;
                    collider.SetColliderInfo(ref dcb.colliderInfo);
                    dcb.Init();

                    ColliderBases.Add(cInstanceID, new ColliderDraw()
                    {
                        Container = childContainer,
                        ColliderBase = new ColliderInfo(collider),
                        DCB = dcb
                    });

                    Main.Instance.OutputWindow_Log($"Collider drawing instance added to this [ID: {cInstanceID}] collider. Type: {dcb.colliderInfo.ColliderType}");

                    childCount++;
                }
            }
        }

        public void RemoveColliderDrawing(int ID)
        {
            GameObject childContainer = ColliderBases[ID].Container;
            DestroyImmediate(childContainer);
            ColliderBases.Remove(ID);
            childCount--;
        }
    }
}
