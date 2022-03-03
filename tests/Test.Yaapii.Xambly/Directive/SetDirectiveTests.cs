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

using System.Xml;
using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class SetDirectiveTests
    {
        [Fact]
        public void SetTextContentOfNode()
        {
            Assert.True(
                new Xambler(new ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("item"),
                        new SetDirective("foobar")
                    )
                ).Apply(
                    new XDocument()
                ).ToString(SaveOptions.DisableFormatting) == "<root><item>foobar</item></root>", "Set content for node failed");
        }

        [Theory]
        [InlineData("&", "&amp;")]
        [InlineData("<", "&lt;")]
        [InlineData(">", "&gt;")]
        public void EscapesInvalidChars(string chr, string result)
        {
            Assert.Equal(
                $"<root><item>{result}</item></root>",
                new Xambler(new ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("item"),
                        new SetDirective(chr)
                    )
                ).Apply(
                    new XDocument()
                ).ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void SetTextDirectlyIntoDomNodes()
        {
            var resolver = new XmlNamespaceManager(new NameTable());
            var dom = new XDocument();
            var root = new XElement("xxx");
            var first = new XElement("a");
            var second = new XElement("b");
            root.Add(first);
            root.Add(second);
            dom.Add(root);

            new SetDirective("alpha")
                    .Exec(
                        dom,
                        new DomCursor(
                                new ManyOf<XNode>(first, second)
                                ),
                        new DomStack(),
                        resolver
                    );

            Assert.Equal(
                "<xxx><a>alpha</a><b>alpha</b></xxx>",
                dom.ToString(SaveOptions.DisableFormatting)
            );

        }

        [Fact]
        public void ThrowsForInvalidCharacter()
        {
            var resolver = new XmlNamespaceManager(new NameTable());

            Assert.Throws<XmlException>(() =>
                new SetDirective("\0invalid").Exec(
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
