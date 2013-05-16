using System.Xml.Linq;

namespace Bender
{
    public enum JsonDataType { None, Null, String, Number, Boolean, Array, Object }

    public class JsonField
    {
        private const string TypeAttributeName = "type";
        private readonly XElement _element;

        public JsonField(XElement element)
        {
            _element = element;
        }

        public JsonDataType DataType
        {
            get
            {
                var type = _element.Attribute(TypeAttributeName);
                return type != null ? type.Value.ParseEnum<JsonDataType>() : JsonDataType.None;
            }
            set
            {
                var type = _element.Attribute(TypeAttributeName);
                if (value == JsonDataType.None && type != null) type.Remove();
                else if (type != null) type.Value = value.ToString().ToLower();
                else _element.Add(new XAttribute(TypeAttributeName, value.ToString().ToLower()));
            }
        }
    }

    public static class JsonDataTypeExtensions
    {
        public static JsonDataType ToJsonValueType(this object source)
        {
            if (source == null) return JsonDataType.Null;
            if (source is bool) return JsonDataType.Boolean;
            if (source is sbyte || source is sbyte || source is byte || source is short ||
                source is ushort || source is int || source is uint || source is long ||
                source is ulong || source is float || source is double || source is decimal) return JsonDataType.Number;
            return JsonDataType.String;
        }
    }
}
