// MIT License
//
// Copyright(c) 2019 ICARUS Consulting GmbH
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
using System.Xml.Linq;
using Xunit;
using Yaapii.Xambly.Cursor;
using Yaapii.Xambly.Stack;

namespace Yaapii.Xambly.Directive.Tests
{
    public class PiDirectiveTest
    {
        [Fact]
        public void AddsProcessingInstructionsToDom()
        {
            Assert.Equal(
                "<root><?ab boom €?><test><?foo some data €?></test></root>",
                new Xambler(
                    new Atoms.Enumerable.ManyOf<IDirective>(
                        new AddDirective("root"),
                        new PiDirective("ab", "boom \u20ac"),
                        new AddDirective("test"),
                        new PiDirective("foo", "some data \u20ac")
                    )
                ).Dom().ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void AddsProcessingInstructionsDirectlyToDom()
        {
            var resolver = new XmlNamespaceManager(new NameTable());
            var dom =
                new Xambler(
                    new Atoms.Enumerable.ManyOf<IDirective>(
                        new AddDirective("xxx")
                    )
                ).Dom();

            new PiDirective("x", "y").Exec(
                dom,
                new DomCursor(
                    new List<XNode>()
                ),
                new DomStack(),
                resolver
            );

            Assert.Equal(
                "<?x y?><xxx />",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }

        [Fact]
        public void PrependsProcessingInstructionsToDomRoot()
        {
            var dom = new Xambler(
                            new Yaapii.Atoms.Enumerable.ManyOf<IDirective>(
                                new PiDirective("alpha", "beta \u20ac"),
                                new AddDirective("x4")
                            )
                        ).Dom();

            Assert.Equal(
                "<?alpha beta €?><x4 />",
                dom.ToString(SaveOptions.DisableFormatting)
            );
        }
    }
}
