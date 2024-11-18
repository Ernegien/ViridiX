using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ViridiX.Mason.Serialization
{
    public static class SerializerCache
    {
        // Cached type info for serialization.
        private static Dictionary<Type, FieldInfo[]> SerializationTypeCache = new Dictionary<Type, FieldInfo[]>();

        public static FieldInfo[] GetOrCacheTypeInfo(Type type)
        {
            // Check if we already have field info cached for the type.
            if (SerializationTypeCache.ContainsKey(type) == false)
            {
                // Get and cache fields for the type.
                CacheFieldsForType(type);
            }

            // Return the cache field info.
            return SerializationTypeCache[type];
        }

        private static void CacheFieldsForType(Type type)
        {
            // Create a list to hold the base type hierarchy temporarily.
            List<string> baseTypes = new List<string>();
            baseTypes.Add(type.Name);

            // Build the base type hierarchy.
            Type baseType = type.BaseType;
            while (baseType != typeof(object) && baseType != typeof(ValueType))
            {
                // Add the base type name to the hierarchy.
                baseTypes.Add(baseType.Name);

                // Recursively walk base types.
                baseType = baseType.BaseType;
            }

            // Loop and create the hierarchy dictionary which is weighted based on the position of the base class in the hierarchy.
            Dictionary<string, int> baseTypeHierarchy = new Dictionary<string, int>();
            for (int i = baseTypes.Count - 1; i >= 0; i--)
                baseTypeHierarchy.Add(baseTypes[i], baseTypeHierarchy.Count);

            // Get a list of all public instance fields in the type.
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            // Sort the fields so that they are in the order they are declared in.
            fields = fields.OrderBy(f => baseTypeHierarchy[f.DeclaringType.Name]).ThenBy(f => f.MetadataToken).ToArray();

            // Add the field info to the cache.
            SerializationTypeCache[type] = fields;
        }
    }
}
