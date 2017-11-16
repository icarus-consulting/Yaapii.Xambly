using System;
using System.Xml;
using Yaapii.Xml.Xembly.Arg;
using Yaapii.Atoms.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.ObjectModel;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.List;

namespace Yaapii.Xml.Xembly.Directive
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
        private static readonly Regex ROOT_ONLY = new Regex("/([^/\\(\\[\\{:]+)");

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
            ICollection<XmlNode> targets;
            string query = this._expr.Raw();

            if(ROOT_ONLY.IsMatch(query))
            {
                targets = this.RootOnly(ROOT_ONLY.Match(query).Value, dom);
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
        private ICollection<XmlNode> Traditional(string query, XmlNode dom, IEnumerable<XmlNode> current)
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
        /// </summary>
        /// <param name="root">Root node name</param>
        /// <param name="dom">Document</param>
        /// <returns>Found nodes</returns>
        private ICollection<XmlNode> RootOnly(string root, XmlNode dom)
        {
            XmlNode target = new XmlDocumentOf(dom).Value();

            var targets = new List<XmlNode>();
            if (root != null && target != null
               && root.Equals("*") || target.Name.Equals(root))
            {
                targets.Add(target);
            }
            return targets;
        }

        /// <summary>
        /// Get roots to start searching from.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="nodes">Current nodes</param>
        /// <returns>Root nodes to start searching from</returns>
        private IEnumerable<XmlNode> Roots(XmlNode dom, IEnumerable<XmlNode> nodes)
        {
            ICollection<XmlNode> roots = new List<XmlNode>();
            if (new Yaapii.Atoms.List.LengthOf(nodes).Value() == 0)
            {
                roots.Add(dom);
            }
            else
            {
                roots.Add(dom.OwnerDocument);
            }

            return roots;
        }
    }
}
