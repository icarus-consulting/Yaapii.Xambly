using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Yaapii.Xml.Xembly.Tests
{
    internal class FkCursor : ICursor
    {
        private readonly List<XmlNode> _src = new List<XmlNode>();
        public FkCursor()
        { }

        public IEnumerator<XmlNode> GetEnumerator()
        {
            return _src.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
