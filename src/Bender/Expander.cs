using System;

namespace Bender
{
    public class Expander
    {
        public static T Expand<T>()
        {
            return (T) Expand(typeof (T));
        }

        public static object Expand(Type type)
        {
            var instance = Activator.CreateInstance(type);
            Traverse(instance);
            return instance;
        }

        private static void Traverse(object @object)
        {
            var properties = @object.GetType().GetDeserializableProperties();

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;

                if ((propertyType.IsList() && !propertyType.IsArray) || propertyType.IsListInterface()) 
                    property.SetValue(@object, propertyType.CreateListOfEnumerableType(), null);
                else if (propertyType.IsClass && !propertyType.IsBclType() && (propertyType.HasParameterlessConstructor() ||
                    propertyType.HasConstructor(@object.GetType())))
                {
                    var propertyValue = Activator.CreateInstance(propertyType,
                        propertyType.HasConstructor(@object.GetType()) ? new[] { @object } : null);
                    property.SetValue(@object, propertyValue, null);
                    Traverse(propertyValue);
                }
            }
        }
    }
}