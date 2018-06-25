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
        public void CopiesXDocument()
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

        [Fact]
        public void CopyOfXmlDocument()
        {
            var dom = new XDocument();
            var content =
                new TextOf(
                    "<?xml version=\"1.0\" encoding=\"utf-16\"?>"
                    + "<target id=\"BBA3CBB0-00F0-43DD-9F74-87C53D35270C\" name=\"j1046rfa_11\">"
                    + "<body><accessibility><readonly>false</readonly><reason /></accessibility><type>default</type><spatials><cartesian machine=\"BEFC5DE8-C579-44A6-ACDF-65293C39C87F\"><translation x=\"-4150.784755\" y=\"-1819.545936\" z=\"955.4282165\" /><rotation rx=\"-2.949491294\" ry=\"-0.8920227643\" rz=\"-3.120333533\" /><configuration><joints><joint name=\"j1\" state=\"irrelevant\"><turn hasvalue=\"true\">0</turn></joint><joint name=\"j2\" state=\"irrelevant\"><turn hasvalue=\"false\">0</turn></joint><joint name=\"j3\" state=\"negative\"><turn hasvalue=\"false\">0</turn></joint><joint name =\"j4\" state=\"irrelevant\"><turn hasvalue=\"true\">0</turn></joint><joint name=\"j5\" state=\"positive\"><turn hasvalue=\"false\">0</turn></joint><joint name=\"j6\" state=\"irrelevant\"><turn hasvalue=\"true\">0</turn></joint></joints><overhead>negative</overhead></configuration></cartesian><axial machine=\"F399FDD7-B9FF-4D5F-8A65-C5EC35D306BE\"><axes><axis name=\"j1\">-4871</axis></axes></axial></spatials><parameters><motiontype>ptp</motiontype><speed>70</speed><acceleration>100</acceleration><zone>C_DIS</zone><referenceframe>b4</referenceframe><toolframe>t2</toolframe><storagetype>local</storagetype><processtype /></parameters><postolpcommands /></body><foot /></target>"
                );
            var xml = XDocument.Parse(content.AsString());
            new Xambler(
                new Joined<IDirective>(
                     new EnumerableOf<IDirective>(
                        new AddDirective("target")
                    ),
                    new CopyOfDirective(xml.Root.Element("body"))
                )
            ).Apply(dom);

            var nav = dom.CreateNavigator();
            Assert.True(
                nav.SelectSingleNode("/target/body/accessibility/readonly").Value == "false"
            );
        }
    }
}