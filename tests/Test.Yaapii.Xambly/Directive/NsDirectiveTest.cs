// MIT License
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

namespace Yaapii.Xambly.Directive.Tests
{
    public class NsDirectiveTest
    {
        [Fact]
        public void SetsDefaultNamespace()
        {
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
                new DomStack()
            );

            Assert.Equal(
                "<f xmlns=\"somens\"><g /></f>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsDefaultNamespaceToRoot()
        {
            var dom =
                XDocument.Parse(
                    "<root><parent /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root")
                .Ns("", "MyDefaultNamespaceUri")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns=\"MyDefaultNamespaceUri\"><parent /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void ReplacesDefaultNamespace()
        {
            var dom =
                XDocument.Parse(
                    "<root />"
                );
            new Xambler(
                new Directives()
                .Xpath("/root")
                .Ns("", "OldValue")
                .Ns("", "MyDefaultNamespaceUri")
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns=\"MyDefaultNamespaceUri\" />",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsDefaultNamespaceToSeveralNodes()
        {
            var dom =
                XDocument.Parse(
                    "<root><parent><child /></parent><anotherParent><anotherChild /></anotherParent></root>"
                );
            new Xambler(
                new Directives()
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
            var root = new XElement("rooot");
            var dom = new XDocument(root);

            new NsDirective("my", "MyNiceNamespace")
            .Exec(
                dom,
                new DomCursor(
                    new ManyOf<XNode>(root)
                ),
                new DomStack()
            );

            Assert.Equal(
                "<my:rooot xmlns:my=\"MyNiceNamespace\" />",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void StroresPrefixedNamespaceInRoot()
        {
            var dom =
                XDocument.Parse(
                    "<root><child /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root/child")
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
            var dom =
                XDocument.Parse(
                    "<root><child /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root/child")
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
            var dom =
                XDocument.Parse(
                    "<root><parent key='value' /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root")
                .Ns("nice", "MyNiceNamespace")
            ).Apply(dom);

            Assert.Equal(
                "<nice:root xmlns:nice=\"MyNiceNamespace\"><nice:parent nice:key=\"value\" /></nice:root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void OptionallyDoesNotChangeChildren()
        {
            var dom =
                XDocument.Parse(
                    "<root><parent key='value' /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root")
                .Ns("nice", "MyNiceNamespace", forNode: true, forAttributes: true, inheritance: false)
            ).Apply(dom);

            Assert.Equal(
                "<nice:root xmlns:nice=\"MyNiceNamespace\"><parent key=\"value\" /></nice:root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsPrefixedNamespaceToRootChildren()
        {
            var dom =
                XDocument.Parse(
                    "<root><parent><child /></parent><anotherParent><anotherChild /></anotherParent></root>"
                );
            new Xambler(
                new Directives()
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
            var dom =
                XDocument.Parse(
                    "<root><parent key='value' /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root/parent")
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
            var dom =
                XDocument.Parse(
                    "<root><parent key='value' /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root/parent")
                .Ns("nice", "MyNiceNamespace", forNode: true, forAttributes: false)
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:nice=\"MyNiceNamespace\"><nice:parent key=\"value\" /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsPrefixedNamespaceOnlyToAttributes()
        {
            var dom =
                XDocument.Parse(
                    "<root><parent key='value' /></root>"
                );
            new Xambler(
                new Directives()
                .Xpath("/root/parent")
                .Ns("nice", "MyNiceNamespace", forNode: false, forAttributes: true)
            ).Apply(dom);

            Assert.Equal(
                "<root xmlns:nice=\"MyNiceNamespace\"><parent nice:key=\"value\" /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
