using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class SetDirectiveTest
    {
        [Fact]
        public void SetTextContentOfNode()
        {
            Assert.True(
                new Xembler(new EnumerableOf<IDirective>(
                        new AddDirective("root"),
                        new AddDirective("item"),
                        new SetDirective("foobar")
                    )).Apply(
                        new XmlDocument()
                        ).InnerXml == "<root><item>foobar</item></root>", "Set content for node failed");
        }

        [Fact]
        public void SetTextDirectlyIntoDomNodes()
        {
            var dom = new XmlDocument();
            var root = dom.CreateElement("xxx");
            var first = dom.CreateElement("a");
            root.AppendChild(first);
            var second = dom.CreateElement("b");
            root.AppendChild(second);
            dom.AppendChild(root);

            new SetDirective("alpha")
                    .Exec(
                        dom,
                        new DomCursor(
                                new EnumerableOf<XmlNode>(first, second)
                                ),
                        new DomStack()
                    );

            Assert.True(dom.InnerXml == "<xxx><a>alpha</a><b>alpha</b></xxx>", "Set content for nodes failed");

        }
    }
}
