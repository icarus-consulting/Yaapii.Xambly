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
using System.Text;
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Xambly.Arg
{
    /// <summary>
    /// XML content with unescaped representation of all XML symbols.
    /// </summary>
    public class Unescaped : IText
    {
        private readonly IScalar<string> source;

        /// <summary>
        /// Un-escape all XML symbols.
        /// </summary>
        /// <param name="src">The XML text</param>
        public Unescaped(IArg src) : this(
            new ScalarOf<string>(() =>
                src.AsString()
            )
        )
        { }

        /// <summary>
        /// Un-escape all XML symbols.
        /// </summary>
        /// <param name="src">The XML text</param>
        public Unescaped(string src) : this(
            new ScalarOf<string>(src)
        )
        { }

        /// <summary>
        /// Un-escape all XML symbols.
        /// </summary>
        /// <param name="src">The XML text</param>

        private Unescaped(IScalar<string> src)
        {
            this.source = src;
        }

        /// <summary>
        /// Un-escaped XML content.
        /// </summary>
        /// <returns>XML string</returns>
        /// <exception cref="ArgumentOutOfRangeException">If fails</exception>
        /// <exception cref="XmlException">If fails</exception>
        public string AsString()
        {
            var str = source.Value();
            var chars = str.ToCharArray();
            var output = new StringBuilder(str.Length);

            for (int idx = 0; idx < chars.Length; idx++)
            {
                if (chars[idx] == '&')
                {
                    var sbuf = new StringBuilder(0);
                    while (chars[idx] != ';')
                    {
                        ++idx;
                        if (idx == chars.Length)
                        {
                            throw new XmlException("reached EOF while parsing XML symbol");
                        }
                        sbuf.Append(chars[idx]);
                    }
                    output.Append(
                        new Symbol(
                            new SubText(
                                sbuf.ToString(),
                                0,
                                sbuf.Length - 1
                            )
                        ).Value()
                    );
                }
                else
                {
                    output.Append(chars[idx]);
                }
            }
            return output.ToString();
        }

        /// <summary>
        /// Compare this with other text.
        /// </summary>
        /// <param name="other">Content for comparison</param>
        /// <returns>Comparision result</returns>
        public bool Equals(IText other)
        {
            return this.AsString().Equals(other.AsString());
        }
    }
}
