using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using Yaapii.Atoms;
using Yaapii.Atoms.Enumerable;
using Yaapii.Atoms.Error;
using Yaapii.Atoms.Func;
using Yaapii.Atoms.Map;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Xambly.XmlNamespaceResolver
{
    /// <summary>
    /// A XML namespace resolver that knows all namespaces from a XML document.
    /// The default namespace (in the root node) can be given a prefix to use it in an XPath.
    /// Default namespaces defined in child nodes can be prefixed by defining a dictionary.
    /// </summary>
    public sealed class ResolverFromDocument : IXmlNamespaceResolver
    {
        private readonly IScalar<IXmlNamespaceResolver> resolver;

        /// <summary>
        /// A XML namespace resolver that knows all namespaces from a XML document.
        /// The default namespace (in the root node) can be given a prefix to use it in an XPath.
        /// Default namespaces defined in child nodes can be prefixed by defining a dictionary.
        /// </summary>
        public ResolverFromDocument(XNode dom, string rootDefNamespacePrefix = "", params string[] defnamespaceAndPrefixDictionary) : this(
            dom,
            rootDefNamespacePrefix,
            new MapOf(defnamespaceAndPrefixDictionary)
        )
        { }

        /// <summary>
        /// A XML namespace resolver that knows all namespaces from a XML document.
        /// The default namespace (in the root node) can be given a prefix to use it in an XPath.
        /// Default namespaces defined in child nodes can be prefixed by defining a dictionary.
        /// </summary>
        public ResolverFromDocument(XNode dom, string rootDefNamespacePrefix, IDictionary<string, string> defnamespaceAndPrefixDictionary) : this(
            ScalarOf.New<IXmlNamespaceResolver>(() =>
            {
                var manager =
                    new XmlNamespaceManager(
                        new NameTable()
                    );
                var root =
                    XDocument.Parse(
                        dom.ToString()
                    ).Root;
                var rootNsAttributes =
                    Filtered.New(attr =>
                        attr.IsNamespaceDeclaration,
                        root.Attributes()
                    );
                Each.New(attr =>
                    {
                        var prefix = attr.Name.LocalName;
                        if (attr.Name.Namespace == XNamespace.None)
                        {
                            manager.AddNamespace(
                                string.Empty,
                                attr.Value
                            );
                            prefix = rootDefNamespacePrefix;
                        }
                        manager.AddNamespace(
                            prefix,
                            attr.Value
                        );
                    },
                    rootNsAttributes
                ).Invoke();
                if (defnamespaceAndPrefixDictionary.Count > 0)
                {
                    var childrenDefaultNsAttributes =
                        Filtered.New(attr =>
                            attr.IsNamespaceDeclaration,
                            root.Descendants().Attributes()
                        );
                    Each.New(attr =>
                        {
                            var prefix = attr.Name.LocalName;
                            new FailWhen(
                                attr.Name.Namespace != XNamespace.None,
                                $"Prefixed namespace declaration ony supported in root node but found in children: {attr}"
                            ).Go();
                            if (defnamespaceAndPrefixDictionary.ContainsKey(attr.Value))
                            {
                                manager.AddNamespace(
                                    defnamespaceAndPrefixDictionary[attr.Value],
                                    attr.Value
                                );
                            }
                            // else: Skipping. The manager knows only one default namespace (from the root node)
                        },
                        childrenDefaultNsAttributes
                    ).Invoke();
                }

                return manager;
            })
        )
        { }

        private ResolverFromDocument(IScalar<IXmlNamespaceResolver> resolver)
        {
            this.resolver = resolver;
        }

        public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
        {
            return
                this.resolver.Value().GetNamespacesInScope(scope);
        }

        public string LookupNamespace(string prefix)
        {
            return
                this.resolver.Value().LookupNamespace(prefix);
        }

        public string LookupPrefix(string namespaceName)
        {
            return
                this.resolver.Value().LookupPrefix(namespaceName);
        }
    }
}
