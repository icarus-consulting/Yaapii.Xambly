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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Xunit;
using Yaapii.Xml.Xambly.Arg;

namespace Yaapii.Xml.Xambly.Tests
{
    public class EscapedTest
    {
        [Fact]
        public void EscapesAndUnescapes()
        {
            var texts = new String[] {
                "",
                "123",
                "test \u20ac привет & <>'\"\\",
                "how are you there,\t\n\rтоварищ? &#0D;",
            };

            foreach (String text in texts)
            {
                Assert.Equal(
                    new Unescaped(new ArgOf(text).AsString()).AsString(),
                    text);
            }
        }

        [Fact]
        public void CantEscapeInvalidXMLChars()
        {
            Assert.Throws<XmlException>(
                () => new ArgOf("\u001b\u0000").AsString());
        }
    }
}

