﻿// MIT License
//
// Copyright(c) 2021 ICARUS Consulting GmbH
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Scalar;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// Namespace directive.
    /// 
    /// Sets namespace of all current nodes selected by the cursor.
    /// All child nodes are moved to the namespace (default).
    /// All attributes are moved to the namespace (default).
    /// The namespace declaration will be done in the root node.
    /// 
    /// If the prefix is empty a default namespace will be created.
    /// which is declared only in the current nodes.
    /// Attributes cannot be added to a default namespace.
    /// 
    /// Hint:
    /// After declaring a namespace the XPath will be affected.
    /// The namespace resolver is not updated by adding namespaces.
    /// To address nodes belonging to a namespace the namesapce resolver
    /// injected to the Xambler object must be set up accordingly.
    /// </summary>
    public class NsDirective : IDirective
    {
        private const string DEFAULT_PURPOSE = "nodesAndAttributes";

        private readonly IScalar<string> prefix;
        private readonly IScalar<XNamespace> ns;
        private readonly IEnumerable<string> purposeDict;
        private readonly string purpose;
        private readonly bool inheritance;


        /// <summary>
        /// Namespace directive.
        /// 
        /// Sets namespace of all current nodes selected by the cursor.
        /// All child nodes are moved to the namespace (default).
        /// All attributes are moved to the namespace (default).
        /// The namespace declaration will be done in the root node.
        /// 
        /// If the prefix is empty a default namespace will be created.
        /// which is declared only in the current nodes.
        /// Attributes cannot be added to a default namespace.
        /// 
        /// Hint:
        /// After declaring a namespace the XPath will be affected.
        /// The namespace resolver is not updated by adding namespaces.
        /// To address nodes belonging to a namespace the namesapce resolver
        /// injected to the Xambler object must be set up accordingly.
        /// </summary>
        /// <param name="prefix">If empty a default namespace will be created</param>
        /// <param name="ns">Namespace</param>
        /// <param name="purpose">Set the namespace to: 'nodes', 'attributes', 'nodesAndAttributes'</param>
        /// <param name="inheritance">Is applied to the children</param>
        public NsDirective(string prefix, string ns, string purpose = DEFAULT_PURPOSE, bool inheritance = true) : this(
            new Arg.AttributeArg(prefix),
            new Arg.AttributeArg(ns),
            purpose,
            inheritance
        )
        { }

        /// <summary>
        /// Namespace directive.
        /// 
        /// Sets namespace of all current nodes selected by the cursor.
        /// All child nodes are moved to the namespace (default).
        /// All attributes are moved to the namespace (default).
        /// The namespace declaration will be done in the root node.
        /// 
        /// If the prefix is empty a default namespace will be created.
        /// which is declared only in the current nodes.
        /// Attributes cannot be added to a default namespace.
        /// 
        /// Hint:
        /// After declaring a namespace the XPath will be affected.
        /// The namespace resolver is not updated by adding namespaces.
        /// To address nodes belonging to a namespace the namesapce resolver
        /// injected to the Xambler object must be set up accordingly.
        /// </summary>
        /// <param name="prefix">If empty a default namespace will be created</param>
        /// <param name="ns">Namespace</param>
        /// <param name="purpose">Set the namespace to: 'nodes', 'attributes', 'nodesAndAttributes'</param>
        /// <param name="inheritance">Is applied to the children</param>
        public NsDirective(IArg prefix, IArg ns, string purpose = DEFAULT_PURPOSE, bool inheritance = true) : this(
            new ScalarOf<string>(() => prefix.Raw()),
            new ScalarOf<XNamespace>(() =>
            {
                XNamespace namesp = ns.Raw();
                return namesp;
            }),
            purpose,
            inheritance
        )
        { }

        private NsDirective(IScalar<string> prefix, IScalar<XNamespace> ns, string purpose, bool inheritance) : this(
            prefix,
            ns,
            new ManyOf(
                "nodes",
                "attributes",
                "nodesAndAttributes"
            ),
            purpose,
            inheritance
        )
        { }

        private NsDirective(IScalar<string> prefix, IScalar<XNamespace> ns, IEnumerable<string> purposeDict, string purpose, bool inheritance)
        {
            this.prefix = prefix;
            this.ns = ns;
            this.purposeDict = purposeDict;
            this.purpose = purpose;
            this.inheritance = inheritance;
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return
                $"NS {this.prefix.Value()}={this.ns.Value().NamespaceName}";
        }

        /// <summary>
        /// Execute it in the given document with current position at the given node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="cursor">Nodes we're currently at</param>
        /// <param name="stack">Execution stack</param>
        /// <param name="context">Context that knows XML namespaces</param>
        /// <returns>New current nodes</returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack, IXmlNamespaceResolver context)
        {
            this.ValidatePurpose();
            try
            {
                if (this.prefix.Value() == string.Empty)
                {
                    this.ApplyDefaultNS(dom, cursor, stack, context);
                }
                else
                {
                    this.ApplyPrefixedNS(dom, cursor, stack, context);
                }
                return cursor;
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException($"Failed to understand XML content, {this}", ex);
            }
        }

        private void ValidatePurpose()
        {
            new FailWhen(
                !this.purposeDict.Contains(this.purpose),
                $"Invalid value for purpose '{this.purpose}'. Valid values are: '{new Atoms.Text.Joined("', '", this.purposeDict)}'."
            ).Go();
        }

        private void ApplyDefaultNS(XNode dom, ICursor cursor, IStack stack, IXmlNamespaceResolver context)
        {
            this.SetNsToNodesSelectedByCursor(dom, cursor, stack, context);
            foreach (var node in cursor)
            {
                new FailWhen(
                    !(node is XElement)
                ).Go();

                var selElement = (node as XElement);
                var elements = selElement.DescendantNodesAndSelf();
                foreach (var element in elements)
                {
                    if (element is XElement)
                    {
                        var el = element as XElement;
                        el.Name = this.ns.Value().GetName(el.Name.LocalName);
                    }
                }
            }
        }

        private void SetNsToNodesSelectedByCursor(XNode dom, ICursor cursor, IStack stack, IXmlNamespaceResolver context)
        {
            new AttrDirective(
                "xmlns",
                this.ns.Value().NamespaceName
            ).Exec(dom, cursor, stack, context);
        }

        private void ApplyPrefixedNS(XNode dom, ICursor cursor, IStack stack, IXmlNamespaceResolver context)
        {
            new Each<XNode>(node =>
                {
                    var candidates =
                        this.Candidates(
                                this.StrictElement(
                                    node
                                )
                            );
                    new Each<XNode>(candidate =>
                        {
                            if (candidate is XElement)
                            {
                                var element = candidate as XElement;
                                if (this.purpose.Equals("nodes") || this.purpose.Equals("nodesAndAttributes"))
                                {
                                    this.SetNodeNamespace(element);
                                }
                                if (this.purpose.Equals("attributes") || this.purpose.Equals("nodesAndAttributes"))
                                {
                                    this.SetAttributeNamespace(element);
                                }
                            }
                        },
                        candidates
                    ).Invoke();
                },
                cursor
            ).Invoke();
            this.SetNsToRoot(dom);
        }

        private IEnumerable<XNode> Candidates(XElement node)
        {
            IEnumerable<XNode> candidates = new ManyOf<XElement>(node);

            if (this.inheritance)
            {
                candidates = node.DescendantNodesAndSelf();
            }

            return candidates;
        }

        private XElement StrictElement(XNode node)
        {
            new FailWhen(
                !(node is XElement)
            ).Go();

            return node as XElement;
        }

        private void SetNodeNamespace(XElement element)
        {
            element.Name = this.ns.Value().GetName(element.Name.LocalName);
        }

        private void SetAttributeNamespace(XElement el)
        {
            var attributes = el.Attributes().ToList();
            el.Attributes().Remove();
            foreach (XAttribute at in attributes)
            {
                el.Add(new XAttribute(this.ns.Value().GetName(at.Name.LocalName), at.Value));
            }
        }

        private void SetNsToRoot(XNode dom)
        {
            var root = this.RootNode(dom);
            var nsKey = XNamespace.Xmlns + this.prefix.Value();
            this.RemoveAttributeWhenExists(root, nsKey);
            this.AddNsAttribute(root, nsKey, this.ns.Value().NamespaceName);
        }

        private XElement RootNode(XNode dom)
        {
            XElement root;

            if (dom is XDocument)
            {
                root = (dom as XDocument).Root;
            }
            else
            {
                root = dom as XElement;
            }
            new FailPrecise(
                new FailNull(root),
                new ArgumentException($"Node is not of type 'XElement'")
            ).Go();

            return root;
        }

        private void RemoveAttributeWhenExists(XElement root, XName name)
        {
            var existing = root.Attribute(name);
            if (existing != null)
            {
                existing.Remove();
            }
        }

        private void AddNsAttribute(XElement root, XName nsKey, string namespaceName)
        {
            root.Add(
                new XAttribute(
                    nsKey,
                    namespaceName
                )
            );
        }
    }
}
