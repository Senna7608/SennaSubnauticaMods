using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Common
{
    public static class RefHelp
    {
        public static object GetPrivateField<T>(this T instance, string fieldName, BindingFlags bindingFlags = BindingFlags.Default)
        {
            return typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | bindingFlags).GetValue(instance);
        }

        public static void SetPrivateField<T>(this T instance, string fieldName, object value, BindingFlags bindingFlags = BindingFlags.Default)
        {
            typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | bindingFlags).SetValue(instance, value);
        }

        public static void InvokePrivateMethod<T>(this T instance, string methodName, BindingFlags bindingFlags = BindingFlags.Default, params object[] parms)
        {
            typeof(T).GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | bindingFlags).Invoke(instance, parms);
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

        public static Type SafeGetTypeFromAssembly(string assemblyName, string typeName)
        {
            try   { return Assembly.Load(assemblyName)?.GetType(typeName, false); }
            catch { return null; }
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
    }
}
