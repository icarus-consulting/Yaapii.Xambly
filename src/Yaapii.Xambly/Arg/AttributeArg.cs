// MIT License
//
// Copyright(c) 2021 ICARUS Consulting GmbH
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

namespace Yaapii.Xambly.Arg
{
    /// <summary>
    /// Argument properly escaped.
    /// </summary>
    public class AttributeArg : IArg
    {
        private readonly string value;

        /// <summary>
        /// Argument properly escaped.
        /// </summary>
        /// <param name="value">Value of it</param>
        /// <exception cref="NotIllegal">If fails</exception>
        public AttributeArg(string value)
        {
            this.value = value;
        }

        /// <summary>
        /// The string representation.
        /// </summary>
        /// <returns>String</returns>
        public string AsString()
        {
            Validate();
            return
                new StringBuilder(this.value.Length + 2 + this.value.Length)
                    .Append('"')
                    .Append(this.value)
                    .Append('"')
                    .ToString();
        }

        /// <summary>
        /// This arg as a string
        /// </summary>
        /// <returns>string</returns>
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
            Validate();
            return this.value;
        }

        /// <summary>
        /// Validates the value by checking for illegal characters
        /// </summary>
        private void Validate()
        {
            foreach (char chr in this.value.ToCharArray())
            {
                new NotIllegal(chr).Value();
            }
        }
    }
}
