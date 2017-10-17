using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Xml.Xembly.Stack
{
    public class DomStack : IStack
    {
        private readonly Stack<ICursor> _cursors = new Stack<ICursor>();

        public ICursor Pop()
        {
            try
            {
                return this._cursors.Pop();
            }
            catch (InvalidOperationException ex)
            {
                throw new ImpossibleModificationException(
                    "stack is empty, can't POP", ex);
            }
        }

        public void Push(ICursor cursor)
        {
            lock (_cursors)
            {
                this._cursors.Push(cursor);
            }
        }
    }
}
