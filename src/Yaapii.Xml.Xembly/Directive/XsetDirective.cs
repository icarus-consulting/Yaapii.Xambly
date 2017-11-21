using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Arg;
using System.Xml.XPath;
using System.Collections;
using System.Collections.Concurrent;

namespace Yaapii.Xml.Xembly.Directive
{
    /// <summary>
    /// Sets by xpath.
    /// </summary>
    public sealed class XsetDirective : IDirective
    {
        /// <summary>
        /// Sets the Textvalue in specified Nodes
        /// </summary>
        private readonly IArg _expr;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="val">Text value to set</param>
        public XsetDirective(string val)
        {
            _expr = new ArgOf(val);
        }

        /// <summary>
        /// This directive as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new FormattedText("XSET {0}", this._expr).AsString();
        }

        /// <summary>
        /// Sets the Text in the 
        /// </summary>
        /// <param name="dom">Node for the changes</param>
        /// <param name="cursor">Elements to change the text for</param>
        /// <param name="stack"></param>
        /// <returns></returns>
        public ICursor Exec(XmlNode dom, ICursor cursor, IStack stack)
        { 
            XPathNavigator nav = dom.CreateNavigator();
            Dictionary<XmlNode, string> values = new Dictionary<XmlNode, string>(0);

            foreach (XmlNode node in cursor)
            {
                values.Add(node, nav.Evaluate(_expr.Raw()).ToString());   
            }

            foreach (KeyValuePair<XmlNode, string> pair in values)
            {
                (pair.Key).InnerText = pair.Value;
            }
            return cursor;
        }

    }
}
