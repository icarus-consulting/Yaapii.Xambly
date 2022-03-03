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

using System.Collections.Generic;
using System.IO;

namespace Yaapii.Xambly.Directive.Tests
{
    public sealed class XsetDirectiveTests
    {
        [Fact]
        public void SetsTextDirectlyIntoDomNodes()
        {
            try
            {
                var resolver = new XmlNamespaceManager(new NameTable());
                var doc = new XDocument();
                var root = new XElement("xxx");
                var first = new XElement("first");

                first.Value = "15";
                root.Add(first);
                var second = new XElement("second");
                second.Value = "13";
                root.Add(second);

                doc.Add(root);
                //doc.Save("XsetDirectiveTests.xml");

                new XsetDirective("sum(/xxx/*/text()) + 6")
                .Exec(
                    doc,
                    new DomCursor(new List<XNode>() { first }),
                    new DomStack(),
                    resolver
                );
                doc.Save("XsetDirectiveTests.xml");

                XPathNavigator nav = doc.CreateNavigator();
                nav.MoveToFirstChild();
                nav.MoveToFirstChild();

                Assert.True(nav.Value == "34");

            }
            finally
            {
                if (File.Exists("XsetDirectiveTests.xml"))
                {
                    File.Delete("XsetDirectiveTests.xml");
                }
            }
        }

    }
}
