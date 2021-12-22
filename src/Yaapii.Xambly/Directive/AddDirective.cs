// MIT License
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
using System.Xml;
using System.Xml.Linq;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Cursor;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// ADD directive.
    /// Adds new node to all current nodes.
    /// </summary>
    public sealed class AddDirective : IDirective
    {
        private readonly IArg _name;

        /// <summary>
        /// ADD directive.
        /// Adds new node to all current nodes.
        /// </summary>
        /// <param name="node">Name of node to add</param>
        public AddDirective(string node)
        {
            this._name = new Arg.AttributeArg(node);
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new Formatted("ADD {0}", this._name).AsString();
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
            var targets = new List<XElement>();
            string label = this._name.Raw();

            foreach (var node in cursor)
            {
                var parent = node as XContainer;
                new FailPrecise(
                     new FailNull(parent),
                     new ArgumentException($"Can't add child to node which is not of type 'XContainer'")
                ).Go();

                var ns = this.Namespace(node);

                XElement element;

                element = ns != null ? new XElement(ns + label) : new XElement(label);

                parent.Add(element);
                targets.Add(element);
            }

            return new DomCursor(targets);
        }

        /// <summary>
        /// Checks for namespace in node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private XNamespace Namespace(XNode node)
        {
            XNamespace ns = null;
            if (node is XElement)
            {
                var elmnt = node as XElement;
                ns = elmnt.Name.Namespace;
            }
            return ns;
        }
    }
}
