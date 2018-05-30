using System;
using Yaapii.Atoms.Text;
namespace Yaapii.Xambly.Error
{
    /// <summary>
    /// When parsing of directives is impossible.
    /// </summary>
    public class ParsingException : Exception
    {
        /// <summary>
        /// When parsing of directives is impossible.
        /// </summary>
        /// <param name="innerException">Original exception</param>
        public ParsingException(Exception innerException) 
            : this(
                new FormattedText("Error parsing script: {0}", innerException.Message).AsString(),
                innerException)
        { }

        /// <summary>
        /// When parsing of directives is impossible.
        /// </summary>
        /// <param name="cause">Cause of it</param>
        public ParsingException(string cause) 
            : this(cause, null)
        { }

        /// <summary>
        /// When parsing of directives is impossible.
        /// </summary>
        /// <param name="cause">Cause of it</param>
        /// <param name="innerException">Original exception</param>
        public ParsingException(string cause, Exception innerException) 
            : base(cause, innerException)
        { }
    }
}
