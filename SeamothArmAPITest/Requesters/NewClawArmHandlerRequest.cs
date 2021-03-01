using UnityEngine;
using SeamothArmAPITest.Handlers;
using SeamothArms.API.Interfaces;

namespace SeamothArmAPITest.Requesters
{
    public class NewClawArmHandlerRequest : ISeamothArmHandlerRequest
    {
        public ISeamothArmHandler GetHandler(ref GameObject arm)
        {
            return arm.AddComponent<NewClawArmHandler>();
        }
    }    
}
