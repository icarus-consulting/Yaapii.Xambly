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

using System;
using System.Collections.Generic;
using System.Xml;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Stack;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Error;
using System.IO;
using System.Text;

namespace Yaapii.Xml.Xembly
{
    /// <summary>
    /// Processor of Xembly directives, main entry point to the module.
    /// 
    /// <para>For example, to modify a DOM document:
    /// 
    /// <example>
    /// XMLDocument dom = ....
    /// new Xembler(
    /// new Directives()
    ///     .xpath("/root")
    ///     .addIfAbsent("employees")
    ///     .add("employee")
    ///     .attr("id", 6564)
    /// ).apply(dom);
    /// </example>
    /// </para>
    /// 
    /// <para>You can also convert your Xembly directives directly to XML document:
    /// 
    /// <example>
    /// String xml = new Xembler(
    /// new Directives()
    ///     .xpath("/root")
    ///     .addIfAbsent("employees")
    ///     .add("employee")
    ///     .attr("id", 6564)
    /// ).xml("root");
    /// </example>
    /// </para>
    /// </summary>
    public sealed class Xembler
    {
        /// <summary>
        /// The directives to apply.
        /// </summary>
        private readonly IEnumerable<IDirective> _directives;

        public Xembler(params IDirective[] directives) : this(
            new EnumerableOf<IDirective>(directives))
        { }

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="directives">Directives</param>
        public Xembler(IEnumerable<IDirective> directives)
        {
            this._directives = directives;
        }

        /// <summary>
        /// Apply all changes to the document/node, redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        /// <param name="dom">DOM.</param>
        public XmlNode ApplyQuietly(XmlNode dom)
        {
            try
            {
                return this.Apply(dom);
            }
            catch (Exception ex)
            {
                throw new IllegalStateException(
                    new FormattedText("quietly failed to apply DOM: {0}", this._directives).AsString(),
                    ex);
            }
        }

        /// <summary>
        /// Apply all changes to the document/node.
        /// </summary>
        /// <returns>Same document/node.</returns>
        /// <param name="dom">DOM document/node</param>
        public XmlNode Apply(XmlNode dom)
        {
            ICursor cursor = new DomCursor(new EnumerableOf<XmlNode>(dom));
            int pos = 1;

            IStack stack = new DomStack();

            foreach (var dir in this._directives)
            {
                try
                {
                    cursor = dir.Exec(dom, cursor, stack);
                }
                catch (ImpossibleModificationException ex)
                {
                    throw new ImpossibleModificationException(
                        new FormattedText("directive {0}: {1}", pos, dir).AsString());
                }
                catch (Exception ex) //TODO: Original catches DOMException. We don't have that. But do we have something similar?
                {
                    throw new ImpossibleModificationException(
                        new FormattedText("Exception at dir {0}: {1}", pos, dir).AsString());
                }
                ++pos;
            }
            return dom;
        }

        /// <summary>
        /// Apply all changes to an empty DOM, redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        public XmlDocument DomQuietly()
        {
            try
            {
                return this.Dom();
            }
            catch (Exception ex)
            {
                throw new IllegalStateException(
                    new FormattedText("failed to create DOM: {0}", this._directives).AsString(),
                    ex);
            }
        }

        /// <summary>
        /// Apply all changes to an empty DOM.
        /// </summary>
        /// <returns>The DOM</returns>
        public XmlDocument Dom()
        {
            var dom = new XmlDocument();
            this.Apply(dom);
            return dom;
        }

        /// <summary>
        /// COnvert to XML Document, redirect all Exceptions to IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        public String XmlQuietly()
        {
            try
            {
                return this.Xml();
            }
            catch (Exception ex)
            {
                throw new IllegalStateException(
                    new FormattedText("quietly failed to build XML: {0}", this._directives).AsString(),
                    ex);
            }
        }

        /// <summary>
        /// Convert to XML Document.
        /// </summary>
        /// <returns>The xml.</returns>
        public string Xml()
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                Dom().WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Escape text before using it as a text value
        /// </summary>
        /// <returns>The escaped.</returns>
        /// <param name="text">Text.</param>
        public String Escaped(string text)
        {
            var output = new StringBuilder(text.Length);
            var chars = text.ToCharArray();
            foreach (var chr in chars)
            {
                var illegal =
                    chr >= 0x00 && chr <= 0x08
                    || chr >= 0x0B && chr <= 0x0C
                    || chr >= 0x0E && chr <= 0x1F
                    || chr >= 0x7F && chr <= 0x84
                    || chr >= 0x86 && chr <= 0x9F;

                if (illegal)
                {
                    output.Append(new FormattedText("\\u{0:000}", (int)chr).AsString());
                }
                else
                {
                    output.Append(chr);
                }
            }
            return output.ToString();
        }
    }
}
