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
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Arg;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive
{
    /// <summary>
    /// ATTR directive.
    /// </summary>
    public sealed class AttrDirective : IDirective
    {
        private readonly IScalar<IArg> name;
        private readonly IScalar<IText> value;

        /// <summary>
        /// ATTR directive.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Text value to set</param>
        public AttrDirective(string name, string value) : this(
            new ScalarOf<IArg>(() => new AttributeArg(name)),
            new ScalarOf<IText>(() => new TextOf(value))
        )
        { }

        /// <summary>
        /// ATTR directive.
        /// </summary>
        /// <param name="name">Attribute name</param>
        /// <param name="value">Text value to set</param>
        public AttrDirective(IScalar<IArg> name, IScalar<IText> value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// String representation.
        /// </summary>
        /// <returns>The string</returns>
        public override string ToString()
        {
            return 
                new FormattedText(
                    "ATTR '{0}', '{1}'",
                    this.name.Value().Raw(),
                    this.value.Value().AsString()
                ).AsString();
        }

        /// <summary>
        /// Execute it in the given document with current position at the given node.
        /// </summary>
        /// <param name="dom">Document</param>
        /// <param name="cursor">Nodes we're currently at</param>
        /// <param name="stack">Execution stack</param>
        /// <returns>New current nodes</returns>
        public ICursor Exec(XNode dom, ICursor cursor, IStack stack)
        {
            var key = this.name.Value().Raw();
            var value = this.value.Value().AsString();

            foreach (var node in cursor)
            {
                try
                {
                    ((XElement)node).SetAttributeValue(key, value);
                }
                catch (InvalidCastException ex)
                {
                    throw new ImpossibleModificationException($"Unable to set attribute to node '{node.ToString(SaveOptions.DisableFormatting)}'. Maybe try to access the root node by the XPath '/' that provides the Document. Instead, use '/*' to get the root Element.", ex);
                }
            }
            return cursor;
        }
    }
}