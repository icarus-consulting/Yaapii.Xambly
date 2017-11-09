using System;
using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Directive;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class CdataDirectiveTest
    {
        [Fact]
        public void AddsCdataSectionToCurrentNode() {
            var dom = new XmlDocument();
            Assert.True(
                    new Xembler(
                            new EnumerableOf<IDirective>(
                                    new AddDirective("root"),
                                    new AddDirective("foo"),
                                    new CdataDirective("Hello World")
                                )
                        ).Apply(
                            dom
                ).InnerXml == "<root><foo><![CDATA[Hello World]]></foo></root>", "add cdata to current node failed");
        }
    }
}
