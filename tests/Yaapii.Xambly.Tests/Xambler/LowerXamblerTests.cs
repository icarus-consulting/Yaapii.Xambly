using Xunit;

namespace Yaapii.Xambly.Tests
{
    public sealed class LowerXamblerTests
    {
        [Fact(Skip = "Skipped - Fix LowerXambler here")]
        public void MakesXmlLower()
        {
            string xml =
                new LowerXambler(
                    new Xambler(
                        new Directives()
                            .Add("PagE")
                        )
                ).Xml();

            Assert.StartsWith(
                "<page>",
                xml
            );
        }

        [Fact(Skip = "Skipped until LowerXembler works")]
        public void CreatesXmlWithHeader()
        {
            string xml =
                new LowerXambler(
                    new Xambler(
                        new Directives()
                            .Add("page")
                            .Add("child-node").Set(" the text\n")
                        )
                ).Xml();

            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                xml
            );
        }

        [Fact(Skip = "Skipped until LowerXembler works")]
        public void CreatesXmlWithoutHeader()
        {
            string xml =
                new LowerXambler(
                    new Xambler(
                        new Directives()
                            .Add("page")
                            .Add("child-node").Set(" the text\n")
                    )
                ).Xml(false);

            Assert.StartsWith(
                "<page><child-node>",
                xml
            );
        }

        [Fact(Skip = "Skipped until LowerXembler works")]
        public void CreatesXmlWithHeaderFromXmlQuietly()
        {
            string xml =
                new LowerXambler(
                    new Xambler(
                        new Directives()
                            .Add("page")
                            .Add("child-node").Set(" the text\n")
                    )
                ).XmlQuietly();

            Assert.StartsWith(
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>",
                xml
            );
        }

        [Fact(Skip = "Skipped until LowerXembler works")]
        public void CreatesXmlWithoutHeaderFromXmlQuietly()
        {
            string xml =
                new LowerXambler(
                    new Xambler(
                        new Directives()
                            .Add("page")
                            .Add("child-node").Set(" the text\n")
                    )
                ).XmlQuietly(false);

            Assert.StartsWith(
                "<page><child-node>",
                xml
            );
        }
    }
}
