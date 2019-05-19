using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RuntimeHelper.SceneHelper
{
    public static class SceneHelper
    { 
        private static List<SceneInfo> InitScenesList()
        {
            List<SceneInfo> gameScenes = new List<SceneInfo>();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                gameScenes.Add(new SceneInfo(SceneManager.GetSceneAt(i)));
            }

            return gameScenes;
        }

        private static void InitRootObjectsList(ref List<SceneInfo> gameScenes)
        {
            foreach (SceneInfo sceneInfo in gameScenes)
            {
                sceneInfo.Scene.GetRootGameObjects(sceneInfo.RootObjects);
            }
        }

        public static void GetRootTransforms(ref List<Transform> transforms)
        {
            List<SceneInfo> gameScenes = InitScenesList();

            InitRootObjectsList(ref gameScenes);            

            foreach (SceneInfo sceneInfo in gameScenes)
            {
                foreach (GameObject rootObject in sceneInfo.RootObjects)
                {
                    transforms.Add(rootObject.transform);
                }
            }
        }
    }
}
