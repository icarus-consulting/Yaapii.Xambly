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

using System.Collections.Generic;
using System.Xml;
using Xunit;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xambly.Cursor;
using Yaapii.Xml.Xambly.Directive;
using Yaapii.Xml.Xambly.Stack;

namespace Yaapii.Xml.Xambly.Directive.Tests
{
    public class PiDirectiveTest
    {
        /// <summary>
        /// Adds the processing instructions to DOM.
        /// </summary>
        [Fact]
        public void AddsProcessingInstructionsToDom() 
        {
            Assert.True(
                new Xambler(
                        new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
                                new AddDirective("root"),
                                new PiDirective("ab", "boom \u20ac"),
                                new AddDirective("test"),
                                new PiDirective("foo", "some data \u20ac")
                            )
                    ).Dom().InnerXml == "<root><?ab boom €?><test><?foo some data €?></test></root>"
            );
        }

        /// <summary>
        /// Adds the processing instructions directly to DOM.
        /// </summary>
        [Fact]
        public void AddsProcessingInstructionsDirectlyToDom()
        {
            var dom = new Xambler(
                            new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
                                new AddDirective("xxx")
                            )
                        ).Dom();

            new PiDirective("x", "y").Exec(
                                        dom,
                                        new DomCursor(
                                            new List<XmlNode>()
                                        ),
                                        new DomStack()
                                    );

            Assert.True(
                dom.InnerXml == "<?x y?><xxx />"
            );
        }

        /// <summary>
        /// Prepends the processing instructions to DOM root.
        /// </summary>
        [Fact]
        public void PrependsProcessingInstructionsToDomRoot()
        {
            var dom = new Xambler(
                            new Yaapii.Atoms.Enumerable.EnumerableOf<IDirective>(
                                new PiDirective("alpha","beta \u20ac"),
                                new AddDirective("x4")
                            )
                        ).Dom();

            Assert.True(
                dom.InnerXml == "<?alpha beta €?><x4 />"
            );
        }
    }
}
