using System.Xml;
using Xunit;

namespace Yaapii.Xambly.Directive.Tests
{
    public sealed class PushDirectiveTests
    {
        [Fact]
        public void PushesAndPops()
        {
            var dom = new XmlDocument();
            var innerXml = new Xambler(
                        new AddDirective("root"),
                        new PushDirective(),
                        new AddDirective("foo"),
                        new AddDirective("bar"),
                        new PopDirective(),
                        new SetDirective("Hello World")
                    ).Apply(dom).InnerXml;

            Assert.True(innerXml == "<root>Hello World</root>", "Push/Pop directive failed");
        }

        [Fact]
        public void PushesAndPopsFalse()
        {
            var dom = new XmlDocument();
            var innerXml = new Xambler(
                        new AddDirective("root"),
                        new AddDirective("foo"),
                        new AddDirective("bar"),
                        new PushDirective(),
                        new UpDirective(),
                        new UpDirective(),
                        new PushDirective(),
                        new PopDirective(),
                        new PopDirective(),
                        new SetDirective("Hello World")
                    ).Apply(dom).InnerXml;

            Assert.False(innerXml == "<root>Hello World</root>", "Push/pop directive failed not but had to");
        }
    }
}
