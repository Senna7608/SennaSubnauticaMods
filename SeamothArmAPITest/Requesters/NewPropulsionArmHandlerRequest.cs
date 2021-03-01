using UnityEngine;
using SeamothArmAPITest.Handlers;
using SeamothArms.API.Interfaces;

namespace SeamothArmAPITest.Requesters
{
    public class NewPropulsionArmHandlerRequest : ISeamothArmHandlerRequest
    {
        public ISeamothArmHandler GetHandler(ref GameObject arm)
        {
            return arm.AddComponent<NewPropulsionArmHandler>();
        }
    }    
}
