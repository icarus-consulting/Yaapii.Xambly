using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Xml.Xambly.Stack;

namespace Yaapii.Xml.Xambly.Tests.Directive
{
    public sealed class PopDirectiveTests
    {
        [Fact]
        public void PushesAndPops()
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

            Assert.True(innerXml == "<root><foo><bar>Hello World</bar></foo></root>", "Push/pop directive failed");
        }

        [Fact]
        public void PopThrowsException()
        {
            var dom = new XmlDocument();

            Assert.Throws<ImpossibleModificationException>(() =>
            {
                var innerXml = new Xambler(
                        new AddDirective("root"),
                        new AddDirective("foo"),
                        new AddDirective("bar"),
                        new PopDirective()
                    ).Apply(dom).InnerXml;
            });
        }
    }
}
