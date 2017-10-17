using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly.Arg
{
    public class Escaped : IText
    {
        private readonly IText _src;

        public Escaped(string src) : this(new TextOf(src))
        { }

        public Escaped(IText src)
        {
            _src = src;
        }

        public string AsString()
        {
            var output = new StringBuilder(_src.AsString().Length);
            foreach (char chr in _src.AsString().ToCharArray())
            {
                if (chr < ' ')
                {
                    output
                        .Append("&#")
                        .Append((int)chr)
                        .Append(';');
                }
                else if (chr == '"')
                {
                    output.Append("&quot;");
                }
                else if (chr == '&')
                {
                    output.Append("&amp;");
                }
                else if (chr == '\'')
                {
                    output.Append("&apos;");
                }
                else if (chr == '<')
                {
                    output.Append("&lt;");
                }
                else if (chr == '>')
                {
                    output.Append("&gt;");
                }
                else
                {
                    output.Append(chr);
                }
            }
            return output.ToString();
        }

        public bool Equals(IText other)
        {
            return other.AsString().Equals(this.AsString());
        }
    }
}
