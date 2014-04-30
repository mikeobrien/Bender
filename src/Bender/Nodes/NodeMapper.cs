using System;
using System.Linq;
using Bender.Collections;
using Bender.Extensions;

namespace Bender.Nodes
{
    public class MappingException : BenderException
    {
        public const string DeserializationMessageFormat = "Error deserializing {0} {1} '{2}' to '{3}': ";
        public const string SerializationMessageFormat = "Error serializing '{0}' to {1} {2} '{3}': ";

        public MappingException(BenderException exception, INode source, INode target, Mode mode) :
            base(exception, GetMessage(exception, source, target, mode)) { }

        public static string GetMessage(BenderException exception, INode source, INode target, Mode mode)
        {
            return (mode == Mode.Deserialize ? 
                DeserializationMessageFormat.ToFormat(source.Format, source.Type, source.Path, target.Path) :
                SerializationMessageFormat.ToFormat(source.Path, target.Format, target.Type, target.Path)) + 
                exception.Message;
        }
    }

    public class FriendlyMappingException : FriendlyBenderException
    {
        public const string FriendlyMessageFormat = "Could not {0} {1} {2} '{3}': ";

        public FriendlyMappingException(FriendlyBenderException exception, INode source, INode target, Mode mode) :
            base(exception, MappingException.GetMessage(exception, source, target, mode),
                 GetMessage(exception, source, target, mode)) { }

        public static string GetMessage(FriendlyBenderException exception, INode source, INode target, Mode mode)
        {
            return (mode == Mode.Deserialize ?
                FriendlyMessageFormat.ToFormat("read", source.Format, source.Type, source.Path) :
                FriendlyMessageFormat.ToFormat("write", target.Format, target.Type, target.Path)) +
                exception.FriendlyMessage;
        }
    }

    public class NodeTypeMismatchException : FriendlyBenderException
    {
        public const string MessageFormat = "Cannot map {0} {1} node to {2} {3} node.";
        public const string FriendlyMessageFormat = "Should be {2} {3} but was {0} {1}.";

        public NodeTypeMismatchException(INode source, INode target) :
            base(MessageFormat, FriendlyMessageFormat,
            source.NodeType.GetArticle(), source.NodeType.ToLower(),
            target.NodeType.GetArticle(), target.NodeType.ToLower()) { }
    }

    public class NodeMapper<TSource, TTarget>
            where TSource : INode
            where TTarget : INode
    {
        private readonly Func<TSource, TTarget, bool> _hasMapping;
        private readonly Action<TSource, TTarget> _mapping;
        private readonly Func<TSource, TTarget, bool> _hasVisitor;
        private readonly Action<TSource, TTarget> _visitors;

        public NodeMapper(
            Func<TSource, TTarget, bool> hasMapping,
            Action<TSource, TTarget> mapping,
            Func<TSource, TTarget, bool> hasVisitor,
            Action<TSource, TTarget> visitors)
        {
            _hasMapping = hasMapping;
            _mapping = mapping;
            _hasVisitor = hasVisitor;
            _visitors = visitors;
        }

        public void Map(TSource source, TTarget target, Mode mode)
        {
            try
            {
                if (_hasMapping(source, target)) _mapping(source, target);
                else
                {
                    if ((source.IsArray() && target.IsObject()) ||
                        (source.IsNonNullValue() && !target.IsValueOrVariable()) ||
                        (!source.IsValueOrVariable() && target.IsValue()))
                        throw new NodeTypeMismatchException(source, target);

                    if ((source.IsValueOrVariable() && target.IsValue()) ||
                        (source.IsValue() && target.IsVariable())) target.Value = source.Value;
                    else if (source.IsNotValue() && target.IsNotValue())
                    {
                        target.Initialize();
                        foreach (var node in source.Cast<TSource>())
                        {
                            try
                            {
                                target.Add(node, x => Map(node, x.As<TTarget>(), mode));
                            }
                            catch (BenderException exception)
                            {
                                if (!HandleExceptions(exception, node, target, mode)) throw;
                            }
                        }
                        target.Validate();
                    }
                }
                if (_hasVisitor(source, target)) _visitors(source, target);
            }
            catch (BenderException exception)
            {
                if (!HandleExceptions(exception, source, target, mode)) throw;
            }
        }

        private bool HandleExceptions(BenderException exception, TSource source, TTarget target, Mode mode)
        {
            if (exception is MappingException || exception is FriendlyMappingException) return false;
            if (exception is FriendlyBenderException) throw new FriendlyMappingException(
                exception.As<FriendlyBenderException>(), source, target, mode);
            throw new MappingException(exception, source, target, mode);
        }
    }
}
