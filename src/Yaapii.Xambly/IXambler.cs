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

using System.Xml;
using System.Xml.Linq;

namespace Yaapii.Xambly
{
    /// <summary>
    /// Interface for Processor of Xambly directives (main entry point to the module)
    /// </summary>
    public interface IXambler
    {
        /// <summary>
        /// Should apply all changes to the document/node.
        /// </summary>
        /// <returns>Same document/node.</returns>
        /// <param name="dom">DOM document/node</param>
        XNode Apply(XNode dom);

        /// <summary>
        /// Should apply all changes to the document/node, but redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        /// <param name="dom">DOM.</param>
        XNode ApplyQuietly(XNode dom);

        /// <summary>
        /// Should apply all changes to an empty DOM.
        /// </summary>
        /// <returns>The DOM</returns>
        XDocument Dom();

        /// <summary>
        /// Should apply all changes to an empty DOM, but redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        XDocument DomQuietly();

        /// <summary>
        /// Should escape text before using it as a text value
        /// </summary>
        /// <returns>The escaped.</returns>
        /// <param name="text">Text.</param>
        string Escaped(string text);

        /// <summary>
        /// Should convert to XML Document.
        /// </summary>
        /// <returns>The xml as string</returns>
        /// <param name="createHeader">Option to get the XML Document with or without header</param>
        string Xml(bool createHeader = true);

        /// <summary>
        /// Should convert to XML Document, but redirect all Exceptions to IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        /// <param name="createHeader">Option to get the XML Document with or without header</param>
        string XmlQuietly(bool createHeader = true);
    }
}
