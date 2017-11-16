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
    public sealed class XsetDirective : IDirective
    {
        
        private readonly IArg _expr;

        public XsetDirective(string val)
        {
            _expr = new ArgOf(val);
        }

        public new string ToString()
        {
            return new FormattedText("XSET {0}", this._expr).AsString();
        }

        public ICursor Exec(XmlNode dom, ICursor cursor, IStack stack)
        { 

            XPathNavigator nav = dom.CreateNavigator();
            

            Dictionary<XmlNode, string> values = new Dictionary<XmlNode, string>(0);

            foreach (XmlNode node in cursor)
            {
                try
                {
                    values.Add(node, nav.Evaluate(_expr.Raw()).ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            foreach (KeyValuePair<XmlNode, string> pair in values)
            {
                (pair.Key).InnerText = pair.Value;
            }
            return cursor;
        }

    }
}


//public Directive.Cursor exec(final Node dom,
//        final Directive.Cursor cursor, final Directive.Stack stack)
//        throws ImpossibleModificationException
//{
//    final XPath xpath = XsetDirective.FACTORY.newXPath();

//    final ConcurrentMap<Node, String> values =
//            new ConcurrentHashMap<Node, String>(0);
//        for (final Node node : cursor) 
//        {
//            try 
//            {
//                values.put(
//                  node,
                  //xpath.evaluate(this.expr.raw(), node, XPathConstants.STRING
                  //              ).toString());
//            } 

//           catch (final XPathExpressionException ex) 
//           {
//                throw new ImpossibleModificationException(String.format("invalid XPath expr '%s'", this.expr), ex);
//           }
//        }
//       
//        for (final Map.Entry<Node, String> entry : values.entrySet()) 
//        {
//            entry.getKey().setTextContent(entry.getValue());
//        }
//        return cursor;
//}
