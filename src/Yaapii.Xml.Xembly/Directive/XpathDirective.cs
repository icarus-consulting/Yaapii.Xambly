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
    public class XpathDirective : IDirective
    {
        private readonly IArg _expr;
        private static readonly Regex ROOT_ONLY = new Regex("/([^/\\(\\[\\{:]+)");

        public XpathDirective(string path)
        {
            this._expr = new ArgOf(path);
        }

        public override string ToString()
        {
            return new FormattedText("XPATH {0}", this._expr).AsString();
        }

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
