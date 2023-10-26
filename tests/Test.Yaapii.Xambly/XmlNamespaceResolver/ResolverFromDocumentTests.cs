using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Map;

namespace Yaapii.Xambly.XmlNamespaceResolver.Test
{
    public sealed class ResolverFromDocumentTests
    {
        [Fact]
        public void KnowsDefaultNamespaces()
        {
            Assert.Equal(
                new MapOf(
                    "", "default"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns=\"default\">" +
                            "<node />" +
                        "</root>"
                    )
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsPrefixedDefalutNamespaces()
        {
            Assert.Equal(
                MapOf.New(
                    "", "default",
                    "def", "default"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns=\"default\">" +
                            "<node />" +
                        "</root>"
                    ),
                    "def"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsDefaultNamespacesFromChildren()
        {
            Assert.Equal(
                MapOf.New(
                    "def1", "childDefault",
                    "def2", "nodeDefault"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root>" +
                            "<child xmlns=\"childDefault\">" +
                                "<node xmlns=\"nodeDefault\" />" +
                            "</child>" +
                        "</root>"
                    ),
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
                new MapOf(
                    "pre", "withPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns:pre=\"withPrefix\">" +
                            "<child>" +
                                "<node />" +
                            "</child>" +
                        "</root>"
                    )
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
                MapOf.New(
                    "", "default",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns=\"default\" xmlns:pre=\"withPrefix\" xmlns:x=\"anotherWithPrefix\">" +
                            "<child>" +
                                "<node />" +
                            "</child>" +
                        "</root>"
                    )
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsPrefixedDefalutAndPrefixedNamespace()
        {
            Assert.Equal(
                MapOf.New(
                    "", "default",
                    "def", "default",
                    "pre", "withPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns=\"default\" xmlns:pre=\"withPrefix\">" +
                            "<child>" +
                                "<node />" +
                            "</child>" +
                        "</root>"
                    ),
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
                MapOf.New(
                    "", "default",
                    "def", "default",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix",
                    "def1", "childDefault",
                    "def2", "nodeDefault"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns=\"default\" xmlns:pre=\"withPrefix\" xmlns:x=\"anotherWithPrefix\">" +
                            "<child xmlns=\"childDefault\">" +
                                "<node xmlns=\"nodeDefault\" />" +
                            "</child>" +
                        "</root>"
                    ),
                    "def",
                    "childDefault", "def1",
                    "nodeDefault", "def2"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }
    }
}
