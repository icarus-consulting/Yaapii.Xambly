// MIT License
//
// Copyright(c) 2022 ICARUS Consulting GmbH
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Yaapii.Atoms.Func;
using Yaapii.Atoms.Text;
using Yaapii.Xambly.Directive;
using Yaapii.Xambly.Error;

namespace Yaapii.Xambly
{

    ///
    /// Collection of <see cref="IDirective"/>s, instantiable from <see cref="String"/>.
    ///
    /// <para>For example, to fetch directives from a string and apply to the
    /// DOM document:
    ///
    /// <code> 
    /// 
    /// dom = &lt;some xmldocument&gt;
    /// 
    /// new Xambler(
    ///   new Directives("XPATH 'root'; ADD 'employee';")
    /// ).Apply(dom);</code>
    /// </para>
    /// <para>{@link Directives} can be used as a builder of Xambly script:
    ///
    /// <code> Document dom = DocumentBuilderFactory.newInstance()
    ///   .newDocumentBuilder().newDocument();
    /// dom.appendChild(dom.createElement("root"));
    /// new Xambler(
    ///   new Directives()
    ///     .xpath("/root")
    ///     .addIf("employees")
    ///     .add("employee")
    ///     .attr("id", 6564)
    ///     .up()
    ///     .xpath("employee[&#64;id='100']")
    ///     .strict(1)
    ///     .remove()
    /// ).apply(dom);</code>
    ///
    /// </para>
    /// <para>
    /// The class is mutable and thread-safe.
    /// author: ICARUS Consulting GmbH
    /// </para>
    ///
    public sealed class Directives : IEnumerable<IDirective>
    {

        // Right margin.
        private const int MARGIN = 80;

        // List of directives.
        private readonly ICollection<IDirective> all = new ThreadsafeCollection<IDirective>();

        /// <summary>
        /// ctor.
        /// </summary>
        public Directives() : this(new List<IDirective>())
        { }

        /// <summary>
        /// ctor.
        /// </summary>
        public Directives(params IDirective[] dirs) : this(
            new List<IDirective>(dirs))
        { }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dirs">directives</param>
        public Directives(IEnumerable<IDirective> dirs)
        {
            Append(dirs);
        }

        /// <summary>
        /// This object as a string representation.
        /// </summary>
        /// <returns>A string representing this object</returns>
        public override String ToString()
        {
            StringBuilder text = new StringBuilder(0);
            int width = 0;
            int idx = 0;
            foreach (IDirective dir in this.all)
            {
                if (idx > 0 && width == 0)
                {
                    text.Append('\n');
                }

                String txt = dir.ToString();
                text.Append(txt).Append(';');
                width += txt.Length;
                if (width > Directives.MARGIN)
                {
                    width = 0;

                }
                ++idx;
            }
            return
                new Trimmed(
                    new TextOf(text)).AsString();
        }

        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IDirective> GetEnumerator()
        {
            return this.all.GetEnumerator();
        }

        /// <summary>
        /// The enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// Append all directives.
        /// <param name="dirs">Directives to append</param>
        public Directives Append(IEnumerable<IDirective> dirs)
        {
            lock (this.all) //By ICARUS - needed? Threadsafeness - we think this must be done as one transaction to not compromise the directive-set which is added
            {
                foreach (IDirective dir in dirs)
                {
                    this.all.Add(dir);
                }
            }

            return this;
        }

        /// <summary>
        /// Add node to all current nodes.
        /// </summary>
        /// <param name="name">Name of the node to add</param>
        /// <returns>This object</returns>
        public Directives Add(Object name)
        {
            try
            {
                this.all.Add(new AddDirective(name.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, ADD({0})",
                        name).AsString(),
                    ex
                );
            }
            return this;
        }

        ///<summary>
        /// Adds a collection of directives, which are a copy
        /// of the provided node.
        ///
        /// <param>For example, you already have a node in an XML document,
        /// which you'd like to add to another XML document:
        /// </param>
        ///
        /// <param> XmlDocument target = parse("&lt;root/&gt;");
        /// XmlNode node = parse("&lt;user name='Jeffrey'/&gt;");
        /// new Xambler(
        ///   new Directives()
        ///     .Xpath("////")
        ///     .Add("jeff")
        ///     .Append(new CopyOf(node))
        /// ).Apply(target);
        /// assert print(target).equals(
        ///   "&lt;root&gt;&lt;jeff name='Jeffrey'&gt;&lt;/root&gt;"
        /// );
        /// </param>
        /// </summary>
        /// <param name="node">Node to add</param>
        /// <returns>the updated list of directives</returns>
        public Directives CopyOf(XNode node)
        {
            try
            {
                new Each<IDirective>(
                    dir => this.all.Add(dir),
                    new CopyOfDirective(node)
                ).Invoke();
            }
            catch (ArgumentException aex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to add xml node '{0}'",
                        node.ToString()
                    ).AsString(),
                    aex
                );
            }

            return this;
        }

        /// <summary>
        /// <para>
        ///    Add multiple nodes and set their text values.
        /// 
        ///  Every pair in the provided map will be treated as a new
        ///  node name and value. It's a convenient utility method that simplifies
        ///  the process of adding a collection of nodes with pre-set values. For
        ///  example:
        /// </para>
        /// <code>
        /// new Directives()
        /// .Add("first", "hello, world!")
        /// .Add(
        ///     new Dictionary&lt;String, Object&gt;()
        ///     { "alpha", 1 },
        ///     { "beta", "2" },
        ///     { "gamma", new Date() })
        /// .Add("second");
        /// </code>
        /// <para>
        ///    If a value provided contains illegal XML characters, a exception will be thrown.To avoid this, it is recommended to use
        ///    <see cref="Yaapii.Xambly.Arg.ElementEscaped"/>
        /// </para>
        /// </summary>
        /// <typeparam name="Key">type of the key</typeparam>
        /// <typeparam name="Value">type of the value</typeparam>
        /// <param name="nodes">the Dictionary with data to create</param>
        /// <returns>this object</returns>
        public Directives Add<Key, Value>(IDictionary<Key, Value> nodes)
        {
            foreach (KeyValuePair<Key, Value> entry in nodes)
            {
                Add(entry.Key.ToString())
                .Set(entry.Value.ToString())
                .Up();
            }
            return this;
        }

        /// <summary>
        /// Add node if it's absent.
        /// </summary>
        /// <param name="name">Name of the node to add</param>
        /// <returns>This object</returns>
        public Directives AddIf(Object name)
        {
            try
            {
                this.all.Add(new AddIfDirective(name.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, ADDIF({0})",
                        name).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Add node if no node with the specified attribute exists.
        /// </summary>
        /// <param name="name">Name of the node to add</param>
        /// <param name="childName">Name of the child element to match</param>
        /// <param name="textContent">Text inside the child to match</param>
        /// <returns>This object</returns>
        public Directives AddIfChildContent(Object name, string childName, string textContent)
        {
            try
            {
                this.all.Add(new AddIfChildDirective(name.ToString(), childName, textContent));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, ADDIFCHILD({0})",
                        name
                    ).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Add node if no node with the specified attribute exists.
        /// </summary>
        /// <param name="name">Name of the node to add</param>
        /// <param name="attributeName">Name of the attribute to check</param>
        /// <param name="attributeValue">Name of the attribute-value to check</param>
        /// <returns>This object</returns>
        public Directives AddIfAttr(Object name, string attributeName, string attributeValue)
        {
            try
            {
                this.all.Add(new AddIfAttributeDirective(name.ToString(), attributeName, attributeValue));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, ADDIFATTRIBUTE({0})",
                        name
                    ).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Insert node before current nodes.
        /// </summary>
        /// <param name="name">Name of the node to add</param>
        /// <returns>This object</returns>
        public Directives InsertBefore(Object name)
        {
            try
            {
                this.all.Add(new InsertBeforeDirective(name.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, INSERTBEFORE({0})",
                        name).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Insert node before current nodes.
        /// </summary>
        /// <param name="name">Name of the node to add</param>
        /// <returns>This object</returns>
        public Directives InsertAfter(Object name)
        {
            try
            {
                this.all.Add(new InsertAfterDirective(name.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, INSERTAFTER({0})",
                        name).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Remove all current nodes and move cursor to their parents.
        /// </summary>
        /// <returns>This object</returns>
        public Directives Remove()
        {
            this.all.Add(new RemoveDirective());
            return this;
        }

        /// <summary>
        /// Set attribute.
        /// If a value provided contains illegal XML characters, a runtime
        /// exception will be thrown. To avoid this, it is recommended to use
        /// <see cref="Yaapii.Xambly.Arg.ElementEscaped"/>.
        /// </summary>
        ///
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">value Value to set</param>
        /// <exception cref="IllegalArgumentException"></exception>
        /// <returns>This object</returns>
        public Directives Attr(Object name, Object value)
        {
            try
            {
                this.all.Add(new AttrDirective(name.ToString(), value.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, ATTR({0}, {1})",
                        name, value).AsString(),
                    ex
                );
            }
            return this;
        }

        ///<summary>
        /// Add processing instruction.
        ///
        /// <para>If a value provided contains illegal XML characters, a runtime
        /// exception will be thrown. To avoid this, it is recommended to use
        /// <see cref="Yaapii.Xambly.Arg.ElementEscaped"/>
        /// </para>
        ///</summary>
        /// <param name="target">target PI name</param>
        /// <param name="data">Data to set</param>
        /// <exception cref="IllegalArgumentException"/>
        /// <returns>This object</returns>
        public Directives Pi(Object target, Object data)
        {
            try
            {
                this.all.Add(new PiDirective(target.ToString(), data.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, PI({0}, {1})",
                        target, data).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Set text content.
        ///
        /// If a value provided contains illegal XML characters, a runtime
        /// exception will be thrown. To avoid this, it is recommended to use
        /// {@link Xambler#escape(String)}.
        /// </summary>
        /// <param name="text">Text to test</param>
        /// <exception cref="IllegalArgumentException"/>
        /// <returns>This object</returns>
        public Directives Set(Object text)
        {
            try
            {
                this.all.Add(new SetDirective(text.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, SET({0})",
                        text).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Set text content.
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <exception cref="IllegalArgumentException"/>
        /// <returns>This object</returns>
        public Directives Xset(Object text)
        {
            try
            {
                this.all.Add(new XsetDirective(text.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, XSET({0})",
                        text
                    ).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Go one node/level up.
        /// </summary>
        /// <returns>This object</returns>
        public Directives Up()
        {
            this.all.Add(new UpDirective());
            return this;
        }

        /// <summary>
        /// Moves cursor to the nodes found by XPath.
        /// </summary>
        /// <param name="path">XPath</param>
        /// <param name="rootDefNamespacePrefix">Default namespace prefix for use in XPath or empty string</param>
        /// <param name="defnamespaceAndPrefixDictionary">Optional tupel defining default namespace prefixes from children nodes. Always write multiples of two: Namespace and prefix</param>
        public Directives Xpath(object path, string rootDefNamespacePrefix = "", params string[] defnamespaceAndPrefixDictionary)
        {
            try
            {
                this.all.Add(
                    new XpathDirective(
                        path.ToString(),
                        rootDefNamespacePrefix,
                        defnamespaceAndPrefixDictionary
                    )
                );
            }
            catch (XmlContentException ex)
            {
                throw
                    new IllegalArgumentException(
                        new Formatted(
                            "failed to understand XML content, XPATH({0})",
                            path
                        ).AsString(),
                        ex
                    );
            }
            return this;
        }

        /// <summary>
        /// Check that there is exactly this number of current nodes.
        /// </summary>
        /// <param name="number">The expected number of nodes</param>
        /// <returns>Thi object</returns>
        public Directives Strict(int number)
        {
            this.all.Add(new StrictDirective(number));
            return this;
        }

        /// <summary>
        /// Push current cursor to stack.
        /// </summary>
        /// <returns>This object</returns>
        public Directives Push()
        {
            this.all.Add(new PushDirective());
            return this;
        }

        /// <summary>
        /// Pop cursor to stack and replace current cursor with it.
        /// </summary>
        /// <returns>This object</returns>
        public Directives Pop()
        {
            this.all.Add(new PopDirective());
            return this;
        }

        /// <summary>
        /// Set CDATA section.
        ///
        /// <para>If a value provided contains illegal XML characters, an
        /// exception will be thrown.To avoid this, it is recommended to use <see cref="Yaapii.Xambly.Arg.ElementEscaped"/>.
        /// </para>
        /// </summary>
        /// <param name="text">Text to set</param>
        /// <exception cref="IllegalArgumentException"/>
        /// <returns>This object</returns>
        public Directives Cdata(Object text)
        {
            try
            {
                this.all.Add(new CdataDirective(text.ToString()));
            }
            catch (XmlContentException ex)
            {
                throw new IllegalArgumentException(
                    new Formatted(
                        "failed to understand XML content, CDATA({0})",
                        text
                    ).AsString(),
                    ex
                );
            }
            return this;
        }

        /// <summary>
        /// Sets namespace of all current nodes selected by the cursor.
        /// Namespace is applied to all child nodes (default).
        /// Namespace is applied to all attributes (default).
        /// The namespace declaration will be done in the root node.
        /// 
        /// If the prefix is empty a default namespace will be created.
        /// which is declared only in the current nodes.
        /// Attributes cannot be added to a default namespace.
        /// 
        /// Hint:
        /// After declaring a namespace the XPath will be affected.
        /// </summary>
        /// <param name="prefix">If empty a default namespace will be created</param>
        /// <param name="ns">Namespace</param>
        /// <param name="purpose">Set the namespace to: 'nodes', 'attributes', 'nodesAndAttributes'</param>
        /// <param name="inheritance">Is applied to the children</param>
        public Directives Ns(string prefix, string ns, string purpose = "nodesAndAttributes", bool inheritance = true)
        {
            this.all.Add(
                new NsDirective(prefix, ns, purpose, inheritance)
            );

            return this;
        }
    }
}
