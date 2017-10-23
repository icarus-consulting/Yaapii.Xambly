using System;
namespace Yaapii.Xml.Xembly.Error
{
    public sealed class IllegalStateException : Exception
    {
        public IllegalStateException() : base()
        { }

        public IllegalStateException(string msg, Exception ex) : base(msg, ex)
        { }
    }
}
