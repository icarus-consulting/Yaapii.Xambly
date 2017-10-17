using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Yaapii.Xml.Xembly.Stack;

namespace Yaapii.Xml.Xembly.Tests.Stack
{
    public class DomStackTest
    {
        [Fact]
        public void AddsAndRetrieves()
        {
            var stack = new DomStack();
            var first = new FkCursor();
            var second = new FkCursor();

            stack.Push(first); stack.Push(second);

            Assert.Equal(stack.Pop(), second);
            Assert.Equal(stack.Pop(), first);
        }

        [Fact]
        public void CantPopEmpty()
        {
            Assert.Throws<ImpossibleModificationException>(() =>
                new DomStack().Pop());
        }
    }
}
