using System;
using System.Xml;
using Yaapii.Atoms;
namespace Yaapii.Xml.Xambly
{
    /// <summary>
    /// Makes a XmlDocument, whatever kind of node you give it to this.
    /// </summary>
    public class XmlDocumentOf : IScalar<XmlDocument>
    {
        /// <summary>
        /// node to inspect
        /// </summary>
        private readonly XmlNode _dom;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dom">Node to make or get Xml Document from</param>
        public XmlDocumentOf(XmlNode dom)
        {
            this._dom = dom;
        }

        /// <summary>
        /// The XmlDocument
        /// </summary>
        /// <returns></returns>
        public XmlDocument Value()
        {
            XmlDocument doc;
            if (this._dom.OwnerDocument == null) //if the ownerdocument is null, this node is the document (see https://msdn.microsoft.com/de-de/library/system.xml.xmlnode.ownerdocument(v=vs.110).aspx)
            {
                doc = this._dom as XmlDocument;
            }
            else
            {
                doc = this._dom.OwnerDocument; //get the owner document
            }

            return doc;
        }
    }
}
