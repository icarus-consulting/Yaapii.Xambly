using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class AttrDirectiveTest
    {
        [Fact]
        public void AddsAttributeToCurrentNode()
        {
            var dom = new XmlDocument();
            Assert.True(
                    new Xembler(
                            new EnumerableOf<IDirective>(
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
                    new DomCursor(new EnumerableOf<XmlNode>(second)),
                    new DomStack()
                );

            Assert.True(
                dom.InnerXml == "<xxx><a /><b x=\"y\" /></xxx>",
                "Directly add attribute failed"
                );

        }

    }
}
