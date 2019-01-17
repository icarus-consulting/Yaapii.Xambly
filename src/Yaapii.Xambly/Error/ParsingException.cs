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
using Yaapii.Atoms.Text;
namespace Yaapii.Xambly.Error
{
    /// <summary>
    /// When parsing of directives is impossible.
    /// </summary>
    public class ParsingException : Exception
    {
        /// <summary>
        /// When parsing of directives is impossible.
        /// </summary>
        /// <param name="innerException">Original exception</param>
        public ParsingException(Exception innerException) 
            : this(
                new FormattedText("Error parsing script: {0}", innerException.Message).AsString(),
                innerException)
        { }

        /// <summary>
        /// When parsing of directives is impossible.
        /// </summary>
        /// <param name="cause">Cause of it</param>
        public ParsingException(string cause) 
            : this(cause, null)
        { }

        /// <summary>
        /// When parsing of directives is impossible.
        /// </summary>
        /// <param name="cause">Cause of it</param>
        /// <param name="innerException">Original exception</param>
        public ParsingException(string cause, Exception innerException) 
            : base(cause, innerException)
        { }
    }
}
