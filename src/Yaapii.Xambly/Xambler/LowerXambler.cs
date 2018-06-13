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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Xambly;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly
{
    /// <summary>
    /// Decorator of Processor of Xambly directives, main entry point to the module.
    /// Lowers names of Elements + Attributes and/or Values of all Nodes
    /// </summary>
    public sealed class LowerXambler : IXambler
    {
        private readonly bool _lowerNodeNames;
        private readonly bool _lowerValues;
        private readonly IXambler _origin;

        /// <summary>
        /// primary ctor
        /// </summary>
        /// <param name="xembler">decorated Xembler</param>
        /// <param name="lowerNodeNames">lower node names</param>
        /// <param name="lowerValues">lower values</param>
        public LowerXambler(IXambler xembler, bool lowerNodeNames = true, bool lowerValues = false)
        {
            _origin = xembler;
            _lowerNodeNames = lowerNodeNames;
            _lowerValues = lowerValues;
        }

        /// <summary>
        /// Apply all lowered changes to the document/node.
        /// </summary>
        /// <returns>Lowered Document/Node</returns>
        /// <param name="dom">DOM document/node</param>
        public XmlNode Apply(XNode dom)
        {
            return ApplyLowered(dom);
        }

        /// <summary>
        /// Apply all lowered changes to an empty DOM, redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        public XmlNode ApplyQuietly(XNode dom)
        {
            try
            {
                return ApplyLowered(dom, true);
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
        public XDocument Dom()
        {
            return LoweredDom();
        }

        /// <summary> Apply all changes to an empty DOM, redirect all exceptions to a IllegalStateException. </summary>
        /// <returns> The quietly. </returns>
        public XDocument DomQuietly()
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
        /// <returns> The XML. </returns>
        /// <param name="createHeader">Option to get the XML Document with or without header (version, encoding)</param>
        public string Xml(bool createHeader = true)
        {
            return LoweredXml(createHeader);
        }

        /// <summary> Convert to XML Documentwith lowered Names and/or Values, redirect all Exceptions to IllegalStateException. </summary>
        /// <returns> The quietly. </returns>
        /// <param name="createHeader">Option to get the XML Document with or without header (version, encoding)</param>
        public String XmlQuietly(bool createHeader = true)
        {
            try
            {
                return LoweredXml(createHeader, true);
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

        private XNode ApplyLowered(XNode dom, bool quietly = false)
        {
            //XDocument doc;
            //if(dom.NodeType == XmlNodeType.Document)
            //{
            //    doc = dom as XDocument;
            //} else {
            //    doc = new XDocument(dom);
            //    doc.Add(dom)            }
            var doc = dom.NodeType == XmlNodeType.Document ? dom as XDocument : new XDocument(dom);
            doc = LoweredDocument(doc);

            return quietly ? _origin.ApplyQuietly(doc) : _origin.Apply(doc);
        }

        private void LoweredElements(IEnumerable<XElement> children, XElement newParentNode, XDocument newDocument)
        {
            //var nodes = children.OfType<XmlNode>();

            foreach (var node in children)
            {
                var el = new XElement(node.Name);
                el.Value = node.Value.ToLower();
                new Each<XAttribute>(
                        attr => el.Add(new XAttribute(attr.Name.LocalName.ToLower(), attr.Value.ToLower())),
                        node.Attributes()
                    ).Invoke();

                //if (node.NodeType != XmlNodeType.Element)
                //{
                //    newNode = newDocument.CreateNode(tNode.NodeType, tNode.Prefix, tNode.LocalName, tNode.NamespaceURI);
                //}
                //else
                //{
                //    var newEle = newDocument.CreateElement(tNode.Prefix, tNode.LocalName.ToLower(), tNode.NamespaceURI);
                //    newNode = newEle;

                //    // Lower Attributes
                //    tNode
                //        .Attributes
                //        .OfType<XmlNode>()
                //        .ToList()
                //        .ForEach(tAttribute => newEle.SetAttribute(tAttribute.Name.ToLower(), tAttribute.Value));
                //}

                //newNode.InnerText = tNode.Value;
                newParentNode.Add(el);

                LoweredElements(node.Elements(), el, newDocument);
            }
        }

        private void LoweredValues(IEnumerable<XElement> children)
        {

            foreach (var child in children)
            {
                child.Value = child.Value.ToLower();

                new Each<XAttribute>(
                        attr => attr.Value = attr.Value.ToLower(),
                        child.Attributes()

                    ).Invoke();

                if (child.HasElements)
                {
                    LoweredValues(child.Elements());
                }
            }
        }

        private XDocument LoweredDocument(XDocument result)
        {
            if (_lowerValues)
            {
                LoweredValues(result.Elements());
            }

            if (_lowerNodeNames)
            {
                result = NameLoweredXmlDocument(result);
            }

            return result;
        }

        private XDocument LoweredDom(bool quietly = false)
        {
            var result = quietly ? _origin.DomQuietly() : _origin.Dom();

            result = LoweredDocument(result);
            return result;
        }

        private string LoweredXml(bool createHeader, bool quietly = false)
        {
            var settings = new XmlWriterSettings();
            settings.ConformanceLevel = createHeader ? ConformanceLevel.Document : ConformanceLevel.Fragment;

            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter, settings))
            {
                LoweredDom(quietly).WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        private XDocument NameLoweredXmlDocument(XDocument xmlDocument)
        {
            var result = new XDocument();

            if (xmlDocument.FirstNode?.NodeType == XmlNodeType.XmlDeclaration)
            {
                var declaration = result.Declaration;
                //var newdecla = new XDeclaration(declaration.Version,declaration.Encoding,declaration.Standalone);// result.CreateXmlDeclaration(declaration.Version, declaration.Encoding, declaration.Standalone);
                result.Declaration = declaration;
            }

            var docType = xmlDocument.DocumentType;
            if (docType != null)
            {
                result.AddFirst(xmlDocument.DocumentType);
                //result.CreateDocumentType(docType.Name, docType.PublicId, docType.SystemId, docType.InternalSubset);
            }

            LoweredElements(xmlDocument.Elements(), result.Root, result);

            return result;
        }
    }
}