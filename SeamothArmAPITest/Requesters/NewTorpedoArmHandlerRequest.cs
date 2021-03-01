using UnityEngine;
using SeamothArmAPITest.Handlers;
using SeamothArms.API.Interfaces;

namespace SeamothArmAPITest.Requesters
{
    public class NewTorpedoArmHandlerRequest : ISeamothArmHandlerRequest
    {
        public ISeamothArmHandler GetHandler(ref GameObject arm)
        {
            return arm.AddComponent<NewTorpedoArmHandler>();
        }
    }
}
