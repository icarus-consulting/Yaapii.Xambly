using System;
using Xunit;
using System.Xml;
using System.Xml.Linq;

namespace Yaapii.Xambly.Tests
{
    public class XmlDocumentOfTest
    {
        [Fact]
        public void GetsDocFromNode()
        {
            var doc = new XDocument();
            //var node = doc.CreateElement("test");
            var child = new XElement("child");
            var node = new XElement("test", child);
            doc.Add(node);
           
            Assert.True(
                new XmlDocumentOf(child).Value().FirstNode == node);
        }
    }
}