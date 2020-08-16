using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

namespace Common.Helpers
{
    public static class ReflectionHelper
    {
        public static object GetPrivateField<T>(this T instance, string fieldName, BindingFlags bindingFlags = BindingFlags.Default)
        {
            return instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | bindingFlags).GetValue(instance);
        }

        public static object GetPrivateProperty<T>(this T instance, string propertyName, BindingFlags bindingFlags = BindingFlags.Default)
        {
            return instance.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | bindingFlags).GetValue(instance, null);
        }

        public static object GetPublicField<T>(this T instance, string fieldName, BindingFlags bindingFlags = BindingFlags.Default)
        {
            return instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | bindingFlags).GetValue(instance);
        }

        public static void SetPrivateField<T>(this T instance, string fieldName, object value, BindingFlags bindingFlags = BindingFlags.Default)
        {
            instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField | bindingFlags).SetValue(instance, value);
        }

        public static void InvokePrivateMethod<T>(this T instance, string methodName, BindingFlags bindingFlags = BindingFlags.Default, params object[] parms)
        {
            instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | bindingFlags).Invoke(instance, parms);
        }

        public static void CloneFieldsInto<T>(this T original, T copy)
        {
            FieldInfo[] fieldsInfo = typeof(T).GetFields(BindingFlags.Instance);

            foreach (FieldInfo fieldInfo in fieldsInfo)
            {
                if (fieldInfo.GetType().IsClass)
                {
                    var origValue = fieldInfo.GetValue(original);
                    var copyValue = fieldInfo.GetValue(copy);

                    origValue.CloneFieldsInto(copyValue);                    
                }
                else
                {
                    var value = fieldInfo.GetValue(original);
                    fieldInfo.SetValue(copy, value);
                }                
            }
        }

        public static bool IsNamespaceExists(string desiredNamespace)
        {
            try
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Type[] types = assemblies[i].GetTypes();

                    for (int j = 0; j < types.Length; j++)
                    {
                        if (types[j].Namespace == desiredNamespace)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static MethodInfo GetAssemblyClassPrivateMethod(string className, string methodName)
        {
            try
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    Type[] types = assemblies[i].GetTypes();

                    for (int j = 0; j < types.Length; j++)
                    {
                        if (types[j].FullName == className)
                        {
                            return types[j].GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static object GetAssemblyClassPublicField(string className, string fieldName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();

                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].FullName == className)
                    {
                        return types[j].GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetValue(types[j]);
                    }
                }
            }
            return null;
        }

        public static void GetPrefabInfo(this GameObject prefab)
        {
            List<string> keywords = new List<string>();

            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            keywords.Add($"prefab name: {prefab.name}");
            keywords.Add("Properties:");

            foreach (PropertyInfo propertyInfo in prefab.GetType().GetProperties(bindingFlags))
            {
                try
                {
                    keywords.Add($"{propertyInfo.Name} = [{propertyInfo.GetValue(prefab, bindingFlags, null, null, null).ToString()}]");
                }
                catch
                {
                    continue;
                }
            }

            keywords.Add("Fields:");

            foreach (FieldInfo fieldInfo in prefab.GetType().GetFields(bindingFlags))
            {
                try
                {
                    keywords.Add($"{fieldInfo.Name} = [{fieldInfo.GetValue(prefab).ToString()}]");
                }
                catch
                {
                    continue;
                }

                if (fieldInfo.GetValue(prefab).GetType() == typeof(GameObject))
                {
                    GameObject go = (GameObject)fieldInfo.GetValue(prefab);
                    go.GetPrefabInfo();
                }
            }

            foreach (string keyword in keywords)
            {
                SNLogger.Log(keyword);
            }
        }


        public static MethodBase GetConstructorMethodBase(Type type, string ctorName)
        {
            List<ConstructorInfo> ctor_Infos = new List<ConstructorInfo>();

            ctor_Infos = AccessTools.GetDeclaredConstructors(type);

            foreach (ConstructorInfo ctor_info in ctor_Infos)
            {
                GetConstructorInfo(ctor_info);

                if (ctor_info.Name == ctorName)
                {
                    return ctor_info as MethodBase;
                }
            }            

            return null;
        }

        [Conditional("DEBUG")]
        public static void GetConstructorInfo(ConstructorInfo constructorInfo)
        {
            ParameterInfo[] pInfos = constructorInfo.GetParameters();

            if (pInfos.Length == 0)
            {
                SNLogger.Debug("", $"this constructor [{constructorInfo.Name}] has no parameters.");
            }
            else
            {
                SNLogger.Debug("", $"listing constructor parameters...");

                foreach (ParameterInfo pInfo in pInfos)
                {
                    SNLogger.Debug("", $"ctor parameter[{pInfo.Position}] = [{pInfo.ToString()}]");
                }
            }
        }
        
    }
}
