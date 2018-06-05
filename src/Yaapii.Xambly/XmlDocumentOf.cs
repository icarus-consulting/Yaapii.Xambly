using System.Xml.Linq;
using Yaapii.Atoms;
namespace Yaapii.Xambly
{
    /// <summary>
    /// Makes a XmlDocument, whatever kind of node you give it to this.
    /// </summary>
    public class XmlDocumentOf : IScalar<XDocument>
    {
        /// <summary>
        /// node to inspect
        /// </summary>
        private readonly XNode dom;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dom">Node to make or get Xml Document from</param>
        public XmlDocumentOf(XNode dom)
        {
            this.dom = dom;
        }

        /// <summary>
        /// The XmlDocument
        /// </summary>
        /// <returns></returns>
        public XDocument Value()
        {
            XDocument doc = dom.Document;
            //if (this.dom.NodeType == System.Xml.XmlNodeType.Document)
            //{
            //    doc = dom.Document;
            //}
            
            //if (this.dom.Document == null) //if the ownerdocument is null, this node is the document (see https://msdn.microsoft.com/de-de/library/system.xml.xmlnode.ownerdocument(v=vs.110).aspx)
            //{
            //    doc = this.dom as XDocument;
            //}
            //else
            //{
            //    doc = this.dom.Document; //get the owner document
            //}

            return doc;
        }
    }
}
