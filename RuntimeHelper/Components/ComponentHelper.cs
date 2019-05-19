using UnityEngine;

namespace RuntimeHelper.Components
{
    public static class ComponentHelper
    {
        public static T FindComponentWithID<T>(this GameObject gameObject, int ID) where T : Component
        {
            foreach (T component in gameObject.GetComponents<T>())
            {
                if (component.GetInstanceID() == ID)
                {
                    return component;
                }
            }

            return null;
        }












    }


    
}
