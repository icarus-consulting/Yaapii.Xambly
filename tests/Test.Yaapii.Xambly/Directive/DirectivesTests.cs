// MIT License
//
// Copyright(c) 2022 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.IO;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive.Tests
{
    public class DirectivesTest
    {
        /// <summary>
        /// Directives can make a document
        /// </summary>
        [Theory]
        [InlineData("/page[@the-name]")]
        [InlineData("/page/big-text[normalize-space(.)='<<hello!!!>>']")]
        public void MakesXmlDocument(string testXPath)
        {
            string xml =
                new Xambler(
                    new Directives()
                        //.pi("xml-stylesheet", "none")
                        .Add("page")
                        .Attr("the-name", "with \u20ac")
                        .Add("child-node").Set(" the text\n").Up()
                        .Add("big-text").Cdata("<<hello!!!>>").Up()
                ).Xml();

            Assert.NotNull(FromXPath(xml, testXPath));
        }

        [Fact]
        public void TakesDirectiveParams()
        {
            string xml =
                new Xambler(
                    new Directives(
                        new AddDirective("page"),
                        new AddDirective("child-node")
                    )
                ).Xml();

            Assert.NotNull(FromXPath(xml, "/page/child-node"));
        }

        [Fact]
        public void TakesDirectiveList()
        {
            string xml =
                new Xambler(
                    new Directives(
                        new ManyOf<IDirective>(
                            new AddDirective("page"),
                            new AddDirective("child-node")
                        )
                    )
                ).Xml();

            Assert.NotNull(FromXPath(xml, "/page/child-node"));
        }

        /// <summary>
        /// Directives throw on incorrect xmlcontent
        /// </summary>
        [Fact]
        public void ThrowsOnBrokenXmlContent()
        {
            Assert.Throws<XmlException>(() =>
            {
                var dom = new XDocument(new XElement("root"));
                new Xambler(new Directives().Add("\u001b")).Apply(dom);
            });
        }

        /// <summary>
        /// Directives can add map of values.
        /// </summary>
        [Theory]
        [InlineData("/root/first[.=1]")]
        [InlineData("/root/second[.='two']")]
        [InlineData("/root/third")]
        public void AddsMapOfValues(string testXPath)
        {
            var dom = new XDocument(new XElement("root"));
            var xml =
                new Xambler(
                    new Directives()
                    .Xpath("//root")
                    .Add(
                        new Dictionary<String, Object>() {
                            { "first", 1 },
                            { "second", "two" }
                        })
                    .Add("third")
                ).Apply(dom).ToString(SaveOptions.DisableFormatting);

            Assert.True(FromXPath(xml, testXPath) != null);
        }

        /// <summary>
        /// Directives can understand case.
        /// </summary>
        [Fact]
        public void AddsElementsCaseSensitively()
        {
            var xml =
                new Xambler(
                    new Directives()
                        .Add("XHtml")
                        .AddIf("Body")
                ).Xml();
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
                new Regex("ADD \"HELLO\";").Matches(xml).Count == 10
            );
        }

        /// <summary>
        /// Directives can push and pop
        /// </summary>
        [Theory]
        [InlineData("/jeff/lebowski[@birthday]")]
        [InlineData("/jeff/los-angeles")]
        [InlineData("/jeff/dude")]
        public void PushesAndPopsCursor(string testXPath)
        {
            var xml =
                new Xambler(
                    new Directives()
                        .Add("jeff")
                        .Push().Add("lebowski")
                        .Push().Xpath("/jeff").Add("dude").Pop()
                        .Attr("birthday", "today").Pop()
                        .Add("los-angeles")
                ).Xml();

            Assert.True(
                null != FromXPath(xml, testXPath)
            );
        }

        /// <summary>
        /// An absolute XPath should set the cursor successfully.
        /// </summary>
        [Fact]
        public void NavigatesFromRootAfterDeletedNode()
        {
            var xml = XDocument.Load(new InputOf("<root><child name='Jerome'><property name='test'/></child></root>").Stream());
            var xambler =
                new Xambler(
                    new Directives()
                        .Xpath("/root/child[@name='Jerome']/property[@name='test']")
                        .Remove()   // Node will be deleted. After this operation the cursor points to the parent nodes.
                        .Xpath("/root/child[@name='Jerome']")
                        .Add("property")
                        .Attr("name", "test2")
                ).Apply(xml);

            Assert.Equal(
                "<root><child name=\"Jerome\"><property name=\"test2\" /></child></root>",
                xambler.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void RejectsAddingToDocumentNode()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
            {
                var xml = new XDocument();
                var xambler =
                    new Xambler(
                        new Directives()
                            .Add("root")
                            .Add("child")
                            .Xpath("/")
                            .Attr("some", "attribute")
                    ).Apply(xml);
            }
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
