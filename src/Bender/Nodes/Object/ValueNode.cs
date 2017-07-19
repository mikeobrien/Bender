using System;
using System.Collections.Generic;
using Bender.Collections;
using Bender.Configuration;
using Bender.Extensions;
using Bender.Nodes.Object.Values;
using Bender.Reflection;
using Bender.NamingConventions;

namespace Bender.Nodes.Object
{
    public class ValueCannotBeNullDeserializationException : FriendlyBenderException
    {
        public ValueCannotBeNullDeserializationException() : base("Value cannot be null.") { }
    }

    public class ValueParseException : FriendlyBenderException
    {
        public ValueParseException(Exception exception, object value, string friendlyMessage) : 
            base(exception, "Error parsing '{0}'. {1}".ToFormat(value.Truncate(50), exception.Message), friendlyMessage) { }
    }

    public class ValueConversionException : BenderException
    {
        public ValueConversionException(Exception exception, object value, 
                CachedType source, CachedType target) : 
            base(exception, "Value '{0}' of type '{1}' cannot be converted to type '{2}': {3}",
                value.Truncate(50), source.FriendlyFullName, target.FriendlyFullName, exception.Message) { }
    }

    public class ValueNode : ObjectNodeBase
    {
        private readonly Dictionary<Type, string> _friendlyParseMessages;

        public ValueNode(
            Context context,
            string name,
            IValue source,
            CachedMember member,
            INode parent,
            int? index = null)
            : base(name, source, member, parent, context, index)
        {
            _friendlyParseMessages = Context.Options.Deserialization.FriendlyParseErrorMessages;
        }

        public override string Type => "value";

        protected override NodeType GetNodeType()
        {
            return NodeType.Value;
        }

        public override object Value
        {
            set => SetValue(value);
            get => GetValue();
        }

        protected override void SetValue(object value)
        {
            var type = SpecifiedType.UnderlyingType;
            if (value != null && !type.IsTypeOf(value))
            {
                if (type.Is<string>()) Source.Instance = value.ToString();
                else if (value as string == "" && !type.Is<string>() && SpecifiedType.IsNullable)
                    Source.Instance = null;
                else if (value is string && !type.Is<string>() && type.IsSimpleType)
                {
                    try
                    {
                        if (type.IsEnum)
                        {
                            Source.Instance = Context.Options.EnumValueNameConventions
                                .GetName(value, SpecifiedType, Context)
                                .ParseEnum(SpecifiedType, Context.Options.Deserialization
                                    .EnumValueComparison.IgnoreCase());
                        }
                        else Source.Instance = value.As<string>().ParseSimpleType(SpecifiedType);
                    }
                    catch (Exception exception)
                    {
                        throw new ValueParseException(exception, value, _friendlyParseMessages[
                            type.IsEnum ? typeof(Enum) : type.Type].ToFormat(value.Truncate(50)));
                    }
                }
                else
                {
                    try
                    {
                        Source.Instance = type.IsEnum ?
                            value.ConvertToEnum(type.Type) : 
                            Convert.ChangeType(value, type.Type);
                    }
                    catch (Exception exception)
                    {
                        throw new ValueConversionException(exception, value, value.ToCachedType(), type);
                    }
                }
            }
            else if (value == null && SpecifiedType.IsValueType && !SpecifiedType.IsNullable)
            {
                if (!Context.Options.Deserialization.IgnoreNullsForValueTypes)
                    throw new ValueCannotBeNullDeserializationException();
            }
            else Source.Instance = value;
        }

        protected override object GetValue()
        {
            var type = SpecifiedType.UnderlyingType;
            var value = base.Value;

            if (type.IsEnum)
            {
                return Context.Options.Serialization.NumericEnumValues ? (object)(int)value :
                    Context.Options.EnumValueNameConventions.GetName(value, SpecifiedType, Context);
            }

            if (value != null && Context.Options.Serialization.NonNumericFloatMode != 
                    NonNumericFloatSerializationMode.Raw &&
                ((type.Is<float>(true) && value.As<float>().IsNonNumeric()) || 
                (type.Is<double>(true) && value.As<double>().IsNonNumeric())))
            {
                switch (Context.Options.Serialization.NonNumericFloatMode)
                {
                    case NonNumericFloatSerializationMode.Name: return value.ToString();
                    case NonNumericFloatSerializationMode.Zero: return 0;
                }
            }
            return value;
        }
    }
}