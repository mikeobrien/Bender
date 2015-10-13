using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Bender.Collections;
using Bender.Extensions;

namespace Bender.Reflection
{
    public class MemberNotFoundException : BenderException
    {
        public MemberNotFoundException(string name, Type type) :
            base("Member {0} not found on type {1}.".ToFormat(name, type)) { }
    }

    public class OnlyPropertiesAndFieldsSupportedException : BenderException
    {
        public OnlyPropertiesAndFieldsSupportedException() : 
            base("Only properties and fields are supported.") { }
    }

    public static class MemberExtensions
    {
        public static bool HasAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return member.GetCustomAttributes(true).Any(x => x.GetType() == typeof(T));
        }

        public static Type GetPropertyOrFieldType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field: return member.As<FieldInfo>().FieldType;
                case MemberTypes.Property: return member.As<PropertyInfo>().PropertyType;
                default: throw new OnlyPropertiesAndFieldsSupportedException();
            }
        }

        public static bool IsPublicPropertyOrField(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field: return member.As<FieldInfo>().IsPublic;
                case MemberTypes.Property: 
                    var property = (PropertyInfo)member;
                    return (property.CanRead && property.GetGetMethod(true).IsPublic) ||
                           (property.CanWrite && property.GetSetMethod(true).IsPublic);
                default: return false;
            }
        }

        public static IEnumerable<MemberInfo> GetPropertiesAndFields(this Type type, 
            params Type[] excludedImpls)
        {
            if (excludedImpls.Any(type.Is)) return Enumerable.Empty<MemberInfo>();

            var members = type.GetMembers(BindingFlags.Instance | BindingFlags.NonPublic |
                BindingFlags.Public)
                .Where(x => (x.MemberType == MemberTypes.Field ||
                       x.MemberType == MemberTypes.Property) && 
                       !x.HasAttribute<CompilerGeneratedAttribute>() && 
                       !x.IsIndexer()).ToList();

            var implementations = type.GetInterfaces()
                .Where(y => excludedImpls.Any(y.Is))
                .SelectMany(x => type.GetInterfaceMap(x).TargetMethods).ToList();

            return members.Where(x => x.MemberType == MemberTypes.Field || 
                (x.MemberType == MemberTypes.Property && !x.As<PropertyInfo>()
                    .GetAccessors(true).Any(implementations.Contains))).ToList();
        }

        public static bool IsIndexer(this MemberInfo member)
        {
            return member.MemberType == MemberTypes.Property && 
                member.As<PropertyInfo>().GetIndexParameters().Length > 0;
        }
  
        public static bool IsReadonly(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field: return member.As<FieldInfo>().IsInitOnly;
                case MemberTypes.Property: return !member.As<PropertyInfo>().CanWrite;
                default: return true;
            }
        }

        public static Action<object, object> BuildSetter(this FieldInfo fieldInfo)
        {
            var @object = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object));
            var field = Expression.Field(Expression.Convert(@object, fieldInfo.DeclaringType), fieldInfo);
            var assign = Expression.Assign(field, Expression.Convert(value, fieldInfo.FieldType));

            return Expression.Lambda<Action<object, object>>(assign, @object, value).Compile();
        }

        public static Func<object, object> BuildGetter(this FieldInfo fieldInfo)
        {
            var @object = Expression.Parameter(typeof(object), "instance");
            var fieldExp = Expression.Field(Expression.Convert(@object, fieldInfo.DeclaringType), fieldInfo);

            return Expression.Lambda<Func<object, object>>(Expression.Convert(fieldExp, typeof(object)), @object).Compile();
        }

        public static Action<object, object> BuildSetter(this PropertyInfo propertyInfo)
        {
            var method = propertyInfo.GetSetMethod(true);

            if (method == null) throw new Exception($"Property {propertyInfo.Name} does not have a setter.");

            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object));

            return Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        Expression.Convert(instance, method.DeclaringType),
                        method,
                        Expression.Convert(value, method.GetParameters()[0].ParameterType)),
                    instance,
                    value).Compile();
        }

        public static Action<object, object, object> BuildIndexedSetter(this PropertyInfo propertyInfo)
        {
            var method = propertyInfo.GetSetMethod(true);
            var instance = Expression.Parameter(typeof(object), "instance");
            var index = Expression.Parameter(typeof(object));
            var value = Expression.Parameter(typeof(object));

            return Expression.Lambda<Action<object, object, object>>(
                    Expression.Call(
                        Expression.Convert(instance, method.DeclaringType),
                        method,
                        Expression.Convert(index, method.GetParameters()[0].ParameterType),
                        Expression.Convert(value, method.GetParameters()[1].ParameterType)),
                    instance, index, value).Compile();
        }

        public static Func<object, object> BuildGetter(this PropertyInfo propertyInfo)
        {
            var method = propertyInfo.GetGetMethod(true);
            var instance = Expression.Parameter(typeof(object), "instance");

            return Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(instance, method.DeclaringType),
                            method),
                        typeof(object)),
                    instance).Compile();
        }

        public static Func<object, object, object> BuildIndexedGetter(this PropertyInfo propertyInfo)
        {
            var method = propertyInfo.GetGetMethod(true);
            var instance = Expression.Parameter(typeof(object), "instance");
            var index = Expression.Parameter(typeof(object));

            return Expression.Lambda<Func<object, object, object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(instance, method.DeclaringType),
                            method,
                            Expression.Convert(index, method.GetParameters()[0].ParameterType)), 
                        typeof(object)),
                    instance, index).Compile();
        }

        public static Action<object, object[]> CreateActionDelegate(this Type type, string name, params Type[] parameters)
        {
            return CreateActionDelegate(type.GetMethod(name, parameters));
        }

        public static Action<object, object[]> CreateActionDelegate(this MethodInfo method)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var parameters = Expression.Parameter(typeof (object[]));

            return Expression.Lambda<Action<object, object[]>>(
                    Expression.Call(
                        Expression.Convert(instance, method.DeclaringType),
                        method,
                        method.GetParameters().Select((arg, index) =>
                            Expression.Convert(
                                Expression.ArrayIndex(parameters, Expression.Constant(index)), 
                                arg.ParameterType))
                            .ToArray()),
                    instance,
                    parameters)
                .Compile();
        }

        public static Func<object, object[], object> CreateFuncDelegate(this Type type, string name, params Type[] parameters)
        {
            return CreateFuncDelegate(type.GetMethod(name, parameters));
        }

        public static Func<object, object[], object> CreateFuncDelegate(this MethodInfo method)
        {
            var instance = Expression.Parameter(typeof(object), "instance");
            var parameters = Expression.Parameter(typeof(object[]));

            return Expression.Lambda<Func<object, object[], object>>(
                    Expression.Convert(
                        Expression.Call(
                            Expression.Convert(instance, method.DeclaringType),
                            method,
                            method.GetParameters().Select((arg, index) =>
                                Expression.Convert(
                                    Expression.ArrayIndex(parameters, Expression.Constant(index)),
                                    arg.ParameterType))
                                .ToArray()),
                        typeof(object)),
                    instance,
                    parameters)
                .Compile();
        }
    }
}
