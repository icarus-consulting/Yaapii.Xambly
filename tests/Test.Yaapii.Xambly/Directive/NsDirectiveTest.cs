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
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class NsDirectiveTest
    {
        [Fact]
        public void SetsDefaultNamespace()
        {
            var resolver = new XmlNamespaceManager(new NameTable());
            var root =
                new XElement("f",
                    new XElement("g")
                );
            var dom = new XDocument(root);

            new NsDirective(
                "",
                "somens"
            ).Exec(
                dom,
                new DomCursor(
                    new ManyOf<XNode>(root)
                ),
                new DomStack(),
                resolver
            );

            Assert.Equal(
                "<f xmlns=\"somens\"><g /></f>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsDefaultNamespaceToRoot()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("parent")
                        .Add("child")
                        .Set("child value")
                .Xpath("/root")
                .Ns("", "MyDefaultNamespaceUri")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns=\"MyDefaultNamespaceUri\"><parent><child>child value</child></parent></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void ReplacesDefaultNamespace()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                .Ns("", "OldValue")
                .Ns("", "MyDefaultNamespaceUri")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns=\"MyDefaultNamespaceUri\" />",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsDefaultNamespaceToSelectedNode()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("parent")
                        .Add("child")
                .Xpath("/root/parent")
                .Ns("", "MyDefaultNamespaceUri")
            ).Apply(dom);

            Assert.Equal(
                "<root><parent xmlns=\"MyDefaultNamespaceUri\"><child /></parent></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsDefaultNamespaceToSeveralNodes()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("parent")
                        .Add("child")
                .Xpath("/root")
                    .Add("anotherParent")
                        .Add("anotherChild")
                .Xpath("/root/*")
                .Ns("", "MyDefaultNamespaceUri")
            ).Apply(dom);

            Assert.Equal(
                "<root><parent xmlns=\"MyDefaultNamespaceUri\"><child /></parent><anotherParent xmlns=\"MyDefaultNamespaceUri\"><anotherChild /></anotherParent></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void SetsPrefixedNamespace()
        {
            var resolver = new XmlNamespaceManager(new NameTable());
            var root = new XElement("rooot");
            var dom = new XDocument(root);

            new NsDirective("my", "MyNiceNamespace")
            .Exec(
                dom,
                new DomCursor(
                    new ManyOf<XNode>(root)
                ),
                new DomStack(),
                resolver
            );

            Assert.Equal(
                "<my:rooot xmlns:my=\"MyNiceNamespace\" />",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void StroresPrefixedNamespaceInRoot()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("child")
                    .Ns("my", "MyNiceNamespace")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:my=\"MyNiceNamespace\"><my:child /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void ReplacesPrefixedNamespace()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("child")
                    .Ns("my", "MyOldUglyNamespace")
                    .Ns("my", "MyNewNiceNamespace")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:my=\"MyNewNiceNamespace\"><my:child /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void ChildrenInheritsNamespace()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("parent")
                    .Attr("key", "value")
                .Xpath("/root")
                .Ns("nice", "MyNiceNamespace")
            ).Apply(dom);

            Assert.Equal(
                "<nice:root xmlns:nice=\"MyNiceNamespace\"><nice:parent nice:key=\"value\" /></nice:root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void ChildrenUnchanged()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("parent")
                    .Attr("key", "value")
                .Xpath("/root")
                .Ns("nice", "MyNiceNamespace", "nodesAndAttributes", false)
            ).Apply(dom);

            Assert.Equal(
                "<nice:root xmlns:nice=\"MyNiceNamespace\"><parent key=\"value\" /></nice:root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsPrefixedNamespaceToRootChildren()
        {
            var dom = new XDocument();
            new Xambler(
                new Directives()
                .Add("root")
                    .Add("parent")
                        .Add("child")
                .Xpath("/root")
                    .Add("anotherParent")
                        .Add("anotherChild")
                .Xpath("/root/*")
                .Ns("nice", "MyNiceNamespace")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:nice=\"MyNiceNamespace\"><nice:parent><nice:child /></nice:parent><nice:anotherParent><nice:anotherChild /></nice:anotherParent></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsPrefixedNamespaceToAttributes()
        {
            var dom = new XDocument();
            var nsResolver = new XmlNamespaceManager(new NameTable());
            nsResolver.AddNamespace("nice", "MyNiceNamespace");
            new Xambler(
                nsResolver,
                new Directives()
                .Add("root")
                    .Add("parent")
                    .Attr("key", "value")
                    .Ns("nice", "MyNiceNamespace")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:nice=\"MyNiceNamespace\"><nice:parent nice:key=\"value\" /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsPrefixedNamespaceOnlyToNodes()
        {
            var dom = new XDocument();
            var nsResolver = new XmlNamespaceManager(new NameTable());
            nsResolver.AddNamespace("nice", "MyNiceNamespace");
            new Xambler(
                nsResolver,
                new Directives()
                .Add("root")
                    .Add("parent")
                    .Attr("key", "value")
                    .Ns("nice", "MyNiceNamespace", "nodes")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:nice=\"MyNiceNamespace\"><nice:parent key=\"value\" /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsPrefixedNamespaceOnlyToAttributes()
        {
            var dom = new XDocument();
            var nsResolver = new XmlNamespaceManager(new NameTable());
            nsResolver.AddNamespace("nice", "MyNiceNamespace");
            new Xambler(
                nsResolver,
                new Directives()
                .Add("root")
                    .Add("parent")
                    .Attr("key", "value")
                    .Ns("nice", "MyNiceNamespace", "attributes")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:nice=\"MyNiceNamespace\"><parent nice:key=\"value\" /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
