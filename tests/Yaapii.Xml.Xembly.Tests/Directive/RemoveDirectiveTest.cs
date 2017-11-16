using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class RemoveDirectiveTest
    {
        [Fact]
        public void RemoveCurrentNode() {

            Assert.True(
            new Xembler(
                new EnumerableOf<IDirective>(
                    new AddDirective("root"),
                    new AddDirective("foobar"),
                    new RemoveDirective()
                )).Apply(
                    new XmlDocument()
                    ).InnerXml == "<root></root>", "Remove node failed");
        }

        [Fact]
        public void ThrowsExceptionOnRemoveRootNode() {
            Assert.Throws<ImpossibleModificationException>(() =>
                {
                    new Xembler(
                        new EnumerableOf<IDirective>(
                            new AddDirective("root"),
                            new RemoveDirective()
                        )).Apply(
                            new XmlDocument()
                            );
                }
            );
        }

        [Fact]
        public void ThrowsExceptionOnRemoveDocumentNode()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
            {
                new Xembler(
                    new EnumerableOf<IDirective>(
                        new RemoveDirective()
                    )).Apply(
                        new XmlDocument()
                        );
            }
            );
        }

        [Fact]
        public void RemoveDomNodesDirectly()
        {
            var dom = new XmlDocument();
            var root = dom.CreateElement("root");
            var first = dom.CreateElement("a");
            root.AppendChild(first);
            var second = dom.CreateElement("b");
            root.AppendChild(second);

            dom.AppendChild(root);

            new RemoveDirective().Exec(
                dom,
                new DomCursor(new EnumerableOf<XmlNode>(first)),
                new DomStack()
            );

            Assert.True(
                dom.InnerXml == "<root><b /></root>", "Remove directive failed"
            );
        }
    }
}
