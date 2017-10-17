using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Xml.Xembly
{
    public sealed class ImpossibleModificationException : Exception
    {
        public ImpossibleModificationException(string cause) : base(cause)
        { }

        public ImpossibleModificationException(string cause, Exception inner) : base(cause, inner)
        { }
    }
}
