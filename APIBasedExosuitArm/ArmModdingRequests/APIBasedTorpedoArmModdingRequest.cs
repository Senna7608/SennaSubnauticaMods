﻿using UnityEngine;
using ModdedArmsHelper.API;
using ModdedArmsHelper.API.Interfaces;
using APIBasedExosuitArms.Handlers;
using System.Collections;

namespace APIBasedExosuitArms.ArmModdingRequest
{
    public class APIBasedTorpedoArmModdingRequest : IArmModdingRequest
    {
        public IExosuitArm GetExosuitArmHandler(GameObject clonedArm)
        {
            return clonedArm.AddComponent<APIBasedTorpedoArmHandler>();
        }
       
        public ISeamothArm GetSeamothArmHandler(GameObject clonedArm)
        {
            return null;
        }

        public IEnumerator SetUpArmAsync(GameObject clonedArm, LowerArmHelper graphicsHelper, IOut<bool> success)
        {
            success.Set(true);
            yield break;
        }
    }    
}
