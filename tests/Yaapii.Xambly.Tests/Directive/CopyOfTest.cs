using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Directive;

namespace Yaapii.Xambly.Tests.Directive
{
    public sealed class CopyOfTest
    {
        [Fact]
        public void CopiesExistingNode()
        {
            var dom = new XDocument();
            var content = 
                new JoinedText(
                    "",
                    "<jeff name='Jeffrey'><first/><second/>",
                    "<?some-pi test?>",
                    "<file a='x'><f><name>\u20ac</name></f></file>",
                    "<!-- some comment -->",
                    "<x><![CDATA[hey you]]></x>  </jeff>"
                );
            var xml = XDocument.Parse(content.AsString());      
            new Xambler(
                new Joined<IDirective>(
                    new EnumerableOf<IDirective>(
                        new AddDirective("dudes")
                    ),
                    new CopyOfDirective(xml.Root)
                )
            ).Apply(dom);

            Assert.True(
                new LengthOf(dom.XPathSelectElements("/dudes/jeff[@name = 'Jeffrey']")).Value() > 0 &&
                new LengthOf(dom.XPathSelectElements("/dudes/jeff[first and second]")).Value() > 0 &&
                new LengthOf(dom.XPathSelectElements("/dudes/jeff/file[@a='x']/f[name='\u20ac']")).Value() > 0
            );
        }

        [Fact]
        public void CopyOfXmlDocument()
        {
            var dom = new XDocument();
            var content =
                new JoinedText(
                    "",
                    "<jeff name='Jeffrey'><first/><second/>",
                    "<?some-pi test?>",
                    "<file a='x'><f><name>\u20ac</name></f></file>",
                    "<!-- some comment -->",
                    "<x><![CDATA[hey you]]></x>  </jeff>"
                );
            var xml = XDocument.Parse(content.AsString());
            new Xambler(
                new Joined<IDirective>(
                     new EnumerableOf<IDirective>(
                        new AddDirective("dudes")
                    ),
                    new CopyOfDirective(xml.Root)
                )
            ).Apply(dom);

            Assert.True(
                new LengthOf(dom.XPathSelectElements("/dudes/jeff[@name = 'Jeffrey']")).Value() > 0
            );
        }
    }
}