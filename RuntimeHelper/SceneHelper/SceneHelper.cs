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

        /*
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

            SearchDefaultObjects(ref transforms);
        }
        */

        public static void GetRootTransforms(GameObject ddolInclude, ref List<Transform> transforms)
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

            List<GameObject> ddolObjects = new List<GameObject>();

            ddolInclude.scene.GetRootGameObjects(ddolObjects);

            foreach (GameObject ddolObject in ddolObjects)
            {
                if (!transforms.Contains(ddolObject.transform))
                {
                    transforms.Add(ddolObject.transform);
                }
            }

            //SearchDefaultObjects(ref transforms);
        }

        private static void SearchDefaultObjects(ref List<Transform> transforms)
        {

            if (uGUI.main != null)
            {
                if (!transforms.Contains(uGUI.main.transform))
                    transforms.Add(uGUI.main.transform);
            }

            if (uGUI_PDA.main != null)
            {
                if (!transforms.Contains(uGUI_PDA.main.transform))
                    transforms.Add(uGUI_PDA.main.transform);
            }

            if (Player.main != null)
            {
                if (!transforms.Contains(Player.main.transform))
                    transforms.Add(Player.main.transform);
            }

            if (uGUI_PopupNotification.main != null)
            {
                if (!transforms.Contains(uGUI_PopupNotification.main.transform))
                    transforms.Add(uGUI_PopupNotification.main.transform);
            }

            if (NotificationManager.main != null)
            {
                if (!transforms.Contains(NotificationManager.main.transform))
                    transforms.Add(NotificationManager.main.transform);
            }

            if (Language.main != null)
            {
                if (!transforms.Contains(Language.main.transform))
                    transforms.Add(Language.main.transform);
            }

        }

    }
}
