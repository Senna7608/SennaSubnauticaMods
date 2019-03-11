using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RuntimeHelper
{
    internal sealed class EditModeStrings
    {
        internal static readonly string[] DIRECTION = new string[3]
        {
            "X",
            "Y",
            "Z"
        };        

        internal static readonly string[] OBJECT_MODES = new string[8]
        {
            "TRANSFORM",
            "ROTATION: x",
            "ROTATION: y",
            "ROTATION: z",
            "POSITION: x",
            "POSITION: y",
            "POSITION: z",
            "SCALE"
        };

        internal static readonly string[] BOXCOLLIDER_MODES = new string[6]
        {
            "COLLIDER SIZE: x",
            "COLLIDER SIZE: y",
            "COLLIDER SIZE: z",
            "COLLIDER CENTER: x",
            "COLLIDER CENTER: y",
            "COLLIDER CENTER: z"
        };

        internal static readonly string[] CAPSULECOLLIDER_MODES = new string[6]
        {
            "COLLIDER RADIUS",
            "COLLIDER HEIGHT",
            "COLLIDER DIRECTION",
            "COLLIDER CENTER: x",
            "COLLIDER CENTER: y",
            "COLLIDER CENTER: z"
        };

        internal static readonly string[] SPHERECOLLIDER_MODES = new string[4]
        {
            "COLLIDER RADIUS",            
            "COLLIDER CENTER: x",
            "COLLIDER CENTER: y",
            "COLLIDER CENTER: z"
        };
    }
}
