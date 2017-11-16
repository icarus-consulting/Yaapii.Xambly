// MIT License
//
// Copyright(c) 2017 ICARUS Consulting GmbH
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

using System.Collections.Generic;
using System.Xml;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Arg;
using Yaapii.Xml.Xembly.Cursor;

namespace Yaapii.Xml.Xembly
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
        /// <exception cref="XmlContentException">If invalid input</exception>
        public AddDirective(string node)
        {
            this._name = new ArgOf(node);
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new FormattedText("ADD {0}", this._name).AsString();
        }

        /// <summary>
        /// Execute it in the given document with current position at the given node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="cursor">Nodes we're currently at</param>
        /// <param name="stack">Execution stack</param>
        /// <returns>New current nodes</returns>
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
