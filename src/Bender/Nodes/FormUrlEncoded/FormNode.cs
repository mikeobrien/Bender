using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Bender.Collections;
using Bender.Extensions;
using Flexo;

namespace Bender.Nodes.FormUrlEncoded
{
    public class FormNode : NodeBase
    {
        public const string NodeFormat = "url encoded";

        public FormNode(NodeType type)
        {
            if (type != NodeType.Object)
                throw new BenderException("Only objects can be serialized.");
            Form = new List<FormValueNode>();
        }

        public FormNode(string form) {
            Form = Exception<JsonParseException>
            .Map(() => ParseForm(form), x => new ParseException(x, Format)); }

        public FormNode(byte[] bytes, Encoding encoding = null) {
            Form = Exception<JsonParseException>
            .Map(() => ParseForm(bytes, encoding), x => new ParseException(x, Format)); }

        public FormNode(Stream stream, Encoding encoding = null) {
            Form = Exception<JsonParseException>
            .Map(() => ParseForm(stream, encoding), x => new ParseException(x, Format));
        }

        public List<FormValueNode> Form { get; private set; }
        public override string Format { get { return NodeFormat; } }

        public override string Type { get { return "form"; } }

        protected override NodeType GetNodeType()
        {
            return NodeType.Object;
        }

        protected override void AddNode(INode node, bool named, Action<INode> modify)
        {
            if (node is FormValueNode) Form.Add(((FormValueNode) node));
            else Form.Add(new FormValueNode(node.Name).Configure(modify).As<FormValueNode>());
        }

        protected override IEnumerable<INode> GetNodes()
        {
            return Form;
        }

        public override void Encode(Stream stream, Encoding encoding = null, bool pretty = false)
        {
            Form.Select(x => "{0}={1}".ToFormat(x.Name, HttpUtility.UrlEncode(x.Value.ToString())))
                .Aggregate((a, i) => a + "&" + i).WriteToStream(stream, encoding);
        }

        private static List<FormValueNode> ParseForm(Stream stream, Encoding encoding)
        {
            return ParseForm(new StreamReader(stream, encoding ?? Encoding.UTF8).ReadToEnd());
        }

        private static List<FormValueNode> ParseForm(byte[] bytes, Encoding encoding)
        {
            return ParseForm((encoding ?? Encoding.UTF8).GetString(bytes));
        }

        private static List<FormValueNode> ParseForm(string form)
        {
            if (form.IsNullOrEmpty()) return Enumerable.Empty<FormValueNode>().ToList();
            return form.Split('&')
                .Select(x => x.Split(new [] {'='}, 2))
                .Select(x => new FormValueNode(
                    HttpUtility.UrlDecode(x[0]), 
                    x.Length > 1 ? HttpUtility.UrlDecode(x[1]) : null))
                .ToList();
        }
    }
}
