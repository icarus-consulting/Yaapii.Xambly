// MIT License
//
// Copyright(c) 2017 ICARUS Consulting GmbH
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

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Yaapii.Atoms.List;
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
        /// <summary> Number of nodes we're expecting. </summary>
        private readonly int _number;

        /// <summary> Ctor. </summary>
        /// <param name="nodes"> Number of node expected </param>
        public StrictDirective(int nodes)
        {
            _number = nodes;
        }

        /// <summary>
        /// Checks for equality
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns>true if equal</returns>
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
        /// <returns></returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack)
        {
            var lengthOfCursor = new Yaapii.Atoms.Enumerable.LengthOf(cursor).Value();

            if (lengthOfCursor != _number)
            {
                if (lengthOfCursor == 0)
                {
                    throw new ImpossibleModificationException(
                        new FormattedText(
                            "no current nodes while {0} expected",
                            _number
                        ).AsString());
                }
                if (lengthOfCursor == 1)
                {
                    throw new ImpossibleModificationException(
                        new FormattedText(
                            "one current node '{0}' while strictly {1} expected",
                            new Yaapii.Atoms.Enumerable.ItemAt<XNode>(cursor).Value().ToString(SaveOptions.DisableFormatting),
                            _number
                        ).AsString());
                }
                throw new ImpossibleModificationException(
                    new FormattedText(
                        "{0} current nodes [{1}] while strictly {2} expected",
                        lengthOfCursor,
                        Names(cursor),
                        _number
                    ).AsString());
            }

            return cursor;
        }

        /// <summary>
        /// Haskcode of this StrictDirective
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _number.GetHashCode();
        }

        /// <summary>
        /// This StrictDirective as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new FormattedText("STRICT \"{0}\"", _number).AsString();
        }

        /// <summary> Node names as a string. </summary>
        /// <param name="nodes"> IEnumerable of nodes </param>
        /// <returns> Text presentation of them </returns>
        private string Names(IEnumerable<XNode> nodes)
        {
            var nodeNames = new Mapped<XNode, string>(
                tNode => new FormattedText(
                    "{0}/{1}", 
                    tNode.Parent?.Name + String.Empty,
                    (tNode as XElement).Name
                ).AsString(),
                nodes
            );

            return new JoinedText(", ", nodeNames).AsString();
        }
    }
}