using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace RuntimeHelper.Objects
{
    public class PropertyHelper
    {
        public class ObjectProperty
        {
            public object Instance;
            public PropertyInfo propertyInfo;

            public string Name
            {
                get
                {
                    return propertyInfo.Name;
                }
            }

            public Type @Type
            {
                get
                {
                    return propertyInfo.PropertyType;
                }
            }

            public override string ToString()
            {
                return $"{Name} = {GetValue()}";
            }

            private readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            public ObjectProperty(object instance, PropertyInfo pInfo)
            {
                Instance = instance;
                propertyInfo = pInfo;
            }

            public ObjectProperty GetProperty()
            {
                return this;
            }


            public object GetValue()
            {
                try
                {
                    return propertyInfo.GetValue(Instance, bindingFlags, null, null, null);
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
                    propertyInfo.SetValue(Instance, value, bindingFlags, null, null, null);
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        public class ObjectProperties : IEnumerable<ObjectProperty>
        {
            public List<ObjectProperty> pObjects;

            public ObjectProperties(object _object)
            {
                pObjects = new List<ObjectProperty>();

                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

                foreach (PropertyInfo propertyInfo in _object.GetType().GetProperties(bindingFlags))
                {
                    pObjects.Add(new ObjectProperty(_object, propertyInfo));
                }
            }

            public IEnumerator<ObjectProperty> GetEnumerator()
            {
                return ((IEnumerable<ObjectProperty>)pObjects).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable<ObjectProperty>)pObjects).GetEnumerator();
            }

            public object GetPropertyValue(string name)
            {
                foreach (ObjectProperty pObject in pObjects)
                {
                    if (pObject.Name == name)
                    {
                        return pObject.GetValue();
                    }
                }

                return null;
            }

            public bool SetPropertyValue(string name, object value)
            {
                foreach (ObjectProperty pObject in pObjects)
                {
                    if (pObject.Name == name)
                    {
                        return pObject.SetValue(value);
                    }
                }

                return false;
            }

            public ObjectProperty GetProperty(string propertyName)
            {
                foreach (ObjectProperty pObject in pObjects)
                {
                    if (pObject.Name == propertyName)
                    {
                        return pObject.GetProperty();
                    }
                }

                return null;
            }


        }
    }
}
