// MIT License
//
// Copyright(c) 2022 ICARUS Consulting GmbH
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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Func;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// XPATH directive.
    /// Moves cursor to the nodes found by XPath.
    /// </summary>
    public class XpathDirective : IDirective
    {
        /// <summary>
        /// An absolute XPath stards with exat on "/" at the beginning.
        /// This regular expression checks if the query stards with exact one "/" followed by any character except a second "/" (like "//")
        /// </summary>
        private const string ABSOLUTE_XPATH_REGEX = @"^((?:\/(?!\/)).*)$";

        private readonly string expr;
        private readonly IXmlNamespaceResolver context;


        /// <summary>
        /// XPATH directive.
        /// Moves cursor to the nodes found by XPath.
        /// </summary>
        public XpathDirective(string path) : this(
            path,
            new XmlNamespaceManager(new NameTable())
        )
        { }


        /// <summary>
        /// XPATH directive.
        /// Moves cursor to the nodes found by XPath.
        /// </summary>
        public XpathDirective(string path, IXmlNamespaceResolver context)
        {
            this.expr = path;
            this.context = context;
        }

        /// <summary>
        /// String representation.
        /// </summary>
        public override string ToString()
        {
            return
                $"XPATH {this.expr}";
        }

        /// <summary>
        /// Execute it in the given document with current position at the given node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="cursor">Nodes we're currently at</param>
        /// <param name="stack">Execution stack</param>
        /// <returns>New current nodes</returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack)
        {
            IEnumerable<XNode> current = cursor;
            if (IsAbsoluteXPath())
            {
                current =
                    new ManyOf<XNode>(
                        new XmlDocumentOf(dom).Value().Document
                    );
            }

            return
                new DomCursor(
                    Traditional(dom, current)
                );

        }

        /// <summary>
        /// Fetch them in traditional way.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="current">Nodes we're currently at</param>
        /// <returns>Found nodes</returns>
        /// <exception cref="ImpossibleModificationException">If fails</exception>"
        private IEnumerable<XNode> Traditional(XNode dom, IEnumerable<XNode> current)
        {
            var targets = new HashSet<XNode>();
            foreach (XNode node in Roots(dom, current))
            {
                IEnumerable<XElement> list;
                try
                {
                    list =
                        node.XPathSelectElements(
                            this.expr,
                            this.context
                        );
                }
                catch (Exception ex)
                {
                    throw
                        new ImpossibleModificationException(
                            $"Invalid XPath expr '{this.expr}' ({ex.Message})",
                            ex
                        );
                }

                new Each<XElement>(
                    elmnt => targets.Add(elmnt),
                    list
                ).Invoke();
            }

            return targets;
        }

        private bool IsAbsoluteXPath()
        {
            return
                new Regex(
                    ABSOLUTE_XPATH_REGEX
                ).IsMatch(this.expr);
        }

        ///// <summary>
        ///// Fetches only root node.
        ///// The root node is found if <paramref name="root"/> contains "*" or the root node name.
        ///// </summary>
        ///// <param name="root">Root node name</param>
        ///// <param name="dom">Document</param>
        ///// <returns>Found nodes</returns>
        //private IEnumerable<XmlNode> RootOnly(string root, XmlNode dom)
        //{
        //    var rootElem = new XmlDocumentOf(dom).Value().DocumentElement;
        //    var targets = new ManyOf<XmlNode>();  // empty list

        //    if (
        //        root != null &&
        //        rootElem != null &&
        //        ("*".Equals(root) || rootElem.Name.Equals(root))
        //    )
        //    {
        //        targets = new ManyOf<XmlNode>(rootElem);
        //    }
        //    return targets;
        //}

        /// <summary>
        /// Get roots to start searching from.
        /// The root nodes are the <paramref name="nodes"/> if there are any or the document root node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="nodes">Current nodes</param>
        /// <returns>Root nodes to start searching from</returns>
        private IEnumerable<XNode> Roots(XNode dom, IEnumerable<XNode> nodes)
        {
            IEnumerable<XNode> roots = nodes;

            if (new LengthOf(nodes).Value() == 0)
            {
                roots =
                    new ManyOf<XNode>(
                        new XmlDocumentOf(
                            dom
                        ).Value().Document
                    );
            }
            roots =
                new Filtered<XNode>((node) =>
                    node != null,
                    roots
                );

            return roots;
        }
    }
}
