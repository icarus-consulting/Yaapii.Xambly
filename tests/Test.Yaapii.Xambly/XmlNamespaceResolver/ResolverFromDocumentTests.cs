using System.Xml.Linq;
using Xunit;
using Yaapii.Atoms.Map;

namespace Yaapii.Xambly.XmlNamespaceResolver.Test
{
    public sealed class ResolverFromDocumentTests
    {
        [Fact]
        public void HasDefaultPrefixForDefaultNamespaces()
        {
            Assert.Equal(
                new MapOf(
                    "def1", "uri",
                    "def2", "ohterUri"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns='uri'>" +
                            "<node xmlns='ohterUri'/>" +
                        "</root>"
                    )
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsPrefixedDefalutNamespaces()
        {
            Assert.Equal(
                new MapOf(
                    "myPrefix", "uri"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns='uri'>" +
                            "<node />" +
                        "</root>"
                    ),
                    "uri", "myPrefix"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsDefaultNamespacesFromChildren()
        {
            Assert.Equal(
                MapOf.New(
                    "prefix", "childUri",
                    "nspx", "nodeUri"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root>" +
                            "<child xmlns='childUri'>" +
                                "<node xmlns='nodeUri' />" +
                            "</child>" +
                        "</root>"
                    ),
                    "childUri", "prefix",
                    "nodeUri", "nspx"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }



        [Fact]
        public void KnowsPrefixedNamespace()
        {
            Assert.Equal(
                new MapOf(
                    "pre", "withPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns:pre='withPrefix'>" +
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
            Assert.Equal(
                MapOf.New(
                    "def1", "default",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns='default' xmlns:pre='withPrefix' >" +
                            "<child xmlns:x='anotherWithPrefix'>" +
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
                    "myDefault", "defaultUri",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns='defaultUri' xmlns:pre='withPrefix'>" +
                            "<child xmlns:x='anotherWithPrefix'>" +
                                "<node />" +
                            "</child>" +
                        "</root>"
                    ),
                    "defaultUri", "myDefault"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void KnowsAllInjectedNamespaces()
        {
            Assert.Equal(
                MapOf.New(
                    "myDefault", "defaultUri",
                    "notUsedPrefix", "notUsed",
                    "pre", "withPrefix",
                    "x", "anotherWithPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns='defaultUri' xmlns:pre='withPrefix'>" +
                            "<child xmlns:x='anotherWithPrefix'>" +
                                "<node />" +
                            "</child>" +
                        "</root>"
                    ),
                    "defaultUri", "myDefault",
                    "notUsed", "notUsedPrefix"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }

        [Fact]
        public void DoesNotOverwritesPrefixedNamespaces()
        {
            Assert.Equal(
                MapOf.New(
                    "myDefault", "defaultUri",
                    "canNotOverwrite", "withPrefix",
                    "pre", "withPrefix"
                ),
                new ResolverFromDocument(
                    XDocument.Parse(
                        "<root xmlns='defaultUri' xmlns:pre='withPrefix'>" +
                            "<child>" +
                                "<node />" +
                            "</child>" +
                        "</root>"
                    ),
                    "defaultUri", "myDefault",
                    "withPrefix", "canNotOverwrite"
                ).GetNamespacesInScope(System.Xml.XmlNamespaceScope.ExcludeXml)
            );
        }
    }
}
