using DopX.Core.Entity.Machine.Memory;
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
                    new EnumerableOf<IDirective>(
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
