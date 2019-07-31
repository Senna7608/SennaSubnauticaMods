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

        public static List<string> CreateComponentInfoList(this Component component)
        {
            List<string> keywords = new List<string>();

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            
            keywords.Add("Properties:");

            foreach (PropertyInfo propertyInfo in component.GetType().GetProperties(bindingFlags))
            {
                try
                {
                    keywords.Add($"{propertyInfo.Name} = [{propertyInfo.GetValue(component, bindingFlags, null, null, null).ToString()}]");
                }
                catch
                {
                    continue;
                }
            }

            keywords.Add("Fields:");

            foreach (FieldInfo fieldInfo in component.GetType().GetFields(bindingFlags))
            {
                try
                {
                    keywords.Add($"{fieldInfo.Name} = [{fieldInfo.GetValue(component).ToString()}]");
                }
                catch
                {
                    continue;
                }
            }

            return keywords;
        }










    }


    
}
