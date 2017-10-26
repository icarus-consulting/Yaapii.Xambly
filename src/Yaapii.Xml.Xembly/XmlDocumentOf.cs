using System;
using System.Xml;
using Yaapii.Atoms;
namespace Yaapii.Xml.Xembly
{
    public class XmlDocumentOf : IScalar<XmlDocument>
    {
        private readonly XmlNode _dom;

        public XmlDocumentOf(XmlNode dom)
        {
            this._dom = dom;
        }

        public XmlDocument Value()
        {
            XmlDocument doc;
            if (this._dom.OwnerDocument == null)
            {
                doc = this._dom as XmlDocument;
                doc.AppendChild(this._dom);
            }
            else
            {
                doc = this._dom.OwnerDocument;
            }

            return doc;
        }
    }
}
