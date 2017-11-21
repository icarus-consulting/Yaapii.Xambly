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
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class AddIfDirectiveTest
    {
        [Fact]
        public void AddNodesToCurrentNode() {

            Assert.True(
                new Xembler(
                    new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("foo"),
                        new UpDirective(),
                        new AddIfDirective("bar"),
                        new UpDirective(),
                        new AddIfDirective("bar")
                    )).Apply(new XmlDocument())
                        .InnerXml == "<root><foo /><bar /></root>",
                        "Same directive added multiple times");
        }

        [Fact]
        public void AddDomNodesDirectly() {
            var dom = new XmlDocument();
            var root = dom.CreateElement("root");
            root.AppendChild(dom.CreateElement("a"));
            root.AppendChild(dom.CreateTextNode("hello"));
            root.AppendChild(dom.CreateComment("some comment"));
            root.AppendChild(dom.CreateCDataSection("CDATA"));
            root.AppendChild(dom.CreateProcessingInstruction("a12","22"));
            dom.AppendChild(root);

            new AddIfDirective("b").Exec(
                dom,
                new DomCursor(new Yaapii.Atoms.Enumerable.EnumerableOf<XmlNode>(root)),
                new DomStack()
            );

            Assert.True(
                dom.InnerXml == "<root><a />hello<!--some comment--><![CDATA[CDATA]]><?a12 22?><b /></root>",
                "Add dom node directly failed");
        }
    }
}
