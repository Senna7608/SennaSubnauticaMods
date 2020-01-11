using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        internal bool isRayEnabled = false;
        
        internal void RaycastMode_Update()
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Scan);

            Transform aimTransform = Player.main.camRoot.GetAimingTransform();
            
            if (Physics.Raycast(aimTransform.position, aimTransform.forward, out RaycastHit hitInfo, 15, 3))
            {
                HandReticle.main.SetInteractText(hitInfo.transform.name, "Press right mouse button\nto connect this gameobject.", false, false, HandReticle.Hand.Right);
                
                if (Input.GetMouseButton(1))
                {
                    OnBaseObjectChange(hitInfo.transform.gameObject);
                    isRayEnabled = false;
                    OutputWindow_Log(MESSAGE_TEXT[MESSAGES.RAYCAST_STATE], isRayEnabled);
                }
            }
        }        
    }
}
