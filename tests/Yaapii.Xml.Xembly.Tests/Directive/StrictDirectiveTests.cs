using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Atoms.List;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public sealed class StrictDirectiveTests
    {
        [Fact]
        public void ChecksNumberOfCurrentNodes()
        {
            var dom = new XmlDocument();

            Assert.True(
            new Xembler(
                    new EnumerableOf<IDirective>(
                            new AddDirective("root"),
                            new AddDirective("foo"),
                            new AddDirective("bar"),
                            new AddDirective("boom"),
                            new UpDirective(),
                            new UpDirective(),
                            new UpDirective(),
                            new StrictDirective(4),
                            new UpDirective(),
                            new SetDirective("Hello World")
                        )).Apply(dom).InnerXml == "<root><foo><boom>Hello World</boom></foo></root>", "Up directive failed");
        }
    }
}
