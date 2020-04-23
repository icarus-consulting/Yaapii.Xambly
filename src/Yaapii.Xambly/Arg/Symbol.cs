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
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Texts;

namespace Yaapii.Xambly.Arg
{
    /// <summary>
    /// Character repensentation of a XML symbol.
    /// </summary>
    public class Symbol : IScalar<Char>
    {
        private readonly IText _src;

        /// <summary>
        /// Convert XML symbol to char.
        /// </summary>
        /// <param name="str">XML symbol</param>
        public Symbol(string str) : this(
            new TextOf(str))
        { }

        /// <summary>
        /// Convert XML symbol to char.
        /// </summary>
        /// <param name="src">XML symbol</param>
        public Symbol(IText src)
        {
            this._src = src;
        }

        /// <summary>
        /// XML symbol as char.
        /// </summary>
        /// <returns>The character</returns>
        public char Value()
        {
            var src = _src.AsString();
            char chr;
            if (src[0] == '#')
            {
                chr = 
                    new NotIllegal((char)
                        new IntOf(
                            new SubText(_src, 1)).Value()).Value();
            }
            else if (String.Compare(src, "apos", true) == 0)
            {
                chr = '\'';
            }
            else if (String.Compare(src, "quot", true) == 0)
            {
                chr = '"';
            }
            else if (String.Compare(src, "lt", true) == 0)
            {
                chr = '<';
            }
            else if (String.Compare(src, "gt", true) == 0)
            {
                chr = '>';
            }
            else if (String.Compare(src, "amp", true) == 0)
            {
                chr = '&';
            }
            else
            {
                throw new XmlException(
                        new Formatted(
                            "unknown XML symbol &{0};",
                            _src
                        ).AsString());
            }
            return chr;
        }
    }
}
