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
        [Fact(Skip = "True")]
        public void ChecksNumberOfCurrentNodes()
        {
            var dom = new XmlDocument();

            Assert.True(
                new Xembler(
                    new AddDirective("root"),
                    new AddDirective("foo"),
                    new AddDirective("bar"),
                    new AddDirective("boom"),
                    new XpathDirective("//*"),
                    new StrictDirective(4),
                    new UpDirective(),
                    new SetDirective("Hello World")
                ).Apply(dom).InnerXml == "<root><foo><boom>Hello World</boom></foo></root>", "Strict directive failed");
        }

        [Fact]
        public void FailsWhenNumberOfCurrentNodesIsTooBig()
        {
            var dom = new XmlDocument();

            Assert.Throws(
                new ImpossibleModificationException("").GetType(), () =>
                {
                    new Xembler(
                       new AddDirective("foo"),
                       new AddDirective("bar"),
                       new UpDirective(),
                       new AddDirective("bar"),
                       new XpathDirective("/foo/bar"),
                       new StrictDirective(1)
                   ).Apply(dom);
                });
        }

        [Fact]
        public void FailsWhenNumberOfCurrentNodesIsZero()
        {
            var dom = new XmlDocument();

            Assert.Throws(new ImpossibleModificationException("").GetType(), () =>
            {
                new Xembler(
                        new EnumerableOf<IDirective>(
                                new AddDirective("foo"),
                                new AddDirective("bar"),
                                new XpathDirective("/foo/bar/boom"),
                                new StrictDirective(1)
                            )).Apply(dom);
            });
        }

        [Fact]
        public void FailsWhenNumberOfCurrentNodesIsTooSmall()
        {
            var dom = new XmlDocument();

            Assert.Throws(new ImpossibleModificationException("").GetType(), () =>
            {
                new Xembler(
                        new EnumerableOf<IDirective>(
                                new AddDirective("root"),
                                new StrictDirective(2)
                            )).Apply(dom);
            });
        }
    }
}
