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

using System.Text;

namespace Yaapii.Xml.Xembly.Arg
{
    /// <summary>
    /// Argument properly escaped.
    /// </summary>
    public class ArgOf : IArg
    {
        private readonly string _value;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="val">Value of it</param>
        /// <exception cref="XmlContentException">If fails</exception>
        public ArgOf(string val)
        {
            foreach (char chr in val.ToCharArray())
            {
                new NotIllegal(chr).Value();
            }
            this._value = val;
        }

        /// <summary>
        /// The string representation.
        /// </summary>
        /// <returns>String</returns>
        public string AsString()
        {
            var escaped = new Escaped(this._value).AsString();
            return
                new StringBuilder(this._value.Length + 2 + escaped.Length)
                        .Append('"')
                        .Append(escaped)
                        .Append('"')
                        .ToString();
        }

        public override string ToString()
        {
            return AsString();
        }

        /// <summary>
        /// Get it's raw value.
        /// </summary>
        /// <returns>Value</returns>
        public string Raw()
        {
            return this._value;
        }
    }
}
