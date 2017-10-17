using System;
using System.Collections.Generic;
using System.Text;

namespace Yaapii.Xml.Xembly
{
    public interface IStack
    {
        void Push(ICursor cursor);
        ICursor Pop();
    }
}
