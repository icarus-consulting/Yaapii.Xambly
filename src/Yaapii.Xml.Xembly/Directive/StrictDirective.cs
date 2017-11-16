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
using System.Linq;
using System.Xml;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly
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

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public ICursor Exec(XmlNode dom, ICursor cursor, IStack stack)
        {
            if (cursor.Count() != _number)
            {
                if (cursor.Count() == 0)
                {
                    throw new ImpossibleModificationException(new FormattedText("no current nodes while {0} expected", _number).AsString());
                }
                if (cursor.Count() == 1)
                {
                    throw new ImpossibleModificationException(new FormattedText("one current node '{0}' while strictly {1} expected", cursor.First().Name, _number).AsString());
                }
                throw new ImpossibleModificationException(new FormattedText("{0} current nodes [{1}] while strictly {2} expected", cursor.Count(), Names(cursor), _number).AsString());
            }

            return cursor;
        }

        public override int GetHashCode()
        {
            return _number.GetHashCode();
        }

        public override string ToString()
        {
            return new FormattedText("STRICT \"{0}\"", _number).AsString();
        }

        /// <summary> Node names as a string. </summary>
        /// <param name="nodes"> IEnumerable of nodes </param>
        /// <returns> Text presentation of them </returns>
        private string Names(IEnumerable<XmlNode> nodes)
        {
            var nodeNames = new Mapped<XmlNode, string>(nodes, tNode => $"{tNode.ParentNode?.Name + String.Empty}/{tNode.Name}");

            return new JoinedText(", ", nodeNames).AsString();
        }
    }
}