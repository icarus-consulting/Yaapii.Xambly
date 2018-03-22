using System.Xml;
using Xunit;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xambly.Directive;

namespace Yaapii.Xml.Xambly.Tests.Directive
{
    public sealed class CopyOfTest
    {
        [Fact]
        public void CopiesExistingNode()
        {
            var dom = new XmlDocument();
            var content = new JoinedText(
                            "",
                            "<jeff name='Jeffrey'><first/><second/>",
                            "<?some-pi test?>",
                            "<file a='x'><f><name>\u20ac</name></f></file>",
                            "<!-- some comment -->",
                            "<x><![CDATA[hey you]]></x>  </jeff>");
            var xml = new XmlDocument();
            xml.LoadXml(content.AsString());
            new Xambler(
                new Directives()
                        .Add("dudes")
                        .Append(new CopyOfDirective(xml.DocumentElement))).Apply(dom);

            Assert.True(
                    new LengthOf(dom.SelectNodes("/dudes/jeff[@name = 'Jeffrey']")).Value() > 0 &&
                    new LengthOf(dom.SelectNodes("/dudes/jeff[first and second]")).Value() > 0 &&
                    new LengthOf(dom.SelectNodes("/dudes/jeff/file[@a='x']/f[name='\u20ac']")).Value() > 0
                );
        }

        [Fact(Skip = "true")]
        public void CopyOfXmlDocument()
        {
            var doc = new XmlDocument();
            var dirs = new CopyOfDirective(doc);
            var xml = new Xambler(dirs).Xml();
        }
    }
}