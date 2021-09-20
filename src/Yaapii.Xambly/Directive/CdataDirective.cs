// MIT License
//
// Copyright(c) 2019 ICARUS Consulting GmbH
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

using System.Xml.Linq;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Arg;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// CDATA directive.
    /// Sets CDATA value of current node
    /// </summary>
    public class CdataDirective : IDirective
    {
        private readonly IArg value;

        /// <summary>
        /// CDATA directive.
        /// Sets CDATA value of current node
        /// </summary>
        /// <param name="val">Text value to set</param>
        public CdataDirective(string val)
        {
            this.value = new AttributeArg(val);
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return 
                new Formatted(
                    $"CDATA {0}",
                    this.value.Raw()
                ).AsString();
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
            var doc = new XmlDocumentOf(dom).Value();

            var val = this.value.Raw();
            foreach (var node in cursor)
            {
                var ctn = node as XContainer;

                new FailPrecise(
                    new FailNull(ctn),
                    new ImpossibleModificationException("Cannot add CData to a node which is not of type XContainer")
                ).Go();

                var cdata = new XCData(val);
                (node as XContainer).Add(cdata);
            }

            return cursor;
        }
    }
}
