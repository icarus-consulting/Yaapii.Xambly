using System;

namespace Yaapii.Xml.Xembly
{
    public sealed class SyntaxException : Exception
    {
        public SyntaxException(string cause, Exception inner) : base (cause, inner)
        { }
    }
}