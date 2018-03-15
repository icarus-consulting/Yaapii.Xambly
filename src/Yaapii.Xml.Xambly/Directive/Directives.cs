﻿// MIT License
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
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xambly;
using Yaapii.Xml.Xambly.Directive;
using Yaapii.Xml.Xambly.Error;
using System.Collections;
using Yaapii.Atoms;

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
    private readonly ICollection<IDirective> _all = new ThreadsafeCollection<IDirective>();

    /// <summary>
    /// ctor.
    /// </summary>
    public Directives() : this(new List<IDirective>())
    { }

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="text">Xambly script</param>
    public Directives(IText text) : this(text.AsString())
    { }

    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="text">Xambly script</param>
    public Directives(String text) : this(Directives.Parse(text))
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
    ///    <see cref="Yaapii.Xml.Xambly.Arg.Escaped"/>
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
    /// <see cref="Yaapii.Xml.Xambly.Arg.Escaped"/>.
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
    /// <see cref="Yaapii.Xml.Xambly.Arg.Escaped"/>
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
    /// {@link Xambler#escape(String)}.
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
    /// exception will be thrown.To avoid this, it is recommended to use <see cref="Yaapii.Xml.Xambly.Arg.Escaped"/>.
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
        XamblyLexer lexer = new XamblyLexer(input);
        lexer.AddErrorListener(new ThrowingErrorListener());

        var tokens = new CommonTokenStream(lexer);
        XamblyParser parser = new XamblyParser(tokens);
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