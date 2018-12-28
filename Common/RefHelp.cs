namespace Common
{
    using System;
    using System.Reflection;

    public static class RefHelp
    {
        public static object GetPrivateField<T>(this T instance, string fieldName, BindingFlags bindingFlags = BindingFlags.Default)
            => typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | bindingFlags).GetValue(instance);

        public static void SetPrivateField<T>(this T instance, string fieldName, object value, BindingFlags bindingFlags = BindingFlags.Default)
            => typeof(T).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | bindingFlags).SetValue(instance, value);

        public static void InvokePrivateMethod<T>(this T instance, string methodName, BindingFlags bindingFlags = BindingFlags.Default, params object[] parms)
            => typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | bindingFlags).Invoke(instance, parms);

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
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Namespace == desiredNamespace)
                        return true;
                }
            }
            return false;
        }
    }
}
