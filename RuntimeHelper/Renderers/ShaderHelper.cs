using System.Collections.Generic;
using System.Reflection;

namespace RuntimeHelper.Renderers
{
    public static class ShaderHelper
    {
        public static List<string> CreateShaderPropertyList()
        {
            List<string> shaderPropertyKeywords = new List<string>();

            foreach (PropertyInfo propertyInfo in typeof(ShaderPropertyID).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                shaderPropertyKeywords.Add(propertyInfo.Name);
            }            

            return shaderPropertyKeywords;
        }
    }
}
