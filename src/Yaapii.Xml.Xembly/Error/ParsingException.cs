using System;
using Yaapii.Atoms.Text;
namespace Yaapii.Xml.Xembly.Error
{
    public class ParsingException : Exception
    {
        public ParsingException(Exception ex) : this(
            new FormattedText("Error parsing script: {0}", ex.Message).AsString(),
            ex)
        { }

        public ParsingException(string msg) : this(msg, null)
        { }

        public ParsingException(string msg, Exception ex) : base(
            msg, ex)
        { }
    }
}
