using System.Collections.Generic;

namespace RuntimeHelper
{
    public static class EditModeStrings
    {
        public static IDictionary<ColliderType, string[]> COLLIDER_MODES = new Dictionary<ColliderType, string[]>();

        public static void Init()
        {
            COLLIDER_MODES.Clear();
            COLLIDER_MODES.Add(ColliderType.BoxCollider, BOXCOLLIDER_MODES);
            COLLIDER_MODES.Add(ColliderType.SphereCollider, SPHERECOLLIDER_MODES);
            COLLIDER_MODES.Add(ColliderType.CapsuleCollider, CAPSULECOLLIDER_MODES);
        }

        public static readonly string[] DIRECTION = new string[3]
        {
            "X",
            "Y",
            "Z"
        };

        public static readonly string[] OBJECT_MODES = new string[10]
        {            
            "Rotation: x",
            "Rotation: y",
            "Rotation: z",
            "Position: x",
            "Position: y",
            "Position: z",
            "Scale: x,y,z",
            "Scale: x",
            "Scale: y",
            "Scale: z",
        };        

        public static readonly string[] BOXCOLLIDER_MODES = new string[6]
        {
            "Collider Size: x",
            "Collider Size: y",
            "Collider Size: z",
            "Collider Center: x",
            "Collider Center: y",
            "Collider Center: z"
        };

        public static readonly string[] CAPSULECOLLIDER_MODES = new string[6]
        {
            "Collider Radius",
            "Collider Height",
            "Collider Direction",
            "Collider Center: x",
            "Collider Center: y",
            "Collider Center: z"
        };

        public static readonly string[] SPHERECOLLIDER_MODES = new string[4]
        {
            "Collider Radius",
            "Collider Center: x",
            "Collider Center: y",
            "Collider Center: z"
        };

        public static readonly string[] RECTTRANSFORM_MODES = new string[17]
        {
            "Position: x",
            "Position: y",
            "Width",
            "Height",
            "Scale: x,y",
            "Scale: x",
            "Scale: y",
            "Pivot: x",
            "Pivot: y",
            "AnchorMin: x",
            "AnchorMin: y",
            "AnchorMax: x",
            "AnchorMax: y",
            "OffsetMin: x",
            "OffsetMin: y",
            "OffsetMax: x",
            "OffsetMax: y",
        };
    }
}
