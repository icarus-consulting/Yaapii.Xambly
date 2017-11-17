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

using System;
using System.Xml;
using Yaapii.Xml.Xembly.Arg;
using Yaapii.Atoms.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;
using Yaapii.Atoms.Scalar;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.List;

namespace Yaapii.Xml.Xembly
{
    /// <summary>
    /// XPATH directive.
    /// Moves cursor to the nodes found by XPath
    /// </summary>
    public class XpathDirective : IDirective
    {
        /// <summary>
        /// XPath factory.
        /// </summary>
        private readonly IArg _expr;
        /// <summary>
        /// Pattern to match root-only XPath queries.
        /// </summary>
        private static readonly Regex ROOT_ONLY = new Regex(@"/([^\/\(\[\{:]+)");

        /// <summary>
        /// XPATH directive.
        /// Moves cursor to the nodes found by XPath
        /// </summary>
        /// <param name="path">XPath</param>
        /// <exception cref="XmlContentException">If invalid input</exception>
        public XpathDirective(string path)
        {
            this._expr = new ArgOf(path);
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new FormattedText("XPATH {0}", this._expr).AsString();
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
            IEnumerable<XmlNode> targets;
            string query = this._expr.Raw();

            //if(false)
            if (ROOT_ONLY.IsMatch(query))
            {
                targets = this.RootOnly(ROOT_ONLY.Match(query).Groups[1].Value, dom);
            }
            else
            {
                targets = this.Traditional(query, dom, cursor);
            }

            return new DomCursor(targets);

        }

        /// <summary>
        /// Fetch them in traditional way.
        /// </summary>
        /// <param name="query">XPath query</param>
        /// <param name="dom">Document</param>
        /// <param name="current">Nodes we're currently at</param>
        /// <returns>Found nodes</returns>
        /// <exception cref="ImpossibleModificationException">If fails</exception>"
        private IEnumerable<XmlNode> Traditional(string query, XmlNode dom, IEnumerable<XmlNode> current)
        {
            var targets = new HashSet<XmlNode>();
            foreach(XmlNode node in this.Roots(dom, current))
            {
                XmlNodeList list;
                try
                {
                    list = node.SelectNodes(query);
                }
                catch(Exception ex)
                {
                    throw new ImpossibleModificationException(
                        new FormattedText("invalid XPath expr '{0}'", query).AsString(), ex);
                }
                int len = list.Count;
                for (int idx = 0; idx < len;++idx)
                {
                    targets.Add(list.Item(idx));
                }
            }

            return targets;
        }

        /// <summary>
        /// Fetches only root node.
        /// The root node is found if <paramref name="root"/> contains "*" or the root node name.
        /// </summary>
        /// <param name="root">Root node name</param>
        /// <param name="dom">Document</param>
        /// <returns>Found nodes</returns>
        private IEnumerable<XmlNode> RootOnly(string root, XmlNode dom)
        {
            var target = new XmlDocumentOf(dom).Value().DocumentElement;
            var targets = new EnumerableOf<XmlNode>();  // empty list

            if (root != null && 
                target != null && 
                ("*".Equals(root) || target.Name.Equals(root)))
            {
                targets = new EnumerableOf<XmlNode>(target);
            }
            return targets;
        }

        /// <summary>
        /// Get roots to start searching from.
        /// The root nodes are the <paramref name="nodes"/> if there are any or the document root node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="nodes">Current nodes</param>
        /// <returns>Root nodes to start searching from</returns>
        private IEnumerable<XmlNode> Roots(XmlNode dom, IEnumerable<XmlNode> nodes)
        {
            IEnumerable<XmlNode> roots = nodes;

            // Return document root if there are no nodes.
            if (new Atoms.List.LengthOf(nodes).Value() == 0)
            {
                roots = new EnumerableOf<XmlNode>(
                    new XmlDocumentOf(
                        dom
                    ).Value().DocumentElement);
            }

            return roots;
        }
    }
}
