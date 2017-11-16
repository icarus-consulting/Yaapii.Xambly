using System;
using Xunit;
using System.Xml;

namespace Yaapii.Xml.Xembly.Tests
{
    public class XmlDocumentOfTest
    {
        [Fact]
        public void GetsDocFromNode()
        {
            var doc = new XmlDocument();
            var node = doc.CreateElement("test");
            doc.AppendChild(node);
           
            Assert.True(
                new XmlDocumentOf(node).Value().ChildNodes.Item(0) == node);
        }
    }
}
