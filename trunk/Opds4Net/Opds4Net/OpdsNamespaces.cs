namespace Opds4Net
{
    /// <summary>
    /// Defines the commonly used opds namespaces.
    /// </summary>
    public class OpdsNamespaces
    {
        /// <summary>
        /// The namespace of Opds itself, introduced opds:price, opds:indirectAcquisition element for a link
        /// and opds:facetGroup, opds:activeFacet attribute for a link. Refer to http://opds-spec.org/specs/opds-catalog-1-1
        /// </summary>
        public static readonly string Opds = "http://opds-spec.org/2010/catalog";

        /// <summary>
        /// Introduced dc:publisher, dc:issued, dc:language, dc:identifier element on an entry.
        /// Refer to http://dublincore.org/documents/dcmi-terms/
        /// </summary>
        public static readonly string DublinCore = "http://purl.org/dc/terms/";

        /// <summary>
        /// Introduced the thr:count attribute on a link.
        /// Refer to http://www.ietf.org/rfc/rfc4685.txt
        /// </summary>
        public static readonly string Threading = "http://purl.org/syndication/thread/1.0";
    }
}
