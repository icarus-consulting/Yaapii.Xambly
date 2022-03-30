﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Arg;
using Yaapii.Xambly.Cursor;

namespace Yaapii.Xambly
{
    /// <summary>
    /// ADDIFXPATH directive.
    /// Adds a node, if child element with specified content as text does not exist.
    /// </summary>
    public class AddIfChildDirective : IDirective
    {
        private readonly IArg name;
        private readonly IArg child;
        private readonly string content;

        /// <summary>
        /// ADDIF directive.
        /// Adds a node, if child element with specified content as text does not exist.
        /// </summary>
        /// <param name="node">Name of node to add</param>
        /// <param name="child">Name of child to match</param>
        /// <param name="content">content to match</param>
        public AddIfChildDirective(string node, string child, string content)
        {
            this.name = new AttributeArg(node);
            this.child = new AttributeArg(child);
            this.content = content;
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new Formatted("ADDIFCHILD {0}", this.name.Raw()).AsString();
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
            var targets = new List<XNode>();
            foreach (var node in cursor)
            {
                var kids = Children(node);
                XNode target = null;
                var len = kids.Count;
                for (int i = 0; i < len; i++)
                {
                    if (Matches(kids[i]))
                    {
                        {
                            target = kids[i];
                            break;
                        }
                    }
                }

                if (target == null)
                {
                    target = new XElement(this.name.Raw());// doc.CreateElement(this.name.Raw());
                    (node as XElement).Add(target);// AppendChild(target);
                }
                targets.Add(target);
            }
            return new DomCursor(targets);
        }

        private bool Matches(XNode node)
        {
            bool matches = false;
            var xElement = node as XElement;
            if (string.Compare(xElement.Name.LocalName, this.name.Raw(), true) == 0)
            {
                foreach (XNode child in Children(node))
                {
                    var xChild = child as XElement;
                    if (string.Compare(xChild.Name.LocalName, this.child.Raw(), true) == 0 && string.Compare(xChild.Value, this.content, true) == 0)
                    {
                        matches = true;
                        break;
                    }
                }
            }
            return matches;
        }

        private IList<XElement> Children(XNode node)
        {
            return new ListOf<XElement>((node as XElement).Elements().OfType<XElement>());
        }
    }
}
