using System.Collections.Generic;
using UnityEngine;

namespace Common.RuntimeHelper
{
    public enum ColliderType
    {
        None,
        BoxCollider,
        CapsuleCollider,
        SphereCollider,
        MeshCollider,
        WheelCollider,
        TerrainCollider
    };

    public struct ColliderInfo
    {
        public ColliderInfo(string name, ColliderType type, List<Transform> transforms, Vector3 size, Vector3 center, float radius, float height, int direction) : this()
        {
            this.name = name;
            this.type = type;
            this.transforms = transforms;
            this.size = size;
            this.center = center;
            this.radius = radius;
            this.height = height;
            this.direction = direction;
        }

        public string name { get; set; }
        public ColliderType type { get; set; }        
        public List<Transform> transforms { get; set; }
        public Vector3 size { get; set; }
        public Vector3 center { get; set; }

        public float radius { get; set; }
        public float height { get; set; }
        public int direction { get; set; }
    }

    
}
