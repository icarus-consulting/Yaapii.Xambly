using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Xml.Xembly.Arg;

namespace Yaapii.Xml.Xembly.Tests.Arg
{
    public class UnescapedTest
    {
        [Fact]
        public void Unescapes()
        {
            var escaped = "\"test € привет &amp; &lt;&gt;&apos;&quot;\\\"";
            var unescaped = "test \u20ac привет & <>'\"\\";

            Assert.Equal(new Unescaped(escaped).AsString(), unescaped);
        }

        [Fact]
        public void CantUnescapeInvalidXMLChars()
        {
            Assert.Throws<XmlException>(
                () => new Unescaped("&#27;&#0000;").AsString());
        }
    }
}
