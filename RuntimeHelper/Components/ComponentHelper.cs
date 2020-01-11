using Common;
using System.Collections.Generic;
using System.Reflection;
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

        public static List<FieldInfo> GetComponentFieldsList(this Component component)
        {
            List<FieldInfo> fieldInfos = new List<FieldInfo>();

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
                        
            foreach (FieldInfo fieldInfo in component.GetType().GetFields(bindingFlags))
            {
                try
                {
                    fieldInfos.Add(fieldInfo);
                }
                catch
                {
                    continue;
                }                
            }

            return fieldInfos;
        }


        public static List<PropertyInfo> GetComponentPropertiesList(this Component component)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;            

            foreach (PropertyInfo propertyInfo in component.GetType().GetProperties(bindingFlags))
            {
                try
                {
                    properties.Add(propertyInfo);
                }
                catch
                {
                    continue;
                }
            }           

            return properties;
        }







    }


    
}
