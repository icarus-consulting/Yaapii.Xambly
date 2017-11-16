using System;
using System.Collections.Generic;
using System.Text;
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

        //AKO
        /**
         * Directives can parse xembly grammar.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void ParsesIncomingGrammar()
        {
            //final Iterable<Directive> dirs = new Directives(
            //    "XPATH '//orders[@id=\"152\"]'; SET 'test';"
            //);
            //MatcherAssert.assertThat(
            //dirs,
            //Matchers.<Directive>iterableWithSize(2)
            //);
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

        //AKO
        /**
         * Directives can throw when XML content is broken.
         * @throws Exception If some problem inside
         */

        public void ThrowsOnBrokenXmlContent()
        {
            //@Test(expected = SyntaxException.class)
            //new Directives("ADD '\u001b';");
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

        //AKO
        /**
         * Directives can add map of values.
         * @throws Exception If some problem inside
         * @since 0.8
         */
        [Fact]
        public void AddsMapOfValues()
        {
            //final Document dom = DocumentBuilderFactory.newInstance()
            //.newDocumentBuilder().newDocument();
            //dom.appendChild(dom.createElement("root"));
            //new Xembler(
            //    new Directives().xpath("/root").add(
            //        new ArrayMap<String, Object>()
            //            .with("first", 1)
            //            .with("second", "two")
            //    ).add("third")
            //).apply(dom);
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
        /**
         * Directives can build a correct modification programme.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void PerformsFullScaleModifications()
        {
            //    final String script = new Directives()
            //    // @checkstyle MultipleStringLiteralsCheck (1 line)
            //    .add("html").attr("xmlns", "http://www.w3.org/1999/xhtml")
            //    .add("body")
            //    .add("p")
            //    .set("\u20ac \\")
            //    .toString();
            //     final Document dom = DocumentBuilderFactory.newInstance()
            //    .newDocumentBuilder().newDocument();
            //     new Xembler(new Directives(script)).apply(dom);
            //     MatcherAssert.assertThat(
            //        XhtmlMatchers.xhtml(dom),
            //        XhtmlMatchers.hasXPaths(
            //        "/xhtml:html",
            //        "/xhtml:html/body/p[.='\u20ac \\']"
            //    )
            //);
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
        /**
         * Directives can understand case.
         * @throws Exception If some problem inside
         * @since 0.14.1
         */
        [Fact]
        public void AddsElementsCaseSensitively()
        {
            //MatcherAssert.assertThat(
            //    new Xembler(new Directives().add("XHtml").addIf("Body")).xml(),
            //    XhtmlMatchers.hasXPaths(
            //        "/XHtml",
            //        "/XHtml/Body"
            //    )
            //);
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
        /**
         * Directives can push and pop.
         * @throws Exception If some problem inside
         */
        [Fact]
        public void pushesAndPopsCursor()
        {
            //    MatcherAssert.assertThat(
            //            XhtmlMatchers.xhtml(
            //        new Xembler(
            //            new Directives()
            //                .add("jeff")
            //                .push().add("lebowski")
            //                .push().xpath("/jeff").add("dude").pop()
            //                .attr("birthday", "today").pop()
            //                .add("los-angeles")
            //        ).xml()
            //    ),
            //    XhtmlMatchers.hasXPaths(
            //        "/jeff/lebowski[@birthday]",
            //        "/jeff/los-angeles",
            //        "/jeff/dude"
            //    )
            //);
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

        [Fact]
        public void ParsesGrammar()
        {
            IEnumerable<IDirective> dirs =
                new Directives(
                    "ADD 'yummy directive';"
            );

            Assert.True(
                new LengthOf(dirs).Value() == 1);
        }
    }
}