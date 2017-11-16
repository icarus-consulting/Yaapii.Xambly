using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Yaapii.Xml.Xembly.Error
{
    internal class ThrowingErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public static ThrowingErrorListener INSTANCE = new ThrowingErrorListener();

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, 
            int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxException("line " + line + ":" + charPositionInLine + " " + msg, e);
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxException("line " + line + ":" + charPositionInLine + " " + msg, e);
        }
    }
}