using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuntimeHelper
{
    public struct SceneInfo
    {
        public SceneInfo(Scene scene) : this()
        {
            Scene = scene;
            RootObjects = new List<GameObject>();
        }

        public Scene Scene { get; set; }
        public List<GameObject> RootObjects { get; set; }
    }
    
    public struct TransformInfo
    {
        public TransformInfo(Transform transform) : this()
        {
            Position = transform.position;
            Rotation = transform.rotation;
            LocalPosition = transform.localPosition;
            LocalRotation = transform.localRotation;
            Scale = transform.localScale;
        }

        public TransformInfo(Vector3 position, Quaternion rotation, Vector3 localPosition, Quaternion localRotation, Vector3 scale) : this()
        {
            Position = position;
            Rotation = rotation;
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            Scale = scale;
        }

        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 LocalPosition { get; set; }
        public Quaternion LocalRotation { get; set; }        
        public Vector3 Scale { get; set; }        
    }

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
        public ColliderInfo(ColliderType colliderType, Vector3 size, Vector3 center, float radius, float height, int direction) : this()
        {            
            ColliderType = colliderType;            
            Size = size;
            Center = center;
            Radius = radius;
            Height = height;
            Direction = direction;
        }

        public ColliderInfo(ColliderInfo colliderInfo) : this()
        {
            ColliderType = colliderInfo.ColliderType;
            Size = colliderInfo.Size;
            Center = colliderInfo.Center;
            Radius = colliderInfo.Radius;
            Height = colliderInfo.Height;
            Direction = colliderInfo.Direction;
        }

        public ColliderType ColliderType { get; set; }        
        public Vector3 Size { get; set; }
        public Vector3 Center { get; set; }

        public float Radius { get; set; }
        public float Height { get; set; }
        public int Direction { get; set; }
    }

    public struct MaterialInfo
    {
        public MaterialInfo(Material material, int index) : this()
        {
            Material = material;
            Index = index;
            ActiveShaders = new List<ShaderInfo>();
            ShaderKeywords = new List<string>();
        }

        public int Index { get; set; }
        public Material Material { get; set; }
        public List<ShaderInfo> ActiveShaders{ get; set; }
        public List<string> ShaderKeywords { get; set; }
    };

   public struct ShaderInfo
   {
        public ShaderInfo(string name, int index, string propertyKeyword, string textureName) : this()
        {
            Name = name;
            Index = index;
            PropertyKeyword = propertyKeyword;
            TextureName = textureName;
        }

        public string Name { get; set; }
        public int Index { get; set; }
        public string PropertyKeyword { get; set; }
        public string TextureName { get; set; }
   };













}
