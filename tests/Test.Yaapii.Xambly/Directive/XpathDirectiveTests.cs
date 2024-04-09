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

using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;
using Yaapii.Xambly.XmlNamespaceResolver;

namespace Yaapii.Xambly.Directive.Tests
{
    public sealed class XpathDirectiveTests
    {
        [Fact]
        public void FindsNodesWithXpathExpression()
        {
            var expected =
                "<root>" +
                    "<foo bar=\"1\">" +
                        "<test />" +
                    "</foo>" +
                    "<bar />" +
                "</root>";
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

            Assert.Equal(
                expected,
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void IgnoresEmptySearches()
        {
            var expected = "<top><hey /></top>";
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

            Assert.Equal(
                expected,
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

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
                new XpathDirective("/*")
                .Exec(
                    dom,
                    new DomCursor(
                        new ManyOf<XNode>(first)
                    ),
                    new DomStack()
                )
            );
        }

        [Fact]
        public void FindsNodesInEmptyDom()
        {
            var dom = new XDocument();

            Assert.Empty(
                new XpathDirective("/some-root")
                .Exec(
                    dom,
                    new DomCursor(
                        new ManyOf<XNode>()
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
                new XpathDirective("//Tag[contains(.,'Tran\"sient')]")
                .Exec(
                    dom,
                    new DomCursor(
                        new ManyOf<XNode>(dom)
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
                new XpathDirective("//Tag[contains(.,\"Tran'sient\")]")
                .Exec(
                    dom,
                    new DomCursor(
                        new ManyOf<XNode>(dom)
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
            var expected = "<high><boom-5 /></high>";
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

            Assert.Equal(
                expected,
                clone.ToString(SaveOptions.DisableFormatting)
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
                new XpathDirective("/root/child")
                .Exec(
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
                new XpathDirective("/root/child")
                .Exec(
                    dom,
                    new DomCursor(new ManyOf<XNode>(strangerCursor)),
                    new DomStack()
                )
            );
        }

        [Fact]
        public void WorksWithDefaultNamespace()
        {
            var xml =
                XDocument.Parse(
                    "<root xmlns='MyDefaultNamespaceUri'><parent key='original' /></root>"
                );

            new Xambler(
                new XpathDirective("/def1:root/def1:parent", new ResolverFromDocument(xml)),
                new AttrDirective("key", "changed")
            ).Apply(xml);

            Assert.Equal(
                "<root xmlns=\"MyDefaultNamespaceUri\"><parent key=\"changed\" /></root>",
                xml.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WorksWithNamedNamespace()
        {
            var xml =
                XDocument.Parse(
                    "<root xmlns:nx='myNiceNamesapce'><nx:parent>Hello</nx:parent></root>"
                );

            new Xambler(
                new XpathDirective("/root/nx:parent", new ResolverFromDocument(xml)),
                new SetDirective("Hello, World!")
            ).Apply(xml);

            Assert.Equal(
                "<root xmlns:nx=\"myNiceNamesapce\"><nx:parent>Hello, World!</nx:parent></root>",
                xml.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WorksWithDefaultNamespaceOfChildren()
        {
            var xml =
                XDocument.Parse(
                    "<root xmlns='MyDefaultNamespaceUri'><parent key='original' /></root>"
                );

            new Xambler(
                new XpathDirective("/def1:root/def1:parent", new ResolverFromDocument(xml)),
                new AttrDirective("key", "changed")
            ).Apply(xml);

            Assert.Equal(
                "<root xmlns=\"MyDefaultNamespaceUri\"><parent key=\"changed\" /></root>",
                xml.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WorksSeveralDefaultNamespacesOfChildren()
        {
            var xml =
                XDocument.Parse(
                    "<root>" +
                        "<child xmlns='childDefaultNamespace'>" +
                            "<lower xmlns='anotherChildDefaultNamespace' />" +
                        "</child>" +
                    "</root>"
                );

            var expected =
                "<root>" +
                    "<child xmlns=\"childDefaultNamespace\">" +
                        "<lower xmlns=\"anotherChildDefaultNamespace\">" +
                            "<node />" +
                        "</lower>" +
                    "</child>" +
                "</root>";
            new Xambler(
                new XpathDirective("/root/defOne:child/defTwo:lower",
                    new ResolverFromDocument(xml,
                        "childDefaultNamespace", "defOne",
                        "anotherChildDefaultNamespace", "defTwo"
                    )
                ),
                new AddDirective("node")
            ).Apply(xml);

            Assert.Equal(
                expected,
                xml.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void WorksWithSeveralDefaultNamespacesOfChildrenAndRootDefaultNamespace()
        {
            var xml =
                XDocument.Parse(
                    "<root xmlns='rootDefaultNamespace'>" +
                        "<child xmlns='childDefaultNamespace'>" +
                            "<lower xmlns='anotherChildDefaultNamespace' />" +
                        "</child>" +
                    "</root>"
                );
            var expected =
                "<root xmlns=\"rootDefaultNamespace\">" +
                    "<child xmlns=\"childDefaultNamespace\">" +
                        "<lower xmlns=\"anotherChildDefaultNamespace\">" +
                            "<node />" +
                        "</lower>" +
                    "</child>" +
                "</root>";
            new Xambler(
                new XpathDirective("/def:root/defOne:child/defTwo:lower",
                    new ResolverFromDocument(xml,
                        "rootDefaultNamespace", "def",
                        "childDefaultNamespace", "defOne",
                        "anotherChildDefaultNamespace", "defTwo"
                    )
                ),
                new AddDirective("node")
            ).Apply(xml);

            Assert.Equal(
                expected,
                xml.ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
