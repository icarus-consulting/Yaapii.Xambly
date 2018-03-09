using System.Xml;

namespace Yaapii.Xml.Xambly
{
    /// <summary>
    /// Interface for Processor of Xambly directives (main entry point to the module)
    /// </summary>
    public interface IXambler
    {
        /// <summary>
        /// Should apply all changes to the document/node.
        /// </summary>
        /// <returns>Same document/node.</returns>
        /// <param name="dom">DOM document/node</param>
        XmlNode Apply(XmlNode dom);

        /// <summary>
        /// Should apply all changes to the document/node, but redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        /// <param name="dom">DOM.</param>
        XmlNode ApplyQuietly(XmlNode dom);

        /// <summary>
        /// Should apply all changes to an empty DOM.
        /// </summary>
        /// <returns>The DOM</returns>
        XmlDocument Dom();

        /// <summary>
        /// Should apply all changes to an empty DOM, but redirect all exceptions to a IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        XmlDocument DomQuietly();

        /// <summary>
        /// Should escape text before using it as a text value
        /// </summary>
        /// <returns>The escaped.</returns>
        /// <param name="text">Text.</param>
        string Escaped(string text);

        /// <summary>
        /// Should convert to XML Document.
        /// </summary>
        /// <returns>The xml as string</returns>
        /// <param name="createHeader">Option to get the XML Document with or without header</param>
        string Xml(bool createHeader = true);

        /// <summary>
        /// Should convert to XML Document, but redirect all Exceptions to IllegalStateException.
        /// </summary>
        /// <returns>The quietly.</returns>
        /// <param name="createHeader">Option to get the XML Document with or without header</param>
        string XmlQuietly(bool createHeader = true);
    }
}