using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Func;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Xambly.XmlNamespaceResolver
{
    /// <summary>
    /// A XML namespace resolver that knows the namespaces from an existing XML.
    /// To enable XPath queries with default namespaces, a prefix for the default namespace can be set by defining tuples of the namespace uri and a prefix.
    /// It is allowed to define additional tuples of namespace uris and prefixes that can be used later in XPath queries.
    /// </summary>
    public sealed class ResolverFromDocument : IXmlNamespaceResolver
    {
        private readonly IScalar<IXmlNamespaceResolver> resolver;

        /// <summary>
        /// A XML namespace resolver that knows the namespaces from an existing XML.
        /// To enable XPath queries with default namespaces, a prefix for the default namespace can be set by defining tuples of the namespace uri and a prefix.
        /// It is allowed to define additional tuples of namespace uris and prefixes that can be used later in XPath queries.
        /// </summary>
        public ResolverFromDocument(XDocument xml) : this(
            ScalarOf.New(xml),
            MapOf.New<string, string>()
        )
        { }

        /// <summary>
        /// A XML namespace resolver that knows the namespaces from an existing XML.
        /// To enable XPath queries with default namespaces, a prefix for the default namespace can be set by defining tuples of the namespace uri and a prefix.
        /// It is allowed to define additional tuples of namespace uris and prefixes that can be used later in XPath queries.
        /// </summary>
        public ResolverFromDocument(XDocument xml, params string[] namespaceUris) : this(
            ScalarOf.New(xml),
            new MapOf(namespaceUris)
        )
        { }

        /// <summary>
        /// A XML namespace resolver that knows the namespaces from an existing XML.
        /// To enable XPath queries with default namespaces, a prefix for the default namespace can be set by defining tuples of the namespace uri and a prefix.
        /// It is allowed to define additional tuples of namespace uris and prefixes that can be used later in XPath queries.
        /// </summary>
        public ResolverFromDocument(XDocument xml, IDictionary<string, string> namespaceUris) : this(
            ScalarOf.New(xml),
            namespaceUris
        )
        { }

        /// <summary>
        /// A XML namespace resolver that knows the namespaces from an existing XML.
        /// To enable XPath queries with default namespaces, a prefix for the default namespace can be set by defining tuples of the namespace uri and a prefix.
        /// It is allowed to define additional tuples of namespace uris and prefixes that can be used later in XPath queries.
        /// </summary>
        public ResolverFromDocument(IScalar<XDocument> xml, IDictionary<string, string> namespaceUris)
        {
            this.resolver =
                new ScalarOf<IXmlNamespaceResolver>(() =>
                {
                    var manager = this.ManagerWithInitialNamespaces(namespaceUris);

                    var counter = 1;
                    Each.New(nsDeclaration =>
                        {
                            var prefix = Prefix(namespaceUris, nsDeclaration, $"def{counter++}");
                            if (IsNotReservedPrefix(prefix))
                            {
                                manager.AddNamespace(
                                    prefix,
                                    nsDeclaration.Value
                                );
                            }
                        },
                        this.NamespaceDeclarationsFromDocument(xml)
                    ).Invoke();

                    return manager;
                });
        }

        /// <summary>
        /// Gets a collection of defined prefix-namespace mappings that are currently in scope.
        /// </summary>
        public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
        {
            return
                this.resolver.Value().GetNamespacesInScope(scope);
        }

        /// <summary>
        /// Gets the namespace URI mapped to the specified prefix.
        /// </summary>
        public string LookupNamespace(string prefix)
        {
            return
                this.resolver.Value().LookupNamespace(prefix);
        }

        /// <summary>
        /// Gets the prefix that is mapped to the specified namespace URI.
        /// </summary>
        public string LookupPrefix(string namespaceName)
        {
            return
                this.resolver.Value().LookupPrefix(namespaceName);
        }

        private XmlNamespaceManager ManagerWithInitialNamespaces(IDictionary<string, string> namespaceUris)
        {
            var manager =
                new XmlNamespaceManager(
                    new NameTable()
                );
            Each.New(pair =>
                manager.AddNamespace(pair.Value, pair.Key),
                namespaceUris
            ).Invoke();

            return manager;
        }

        private string Prefix(IDictionary<string, string> namespaceUris, XAttribute nsDeclaration, string defPrefix)
        {
            var prefix = nsDeclaration.Name.LocalName;
            if (IsDefaultNamespace(nsDeclaration))
            {
                prefix = PrefixFromGivenList(namespaceUris, nsDeclaration, defPrefix);
            }
            // else: Is prefixed Namespace

            return prefix;
        }

        private bool IsDefaultNamespace(XAttribute nsDeclaration)
        {
            return
                nsDeclaration.Name.Namespace == XNamespace.None;
        }

        private string PrefixFromGivenList(IDictionary<string, string> namespaceUris, XAttribute nsDeclaration, string defPrefix)
        {
            var prefix = defPrefix;

            if (namespaceUris.ContainsKey(nsDeclaration.Value))
            {
                prefix = namespaceUris[nsDeclaration.Value];
            }

            return prefix;
        }

        private bool IsNotReservedPrefix(string prefix)
        {
            return prefix != "xml" && prefix != "xmlns";
        }

        private IEnumerable<XAttribute> NamespaceDeclarationsFromDocument(IScalar<XDocument> xml)
        {
            return
                Filtered.New(attribute =>
                    attribute.IsNamespaceDeclaration,
                    xml.Value().Root.DescendantsAndSelf().Attributes()
                );
        }
    }
}
