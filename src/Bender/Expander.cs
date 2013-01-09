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

                if (propertyType.IsArray || propertyType.IsList()) property.SetValue(@object, propertyType.CreateList(), null);
                else if (propertyType.IsClass && !propertyType.IsBclType())
                {
                    var propertyValue = Activator.CreateInstance(propertyType,
                        propertyType.GetConstructor(new[] { @object.GetType() }) != null ? new[] { @object } : null);
                    property.SetValue(@object, propertyValue, null);
                    Traverse(propertyValue);
                }
            }
        }
    }
}