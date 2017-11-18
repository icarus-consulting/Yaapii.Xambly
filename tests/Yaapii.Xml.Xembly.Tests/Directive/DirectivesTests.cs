using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using Xunit;
using Yaapii.Atoms.IO;
using Yaapii.Atoms.List;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Directive.Tests
{
    public class DirectivesTests
    {
        /// <summary>
        /// Directives can make a document
        /// </summary>
        [Fact]
        public void MakesXmlDocument()
        {
            string xml =
                new Xembler(
                    new Directives()
                        //.pi("xml-stylesheet", "none")
                        .Add("page")
                        .Attr("the-name", "with \u20ac")
                        .Add("child-node").Set(" the text\n").Up()
                        .Add("big-text").Cdata("<<hello!!!>>").Up()
                ).Xml();

            Assert.NotNull(FromXPath(xml, "/page[@the-name]"));
            Assert.NotNull(FromXPath(xml, "/page/big-text[normalize-space(.)='<<hello!!!>>']"));
        }

        /// <summary>
        /// Directives can parse Xembly grammar
        /// </summary>
        [Fact]
        public void ParsesIncomingGrammar()
        {
            IEnumerable<IDirective> dirs =
               new Directives(
                   "ADD 'yummy directive';"
           );

            Assert.True(
                new Yaapii.Atoms.List.LengthOf(dirs).Value() == 1);
        }

        /// <summary>
        /// Directives throw on incorrect grammar
        /// </summary>
        [Fact]
        public void ThrowsOnBrokenGrammar()
        {
            Assert.Throws<SyntaxException>(() =>
                new Directives("not a xembly at all"));
        }

        /// <summary>
        /// Directives throw on incorrect xmlcontent
        /// </summary>
        [Fact]
        public void ThrowsOnBrokenXmlContent()
        {
            Assert.Throws<SyntaxException>(() =>
                new Directives("ADD '\u001b';"));
        }


        /// <summary>
        /// Directives throw on incorrectly escaped xmlcontent
        /// </summary>
        [Fact]
        public void ThrowsOnBrokenEscapedXmlContent()
        {
            Assert.Throws<SyntaxException>(() =>
                new Directives("ADD '&#27;';"));

        }

        /// <summary>
        /// Directives can add map of values.
        /// </summary>
        [Fact(Skip = "True")]
        public void AddsMapOfValues()
        {
            var dom = new XmlDocument();
            dom.AppendChild(dom.CreateElement("root"));
            new Xembler(
                new Directives()
                .Xpath("/root")
                .Add(
                    new Dictionary<String, Object>() {
                            { "first", 1 },{ "second", "two" }
                    })
                .Add("third")
            ).Apply(dom);

            throw new NotImplementedException();

            //MatcherAssert.assertThat(
            //    XhtmlMatchers.xhtml(dom),
            //    XhtmlMatchers.hasXPaths(
            //        "/root/first[.=1]",
            //        "/root/second[.='two']",
            //        "/root/third"
            //    )
            //);
        }

        /// <summary>
        /// Directives can ignore empty input.
        /// </summary>
        [Fact]
        public void IgnoresEmptyInput()
        {
            Assert.Empty(
                new Directives("\n\t   \r"));
        }

        /// <summary>
        /// Directives can build a correct modification programme.
        /// </summary>
        [Fact]
        public void PerformsFullScaleModifications()
        {
            Assert.True(
                new Xembler(
                    new Directives(
                        new Directives()
                            .Add("html").Attr("xmlns", "http://www.w3.org/1999/xhtml")
                            .Add("body")
                            .Add("p")
                            .Set("\u20ac \\")
                            .ToString()
                    )
                ).Apply(
                    new XmlDocument()
                ).InnerXml == "<html xmlns=\"http://www.w3.org/1999/xhtml\"><body><p>€ \\</p></body></html>");
        }

        //NOT FOR NOW - CopyTo missing atm
        /// <summary>
        /// Directives can copy an existing node
        /// </summary>
        [Fact]
        public void CopiesExistingNode()
        {
            //final Document dom = DocumentBuilderFactory.newInstance()
            //.newDocumentBuilder().newDocument();
            //new Xembler(
            //    new Directives().add("dudes").append(
            //        Directives.copyOf(
            //            new XMLDocument(
            //                StringUtils.join(
            //                    "<jeff name='Jeffrey'><first/><second/>",
            //                    "<?some-pi test?>",
            //                    "<file a='x'><f><name>\u20ac</name></f></file>",
            //                    "<!-- some comment -->",
            //                    "<x><![CDATA[hey you]]></x>  </jeff>"
            //                )
            //            ).node()
            //        )
            //    )
            //).apply(dom);
            //MatcherAssert.assertThat(
            //    XhtmlMatchers.xhtml(dom),
            //    XhtmlMatchers.hasXPaths(
            //        "/dudes/jeff[@name = 'Jeffrey']",
            //        "/dudes/jeff[first and second]",
            //        "/dudes/jeff/file[@a='x']/f[name='\u20ac']"
            //    )
            //);
        }

        /// <summary>
        /// Directives can understand case.
        /// </summary>
        [Fact]
        public void AddsElementsCaseSensitively()
        {
            var xml = new Xembler(new Directives().Add("XHtml").AddIf("Body")).Xml();
            Assert.True(
                 xml == "<?xml version=\"1.0\" encoding=\"utf-16\"?><XHtml><Body /></XHtml>"
                );
        }

        /// <summary>
        /// Directives can convert to string.
        /// </summary>
        [Fact]
        public void ConvertsToString()
        {
            Directives dirs = new Directives();
            for (int idx = 0; idx < 10; ++idx)
            {
                dirs.Add("HELLO");
            }

            var xml = dirs.ToString();

            Assert.True(
                new Regex("ADD \"HELLO\";").Matches(xml).Count == 10);
        }

        /// <summary>
        /// Directives can push and pop
        /// </summary>
        [Fact]
        public void PushesAndPopsCursor()
        {
            var xml = new Xembler(
                             new Directives()
                                 .Add("jeff")
                                 .Push().Add("lebowski")
                                 .Push().Xpath("/jeff").Add("dude").Pop()
                                 .Attr("birthday", "today").Pop()
                                 .Add("los-angeles")
                       ).Xml();

            Assert.True(xml == "<?xml version=\"1.0\" encoding=\"utf-16\"?><jeff><lebowski birthday=\"today\" /><los-angeles /></jeff>");
        }

        /// <summary>
        /// Directives can use namespaces.
        /// </summary>
        [Fact]
        public void PrefixesItemsWithNamespaces()
        {
            var xml =
                new Xembler(
                    new Directives()
                        .Add("bbb")
                        .Attr("xmlns:x", "http://www.w3.org/1999/xhtml")
                        .Add("x:node").Set("HELLO WORLD!")
                ).Xml();

            Assert.NotNull(FromXPath(xml, "//x:node"));
        }

        /// <summary>
        /// Directives can accept directives from multiple threads.
        /// </summary>
        [Fact()]
        public void AcceptsFromMultipleThreads()
        {
            var dirs = new Directives().Add("mt6");

            var threadStart = new ThreadStart(() =>
            {
                lock (dirs)
                {
                    dirs.Add("fo9")
                    .Attr("yu", "")
                    .Set("some text 90")
                    .Add("tr4")
                    .Attr("s2w3", "")
                    .Set("some other text 76")
                    .Up()
                    .Up();
                }
            });

            for (int i = 0; i < 50; i++)
            {
                new Thread(threadStart).Start();
            }

            var lst = new List<string>();
            var xembler = new Xembler(dirs);
            var xml = xembler.Xml();

            threadStart = new ThreadStart(() =>
            {
                var xmlContent = String.Empty;
                xmlContent = xembler.Xml();

                lst.Add(xmlContent);
            });

            for (int i = 0; i < 50; i++)
            {
                new Thread(threadStart).Start();
            }

            Assert.All(lst, tXml => tXml.Equals(xml));
            Assert.EndsWith("<mt6>" + new RepeatedText("<fo9 yu=\"\">some text 90<tr4 s2w3=\"\">some other text 76</tr4></fo9>", 50).AsString() + "</mt6>", xml);
        }

        /// <summary>
        /// Directives can parse Xembly script
        /// </summary>
        [Fact]
        public void ParsesGrammar()
        {
            IEnumerable<IDirective> dirs =
                new Directives(
                    "ADD 'yummy directive';"
            );

            Assert.True(
                new Yaapii.Atoms.List.LengthOf(dirs).Value() == 1);
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