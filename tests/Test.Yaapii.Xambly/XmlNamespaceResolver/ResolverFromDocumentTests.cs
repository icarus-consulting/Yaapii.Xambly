using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Map;

namespace Yaapii.Xambly.XmlNamespaceResolver.Test
{
    public sealed class ResolverFromDocumentTests
    {
        [Fact]
        public void KnowsDefalutNamespaces()
        {
            var expected =
                new MapOf(
                    "", "default"
                );
            var xml =
                "<root xmlns=\"default\">" +
                    "<node />" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml)
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsPrefixedDefalutNamespaces()
        {
            var expected =
                MapOf.New(
                    "", "default",
                    "def", "default"
                );
            var xml =
                "<root xmlns=\"default\">" +
                    "<node />" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml),
                    "def"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsDefaultNamespacesFromChildren()
        {
            var expected =
                MapOf.New(
                    "def1", "childDefault",
                    "def2", "nodeDefault"
                );
            var xml =
                "<root>" +
                    "<child xmlns=\"childDefault\">" +
                        "<node xmlns=\"nodeDefault\" />" +
                    "</child>" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml),
                    "irrelevant",
                    "childDefault", "def1",
                    "nodeDefault", "def2"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }



        [Fact]
        public void KnowsPrefixedNamespace()
        {
            var expected =
                new MapOf(
                    "pre", "withPrefix"
                );
            var xml =
                "<root xmlns:pre=\"withPrefix\">" +
                    "<child>" +
                        "<node />" +
                    "</child>" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml)
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsDefalutAndPrefixedNamespace()
        {
            var expected =
                MapOf.New(
                    "", "default",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix"
                );
            var xml =
                "<root xmlns=\"default\" xmlns:pre=\"withPrefix\" xmlns:x=\"anotherWithPrefix\">" +
                    "<child>" +
                        "<node />" +
                    "</child>" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml)
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsPrefixedDefalutAndPrefixedNamespace()
        {
            var expected =
                MapOf.New(
                    "", "default",
                    "def", "default",
                    "pre", "withPrefix"
                );
            var xml =
                "<root xmlns=\"default\" xmlns:pre=\"withPrefix\">" +
                    "<child>" +
                        "<node />" +
                    "</child>" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml),
                    "def"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsPrefixedDefaultAndChildrenDefaultAndPrefixedNamespace()
        {
            var expected =
                MapOf.New(
                    "", "default",
                    "def", "default",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix",
                    "def1", "childDefault",
                    "def2", "nodeDefault"
                );
            var xml =
                "<root xmlns=\"default\" xmlns:pre=\"withPrefix\" xmlns:x=\"anotherWithPrefix\">" +
                    "<child xmlns=\"childDefault\">" +
                        "<node xmlns=\"nodeDefault\" />" +
                    "</child>" +
                "</root>";

            Assert.Equal(
                expected,
                new ResolverFromDocument(
                    XDocument.Parse(xml),
                    "def",
                    "childDefault", "def1",
                    "nodeDefault", "def2"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }
    }
}
