using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bender.Collections;
using Bender.Nodes;
using Bender.Reflection;

namespace Tests
{
    public static class TestExtensions
    {
        public static object GetPropertyOrFieldValue(this object @object, string name)
        {
            var members = @object.GetType().GetMember(name);
            if (!members.Any()) throw new MemberNotFoundException(name, @object.GetType());
            var member = members.First();
            switch (member.MemberType)
            {
                case MemberTypes.Field: return member.As<FieldInfo>().GetValue(@object);
                case MemberTypes.Property: return member.As<PropertyInfo>().GetValue(@object, null);
                default: throw new OnlyPropertiesAndFieldsSupportedException();
            }
        }

        public static void SetPropertyOrFieldValue(this object @object, string name, object value)
        {
            var members = @object.GetType().GetMember(name);
            if (!members.Any()) throw new MemberNotFoundException(name, @object.GetType());
            var member = members.First();
            switch (member.MemberType)
            {
                case MemberTypes.Field: member.As<FieldInfo>().SetValue(@object, value); break;
                case MemberTypes.Property: member.As<PropertyInfo>().SetValue(@object, value, null); break;
                default: throw new OnlyPropertiesAndFieldsSupportedException();
            }
        }

        public static Type MakeGenericDictionaryType<TKey>(this Type type)
        {
            return typeof(Dictionary<,>).MakeGenericType(typeof(TKey), type);
        }

        public static Type MakeGenericListType(this Type type)
        {
            return typeof(List<>).MakeGenericType(type);
        }

        public static void Add(this INode node, string name, NodeType type, Metadata metadata, Action<INode> modify)
        {
            node.Add(new Node(name, metadata: metadata) { NodeType = type }, modify);
        }

        public static void Add(this INode node, NodeType type, Metadata metadata, Action<INode> modify)
        {
            node.Add(new Node(metadata: metadata) { NodeType = type }, modify);
        }

        public static DateTime SubtractUtcOffset(this DateTime date)
        {
            return date.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(date).Hours);
        }
    }
}
