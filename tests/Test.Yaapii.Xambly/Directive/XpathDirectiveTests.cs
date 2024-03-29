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

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public sealed class XpathDirectiveTests
    {
        /// <summary>
        /// XpathDirective can find nodes.
        /// </summary>
        [Theory]
        [InlineData("/root/foo[@bar=1]/test")]
        [InlineData("/root/bar")]
        public void FindsNodesWithXpathExpression(string testXPath)
        {
            var dom = new XDocument();
            new Xambler(
               new ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("foo"),
                        new AttrDirective("bar", "1"),
                        new UpDirective(),
                        new AddDirective("bar")
                )
            ).Apply(dom);

            new Xambler(
                new XpathDirective("//*[@bar=1]"),
                new AddDirective("test")
            ).Apply(dom);

            Assert.True(
                null != FromXPath(
                    dom.ToString(SaveOptions.DisableFormatting),
                    testXPath
                )
            );
        }

        /// <summary>
        /// XpathDirective can ignore empty searches.
        /// </summary>
        [Fact]
        public void IgnoresEmptySearches()
        {
            var dom =
                new XDocument(
                    new XElement("top")
                );

            new Xambler(
                    new XpathDirective("/nothing"),
                    new XpathDirective("/top"),
                    new StrictDirective(1),
                    new AddDirective("hey")
            ).Apply(dom);
            Assert.NotNull(FromXPath(dom.ToString(SaveOptions.DisableFormatting), "/top/hey"));
        }

        /// <summary>
        /// XpathDirective can find nodes by XPath.
        /// </summary>
        [Fact]
        public void FindsNodesByXpathDirectly()
        {
            var dom = new XDocument();
            var root = new XElement("xxx");
            var first = new XElement("a");
            var second = new XElement("b");
            root.Add(first);
            root.Add(second);
            dom.Add(root);

            Assert.Contains(
                root,
                new XpathDirective(
                    "/*")
                .Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.ManyOf<XNode>(first)
                    ),
                    new DomStack()
                )
            );
        }

        /// <summary>
        /// XpathDirective can find nodes in empty DOM.
        /// </summary>
        [Fact]
        public void FindsNodesInEmptyDom()
        {
            var dom = new XDocument();

            Assert.Empty(
                new XpathDirective(
                    "/some-root").Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.ManyOf<XNode>()
                    ),
                    new DomStack()
                )
            );
        }

        [Fact]
        public void WorksWithDoubleQuotes()
        {
            var dom = new XDocument();

            new Xambler(
                new AddDirective("Tags"),
                new AddDirective("Tag"),
                new SetDirective("Tran\"sient")
            ).Apply(dom);


            Assert.NotEmpty(
                new XpathDirective(
                    "//Tag[contains(.,'Tran\"sient')]").Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.ManyOf<XNode>(dom)
                    ),
                    new DomStack()
                )
            );
        }

        [Fact]
        public void WorksWithSingleQuotes()
        {
            var dom = new XDocument();

            new Xambler(
                new AddDirective("Tags"),
                new AddDirective("Tag"),
                new SetDirective("Tran'sient")
            ).Apply(dom);


            Assert.NotEmpty(
                new XpathDirective(
                    "//Tag[contains(.,\"Tran'sient\")]").Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.ManyOf<XNode>(dom)
                    ),
                    new DomStack()
                )
            );
        }

        /// <summary>
        /// XpathDirective can find root in cloned document.
        /// </summary>
        [Fact]
        public void FindsRootInClonedNode()
        {
            var dom =
                new XDocument(
                    new XElement("high")
                );

            var clone = new XDocument(dom);
            new Xambler(
                new XpathDirective("/*"),
                new StrictDirective(1),
                new AddDirective("boom-5")
            ).Apply(clone);

            Assert.NotNull(
                FromXPath(clone.ToString(SaveOptions.DisableFormatting), "/high/boom-5")
            );
        }

        [Fact]
        public void NavigatesFromRoot()
        {
            var dom = new XDocument();
            var root = new XElement("root");
            var first = new XElement("child");
            root.Add(first);
            dom.Add(root);

            Assert.Contains(
                first,
                new XpathDirective(
                    "/root/child"
                ).Exec(
                    dom,
                    new DomCursor(new ManyOf<XNode>(first)),
                    new DomStack()
                )
            );
        }

        [Fact]
        public void NavigatesFromRootWithStrangerCursor()
        {
            var dom = new XDocument();
            var root = new XElement("root");
            var first = new XElement("child");
            root.Add(first);
            dom.Add(root);

            // this element doesn't belongs to the dom!
            var strangerCursor = new XElement("deleted");

            Assert.Contains(
                first,
                new XpathDirective(
                    "/root/child"
                ).Exec(
                    dom,
                    new DomCursor(new ManyOf<XNode>(strangerCursor)),
                    new DomStack()
                )
            );
        }

        /// <summary>
        /// A navigator from an Xml and XPath
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private XPathNavigator FromXPath(string xml, string xpath)
        {
            var nav =
                new XPathDocument(
                    new StringReader(xml)
                ).CreateNavigator();

            var nsm = NamespacesOfDom(xml);
            return nav.SelectSingleNode(xpath, nsm);
        }

        private XmlNamespaceManager NamespacesOfDom(string xml)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            return NamespacesOfDom(xDoc);
        }

        private XmlNamespaceManager NamespacesOfDom(XmlDocument xDoc)
        {
            XmlNamespaceManager result = new XmlNamespaceManager(xDoc.NameTable);

            IDictionary<string, string> localNamespaces = null;
            XPathNavigator xNav = xDoc.CreateNavigator();
            while (xNav.MoveToFollowing(XPathNodeType.Element))
            {
                localNamespaces = xNav.GetNamespacesInScope(XmlNamespaceScope.Local);
                foreach (var localNamespace in localNamespaces)
                {
                    string prefix = localNamespace.Key;
                    if (string.IsNullOrEmpty(prefix))
                        prefix = "";

                    result.AddNamespace(prefix, localNamespace.Value);
                }
            }

            return result;
        }
    }
}
