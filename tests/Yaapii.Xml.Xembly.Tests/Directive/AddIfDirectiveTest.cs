using System;
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
                    new EnumerableOf<IDirective>(
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
                new DomCursor(new EnumerableOf<XmlNode>(root)),
                new DomStack()
            );

            Assert.True(
                dom.InnerXml == "<root><a />hello<!--some comment--><![CDATA[CDATA]]><?a12 22?><b /></root>",
                "Add dom node directly failed");
        }
    }
}
