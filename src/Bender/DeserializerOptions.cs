using System;

namespace Bender
{
    public class DeserializerOptions
    {
        private readonly Options _options;

        public DeserializerOptions(Options options)
        {
            _options = options;
        }

        public DeserializerOptions AddReader<T>(Func<ReaderContext, T> reader)
        {
            _options.AddReader(reader);
            return this;
        }

        public DeserializerOptions AddReader<T>(Func<ReaderContext, T> reader, bool handleNullable) where T : struct
        {
            _options.AddReader(reader, handleNullable);
            return this;
        }

        public DeserializerOptions DefaultNonNullableTypesWhenEmpty()
        {
            _options.DefaultNonNullableTypesWhenEmpty = true;
            return this;
        }

        public DeserializerOptions IgnoreUnmatchedNodes()
        {
            _options.IgnoreUnmatchedNodes = true;
            return this;
        }

        public DeserializerOptions FailOnUnmatchedXmlAttributes()
        {
            _options.IgnoreUnmatchedXmlAttributes = false;
            return this;
        }

        public DeserializerOptions IgnoreTypeXmlElementNames()
        {
            _options.IgnoreTypeXmlElementNames = true;
            return this;
        }

        public DeserializerOptions IgnoreCase()
        {
            _options.IgnoreCase = true;
            return this;
        }

        public DeserializerOptions ExcludeTypes(Func<Type, bool> typeFilter)
        {
            _options.ExcludedTypes.Add(typeFilter);
            return this;
        }

        public DeserializerOptions ExcludeType<T>()
        {
            _options.ExcludedTypes.Add(x => x == typeof(T));
            return this;
        }

        public DeserializerOptions WithDefaultGenericTypeXmlNameFormat(string typeNameFormat)
        {
            _options.GenericTypeXmlNameFormat = typeNameFormat;
            return this;
        }

        public DeserializerOptions WithDefaultGenericListXmlNameFormat(string listNameFormat)
        {
            _options.GenericListXmlNameFormat = listNameFormat;
            return this;
        }

        public DeserializerOptions WithFriendlyParseErrorMessage<T>(string message)
        {
            _options.FriendlyParseErrorMessages[typeof (T)] = message;
            return this;
        }
    }
}