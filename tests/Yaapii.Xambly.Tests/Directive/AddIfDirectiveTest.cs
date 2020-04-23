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
using Yaapii.Atoms.Lists;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class AddIfDirectiveTest
    {
        [Fact]
        public void AddNodesToCurrentNode() {

            Assert.Equal(
                "<root><foo /><bar /></root>",
                new Xambler(
                    new ManyOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("foo"),
                        new UpDirective(),
                        new AddIfDirective("bar"),
                        new UpDirective(),
                        new AddIfDirective("bar")
                    )
                ).Apply(
                    new XDocument()
                ).ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddDomNodesDirectly() {
            var dom = new XDocument();
            var root =
                new XElement("root",
                    new XElement("a"),
                    new XText("hello"),
                    new XComment("some comment"),
                    new XCData("CDATA"),
                    new XProcessingInstruction("a12", "22")
                );
            dom.Add(root);
            new AddIfDirective("b").Exec(
                dom,
                new DomCursor(new ManyOf<XNode>(root)),
                new DomStack()
            );

            Assert.Equal(
                "<root><a />hello<!--some comment--><![CDATA[CDATA]]><?a12 22?><b /></root>",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
