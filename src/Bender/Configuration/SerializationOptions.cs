using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;
using Bender.Nodes.Object;

namespace Bender.Configuration
{
    public enum XmlValueNodeType { Attribute, Element }

    public enum SerializationType { SpecifiedType, ActualType }

    public static class SerializationTypeExtensions
    {
        public static bool IsSpecified(this SerializationType type)
        {
            return type == SerializationType.SpecifiedType;
        }

        public static bool IsActual(this SerializationType type)
        {
            return type == SerializationType.ActualType;
        }
    }

    public class SerializationOptions
    {
        public SerializationOptions(Options options)
        {
            XmlValueNodeType = XmlValueNodeType.Element;
            XmlNamespaces = new Dictionary<string, XNamespace>();
            SerializationType = SerializationType.SpecifiedType;

            Writers = new WriterConventions(options);

            Writers.AddValueWriter<Version>((v, s, t, o) => v.ToString());
            Writers.AddValueWriter<MailAddress>((v, s, t, o) => v.ToString());
            Writers.AddValueWriter<IPAddress>((v, s, t, o) => v.ToString());
            Writers.AddValueWriter<byte[]>((v, s, t, o) => Convert.ToBase64String(v));
            Writers.AddValueWriter<SqlConnectionStringBuilder>((v, s, t, o) => v.ToString());
        }

        public bool PrettyPrint { get; set; }
        public bool IncludeNullMembers { get; set; }
        public bool NumericEnumValues { get; set; }
        public SerializationType SerializationType { get; set; }

        public XmlValueNodeType XmlValueNodeType { get; set; }
        public XNamespace DefaultXmlNamespace { get; set; }
        public bool OmitXmlDeclaration { get; set; }
        public Dictionary<string, XNamespace> XmlNamespaces { get; set; }

        public WriterConventions Writers { get; set; }
    }
}