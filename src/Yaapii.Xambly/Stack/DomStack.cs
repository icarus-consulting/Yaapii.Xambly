// MIT License
//
// Copyright(c) 2022 ICARUS Consulting GmbH
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

namespace Yaapii.Xambly.Stack
{
    /// <summary>
    /// Stack of DOM cursors.
    /// </summary>
    public class DomStack : IStack
    {
        private readonly Stack<ICursor> _cursors = new Stack<ICursor>();

        /// <summary>
        /// Pop cursor.
        /// </summary>
        /// <returns>Cursor recently added</returns>
        /// <exception cref="ImpossibleModificationException">If fails</exception>"
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

        /// <summary>
        /// Push cursor.
        /// </summary>
        /// <param name="cursor">Cursor to push</param>
        /// <exception cref="ImpossibleModificationException">If fails</exception>"
        public void Push(ICursor cursor)
        {
            lock (_cursors)
            {
                this._cursors.Push(cursor);
            }
        }
    }
}
