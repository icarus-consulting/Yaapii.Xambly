using System;
using System.Collections.Generic;
using System.Xml;
using Yaapii.Xml.Xembly.Cursor;
using Yaapii.Atoms.List;
using Yaapii.Xml.Xembly.Stack;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Error;
using System.IO;
using System.Text;

namespace Yaapii.Xml.Xembly
{
    public sealed class Xembler
    {
        private readonly IEnumerable<IDirective> _directives;

        public Xembler(IEnumerable<IDirective> directives)
        {
            this._directives = directives;
        }

        public XmlNode ApplyQuietly(XmlNode dom)
        {
            try
            {
                return this.Apply(dom);
            }
            catch(Exception ex)
            {
                throw new IllegalStateException(
                    new FormattedText("quietly failed to apply DOM: {0}", this._directives).AsString(),
                    ex);
            }
        }

        /// <summary>
        /// Apply all changes to the document/node
        /// </summary>
        /// <returns>Same document/node.</returns>
        /// <param name="dom">DOM document/node</param>
        public XmlNode Apply(XmlNode dom)
        {
            ICursor cursor = new DomCursor(new EnumerableOf<XmlNode>(dom));
            int pos = 1;

            IStack stack = new DomStack();

            foreach (var dir in this._directives)
            {
                try
                {
                    cursor = dir.Exec(dom, cursor, stack);
                }
                catch (ImpossibleModificationException ex)
                {
                    throw new ImpossibleModificationException(
                        new FormattedText("directive {0}: {1}", pos, dir).AsString());
                }
                catch (Exception ex) //TODO: Original catches DOMException. We don't have that. But do we have something similar?
                {
                    throw new ImpossibleModificationException(
                        new FormattedText("Exception at dir {0}: {1}", pos, dir).AsString());
                }
                ++pos;
            }
            return dom;
        }

        public XmlDocument DomQuietly()
        {
            try
            {
                return this.Dom();
            }
            catch(Exception ex)
            {
                throw new IllegalStateException(
                    new FormattedText("failed to create DOM: {0}", this._directives).AsString(),
                    ex);
            }
        }

        public XmlDocument Dom()
        {
            var dom = new XmlDocument();
            this.Apply(dom);
            return dom;
        }

        public String XmlQuietly()
        {
            try
            {
                return this.Xml();
            }
            catch (Exception ex)
            {
                throw new IllegalStateException(
                    new FormattedText("quielty failed to build XML: {0}", this._directives).AsString(),
                    ex);
            }
        }

        public string Xml()
        {
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                Dom().WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                return stringWriter.GetStringBuilder().ToString();
            }
        }

        public String Escaped(string text)
        {
            var output = new StringBuilder(text.Length);
            var chars = text.ToCharArray();
            foreach (var chr in chars)
            {
                var illegal =
                    chr >= 0x00 && chr <= 0x08
                    || chr >= 0x0B && chr <= 0x0C
                    || chr >= 0x0E && chr <= 0x1F
                    || chr >= 0x7F && chr <= 0x84
                    || chr >= 0x86 && chr <= 0x9F;

                if (illegal)
                {
                    output.Append(new FormattedText("\\u{0:000}x", (int)chr).AsString()); //todo: is this the correct illegal char replacement? original is java String.format("\\u%04x"...) 
                }
                else
                {
                    output.Append(chr);
                }
            }
            return output.ToString();
        }
    }
}
