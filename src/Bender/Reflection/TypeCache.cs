using System;
using System.Collections.Concurrent;

namespace Bender.Reflection
{
    public static class TypeCache
    {
        private static readonly ConcurrentDictionary<Type, CachedType> Cache = 
            new ConcurrentDictionary<Type, CachedType>(); 

        public static CachedType GetType(Type type)
        {
            if (!Cache.ContainsKey(type))
            {
                var cachedType = new CachedType(type);
                if (Cache.TryAdd(type, cachedType)) return cachedType;
            }
            return Cache[type];
        }

        public static CachedType GetCachedType(this Type type)
        {
            return GetType(type);
        }

        public static CachedType GetCachedType(this object instance)
        {
            return GetType(instance.GetType());
        }
    }
}
