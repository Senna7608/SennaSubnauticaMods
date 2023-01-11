using UnityEngine;

namespace RuntimeHelper
{
    public partial class RuntimeHelperManager
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

                HandReticle.main.SetText(HandReticle.TextType.Hand, $"Target: {target.name}\nTechType: {techType}\ndistance: {distance:F1} m", false);

                HandReticle.main.SetText(HandReticle.TextType.HandSubscript, handSubScript, false, GameInput.Button.RightHand);

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
