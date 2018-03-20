using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Xml.Xambly
{
    /// <summary>
    /// When further modification is impossible.
    /// </summary>
    public sealed class ImpossibleModificationException : Exception
    {
        /// <summary>
        /// When further modification is impossible.
        /// </summary>
        /// <param name="cause">Cause of it</param>
        public ImpossibleModificationException(string cause) : base(cause)
        { }

        /// <summary>
        /// When further modification is impossible.
        /// </summary>
        /// <param name="cause">Cause of it</param>
        /// <param name="inner">Original exception</param>
        public ImpossibleModificationException(string cause, Exception inner) : base(cause, inner)
        { }
    }
}
