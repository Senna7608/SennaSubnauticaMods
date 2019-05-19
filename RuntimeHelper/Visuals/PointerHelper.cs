using Common;
using UnityEngine;

namespace RuntimeHelper.Visuals
{
    public enum PointerType
    {
        Object,
        Collider
    }

    public static class PointerHelper
    {        
        public static float LineWidth = 0.004f;
        
        public static void CreatePointerLine(this GameObject containerBase, PointerType pointerType)
        {
            GameObject pointerLine = new GameObject("RH_POINTER_LINE");
            pointerLine.transform.SetParent(containerBase.transform, false);
            pointerLine.transform.localPosition = Vector3.zero;

            switch (pointerType)
            {
                case PointerType.Object:
                    pointerLine.AddLineRendererToContainer(LineWidth, Color.green, true);
                    break;
                case PointerType.Collider:                
                    pointerLine.AddLineRendererToContainer(LineWidth, Color.red, true);
                    break;
            }

            pointerLine.GetOrAddComponent<TracePlayerPos>().PointerType = pointerType;                       
        }
    }

    public class TracePlayerPos : MonoBehaviour
    {
        public PointerType PointerType;

        private void LateUpdate()
        {
            if (Player.main != null)
            {                
                Vector3 arrow = Player.main.playerArrowTransform.position;

                if (PointerType == PointerType.Object)
                {
                    Vector3 target = gameObject.transform.position;

                    float distance = Vector3.Distance(target, arrow);

                    if (distance > 10f)
                    {
                        HandReticle.main.SetInteractText($"{(int)distance} m", false, HandReticle.Hand.None);
                    }                    
                }

                gameObject.DrawLine(gameObject.transform.position, Player.main.playerArrowTransform.position);
            }
        }
    }
}
