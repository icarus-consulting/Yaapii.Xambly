using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yaapii.Xml.Xembly.Cursor
{
    public class DomCursor : ICursor
    {
        private readonly IEnumerable<XmlNode> _nodes;

        public DomCursor(IEnumerable<XmlNode> nodes)
        {
            _nodes = nodes;
        }

        public IEnumerator<XmlNode> GetEnumerator()
        {
            return this._nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
