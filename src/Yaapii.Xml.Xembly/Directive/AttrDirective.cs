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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;
using Yaapii.Atoms.Text;
using Yaapii.Xml.Xembly.Arg;

namespace Yaapii.Xml.Xembly
{
    public sealed class AttrDirective : IDirective
    {
        private readonly IScalar<IArg> _name;
        private readonly IScalar<IArg> _value;

        public AttrDirective(string name, string value) 
            : this(
                  new ScalarOf<IArg>(() => new ArgOf(name)),
                  new ScalarOf<IArg>(() => new ArgOf(value))
                  )
        {

        }

        public AttrDirective(IScalar<IArg> name, IScalar<IArg> value)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
        {
            return new FormattedText(
                            "ATTR {0}, {1}",
                            this._name.Value().Raw(),
                            this._value.Value().Raw()
                        ).AsString();
        }

        public ICursor Exec(XmlNode dom, ICursor cursor, IStack stack)
        {
            var key = _name.Value().Raw();
            var value = _value.Value().Raw();

            foreach (var node in cursor)
            {
                ((XmlElement)node).SetAttribute(key, value);
            }

            return cursor;
        }
    }
}
