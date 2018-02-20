using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Xunit;
using Yaapii.Xml.Xambly.Cursor;
using Yaapii.Xml.Xambly.Stack;

namespace Yaapii.Xml.Xambly.Directive.Tests
{
    public sealed class XpathDirectiveTests
    {
        /// <summary>
        /// XpathDirective can find nodes.
        /// </summary>
        [Theory]
        [InlineData("/root/foo[@bar=1]/test")]
        [InlineData("/root/bar")]
        public void FindsNodesWithXpathExpression(string testXPath)
        {
            var dom = new XmlDocument();

            new Xambler(
                new Directives(
                    "ADD 'root'; ADD 'foo'; ATTR 'bar', '1'; UP; ADD 'bar';"
                )
            ).Apply(dom);
            new Xambler(
                new Directives(
                    "XPATH '//*[@bar=1]'; ADD 'test';"
                )
            ).Apply(dom);

            Assert.True(
                null != FromXPath(
                    dom.InnerXml.ToString(),
                    testXPath
                )
            );
        }

        /// <summary>
        /// XpathDirective can ignore empty searches.
        /// </summary>
        [Fact]
        public void IgnoresEmptySearches()
        {
            var dom = new XmlDocument();
            dom.AppendChild(dom.CreateElement("top"));

            new Xambler(
                new Directives(
                    "XPATH '/nothing'; XPATH '/top'; STRICT '1'; ADD 'hey';"
                )
            ).Apply(dom);
            Assert.NotNull(FromXPath(dom.InnerXml.ToString(), "/top/hey"));
        }

        /// <summary>
        /// XpathDirective can find nodes by XPath.
        /// </summary>
        [Fact]
        public void FindsNodesByXpathDirectly()
        {
            var dom = new XmlDocument();
            var root = dom.CreateElement("xxx");
            var first = dom.CreateElement("a");
            root.AppendChild(first);
            var second = dom.CreateElement("b");
            root.AppendChild(second);
            dom.AppendChild(root);
            Assert.Contains(
                root,
                new XpathDirective(
                    "/*")
                .Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.EnumerableOf<XmlNode>(first)
                    ),
                    new DomStack()
                )
            );
        }

        /// <summary>
        /// XpathDirective can find nodes in empty DOM.
        /// </summary>
        [Fact]
        public void FindsNodesInEmptyDom()
        {
            var dom = new XmlDocument();

            Assert.Empty(
                new XpathDirective(
                    "/some-root").Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.EnumerableOf<XmlNode>()
                    ),
                    new DomStack()
                )
            );
        }

        [Fact]
        public void WorksWithDoubleQuotes()
        {
            var dom = new XmlDocument();

            new Xambler(
                new Directives().Add("Tags").Add("Tag").Set("Transient")
            ).Apply(dom);
            

            Assert.NotEmpty(
                new XpathDirective(
                    "//Tag[contains(.,'Transient')]").Exec(
                    dom,
                    new DomCursor(
                        new Atoms.Enumerable.EnumerableOf<XmlNode>(dom)
                    ),
                    new DomStack()
                )
            );
        }

        /// <summary>
        /// XpathDirective can find root in cloned document.
        /// </summary>
        [Fact]
        public void FindsRootInClonedNode()
        {
            var dom = new XmlDocument();
            dom.AppendChild(
                dom.CreateElement(
                    "high"));
            var clone = dom.CloneNode(true);
            new Xambler(
                new Directives(
                    "XPATH '/*'; STRICT '1'; ADD 'boom-5';"
                )
            ).Apply(clone);
            Assert.NotNull(
                FromXPath(clone.InnerXml.ToString(), "/high/boom-5")
            );
        }

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
    }
}
