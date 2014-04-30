using System;
using Bender.Configuration;
using Bender.Reflection;

namespace Bender.Nodes.Object.Values
{
    public class ValueFactory
    {
        public static IValue Create(CachedType type)
        {
            return new SimpleValue(type);
        }

        public static IValue Create(object @object, CachedType type, Options options)
        {
            return Create(@object, type, false, options);
        }

        public static IValue Create(object @object, CachedType type, bool @readonly, Options options)
        {
            return new SimpleValue(@object, UseActualType(type, options) ? @object.GetCachedType() : type, @readonly);
        }

        public static IValue Create(Mode mode, IValue @object, CachedMember member, Options options)
        {
            return new MemberValue(@object, member, x => mode.IsSerialize() && UseActualType(x, options));
        }

        public static IValue Create(IValue source, Func<object> factory)
        {
            return new LazyValue(source, factory);
        }

        private static bool UseActualType(CachedType type, Options options)
        {
            return options.Serialization.SerializationType.IsActual() || 
                type == null || type.Type == typeof(object);
        }
    }
}
