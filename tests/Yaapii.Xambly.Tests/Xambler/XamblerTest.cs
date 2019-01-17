// MIT License
//
// Copyright(c) 2017 ICARUS Consulting GmbH
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

using Xunit;

namespace Yaapii.Xambly.Tests
{
    public class XamblerTest
    {
        [Fact]
        public void CreatesXmlWithHeader()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).Xml();

            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                xml
            );
        }

        [Fact]
        public void CreatesXmlWithoutHeader()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).Xml(false);

            Assert.StartsWith(
                "<page><child-node>",
                xml
            );
        }

        [Fact]
        public void CreatesXmlWithHeaderFromXmlQuietly()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).XmlQuietly();

            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                xml
            );
        }

        [Fact]
        public void CreatesXmlWithoutHeaderFromXmlQuietly()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).XmlQuietly(false);

            Assert.StartsWith(
                "<page><child-node>",
                xml
            );
        }
    }
}
