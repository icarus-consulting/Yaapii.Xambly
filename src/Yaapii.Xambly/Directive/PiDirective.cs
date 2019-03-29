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
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Arg;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// PI directive.
    /// Adds processing instruction.
    /// </summary>
    public class PiDirective : IDirective
    {
        private readonly IArg target;
        private readonly IArg data;

        /// <summary>
        /// PI directive.
        /// Adds processing instruction.
        /// </summary>
        /// <param name="target">Target</param>
        /// <param name="data">Data</param>
        public PiDirective(string target, string data)
        {
            this.target = new AttributeArg(target);
            this.data = new AttributeArg(data);
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new FormattedText(
                "PI {0} {1}",
                this.target.Raw(),
                this.data.Raw()
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
            var pi = new XProcessingInstruction(this.target.Raw(), this.data.Raw());
            // if cursor list is empty
            if(new LengthOf(cursor).Value() == 0){
                doc.Root.AddBeforeSelf(pi);
            } else {
                foreach (var node in cursor)
                {
                    (node as XContainer).Add(pi);
                }
            }

            return cursor;
        }
    }
}
