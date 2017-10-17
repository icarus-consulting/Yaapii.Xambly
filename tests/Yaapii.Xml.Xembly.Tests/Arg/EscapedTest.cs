using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Xml.Xembly.Arg;

namespace Yaapii.Xml.Xembly.Tests
{
    public class EscapedTest
    {
        [Fact]
        public void EscapesAndUnescapes()
        {
            var texts = new String[] {
                "",
                "123",
                "test \u20ac привет & <>'\"\\",
                "how are you there,\t\n\rтоварищ? &#0D;",
            };

            foreach (String text in texts)
            {
                Assert.Equal(
                    new Unescaped(new ArgOf(text).AsString()).AsString(),
                    text);
            }
        }

        [Fact]
        public void CantEscapeInvalidXMLChars()
        {
            Assert.Throws<XmlException>(
                () => new ArgOf("\u001b\u0000").AsString());
        }
    }
}

