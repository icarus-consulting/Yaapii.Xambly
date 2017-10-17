using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;

namespace Yaapii.Xml.Xembly.Arg
{
    public class ArgOf : IArg
    {
        private readonly string _value;

        public ArgOf(string val)
        {
            foreach (char chr in val.ToCharArray())
            {
                new NotIllegal(chr).Value();
            }
            this._value = val;
        }

        public string AsString()
        {
            var escaped = new Escaped(this._value).AsString();
            return
                new StringBuilder(this._value.Length + 2 + escaped.Length)
                        .Append('"')
                        .Append(escaped)
                        .Append('"')
                        .ToString();
        }

        public override string ToString()
        {
            return AsString();
        }

        public string Raw()
        {
            return this._value;
        }
    }
}
