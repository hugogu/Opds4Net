using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Opds4Net
{
    /// <summary>
    /// Defines the commonly used opds namespaces.
    /// </summary>
    public static class OpdsNamespaces
    {
        private static ICollection<KeyValuePair<XmlQualifiedName, string>> namespaces = new List<KeyValuePair<XmlQualifiedName, string>>();

        /// <summary>
        /// The namespace of Opds itself, introduced opds:price, opds:indirectAcquisition element for a link
        /// and opds:facetGroup, opds:activeFacet attribute for a link. Refer to http://opds-spec.org/specs/opds-catalog-1-1
        /// </summary>
        public static readonly KeyValuePair<string, string> Opds = new KeyValuePair<string, string>("opds", "http://opds-spec.org/2010/catalog");

        /// <summary>
        /// Introduced dc:publisher, dc:issued, dc:language, dc:identifier element on an entry.
        /// Refer to http://dublincore.org/documents/dcmi-terms/
        /// </summary>
        public static readonly KeyValuePair<string, string> DublinCore = new KeyValuePair<string, string>("dc", "http://purl.org/dc/terms/");

        /// <summary>
        /// Introduced the thr:count attribute on a link.
        /// Refer to http://www.ietf.org/rfc/rfc4685.txt
        /// </summary>
        public static readonly KeyValuePair<string, string> Threading = new KeyValuePair<string, string>("thr", "http://purl.org/syndication/thread/1.0");

        /// <summary>
        /// Get all the namespace definitions defined within this class.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<XmlQualifiedName, string>> GetAll()
        {
            if (namespaces.Count == 0)
            {
                var type = typeof(OpdsNamespaces);
                foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    var value = (KeyValuePair<string, string>)field.GetValue(null);

                    namespaces.Add(new KeyValuePair<XmlQualifiedName, string>(new XmlQualifiedName(value.Key, XNamespace.Xmlns.ToString()), value.Value));
                }
            }

            return namespaces;
        }
    }
}
