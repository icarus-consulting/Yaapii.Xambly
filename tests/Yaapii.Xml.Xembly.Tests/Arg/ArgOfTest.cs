using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Yaapii.Xml.Xembly.Arg;

namespace Yaapii.Xml.Xembly.Tests
{
    public class ArgOfTest
    {
        [Fact]
        public void Ctor()
        {
            var raw = "test \u20ac привет & <>'\"\\";
            Assert.True(new ArgOf(raw).Raw() == raw);
        }

        [Fact]
        public void CtorEscapes()
        {
            var raw = "test \u20ac привет & <>'\"\\";
            Assert.Equal(
                new ArgOf(raw).AsString(),
                "\"" + new Escaped(raw).AsString() + "\"");
        }
    }
}
