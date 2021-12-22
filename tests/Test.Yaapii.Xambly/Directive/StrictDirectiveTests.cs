// MIT License
//
// Copyright(c) 2021 ICARUS Consulting GmbH
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

using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly.Directive.Tests
{
    public sealed class StrictDirectiveTests
    {
        [Fact]
        public void ChecksNumberOfCurrentNodes()
        {
            var dom = 
                new XDocument(
                    new XElement("root",
                        new XElement("foo",
                            new XElement("bar",
                                new XElement("boom")
                            )
                        )
                    )
                );
            new Xambler(
                    new XpathDirective("//*"),
                    new StrictDirective(4)
                ).Apply(dom);

            Assert.True(
                null != FromXPath(
                    dom.ToString(SaveOptions.DisableFormatting), "/root/foo") &&
                null != FromXPath(
                    dom.ToString(SaveOptions.DisableFormatting), "/root/foo/bar/boom"));
        }

        [Fact]
        public void FailsWhenNumberOfCurrentNodesIsTooBig()
        {
            var dom = new XDocument();

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
            var dom = new XDocument();

            Assert.Throws(new ImpossibleModificationException("").GetType(), () =>
            {
                new Xambler(
                        new Yaapii.Atoms.Enumerable.ManyOf<IDirective>(
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
            var dom = new XDocument();

            Assert.Throws(new ImpossibleModificationException("").GetType(), () =>
            {
                new Xambler(
                        new Yaapii.Atoms.Enumerable.ManyOf<IDirective>(
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
