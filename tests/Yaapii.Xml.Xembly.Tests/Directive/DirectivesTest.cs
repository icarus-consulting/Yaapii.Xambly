using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Yaapii.Atoms.List;

namespace Yaapii.Xml.Xembly.Directive.Tests
{
    public class DirectivesTest
    {
        [Fact]
        public void ParsesGrammar()
        {
            IEnumerable<IDirective> dirs = 
                new Directives(
                    "ADD 'yummy directive';"
            );

            Assert.True(
                new LengthOf(dirs).Value() == 1);

            //MatcherAssert.assertThat(
            //    dirs,
            //    Matchers.< Directive > iterableWithSize(2)
            //);
        }
    }
}