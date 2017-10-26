using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Arg;
using Yaapii.Xml.Xembly.Cursor;

namespace Yaapii.Xml.Xembly
{
    public sealed class AddDirective : IDirective
    {
        private readonly IArg _name;

        public AddDirective(string node)
        {
            this._name = new ArgOf(node);
        }

        public new string ToString()
        {
            return new FormattedText("ADD {0}", this._name).AsString();
        }

        public ICursor Exec(XmlNode dom, ICursor cursor, IStack stack)
        {
            var targets = new List<XmlNode>();
            string label = this._name.Raw();
            XmlDocument doc;

            if(dom is XmlDocument)
            {
                doc = dom as XmlDocument;

            } else if(dom.OwnerDocument == null)
            {
                doc = new XmlDocument();
                doc.AppendChild(dom);
            } else
            {
                doc = dom.OwnerDocument;
            }

            foreach(var node in cursor)
            {
                var element = doc.CreateElement(label);
                node.AppendChild(element);
                targets.Add(element);
            }

            return new DomCursor(targets);
        }
    }
}
