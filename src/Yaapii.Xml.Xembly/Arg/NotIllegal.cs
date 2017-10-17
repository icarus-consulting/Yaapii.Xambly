using System;
using System.Collections.Generic;
using System.Text;
using Yaapii.Atoms;
using Yaapii.Atoms.Text;

namespace Yaapii.Xml.Xembly.Arg
{
    public class NotIllegal : IScalar<Char>
    {
        private readonly char _chr;

        public NotIllegal(char chr)
        {
            this._chr = chr;
        }

        public Char Value()
        {

            this.Range(_chr, 0x00, 0x08);
            this.Range(_chr, 0x0B, 0x0C);
            this.Range(_chr, 0x0E, 0x1F);
            this.Range(_chr, 0x7F, 0x84);
            this.Range(_chr, 0x86, 0x9F);
            return _chr;
        }

        private void Range(char c, int left, int right)
        {
            if (c >= left && c <= right)
            {
                throw new System.Xml.XmlException(
                    new FormattedText(
                        "Character {0} is in the restricted XML range {1} - {2}, see http://www.w3.org/TR/2004/REC-xml11-20040204/#charsets",
                        c, left, right
                        ).AsString());
            }
        }
    }
}
