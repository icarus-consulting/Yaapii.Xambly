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
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Xambly.Directive
{
    ///<summary>
    /// Creates a collection of directives, which can create a copy
    /// of provided node.
    ///
    /// <param>For example, you already have a node in an XML document,
    /// which you'd like to add to another XML document:
    /// </param>
    ///
    /// <param> XmlDocument target = parse("&lt;root/&gt;");
    /// XmlNode node = parse("&lt;user name='Jeffrey'/&gt;");
    /// new Xambler(
    ///   new Directives()
    ///     .Xpath("////")
    ///     .Add("jeff")
    ///     .Append(new CopyOf(node))
    /// ).Apply(target);
    /// assert print(target).equals(
    ///   "&lt;root&gt;&lt;jeff name='Jeffrey'&gt;&lt;/root&gt;"
    /// );
    /// </param>
    /// </summary>
    public sealed class CopyOfDirective : IEnumerable<IDirective>
    {
        private readonly IScalar<XmlNode> _node;


        ///<summary>
        /// Creates a collection of directives, which can create a copy
        /// of provided node.
        ///
        /// <param>For example, you already have a node in an XML document,
        /// which you'd like to add to another XML document:
        /// </param>
        ///
        /// <param> XmlDocument target = parse("&lt;root/&gt;");
        /// XmlNode node = parse("&lt;user name='Jeffrey'/&gt;");
        /// new Xambler(
        ///   new Directives()
        ///     .Xpath("////")
        ///     .Add("jeff")
        ///     .Append(new CopyOf(node))
        /// ).Apply(target);
        /// assert print(target).equals(
        ///   "&lt;root&gt;&lt;jeff name='Jeffrey'&gt;&lt;/root&gt;"
        /// );
        /// </param>
        /// </summary>
        /// <param name="node"><see cref="XmlNode"/> to analyze</param>
        /// <returns>Collection of directives</returns>
        public CopyOfDirective(XmlNode node) : this(new ScalarOf<XmlNode>(node))
        { }

        ///<summary>
        /// Creates a collection of directives, which can create a copy
        /// of provided node.
        ///
        /// <param>For example, you already have a node in an XML document,
        /// which you'd like to add to another XML document:
        /// </param>
        ///
        /// <param> XmlDocument target = parse("&lt;root/&gt;");
        /// XmlNode node = parse("&lt;user name='Jeffrey'/&gt;");
        /// new Xambler(
        ///   new Directives()
        ///     .Xpath("////")
        ///     .Add("jeff")
        ///     .Append(new CopyOf(node))
        /// ).Apply(target);
        /// assert print(target).equals(
        ///   "&lt;root&gt;&lt;jeff name='Jeffrey'&gt;&lt;/root&gt;"
        /// );
        /// </param>
        /// </summary>
        /// <param name="node"><see cref="XmlNode"/> to analyze</param>
        /// <returns>Collection of directives</returns>
        private CopyOfDirective(IScalar<XmlNode> node)
        {
            _node = node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDirective> GetEnumerator()
        {
            var dirs = new Directives();
            var node = _node.Value();
            if (node.NodeType == XmlNodeType.Element)
            {
                dirs.Add(node.Name);
                foreach (XmlAttribute attr in node.Attributes)
                {
                    dirs.Attr(attr.Name, attr.Value);
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.NodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                        dirs.Set(child.Value);
                        break;
                    case XmlNodeType.Element:
                        dirs.Append(new CopyOfDirective(child)).Up();
                        break;
                    case XmlNodeType.ProcessingInstruction:
                        dirs.Pi(child.Name, child.Value);
                        break;
                    case XmlNodeType.Attribute:
                    case XmlNodeType.Comment:
                    case XmlNodeType.Document:
                    case XmlNodeType.DocumentFragment:
                    case XmlNodeType.DocumentType:
                        break;
                    default:
                        throw new ArgumentException($"unsupported type {child.NodeType} of node {node.Name}");
                }
            }

            return dirs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
