using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Xunit;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Directive;
using Yaapii.Xml.Xembly.Stack;
using static System.Net.Mime.MediaTypeNames;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public sealed class XsetDirectiveTests
    {
        [Fact]
        public void setsTextDirectlyIntoDomNodes()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlElement root = doc.CreateElement("xxx");
                XmlElement first = doc.CreateElement("first");

                first.InnerText = "15";
                root.AppendChild(first);
                XmlElement second = doc.CreateElement("second");
                second.InnerText = "13";
                root.AppendChild(second);

                doc.AppendChild(root);
                doc.Save("XsetDirectiveTests.xml");

                new XsetDirective("sum(/xxx/*/text()) + 6").Exec(
                doc, new DomCursor(new List<XmlNode>() { first }),
                new DomStack());
                doc.Save("XsetDirectiveTests.xml");

                XPathNavigator nav = doc.CreateNavigator();
                nav.MoveToFirstChild();
                nav.MoveToFirstChild();

                Assert.True(nav.Value == "34");

            }
            finally
            {
                if (File.Exists("XsetDirectiveTests.xml"))
                {
                    File.Delete("XsetDirectiveTests.xml");
                }
            }
        }

    }
}
