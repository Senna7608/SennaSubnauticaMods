using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelper
    {
        internal bool isRayEnabled = false;

        private const string handSubScript = "Press right mouse button\nto connect this gameobject.";

        internal void RaycastMode_Update()
        {
            HandReticle.main.SetIcon(HandReticle.IconType.Scan);

            //Transform aimTransform = Player.main.camRoot.GetAimingTransform();

            //if (Physics.Raycast(aimTransform.position, aimTransform.forward, out RaycastHit hitInfo, 15, 3))
            if (Targeting.GetTarget(Player.main.gameObject, 10f, out GameObject target, out float distance))
            {
                TechType techType = CraftData.GetTechType(target);

                // HandReticle.main.SetInteractText(hitInfo.transform.name, "Press right mouse button\nto connect this gameobject.", false, false, HandReticle.Hand.Right);
                HandReticle.main.SetInteractText($"Target: {target.name}\nTechType: {techType}\ndistance: {distance:F1} m", handSubScript, false, false, HandReticle.Hand.Right);
                

                if (Input.GetMouseButton(1))
                {
                    //OnBaseObjectChange(hitInfo.transform.gameObject);
                    OnBaseObjectChange(target);
                    isRayEnabled = false;
                    OutputWindow_Log(MESSAGE_TEXT[MESSAGES.RAYCAST_STATE], isRayEnabled);
                }
            }
        }        
    }
}
