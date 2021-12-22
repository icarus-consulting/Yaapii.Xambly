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

using System.Xml;
using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Error;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class AttrDirectiveTest
    {
        [Fact]
        public void AddsAttributeToCurrentNode()
        {
            var dom = new XDocument();
            Assert.True(
                    new Xambler(
                            new Yaapii.Atoms.Enumerable.ManyOf<IDirective>(
                                    new AddDirective("root"),
                                    new AddDirective("foo"),
                                    new UpDirective(),
                                    new AttrDirective("bar", "test")
                                )
                        ).Apply(
                            dom
                        ).ToString(SaveOptions.DisableFormatting) == "<root bar=\"test\"><foo /></root>", "add attribute to current node failed");
        }

        [Theory]
        [InlineData("&")]
        [InlineData("<")]
        [InlineData(">")]
        [InlineData("\"")]
        [InlineData("'")]
        [InlineData("9")]
        public void RejectsInvalidNameChars(string chr)
        {
            Assert.Throws<ImpossibleModificationException>(() =>
                new Xambler(new Atoms.Enumerable.ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("item"),
                        new AttrDirective(chr, "beep")
                    )
                ).Apply(
                    new XDocument()
                )
            );
        }

        [Theory]
        [InlineData("&", "&amp;")]
        [InlineData("<", "&lt;")]
        [InlineData(">", "&gt;")]
        [InlineData("\"", "&quot;")]
        public void EscapesInvalidValueChars(string chr, string result)
        {
            Assert.Equal(
                $"<root><item attr=\"{result}\" /></root>",
                new Xambler(new Atoms.Enumerable.ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("item"),
                        new AttrDirective("attr", chr)
                    )
                ).Apply(
                    new XDocument()
                ).ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsAttributeDirectly()
        {
            var resolver = new XmlNamespaceManager(new NameTable());
            var dom = new XDocument();
            var root = new XElement("xxx");
            var first = new XElement("a");
            var second = new XElement("b");
            root.Add(first);
            root.Add(second);
            dom.Add(root);

            new AttrDirective("x", "y").Exec(
                    dom,
                    new DomCursor(new ManyOf<XNode>(second)),
                    new DomStack(),
                    resolver
                );

            Assert.Equal(
                "<xxx><a /><b x=\"y\" /></xxx>",
                dom.ToString(SaveOptions.DisableFormatting)
            );

        }

        [Fact]
        public void ThrowsForInvalidCharacter()
        {
            var resolver = new XmlNamespaceManager(new NameTable());

            Assert.Throws<XmlException>(() =>
                new AttrDirective("valid", "\0invalid")
                    .Exec(
                    new XDocument(),
                    new DomCursor(
                        new ManyOf<XNode>(
                            new XElement("rot")
                        )
                    ),
                    new DomStack(),
                    resolver
                )
            );
        }
    }
}
