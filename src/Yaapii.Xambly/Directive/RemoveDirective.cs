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

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// REMOVE directive.
    /// Removes all current nodes.
    /// </summary>
    public sealed class RemoveDirective : IDirective
    {
        /// <summary>
        /// REMOVE directive.
        /// Removes all current nodes.
        /// </summary>
        public RemoveDirective()
        { }

        /// <summary>
        /// String representation.
        /// </summary>
        public override string ToString()
        {
            return "REMOVE";
        }

        /// <summary>
        /// Execute it in the given document with current position at the given node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="cursor">Nodes we're currently at</param>
        /// <param name="stack">Execution stack</param>
        /// <param name="context">Context that knows XML namespaces</param>
        /// <returns>New current nodes</returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack, IXmlNamespaceResolver context)
        {
            var parents = new HashSet<XNode>();

            foreach (var node in cursor)
            {
                XNode parent = node.Parent;
                new FailPrecise(
                    new FailNull(
                        parent
                    ),
                    new IllegalArgumentException("you can't delete root document element from XML")
                ).Go();
                new FailPrecise(
                    new FailWhen(
                        parent.NodeType == XmlNodeType.Document
                    ),
                    new IllegalArgumentException("you can't delete root document element from XML")
                ).Go();
                node.Remove();
                parents.Add(parent);
            }

            return new DomCursor(parents);
        }
    }
}
