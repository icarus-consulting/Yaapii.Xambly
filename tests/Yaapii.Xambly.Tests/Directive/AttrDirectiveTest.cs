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

using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xambly.Cursor;
using Yaapii.Xml.Xambly.Stack;

namespace Yaapii.Xml.Xambly.Directive.Tests
{
    public class AttrDirectiveTest
    {
        [Fact]
        public void AddsAttributeToCurrentNode()
        {
            var dom = new XmlDocument();
            Assert.True(
                    new Xambler(
                            new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
                                    new AddDirective("root"),
                                    new AddDirective("foo"),
                                    new UpDirective(),
                                    new AttrDirective("bar","test")
                                )
                        ).Apply(
                            dom
                        ).InnerXml == "<root bar=\"test\"><foo /></root>", "add attribute to current node failed");
        }

        [Fact]
        public void AddsAttributeDirectly()
        {

            var dom = new XmlDocument();
            var root = dom.CreateElement("xxx");
            var first = dom.CreateElement("a");
            root.AppendChild(first);
            var second = dom.CreateElement("b");
            root.AppendChild(second);
            dom.AppendChild(root);

            new AttrDirective("x", "y").Exec(
                    dom,
                    new DomCursor(new Yaapii.Atoms.Enumerable.EnumerableOf<XmlNode>(second)),
                    new DomStack()
                );

            Assert.True(
                dom.InnerXml == "<xxx><a /><b x=\"y\" /></xxx>",
                "Directly add attribute failed"
                );

        }

    }
}
