using RuntimeHelper.Components;
using RuntimeHelper.Visuals;
using System.Collections.Generic;
using System.Reflection;
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
            Parent = transform.parent ?? null;
            Position = transform.position;
            Rotation = transform.rotation;
            LocalPosition = transform.localPosition;
            LocalRotation = transform.localRotation;
            Scale = transform.localScale;
        }

        public TransformInfo(Transform transform, Vector3 position, Quaternion rotation, Vector3 localPosition, Quaternion localRotation, Vector3 scale) : this()
        {
            Parent = transform.parent ?? null;
            Position = position;
            Rotation = rotation;
            LocalPosition = localPosition;
            LocalRotation = localRotation;
            Scale = scale;
        }

        public Transform Parent { get; set; }
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

        public ColliderInfo(Collider collider) : this()
        {
            switch (collider.GetColliderType())
            {
                case ColliderType.BoxCollider:
                    BoxCollider bc = (BoxCollider)collider;
                    ColliderType = ColliderType.BoxCollider;
                    Size = bc.size;
                    Center = bc.center;
                    break;

                case ColliderType.CapsuleCollider:
                    CapsuleCollider cc = (CapsuleCollider)collider;
                    ColliderType = ColliderType.CapsuleCollider;
                    Radius = cc.radius;
                    Center = cc.center;
                    Direction = cc.direction;
                    Height = cc.height;
                    break;

                case ColliderType.SphereCollider:
                    SphereCollider sc = (SphereCollider)collider;
                    ColliderType = ColliderType.SphereCollider;
                    Radius = sc.radius;
                    Center = sc.center;
                    break;
            }
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
        public ShaderInfo(string name, int index, Shader shader, string keyword, string keywordValue) : this()
        {
            Shader = shader;
            Name = name;
            Index = index;
            Keyword = keyword;
            KeywordValue = keywordValue;
        }

        public string Name { get; set; }
        public int Index { get; set; }
        public Shader Shader { get; set; }
        public string Keyword { get; set; }
        public string KeywordValue { get; set; }
   };


    public struct ColliderDraw
    {
        public ColliderDraw(ColliderInfo colliderBase, GameObject container, DrawColliderBounds dcb) : this()
        {
            ColliderBase = colliderBase;
            Container = container;
            DCB = dcb;
        }

        public ColliderInfo ColliderBase { get; set; }
        public GameObject Container { get; set; }
        public DrawColliderBounds DCB { get; set; }
    }


    public struct ComponentInfo
    {
        public ComponentInfo(Component component) : this()
        {
            _Component = component;
            _ID = _Component.GetInstanceID();
            _PropertyInfo = _Component.GetComponentPropertiesList();
            _FieldInfo = _Component.GetComponentFieldsList();
        }

        public Component _Component { get; private set; }

        public int _ID { get; private set; }

        public List<PropertyInfo> _PropertyInfo { get; private set; }

        public List<FieldInfo> _FieldInfo { get; private set; }
    }








}
