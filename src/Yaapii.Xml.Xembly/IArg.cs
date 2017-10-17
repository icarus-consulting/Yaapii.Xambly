using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly
{
    public interface IArg
    {
        string AsString();
        string Raw();
    }
}
