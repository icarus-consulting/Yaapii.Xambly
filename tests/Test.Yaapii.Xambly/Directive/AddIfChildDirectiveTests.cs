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

using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Xambly.Directive;

namespace Yaapii.Xambly
{
    public sealed class AddIfChildDirectiveTests
    {
        [Fact]
        public void DoesntAddTwice()
        {
            Assert.Equal(
                "<root>\r\n  <sub>\r\n    <dontOverrideMe>please</dontOverrideMe>\r\n    <child />\r\n    <child />\r\n  </sub>\r\n</root>",
                new Xambler(
                    new ManyOf<IDirective>(
                        new PushDirective(),
                        new AddDirective("root"),
                        new PushDirective(),
                        new AddDirective("sub"),
                        new AddDirective("dontOverrideMe"),
                        new SetDirective("please"),
                        new PopDirective(),
                        new AddIfChildDirective("sub", "dontOverrideMe", "please"),
                        new AddDirective("child"),
                        new UpDirective(),
                        new UpDirective(),
                        new AddIfChildDirective("sub", "dontOverrideMe", "please"),
                        new AddDirective("child")
                    )
                ).Apply(new XDocument()).ToString()
            );
        }
    }
}
