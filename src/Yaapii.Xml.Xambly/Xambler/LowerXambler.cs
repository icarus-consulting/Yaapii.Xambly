// MIT License
//
// Copyright(c) 2017 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Linq;
using System.Xml;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xambly;
using Yaapii.Xml.Xambly.Error;

namespace Yaapii.Xml.Xembly
{
    /// <summary>
    /// Decorator of Processor of Xembly directives, main entry point to the module.
    /// Lowers names of Elements + Attributes and/or Values of all Nodes
    /// </summary>
    public sealed class LowerXambler : IXambler
    {
        private readonly bool _lowerNames;
        private readonly bool _lowerValues;
        private readonly IXambler _origin;

        /// <summary>
        /// primary ctor
        /// </summary>
        /// <param name="xembler">decorated Xembler</param>
        /// <param name="lowerNames">lower names</param>
        /// <param name="lowerValues">lower values</param>
        public LowerXambler(IXambler xembler, bool lowerNames = true, bool lowerValues = false)
        {
            _origin = xembler;
            _lowerNames = lowerNames;
            _lowerValues = lowerValues;
        }

        /// <summary>
        /// Apply all lowered changes to the document/node.
        /// </summary>
        /// <returns>Lowered Document/Node</returns>
        /// <param name="dom">DOM document/node</param>
        public XmlNode Apply(XmlNode dom)
        {
            return ApllyLowered(dom);
        }

        /// <summary>
        /// Apply all lowered changes to an empty DOM, redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        public XmlNode ApplyQuietly(XmlNode dom)
        {
            try
            {
                return ApllyLowered(dom, true);
            }
            catch (IllegalStateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw 
                    new IllegalStateException(
                        new FormattedText("Quietly failed to apply lower DOM: {0}", ex.Message).AsString(), ex);
            }
        }

        /// <summary> Apply all changes to an empty DOM and lowers content. </summary>
        /// <returns> The DOM with lowerd names </returns>
        public XmlDocument Dom()
        {
            return LoweredDom();
        }

        /// <summary> Apply all changes to an empty DOM, redirect all exceptions to a IllegalStateException. </summary>
        /// <returns> The quietly. </returns>
        public XmlDocument DomQuietly()
        {
            try
            {
                return LoweredDom(true);
            }
            catch (IllegalStateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new IllegalStateException(new FormattedText("Failed to create lower DOM: {0}", ex.Message).AsString(), ex);
            }
        }

        /// <summary> Escape text before using it as a text value </summary>
        /// <returns> The escaped. </returns>
        /// <param name="text"> Text. </param>
        public string Escaped(string text)
        {
            return _origin.Escaped(text);
        }

        /// <summary> Convert to XML Document with lowered Names and/or Values. </summary>
        /// <returns> The xml. </returns>
        public string Xml()
        {
            return LoweredXml();
        }

        /// <summary> Convert to XML Documentwith lowered Names and/or Values, redirect all Exceptions to IllegalStateException. </summary>
        /// <returns> The quietly. </returns>
        public String XmlQuietly()
        {
            try
            {
                return LoweredXml(true);
            }
            catch (IllegalStateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw 
                    new IllegalStateException(
                        new FormattedText("Quietly failed to build lower XML: {0}", ex.Message).AsString(), ex);
            }
        }

        private XmlNode ApllyLowered(XmlNode dom, bool quietly = false)
        {
            var doc = dom.NodeType == XmlNodeType.Document ? dom as XmlDocument : new XmlDocument().AppendChild(dom).OwnerDocument;
            doc = LoweredDocument(doc);

            return quietly ? _origin.ApplyQuietly(doc.DocumentElement) : _origin.Apply(doc.DocumentElement);
        }

        private void LowerDescendantsNames(XmlNodeList children, XmlNode newParentNode, XmlDocument newDocument)
        {
            var nodes = children.OfType<XmlNode>();

            foreach (var tNode in nodes)
            {
                XmlNode newNode;

                if (tNode.NodeType != XmlNodeType.Element)
                {
                    newNode = newDocument.CreateNode(tNode.NodeType, tNode.Prefix, tNode.LocalName, tNode.NamespaceURI);
                }
                else
                {
                    var newEle = newDocument.CreateElement(tNode.Prefix, tNode.LocalName.ToLower(), tNode.NamespaceURI);
                    newNode = newEle;

                    // Lower Attributes
                    tNode
                        .Attributes
                        .OfType<XmlNode>()
                        .ToList()
                        .ForEach(tAttribute => newEle.SetAttribute(tAttribute.Name.ToLower(), tAttribute.Value));
                }

                newNode.Value = tNode.Value;
                newParentNode.AppendChild(newNode);

                LowerDescendantsNames(tNode.ChildNodes, newNode, newDocument);
            }
        }

        private void LowerDescendantsValues(XmlNodeList children)
        {
            var childrenList = children.OfType<XmlNode>();

            foreach (var tChild in childrenList)
            {
                if (!String.IsNullOrEmpty(tChild.Value))
                {
                    tChild.Value = tChild.Value.ToLower();
                }

                if (tChild.NodeType == XmlNodeType.Element)
                {
                    (tChild as XmlElement)
                    .Attributes.OfType<XmlAttribute>()
                    .ToList()
                    .ForEach(tAttribute => tAttribute.Value = tAttribute.Value.ToLower());
                }

                if (tChild.HasChildNodes)
                {
                    LowerDescendantsValues(tChild.ChildNodes);
                }
            }
        }

        private XmlDocument LoweredDocument(XmlDocument result)
        {
            if (_lowerValues)
            {
                LowerDescendantsValues(result.ChildNodes);
            }

            if (_lowerNames)
            {
                result = NameLoweredXmlDocument(result);
            }

            return result;
        }

        private XmlDocument LoweredDom(bool quietly = false)
        {
            var result = quietly ? _origin.DomQuietly() : _origin.Dom();

            result = LoweredDocument(result);
            return result;
        }

        private string LoweredXml(bool quietly = false)
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                LoweredDom(quietly).WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        private XmlDocument NameLoweredXmlDocument(XmlDocument xmlDocument)
        {
            var result = new XmlDocument();

            if (xmlDocument.FirstChild?.NodeType == XmlNodeType.XmlDeclaration)
            {
                var declaration = result.FirstChild as XmlDeclaration;
                var newdecla = result.CreateXmlDeclaration(declaration.Version, declaration.Encoding, declaration.Standalone);
                result.AppendChild(declaration);
            }

            var docType = xmlDocument.DocumentType;
            if (docType != null)
            {
                result.CreateDocumentType(docType.Name, docType.PublicId, docType.SystemId, docType.InternalSubset);
            }

            LowerDescendantsNames(xmlDocument.ChildNodes, result, result);

            return result;
        }
    }
}