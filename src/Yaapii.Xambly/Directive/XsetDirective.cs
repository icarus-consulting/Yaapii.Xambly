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
        private readonly IArg expr;

        /// <summary>
        /// Sets by xpath.
        /// </summary>
        /// <param name="val">Text value to set</param>
        public XsetDirective(string val)
        {
            this.expr = new AttributeArg(val);
        }

        /// <summary>
        /// This directive as a string
        /// </summary>
        public override string ToString()
        {
            return new Formatted("XSET {0}", this.expr).AsString();
        }

        /// <summary>
        /// Sets the Text in the 
        /// </summary>
        /// <param name="dom">Node for the changes</param>
        /// <param name="cursor">Elements to change the text for</param>
        /// <param name="stack"></param>
        /// <returns>New current nodes</returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack)
        {
            var nav = dom.CreateNavigator();
            var values = new Dictionary<XElement, string>(0);

            foreach (XNode node in cursor)
            {
                var elmnt = node as XElement;
                values.Add(
                    elmnt,
                    nav.Evaluate(
                        this.expr.Raw()
                    ).ToString()
                );
            }
            foreach (KeyValuePair<XElement, string> pair in values)
            {
                pair.Key.Value = pair.Value;
            }

            return cursor;
        }

    }
}
