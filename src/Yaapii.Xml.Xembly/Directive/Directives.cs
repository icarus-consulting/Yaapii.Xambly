// MIT License
//
// Copyright(c) 2017 ICARUS Consulting GmbH
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

using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly;
using Yaapii.Xml.Xembly.Directive;
using Yaapii.Xml.Xembly.Error;
using System.Collections;


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
/// new Xembler(
///   new Directives("XPATH 'root'; ADD 'employee';")
/// ).Apply(dom);</code>
/// </para>
/// <para>{@link Directives} can be used as a builder of Xembly script:
///
/// <code> Document dom = DocumentBuilderFactory.newInstance()
///   .newDocumentBuilder().newDocument();
/// dom.appendChild(dom.createElement("root"));
/// new Xembler(
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

    //Right margin.
    private const int MARGIN = 80;

    //List of directives.
    private readonly ICollection<IDirective> _all = new ThreadsafeCollection<IDirective>();

    /// <summary>
    /// ctor
    /// </summary>
    public Directives() : this(new List<IDirective>())
    { }

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="text">Xembly script</param>
    public Directives(String text) : this(Directives.Parse(text))
    { }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="dirs">directives</param>
    public Directives(IEnumerable<IDirective> dirs)
    {
        this.Append(dirs);
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
        foreach (IDirective dir in this._all)
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
            new TrimmedText(
                new TextOf(text)).AsString();
    }

    /// <summary>
    /// The enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<IDirective> GetEnumerator()
    {
        return this._all.GetEnumerator();
    }

    /// <summary>
    /// The enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    ///<summary>
    /// Creates a collection of directives, which can create a copy
    /// of provided node.
    ///
    /// <param>For example, you already have a node in an XML document,
    /// which you'd like to add to another XML document:
    /// </param>
    ///
    /// <param> Document target = parse("&lt;root/&gt;");
    /// Node node = parse("&lt;user name='Jeffrey'/&gt;");
    /// new Xembler(
    ///   new Directives()
    ///     .xpath("////")
    ///     .add("jeff")
    ///     .append(Directives.copyOf(node))
    /// ).apply(target);
    /// assert print(target).equals(
    ///   "&lt;root&gt;&lt;jeff name='Jeffrey'&gt;&lt;/root&gt;"
    /// );
    /// </param>
    /// </summary>
    /// <param name="node"><see cref="XmlNode"/> to analyze</param>
    /// <returns>Collection of directives</returns>
    public static IEnumerable<IDirective> CopyOf(XmlNode node)
    {
        throw new NotImplementedException();
        //    Directives dirs = new Directives();
        //    if (node.Attributes.Count > 0)
        //    {
        //        var attrs = node.Attributes;
        //        int len = attrs.getLength();
        //        for (int idx = 0; idx < len; ++idx)
        //        {
        //            Attr attr = Attr.class.cast(attrs.item(idx));
        //dirs.attr(attr.getNodeName(), attr.getNodeValue());
        //        }
        //    }
        //    if (node.hasChildNodes()) {
        //        final NodeList children = node.getChildNodes();
        //        final int len = children.getLength();
        //        for (int idx = 0; idx<len; ++idx) {
        //            final Node child = children.item(idx);
        //            switch (child.getNodeType()) {
        //                case Node.ELEMENT_NODE:
        //                    dirs.add(child.getNodeName())
        //                        .append(Directives.copyOf(child))
        //                        .up();
        //                    break;
        //                case Node.ATTRIBUTE_NODE:
        //                    dirs.attr(child.getNodeName(), child.getNodeValue());
        //                    break;
        //                case Node.TEXT_NODE:
        //                case Node.CDATA_SECTION_NODE:
        //                    if (len == 1) {
        //                        dirs.set(child.getTextContent());
        //                    } else if (!child.getTextContent().trim().isEmpty()) {
        //                        throw new IllegalArgumentException(
        //                            new FormattedText(
        //                                // @checkstyle LineLength (1 line)
        //                                "TEXT node #%d is not allowed together with other %d nodes in %s",
        //                                idx, len, child.getNodeName()
        //                            )
        //                        );
        //                    }
        //                    break;
        //                case Node.PROCESSING_INSTRUCTION_NODE:
        //                    dirs.pi(child.getNodeName(), child.getNodeValue());
        //                    break;
        //                case Node.ENTITY_NODE:
        //                case Node.COMMENT_NODE:
        //                    break;
        //                default:
        //                    throw new IllegalArgumentException(
        //                        new FormattedText(
        //                            "unsupported type %d of node %s",
        //                            child.getNodeType(), child.getNodeName()
        //                        )
        //                    );
        //            }
        //        }
        //    }
        //    return dirs;
    }

    /// Append all directives.
    /// <param name="dirs">Directives to append</param>
    public Directives Append(IEnumerable<IDirective> dirs)
    {
        lock (this._all) //By ICARUS - needed? Threadsafeness - we think this must be done as one transaction to not compromise the directive-set which is added
        {
            foreach (IDirective dir in dirs)
            {
                this._all.Add(dir);
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
            this._all.Add(new AddDirective(name.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
                    "failed to understand XML content, ADD({0})",
                    name).AsString(),
                ex
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
    ///    <see cref="Yaapii.Xml.Xembly.Arg.Escaped"/>
    /// </para>
    /// </summary>
    /// <typeparam name="Key">type of the key</typeparam>
    /// <typeparam name="Value">type of the value</typeparam>
    /// <param name="nodes">the Dictionary with data to create</param>
    /// <returns>this object</returns>
    public Directives Add<Key, Value>(Dictionary<Key, Value> nodes)
    {
        foreach (KeyValuePair<Key, Value> entry in nodes)
        {
            this.Add(entry.Key.ToString())
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
            this._all.Add(new AddIfDirective(name.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
                    "failed to understand XML content, ADDIF({0})",
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
        this._all.Add(new RemoveDirective());
        return this;
    }

    /// <summary>
    /// Set attribute.
    /// If a value provided contains illegal XML characters, a runtime
    /// exception will be thrown. To avoid this, it is recommended to use
    /// <see cref="Yaapii.Xml.Xembly.Arg.Escaped"/>.
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
            this._all.Add(new AttrDirective(name.ToString(), value.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
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
    /// <see cref="Yaapii.Xml.Xembly.Arg.Escaped"/>
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
            this._all.Add(new PiDirective(target.ToString(), data.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
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
    /// {@link Xembler#escape(String)}.
    /// </summary>
    /// <param name="text">Text to test</param>
    /// <exception cref="IllegalArgumentException"/>
    /// <returns>This object</returns>
    public Directives Set(Object text)
    {
        try
        {
            this._all.Add(new SetDirective(text.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
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
            this._all.Add(new XsetDirective(text.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
                    "failed to understand XML content, XSET({0})",
                    text).AsString(),
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
        this._all.Add(new UpDirective());
        return this;
    }

    /// <summary>
    /// Go to xpath.
    /// </summary>
    /// <param name="path">path to go to</param>
    /// <exception cref="IllegalArgumentException"/>
    /// <returns>This object</returns>
    public Directives Xpath(Object path)
    {
        try
        {
            this._all.Add(new XpathDirective(path.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
                    "failed to understand XML content, XPATH({0})",
                    path).AsString(),
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
        this._all.Add(new StrictDirective(number));
        return this;
    }

    /// <summary>
    /// Push current cursor to stack.
    /// </summary>
    /// <returns>This object</returns>
    public Directives Push()
    {
        this._all.Add(new PushDirective());
        return this;
    }

    /// <summary>
    /// Pop cursor to stack and replace current cursor with it.
    /// </summary>
    /// <returns>This object</returns>
    public Directives Pop()
    {
        this._all.Add(new PopDirective());
        return this;
    }

    /// <summary>
    /// Set CDATA section.
    ///
    /// <para>If a value provided contains illegal XML characters, an
    /// exception will be thrown.To avoid this, it is recommended to use <see cref="Yaapii.Xml.Xembly.Arg.Escaped"/>.
    /// </para>
    /// </summary>
    /// <param name="text">Text to set</param>
    /// <exception cref="IllegalArgumentException"/>
    /// <returns>This object</returns>
    public Directives Cdata(Object text)
    {
        try
        {
            this._all.Add(new CdataDirective(text.ToString()));
        }
        catch (XmlContentException ex)
        {
            throw new IllegalArgumentException(
                new FormattedText(
                    "failed to understand XML content, CDATA({0})",
                    text).AsString(),
                ex
            );
        }
        return this;
    }

    /// <summary>
    /// Parse script.
    /// </summary>
    /// <param name="script">Script to parse</param>
    /// <returns>Collection of directives</returns>
    private static ICollection<IDirective> Parse(String script)
    {
        var input = new AntlrInputStream(script);
        XemblyLexer lexer = new XemblyLexer(input);
        lexer.AddErrorListener(new ThrowingErrorListener());

        var tokens = new CommonTokenStream(lexer);
        XemblyParser parser = new XemblyParser(tokens);
        parser.AddErrorListener(new ThrowingErrorListener());

        try
        {
            return parser.directives().ret;
        }
        catch (RecognitionException ex)
        {
            throw new SyntaxException(script, ex);
        }
        catch (ParsingException ex)
        {
            throw new SyntaxException(script, ex);
        }
    }
}