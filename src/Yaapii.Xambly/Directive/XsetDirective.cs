using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Arg;

namespace Yaapii.Xambly.Directive
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
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack)
        { 
            XPathNavigator nav = dom.CreateNavigator();
            Dictionary<XElement, string> values = new Dictionary<XElement, string>(0);

            foreach (XNode node in cursor)
            {
                var elmnt = node as XElement;
                values.Add(elmnt, nav.Evaluate(_expr.Raw()).ToString());   
            }

            foreach (KeyValuePair<XElement, string> pair in values)
            {
                (pair.Key).Value = pair.Value;
            }
            return cursor;
        }

    }
}
