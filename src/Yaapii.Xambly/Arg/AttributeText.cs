﻿// MIT License
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

using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Xambly.Arg
{
    /// <summary>
    /// XML content with escaped representation of all unprintable XML symbols.
    /// </summary>
    public class AttributeText : IText
    {
        private readonly IText src;

        /// <summary>
        /// Escape all unprintable characters.
        /// </summary>
        /// <param name="src">Raw text</param>
        public AttributeText(string src) : this(new TextOf(src))
        { }

        /// <summary>
        /// Escape all unprintable characters.
        /// </summary>
        /// <param name="src">Raw text</param>
        public AttributeText(IText src)
        {
            this.src = src;
        }

        /// <summary>
        /// Clean text.
        /// </summary>
        /// <returns>The text</returns>
        public string AsString()
        {
            Validate();
            return
                new StringBuilder(this.src.AsString().Length + 2 + this.src.AsString().Length)
                    .Append('"')
                    .Append(this.src.AsString())
                    .Append('"')
                    .ToString();
        }

        /// <summary>
        /// Compare this with other text.
        /// </summary>
        /// <param name="other">Content for comparision</param>
        /// <returns>Comparision result</returns>
        public bool Equals(IText other)
        {
            return other.AsString().Equals(this.AsString());
        }


        /// <summary>
        /// Validates the value by checking for illegal characters
        /// </summary>
        private void Validate()
        {
            foreach (char chr in this.src.AsString().ToCharArray())
            {
                new NotIllegal(chr).Value();
            }
        }
    }
}
