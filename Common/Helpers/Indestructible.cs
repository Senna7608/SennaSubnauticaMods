using UnityEngine;
#pragma warning disable CS1591

namespace Common.Helpers
{
    public class Indestructible : MonoBehaviour
    {
        public void Awake()
        {
            gameObject.AddComponent<SceneCleanerPreserve>();
            DontDestroyOnLoad(gameObject);
            SNLogger.Log("Indestructible component added.");
        }
    }
}
