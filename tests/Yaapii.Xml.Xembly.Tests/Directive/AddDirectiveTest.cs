using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class AddDirectiveTest
    {
        [Fact]
        public void AddNodesToCurrentNodes()
        {
            Assert.True(
                    new Xembler(
                        new EnumerableOf<IDirective>(
                                new AddDirective("root"),
                                new AddDirective("item")
                            )).Dom().InnerXml == "<root><item /></root>","Add Directive failed");
        }

        [Fact]
        public void AddNodesToXmlDocument()
        {
            Assert.True(
            new Xembler(
                    new EnumerableOf<IDirective>(
                            new AddDirective("root"),
                            new AddDirective("item")
                        )).Apply(new XmlDocument()).InnerXml == "<root><item /></root>","Add Directive failed");
        }
    }
}
