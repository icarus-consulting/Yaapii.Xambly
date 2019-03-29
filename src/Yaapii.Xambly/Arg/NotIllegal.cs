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
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Xambly.Arg
{
    /// <summary>
    /// A legal XML character.
    /// </summary>
    public class NotIllegal : IScalar<Char>
    {
        private readonly char _chr;

        /// <summary>
        /// Validate char number and throw exception if it's not legal.
        /// </summary>
        /// <param name="chr"></param>
        public NotIllegal(char chr)
        {
            this._chr = chr;
        }

        /// <summary>
        /// Validate char number and throw exception if it's not legal.
        /// </summary>
        /// <returns>The same number</returns>
        public Char Value()
        {
            this.Range(_chr, 0x00, 0x08);
            this.Range(_chr, 0x0B, 0x0C);
            this.Range(_chr, 0x0E, 0x1F);
            this.Range(_chr, 0x7F, 0x84);
            this.Range(_chr, 0x86, 0x9F);
            return _chr;
        }

        /// <summary>
        /// Throw if number is in the range.
        /// </summary>
        /// <param name="c">Char number</param>
        /// <param name="left">Left number</param>
        /// <param name="right">Right number</param>
        /// <exception cref="System.Xml.XmlException">If illegal</exception>
        private void Range(char c, int left, int right)
        {
            if (c >= left && c <= right)
            {
                throw new System.Xml.XmlException(
                    new FormattedText(
                        "Character {0} is in the restricted XML range {1} - {2}, see http://www.w3.org/TR/2004/REC-xml11-20040204/#charsets",
                        c, left, right
                    ).AsString()
                );
            }
        }
    }
}
