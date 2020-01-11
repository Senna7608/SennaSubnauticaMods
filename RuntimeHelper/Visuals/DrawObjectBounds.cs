using Common;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeHelper.Visuals
{
    public class DrawObjectBounds : MonoBehaviour
    {        
        public TransformInfo transformBase;

        private List<GameObject> lineContainers = new List<GameObject>();

        private void Awake()
        {
            Transform parent = transform.parent;
           
            transformBase = new TransformInfo(parent);
            
            if (parent.GetType().Equals(typeof(RectTransform)))
            {                
                gameObject.CreateLineContainers(ref lineContainers, ContainerType.Rectangle, 0.008f, Color.green, false);

                lineContainers.DrawRectangle((RectTransform)parent);
            }
            else
            {                
                gameObject.CreateLineContainers(ref lineContainers, ContainerType.Box, 0.008f, Color.green, false);

                Vector3 newSize = LineHelper.CompensateSizefromScale(new Vector3(0.6f, 0.6f, 0.6f), transform.parent.localScale);

                lineContainers.DrawBox(Vector3.zero, newSize);
            }

            gameObject.CreatePointerLine(PointerType.Object);

            Main.AllVisuals.Add(gameObject);

            if (gameObject.GetComponentInParent<Collider>() != null)
            {
                GameObject colliderContainerBase = gameObject.GetOrAddVisualBase(BaseType.Collider);
                colliderContainerBase.GetOrAddComponent<DrawColliderControl>();                
            }

        }

        public void IsDraw(bool value)
        {
            gameObject.SetActive(value);

        }        
    }
}
