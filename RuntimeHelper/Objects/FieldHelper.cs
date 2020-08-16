using Common.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RuntimeHelper.Objects
{
    public class FieldHelper
    {
        public class ObjectField
        {
            public object Instance;
            public FieldInfo fieldInfo;

            public string Name
            {
                get
                {
                    return fieldInfo.Name;
                }
            }

            public Type FieldType
            {
                get
                {
                    return fieldInfo.FieldType;
                }
            }

            public override string ToString()
            {
                return $"{Name} = {GetValue()}";
            }

            private readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            public ObjectField(object instance, FieldInfo fInfo)
            {
                Instance = instance;
                fieldInfo = fInfo;
            }

            public object GetField()
            {
                return Instance.GetPrivateField(Name, bindingFlags);
            }

            public object GetValue()
            {
                try
                {
                    return fieldInfo.GetValue(Instance);
                }
                catch
                {
                    return null;
                }
            }

            public bool SetValue(object value)
            {
                try
                {
                    fieldInfo.SetValue(Instance, value, bindingFlags, null, null);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        public class ObjectFields : IEnumerable<ObjectField>
        {
            private List<ObjectField> fObjects;

            public ObjectFields(object _object)
            {
                fObjects = new List<ObjectField>();

                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

                foreach (FieldInfo fieldInfo in _object.GetType().GetFields(bindingFlags))
                {
                    fObjects.Add(new ObjectField(_object, fieldInfo));
                }
            }

            public IEnumerator<ObjectField> GetEnumerator()
            {
                return ((IEnumerable<ObjectField>)fObjects).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<ObjectField>)fObjects).GetEnumerator();
            }

            public object GetFieldValue(string name)
            {
                foreach (ObjectField fObject in fObjects)
                {
                    if (fObject.Name == name)
                    {
                        return fObject.GetValue();
                    }
                }

                return null;
            }

            public bool SetFieldValue(string name, object value)
            {
                foreach (ObjectField fObject in fObjects)
                {
                    if (fObject.Name == name)
                    {
                        return fObject.SetValue(value);
                    }
                }

                return false;
            }

            public object GetField(string fieldName)
            {
                foreach (ObjectField fObject in fObjects)
                {
                    if (fObject.Name == fieldName)
                    {
                        return fObject.GetField();
                    }
                }

                return null;
            }
        }
    }
}
