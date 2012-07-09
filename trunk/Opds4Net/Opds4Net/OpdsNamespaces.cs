using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using NamePair = System.Collections.Generic.KeyValuePair<System.Xml.XmlQualifiedName, string>;
using StringPair = System.Collections.Generic.KeyValuePair<string, string>;

namespace Opds4Net
{
    /// <summary>
    /// Defines and customs the commonly used opds namespaces.
    /// </summary>
    public static class OpdsNamespaces
    {
        private static ICollection<NamePair> namespaces = new List<NamePair>();

        static OpdsNamespaces()
        {
            foreach (var field in typeof(OpdsNamespaces).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                var value = (StringPair)field.GetValue(null);

                AddToNamespaces(value.Key, value.Value);
            }
        }

        /// <summary>
        /// The namespace of Opds itself, introduced opds:price, opds:indirectAcquisition element for a link
        /// and opds:facetGroup, opds:activeFacet attribute for a link. Refer to http://opds-spec.org/specs/opds-catalog-1-1
        /// </summary>
        public static readonly StringPair Opds = new StringPair("opds", "http://opds-spec.org/2010/catalog");

        /// <summary>
        /// Introduced dc:publisher, dc:issued, dc:language, dc:identifier element on an entry.
        /// Refer to http://dublincore.org/documents/dcmi-terms/ 
        /// and http://web.resource.org/rss/1.0/modules/dcterms
        /// </summary>
        public static readonly StringPair DublinCore = new StringPair("dc", "http://purl.org/dc/terms/");

        /// <summary>
        /// Introduced the thr:count attribute on a link.
        /// Refer to http://www.ietf.org/rfc/rfc4685.txt
        /// </summary>
        public static readonly StringPair Threading = new StringPair("thr", "http://purl.org/syndication/thread/1.0");

        /// <summary>
        /// Refer to http://www.opensearch.org/Specifications/OpenSearch/1.1
        /// </summary>
        public static readonly StringPair OpenSearch = new StringPair("opensearch", "http://a9.com/-/spec/opensearch/1.1/");

        /// <summary>
        /// Introduced the relevance:score on for an entry.
        /// Refer to http://www.opensearch.org/Specifications/OpenSearch/Extensions/Relevance/1.0/Draft_1
        /// </summary>
        public static readonly StringPair Relevance = new StringPair("relevance", "http://a9.com/-/opensearch/extensions/relevance/1.0/");

        /// <summary>
        /// 
        /// </summary>
        public static readonly StringPair Xsi = new StringPair("xsi", "http://www.w3.org/2001/XMLSchema-instance");

        /// <summary>
        /// Regist a new namespace to opds feed.
        /// </summary>
        /// <param name="namespace">The local name of the namespace</param>
        /// <param name="value">The value of the namespace</param>
        public static void RegistNamespace(string @namespace, string value)
        {
            if (namespaces.Any(n => n.Key.Name == @namespace))
            {
                throw new InvalidOperationException("The given namespace already registed.");
            }

            AddToNamespaces(@namespace, value);
        }

        /// <summary>
        /// Get all the namespace definitions defined within this class.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<NamePair> GetAll()
        {
            return new SynchronizedReadOnlyCollection<NamePair>(namespaces, namespaces);
        }

        private static void AddToNamespaces(string name, string value)
        {
            lock (namespaces)
            {
                namespaces.Add(new NamePair(new XmlQualifiedName(name, XNamespace.Xmlns.ToString()), value));
            }
        }
    }
}
