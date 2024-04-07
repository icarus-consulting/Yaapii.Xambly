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
using Yaapii.Xambly.Error;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class RemoveDirectiveTest
    {
        [Fact]
        public void RemoveCurrentNode()
        {

            Assert.Equal(
                "<root />",
                new Xambler(
                    new ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("foobar"),
                        new RemoveDirective()
                    )
                ).Apply(
                    new XDocument()
                ).ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void ThrowsExceptionOnRemoveRootNode()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
            {
                new Xambler(
                    new ManyOf<IDirective>(
                        new AddDirective("root"),
                        new RemoveDirective()
                    )
                ).Apply(
                    new XDocument()
                );
            }
            );
        }

        [Fact]
        public void ThrowsExceptionOnRemoveDocumentNode()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
            {
                new Xambler(
                    new ManyOf<IDirective>(
                        new RemoveDirective()
                    )
                ).Apply(
                    new XDocument()
                );
            }
            );
        }

        [Fact]
        public void RemoveDomNodesDirectly()
        {
            var dom = new XDocument();
            var root = new XElement("root");
            var first = new XElement("a");
            root.Add(first);
            root.Add(new XElement("b"));
            dom.Add(root);

            new RemoveDirective().Exec(
                dom,
                new DomCursor(new ManyOf<XNode>(first)),
                new DomStack()
            );

            Assert.Equal(
                "<root><b /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void CursorPointsToParents()
        {
            var dom = new XDocument();
            var root = new XElement("root");
            var first = new XElement("a");
            var frstChild = new XElement("a_child");
            first.Add(frstChild);

            var second = new XElement("b");
            var scndChild = new XElement("b_child");
            second.Add(scndChild);

            root.Add(first);
            root.Add(second);
            dom.Add(root);

            var cursor =
                new RemoveDirective().Exec(
                    dom,
                    new DomCursor(new ManyOf<XNode>(frstChild, scndChild)),
                    new DomStack()
                );
            Assert.Equal(
                new ManyOf<XNode>(first, second),
                cursor
            );
        }
    }
}
