using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly.Arg
{
    public class Unescaped : IText
    {
        private readonly IScalar<string> _src;

        public Unescaped(IArg src) : this(new ScalarOf<string>(() => src.AsString()))
        { }

        public Unescaped(string src) : this(new ScalarOf<string>(src))
        { }

        private Unescaped(IScalar<string> src)
        {
            this._src = src;
        }

        public string AsString()
        {
            var str = _src.Value();
            var chars = str.ToCharArray();
            if (chars.Length < 2)
                throw new ArgumentOutOfRangeException(
                    "Internal error, argument can't be shorter than 2 chars");

            int len = chars.Length - 1; //cut off trailing "
            var output = new StringBuilder(str.Length);

            for (int idx = 1; idx < len; idx++) //1 -> cut off leading "
            {
                if (chars[idx] == '&')
                {
                    var sbuf = new StringBuilder(0);
                    while (chars[idx] != ';')
                    {
                        ++idx;
                        if (idx == chars.Length)
                        {
                            throw new XmlException("reached EOF while parsing XML symbol");
                        }
                        sbuf.Append(chars[idx]);
                    }
                    output.Append(
                        new Symbol(
                            new SubText(
                                sbuf.ToString(), 0, sbuf.Length - 1)).Value());
                }
                else
                {
                    output.Append(chars[idx]);
                }
            }
            return output.ToString();
        }

        public bool Equals(IText other)
        {
            return this.AsString().Equals(other.AsString());
        }
    }
}
