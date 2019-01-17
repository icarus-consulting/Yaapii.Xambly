// MIT License
//
// Copyright(c) 2019 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
