using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Atoms.List;

namespace Yaapii.Xml.Xembly.Directive.Tests
{
    public class DirectivesTest
    {
        //MTU
        [Fact]
        public void MakesXmlDocument()
        {
            //MatcherAssert.assertThat(
            //    XhtmlMatchers.xhtml(
            //    new Xembler(
            //        new Directives()
            //            .pi("xml-stylesheet", "none")
            //            .add("page")
            //            .attr("the-name", "with \u20ac")
            //            .add("child-node").set(" the text\n").up()
            //            .add("big_text").cdata("<<hello\n\n!!!>>").up()
            //    ).xml()
            //),
            //XhtmlMatchers.hasXPaths(
            //    "/page[@the-name]",
            //    "/page/big_text[.='<<hello\n\n!!!>>']"
            //)
            //);
        }

        /// <summary>
        /// Directives can parse xembly grammar. 
        /// </summary>
        [Fact]
        public void ParsesIncomingGrammar()
        {
            IEnumerable<IDirective> dirs =
               new Directives(
                   "ADD 'yummy directive';"
           );

            Assert.True(
                new LengthOf(dirs).Value() == 1);
        }

        //MTU
        /**
         * Directives can throw when grammar is broken.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void ThrowsOnBrokenGrammar()
        {
            //@Test(expected = SyntaxException.class)
            //new Directives("not a xembly at all");
        }


        /// <summary>
        /// Directives can throw when grammar is broken.
        /// </summary>
        [Fact]
        public void ThrowsOnBrokenXmlContent()
        {
            //@Test(expected = SyntaxException.class)
            //new Directives("ADD '\u001b';");
            Assert.Throws<SyntaxException>(
                    () => new Directives("ADD '\u001b';")
                );
        }

        //MTU
        /**
         * Directives can throw when escaped XML content is broken.
         * @throws Exception If some problem inside
         */
        public void ThrowsOnBrokenEscapedXmlContent()
        {
            //    @Test(expected = SyntaxException.class)
            //new Directives("ADD '&#27;';");
        }

        /// <summary>
        /// Directives can add map of values.
        /// </summary>
        [Fact]
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

        //MTU
        /**
         * Directives can ignore empty input.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void IgnoresEmptyInput()
        {
            //MatcherAssert.assertThat(
            //new Directives("\n\t   \r"),
            //Matchers.emptyIterable());
        }

        //AKO
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

        //MTU
        /**
         * Directives can copy an existing node.
         * @throws Exception If some problem inside
         * @since 0.13
         */
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

        //AKO
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

        //MTU
        /**
         * Directives can convert to string.
         * @throws Exception If some problem inside
         * @since 0.15.2
         */
        [Fact]
        public void ConvertsToString()
        {
            //final Directives dirs = new Directives();
            //for (int idx = 0; idx < Tv.TEN; ++idx)
            //{
            //    dirs.add("HELLO");
            //}
            //MatcherAssert.assertThat(
            //    dirs,
            //    Matchers.hasToString(Matchers.containsString("8:"))
            //    );
            //MatcherAssert.assertThat(
            //    new Directives(dirs.toString()),
            //    Matchers.not(Matchers.emptyIterable())
            //);
        }

        //AKO
        /// <summary>
        /// Directives can push and pop.
        /// </summary>
        [Fact]
        public void pushesAndPopsCursor()
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

        //MTU
        /**
         * Directives can use namespaces.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void PrefixesItemsWithNamespaces()
        {
            //MatcherAssert.assertThat(
            //new Xembler(
            //    new Directives()
            //        .add("bbb")
            //        .attr("xmlns:x", "http://www.w3.org/1999/xhtml")
            //        .add("x:node").set("HELLO WORLD!")
            //).xml(),
            //XhtmlMatchers.hasXPath("//xhtml:node")
            //);
        }

        //AKO
        /**
         * Directives can accept directives from multiple threads.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void AcceptsFromMultipleThreads()
        {
            //final Directives dirs = new Directives().add("mt6");
            //new Callable<Void>() {
            //@Parallel(threads = Tv.FIFTY)
            //@Override
            //public Void call() throws Exception
            //{
            //    dirs.append(
            //            new Directives()
            //                .add("fo9").attr("yu", "").set("some text 90")
            //                .add("tr4").attr("s2w3", "").set("some other text 76")
            //                .up().up()
            //        );
            //        return null;
            //    }
            //} .call();
            //MatcherAssert.assertThat(
            //    XhtmlMatchers.xhtml(new Xembler(dirs).xml()),
            //            XhtmlMatchers.hasXPath("/mt6[count(fo9[@yu])=50]")
            //        );
        }


    }
}