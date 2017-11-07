using System;
using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Arg;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Directive;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class NsDirectiveTest
    {
        [Fact]
        public void SetsNsAttr() {
            var dom = new XmlDocument();
            var root = dom.CreateElement("f");
            dom.AppendChild(root);

            new NsDirective(
                new ArgOf("somens")
            ).Exec(
                dom,
                new DomCursor(
                    new EnumerableOf<XmlNode>(root)
                ),
                new DomStack()
            );

            Assert.True(dom.InnerXml == "<f xmlns=\"somens\" />");
        }
    }
}
