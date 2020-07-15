using UnityEngine;

namespace Common
{
    public class Indestructible : MonoBehaviour
    {
        public void Awake()
        {
            gameObject.AddComponent<SceneCleanerPreserve>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
