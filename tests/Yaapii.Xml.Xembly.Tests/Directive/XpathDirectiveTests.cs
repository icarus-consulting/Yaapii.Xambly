using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Xunit;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly.Tests.Directive
{
    public sealed class XpathDirectiveTests
    {
        [Fact]
        public void FindsNodesWithXpathExpression()
        {
            //var dom = new XmlDocument();

            //var xembler = new Xembler(
            //    new AddDirective("root"),
            //    new AddDirective("foo"),
            //    new SetDirective("Hello World")
            //).Apply(dom);

            //new Xembler(
            //    new XpathDirective(@"/root"),
            //    new XpathDirective(@"/foo"),
            //    new SetDirective("Changed World")
            //    ).Apply(dom);
            var dom = new XmlDocument();

            new Xembler(
                new Directives(
                    "ADD 'root'; ADD 'foo'; ATTR 'bar', '1'; UP; ADD 'bar';")).Apply(dom);
            new Xembler(
                new Directives(
                    "XPATH '//*[@bar=1]'; ADD 'test';")).Apply(dom);


            //var dirs = new Directives(
            //    "ADD 'root'; ADD 'foo'; ATTR 'bar', '1'; UP; ADD 'bar';" 
            //    + "XPATH '//*[@bar=1]'; ADD 'test';"
                //new JoinedText(
                //    "ADD 'root'; ADD 'foo'; ATTR 'bar', '1'; UP; ADD 'bar';",
                //    "XPATH '//*[@bar=1]'; ADD 'test';"
                //).AsString()
            //);

            //new Xembler(dirs).Apply(dom);
            //XhtmlMatchers.xhtml(dom),
            //    XhtmlMatchers.hasXPaths(
            //        "/root/foo[@bar=1]/test",
            //        "/root/bar"

            //final Document dom = DocumentBuilderFactory.newInstance()
            //    .newDocumentBuilder().newDocument();
            //new Xembler(dirs).apply(dom);
            //MatcherAssert.assertThat(
            //    XhtmlMatchers.xhtml(dom),
            //    XhtmlMatchers.hasXPaths(
            //        "/root/foo[@bar=1]/test",
            //        "/root/bar"
            //    )
            //);

            Assert.NotNull(
                FromXPath(
                    dom.InnerXml.ToString(), "/root/foo[@bar=1]/test")
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
