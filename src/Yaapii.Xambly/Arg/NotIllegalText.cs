using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Scalar;

namespace Yaapii.Xambly.Arg
{
    /// <summary>
    /// A text with only legal xml characters
    /// </summary>
    public sealed class NotIllegalText : IText
    {
        private readonly IScalar<string> text;

        /// <summary>
        /// A text with only legal xml characters
        /// </summary>
        public NotIllegalText(string text)
        {
            this.text = new ScalarOf<string>(() =>
            {
                foreach (var character in text)
                {
                    new NotIllegal(character).Value();
                }
                return text;
            });
        }

        /// <summary>
        /// Text with only legal xml characters
        /// </summary>
        public string AsString()
        {
            return this.text.Value();
        }
    }
}
