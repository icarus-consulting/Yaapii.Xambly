using System.Xml;
using Xunit;
using Yaapii.Atoms.List;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public class UpDirectiveTest
    {
        [Fact]
        public void JumpsToParentsWhenTheyExist()
        {
            var dom = new XmlDocument();

            Assert.True(
                new Xembler(
                    new AddDirective("root"),
                    new AddDirective("foo"),
                    new AddDirective("bar"),
                    new UpDirective(),
                    new SetDirective("Hello World")
                ).Apply(dom).InnerXml == "<root><foo>Hello World</foo></root>", "Up directive failed");
        }

        [Fact]
        public void ThrowsExceptionWhenNoParents()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
            {
                new Xembler(
                        new EnumerableOf<IDirective>(
                                    new AddDirective("foo"),
                                    new UpDirective(),
                                    new UpDirective(),
                                    new UpDirective()
                                )).Apply(new XmlDocument());
            });
        }
    }
}
