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

using System.Xml;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xambly.Error;

namespace Yaapii.Xml.Xambly.Directive
{
    /// <summary>
    /// Namespace directive.
    /// Sets namespace of all current nodes
    /// </summary>
    public class NsDirective : IDirective
    {
        private readonly IArg _namespace;

        /// <summary>
        /// Namespace directive.
        /// Sets namespace of all current nodes
        /// </summary>
        /// <param name="nsp"></param>
        public NsDirective(IArg nsp)
        {
            _namespace = nsp;
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new FormattedText(
                            "NS {0}",
                            this._namespace
                        ).AsString();
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
            try
            {
                var attr = new AttrDirective(
                                    "xmlns",
                                    this._namespace.Raw()
                                );
                return attr.Exec(dom, cursor, stack);
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException("can't set xmlns",ex);
            }
        }
    }
}
