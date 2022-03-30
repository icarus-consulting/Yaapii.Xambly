// MIT License
//
// Copyright(c) 2022 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,
// modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
// is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Xml.Linq;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// STRICT directive.
    ///
    /// The class is immutable and thread-safe.
    /// </summary>
    public sealed class StrictDirective : IDirective
    {
        private readonly int number;

        /// <summary>
        /// STRICT directive.
        ///
        /// The class is immutable and thread-safe.
        /// </summary>
        /// <param name="nodes">Number of node expected</param>
        public StrictDirective(int nodes)
        {
            this.number = nodes;
        }

        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj">object to check</param>
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Execute the directive
        /// </summary>
        /// <param name="dom">node to execute on</param>
        /// <param name="cursor">cursor</param>
        /// <param name="stack">the stack</param>
        /// <returns>New current nodes</returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack)
        {
            var lengthOfCursor = new Atoms.Enumerable.LengthOf(cursor).Value();

            if (lengthOfCursor != this.number)
            {
                if (lengthOfCursor == 0)
                {
                    throw new ImpossibleModificationException(
                        new Formatted(
                            "no current nodes while {0} expected",
                            this.number
                        ).AsString());
                }
                if (lengthOfCursor == 1)
                {
                    throw new ImpossibleModificationException(
                        new Formatted(
                            "one current node '{0}' while strictly {1} expected",
                            new Yaapii.Atoms.Enumerable.ItemAt<XNode>(cursor).Value().ToString(SaveOptions.DisableFormatting),
                            this.number
                        ).AsString());
                }
                throw new ImpossibleModificationException(
                    new Formatted(
                        "{0} current nodes [{1}] while strictly {2} expected",
                        lengthOfCursor,
                        Names(cursor),
                        this.number
                    ).AsString());
            }

            return cursor;
        }

        /// <summary>
        /// Haskcode of this StrictDirective
        /// </summary>
        public override int GetHashCode()
        {
            return this.number.GetHashCode();
        }

        /// <summary>
        /// This StrictDirective as a string
        /// </summary>
        public override string ToString()
        {
            return new Formatted("STRICT \"{0}\"", this.number).AsString();
        }

        private string Names(IEnumerable<XNode> nodes)
        {
            var nodeNames =
                new Mapped<XNode, string>(
                    tNode => new Formatted(
                        "{0}/{1}",
                        tNode.Parent?.Name + string.Empty,
                        (tNode as XElement).Name
                    ).AsString(),
                    nodes
                );

            return new Atoms.Text.Joined(", ", nodeNames).AsString();
        }
    }
}
