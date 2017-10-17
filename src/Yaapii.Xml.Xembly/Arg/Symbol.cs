using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly.Arg
{
    public class Symbol : IScalar<Char>
    {
        private readonly IText _src;

        public Symbol(string str) : this(
            new TextOf(str))
        { }

        public Symbol(IText src)
        {
            this._src = src;
        }

        public char Value()
        {
            var src = _src.AsString();
            char chr;
            if (src[0] == '#')
            {
                chr = 
                    new NotIllegal((char)
                        new IntOf(
                            new SubText(_src, 1)).Value()).Value();
            }
            else if (String.Compare(src, "apos", true) == 0)
            {
                chr = '\'';
            }
            else if (String.Compare(src, "quot", true) == 0)
            {
                chr = '"';
            }
            else if (String.Compare(src, "lt", true) == 0)
            {
                chr = '<';
            }
            else if (String.Compare(src, "gt", true) == 0)
            {
                chr = '>';
            }
            else if (String.Compare(src, "amp", true) == 0)
            {
                chr = '&';
            }
            else
            {
                throw new XmlException(
                        new FormattedText(
                            "unknown XML symbol &{0};",
                            _src
                        ).AsString());
            }
            return chr;
        }
    }
}
