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

using System.Xml;
using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Arg;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class NsDirectiveTest
    {
        [Fact]
        public void SetsNsAttr() {
            var root = 
                new XElement("f",
                    new XElement("g")
                );
            var dom = new XDocument(root);
           
            new NsDirective(
                new ArgOf("somens")
            ).Exec(
                dom,
                new DomCursor(
                    new EnumerableOf<XNode>(root)
                ),
                new DomStack()
            );

            Assert.Equal("<f xmlns=\"somens\"><g /></f>",dom.ToString(SaveOptions.DisableFormatting));
        }

        [Fact]
        public void SetsNamsespaceForHtml()
        {
            //var root = new XElement("html",
            //                new XElement("head")
            //                //new XElement("body")
            //            );
            var dom = new XDocument();

            new Xambler(
                new AddDirective("html"),
                new NsDirective("http://www.w3.org/1999/xhtml"),
                new AddDirective("head"),
                new UpDirective(),
                new AddDirective("body")
            ).Apply(dom);


            //new NsDirective("http://www.w3.org/1999/xhtml")
            //    .Exec(
            //        dom,
            //        new DomCursor(
            //            new EnumerableOf<XNode>(root)
            //        ),
            //        new DomStack()
            //);

            Assert.Equal("<html xmlns=\"http://www.w3.org/1999/xhtml\"><head /><body /></html>", dom.ToString(SaveOptions.DisableFormatting));
        }
    }
}
