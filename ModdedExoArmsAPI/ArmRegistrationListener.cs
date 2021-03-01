using System.Collections.Generic;
using UnityEngine;
using ModdedArmsHelper.API;
using Common;
using ModdedArmsHelper.API.Interfaces;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace ModdedArmsHelper
{
    [DisallowMultipleComponent]
    internal sealed class ArmRegistrationListener : MonoBehaviour
    {
        private void Awake()
        {
            SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(OnSceneLoaded);
            SNLogger.Log("Arm registration listener started.");            
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Main")
            {
                enabled = false;
                SNLogger.Log("Main scene loaded. Arm registration listener now shutdown.");
            }
        }

        private void Update()
        {
            if (Main.graphics != null && ArmServices.main.isWaitForRegistration)
            {
                RegisterNewArm();                
            }
        }

        private void RegisterNewArm()
        {
            foreach (KeyValuePair<CraftableModdedArm, IArmModdingRequest> kvp in ArmServices.main.waitForRegistration)
            {
                if (Main.graphics.ModdedArmPrefabs.ContainsKey(kvp.Key.TechType))
                {                    
                    continue;
                }

                Main.graphics.RegisterNewArm(kvp.Key, kvp.Value);                
            }

            ArmServices.main.isWaitForRegistration = false;
        }
    }
}
