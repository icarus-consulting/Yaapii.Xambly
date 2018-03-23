using Xunit;

namespace Yaapii.Xambly.Tests
{
    public class XamblerTest
    {
        [Fact]
        public void CreatesXmlWithHeader()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).Xml();

            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                xml
            );
        }

        [Fact]
        public void CreatesXmlWithoutHeader()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).Xml(false);

            Assert.StartsWith(
                "<page><child-node>",
                xml
            );
        }

        [Fact]
        public void CreatesXmlWithHeaderFromXmlQuietly()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).XmlQuietly();

            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                xml
            );
        }

        [Fact]
        public void CreatesXmlWithoutHeaderFromXmlQuietly()
        {
            string xml =
                new Xambler(
                    new Directives()
                        .Add("page")
                        .Add("child-node").Set(" the text\n")
                ).XmlQuietly(false);

            Assert.StartsWith(
                "<page><child-node>",
                xml
            );
        }
    }
}
