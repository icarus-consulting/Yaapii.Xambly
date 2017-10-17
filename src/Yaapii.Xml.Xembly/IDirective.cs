using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace Yaapii.Xml.Xembly
{
    public interface IDirective
    {
        ICursor Exec(XmlNode dom, ICursor cursor, IStack stack);
    }
}
