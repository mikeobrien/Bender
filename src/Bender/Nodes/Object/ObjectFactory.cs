using System;
using System.Collections.Generic;
using System.Linq;
using Bender.Reflection;

namespace Bender.Nodes.Object
{
    public class SimpleTypeInstantiationNotSupportedException : BenderException
    {
        public const string MessageFormat = "Could not instantiate type '{0}'. Instantiation of simple types not supported.";

        public SimpleTypeInstantiationNotSupportedException(CachedType type) :
            base(MessageFormat, type.FriendlyFullName) { }
    }

    public class ObjectCreationException : BenderException
    {
        public const string MessageFormat = "Could not instantiate type '{0}'. {1}";

        public ObjectCreationException(CachedType type, Exception exception) :
            base(exception, MessageFormat, type.FriendlyFullName, exception.Message) { }
    }

    public class ObjectFactory
    {
        public static object CreateInstance(
            CachedType type,
            Func<CachedType, object[], object> factory = null, 
            params object[] dependencies)
        {
            if (type.IsSimpleType) throw new SimpleTypeInstantiationNotSupportedException(type);
            try
            {
                if (type.IsArray) return type.CreateArray();
                if (type.IsInterface)
                {
                    if (type.IsGenericListInterface) return type.CreateGenericListInstance();
                    if (type.IsListInterface) return new List<object>();
                    if (type.IsGenericDictionaryInterface) return type.CreateGenericDictionaryInstance();
                    if (type.IsDictionaryInterface) return new Dictionary<object, object>();
                    if (type.IsGenericEnumerableInterface) return type.CreateGenericListInstance();
                    if (type.IsEnumerableInterface) return new List<object>();
                }
                return (factory ?? DefaultFactory)(type, dependencies.Where(x => x != null).ToArray());
            }
            catch (Exception exception)
            {
                throw new ObjectCreationException(type, exception);
            }
        }

        public static Func<CachedType, object[], object> DefaultFactory = (t, d) =>
            t.Type.CreateInstance(d.Any() && t.Type.HasMatchingConstructor(d) ? d : null);
    }
}
