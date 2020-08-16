using UnityEngine;

namespace Common.Helpers
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
