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

namespace Yaapii.Xambly.Directive.Tests
{
    public class UpDirectiveTest
    {
        [Fact]
        public void JumpsToParentsWhenTheyExist()
        {
            var dom = new XDocument();

            Assert.True(
                new Xambler(
                    new AddDirective("root"),
                    new AddDirective("foo"),
                    new AddDirective("bar"),
                    new UpDirective(),
                    new SetDirective("Hello World")
                ).Apply(dom).ToString(SaveOptions.DisableFormatting) == "<root><foo>Hello World</foo></root>", "Up directive failed");
        }

        [Fact]
        public void ThrowsExceptionWhenNoParents()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
            {
                new Xambler(
                        new Yaapii.Atoms.Enumerable.ManyOf<IDirective>(
                                    new AddDirective("foo"),
                                    new UpDirective(),
                                    new UpDirective(),
                                    new UpDirective()
                                )).Apply(new XDocument());
            });
        }
    }
}
