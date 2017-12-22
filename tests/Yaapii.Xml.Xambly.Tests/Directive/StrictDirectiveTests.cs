using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Xunit;

namespace Yaapii.Xml.Xambly.Directive.Tests
{
    public sealed class StrictDirectiveTests
    {
        [Fact]
        public void ChecksNumberOfCurrentNodes()
        {
            var dom = new XmlDocument();
            new Xambler(
                new Directives(
                    "ADD 'root'; ADD 'foo'; ADD 'bar'; ADD 'boom'; XPATH '//*'; STRICT '4';"
                )).Apply(dom);

            Assert.True(
                null != FromXPath(
                    dom.InnerXml.ToString(), "/root/foo") &&
                null != FromXPath(
                    dom.InnerXml.ToString(), "/root/foo/bar/boom"));
        }

        [Fact]
        public void FailsWhenNumberOfCurrentNodesIsTooBig()
        {
            var dom = new XmlDocument();

            Assert.Throws(
                new ImpossibleModificationException("").GetType(), () =>
                {
                    new Xambler(
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
                new Xambler(
                        new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
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
                new Xambler(
                        new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
                                new AddDirective("root"),
                                new StrictDirective(2)
                            )).Apply(dom);
            });
        }
        #region Helper
        /// <summary>
        /// A navigator from an Xml and XPath
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private XPathNavigator FromXPath(string xml, string xpath)
        {
            var nav =
                new XPathDocument(
                    new StringReader(xml)
                ).CreateNavigator();

            var nsm = NamespacesOfDom(xml);
            return nav.SelectSingleNode(xpath, nsm);
        }

        private XmlNamespaceManager NamespacesOfDom(string xml)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            return NamespacesOfDom(xDoc);
        }

        private XmlNamespaceManager NamespacesOfDom(XmlDocument xDoc)
        {
            XmlNamespaceManager result = new XmlNamespaceManager(xDoc.NameTable);

            IDictionary<string, string> localNamespaces = null;
            XPathNavigator xNav = xDoc.CreateNavigator();
            while (xNav.MoveToFollowing(XPathNodeType.Element))
            {
                localNamespaces = xNav.GetNamespacesInScope(XmlNamespaceScope.Local);
                foreach (var localNamespace in localNamespaces)
                {
                    string prefix = localNamespace.Key;
                    if (string.IsNullOrEmpty(prefix))
                        prefix = "";

                    result.AddNamespace(prefix, localNamespace.Value);
                }
            }

            return result;
        }
        # endregion Helper
    }
}
