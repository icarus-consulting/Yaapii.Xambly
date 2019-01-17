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

using System;
using System.Xml.Linq;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Arg;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// Namespace directive.
    /// Sets namespace of all current nodes
    /// </summary>
    public class NsDirective : IDirective
    {
        private readonly IArg nsp;
        private readonly IArg prefix;

        public NsDirective(string prefix, string nsp) : this(new ArgOf(prefix),new ArgOf(nsp))
        { }

        public NsDirective(string nsp): this(new ArgOf(""), new ArgOf(nsp))
        { }

        public NsDirective(IArg nsp) : this(new ArgOf(""),nsp)
        { }

        /// <summary>
        /// Namespace directive.
        /// Sets namespace of all current nodes
        /// </summary>
        /// <param name="nsp"></param>
        public NsDirective(IArg prefix,IArg nsp)
        {
            this.prefix = prefix;
            this.nsp = nsp;
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return new FormattedText(
                            "NS {0}",
                            this.nsp
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
            throw new ImpossibleModificationException("Modifying namespaces is not supported at the moment.");
            try
            {
                XElement element = null;
                if(dom is XDocument)
                {
                    element = (dom as XDocument).Root;
                } else
                {
                    element = dom as XElement;
                }

                new FailPrecise(
                    new FailNull(element),
                    new ArgumentException($"Node is not of type 'XElement'")
                ).Go();

                XNamespace xnsp = this.nsp.Raw();

                ApplyNamespace(element, xnsp);

                return cursor;
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException("can't set xmlns",ex);
            }
        }

        private void ApplyNamespace(XElement xelem, XNamespace xmlns)
        {
            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;
            foreach (var e in xelem.Elements())
                ApplyNamespace(e, xmlns);

        }

    }
}
