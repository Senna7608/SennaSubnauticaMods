using UnityEngine;

namespace Common
{
    public static class InputHelper
    {
        public static string GetKeyCodeAsInputName(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Mouse0:
                    return "MouseButtonLeft";
                case KeyCode.Mouse1:
                    return "MouseButtonRight";
                case KeyCode.Mouse2:
                    return "MouseButtonMiddle";

                case KeyCode.Alpha0:
                    return "0";
                case KeyCode.Alpha1:
                    return "1";
                case KeyCode.Alpha2:
                    return "2";
                case KeyCode.Alpha3:
                    return "3";
                case KeyCode.Alpha4:
                    return "4";
                case KeyCode.Alpha5:
                    return "5";
                case KeyCode.Alpha6:
                    return "6";
                case KeyCode.Alpha7:
                    return "7";
                case KeyCode.Alpha8:
                    return "8";
                case KeyCode.Alpha9:
                    return "9";                    

                case KeyCode.JoystickButton0:
                    return "ControllerButtonA";
                case KeyCode.JoystickButton1:
                    return "ControllerButtonB";
                case KeyCode.JoystickButton2:
                    return "ControllerButtonX";
                case KeyCode.JoystickButton3:
                    return "ControllerButtonY";
                case KeyCode.JoystickButton4:
                    return "ControllerButtonLeftBumper";
                case KeyCode.JoystickButton5:
                    return "ControllerButtonRightBumper";
                case KeyCode.JoystickButton6:
                    return "ControllerButtonBack";
                case KeyCode.JoystickButton7:
                    return "ControllerButtonHome";
                case KeyCode.JoystickButton8:
                    return "ControllerButtonLeftStick";
                case KeyCode.JoystickButton9:
                    return "ControllerButtonRightStick";
                default:
                    return keyCode.ToString();
            }
        }


    }
}
