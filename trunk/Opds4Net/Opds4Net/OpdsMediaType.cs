namespace Opds4Net
{
    /// <summary>
    /// The predefined "type" parameter values to help clients distinguish between relations to OPDS Catalog Entries and OPDS Catalog Feeds.
    /// For more information about media type, refer to http://tools.ietf.org/html/rfc2046. and http://tools.ietf.org/html/rfc3023 and http://tools.ietf.org/html/rfc5023
    /// </summary>
    public static class OpdsMediaType
    {
        /// <summary>
        /// The complete media type for a relation to an OPDS Catalog Feed Document Resource
        /// </summary>
        public const string Feed = "application/atom+xml;profile=opds-catalog";

        /// <summary>
        /// The complete media type for a relation to a Navigation Feed
        /// </summary>
        public const string NavigationFeed = "application/atom+xml;profile=opds-catalog;kind=navigation";

        /// <summary>
        /// The complete media type for a relation to an Acquisition Feed
        /// </summary>
        public const string AcquisitionFeed = "application/atom+xml;profile=opds-catalog;kind=acquisition";

        /// <summary>
        /// The complete media type for a relation to an OPDS Catalog Entry Document Resource
        /// </summary>
        public const string Entry = "application/atom+xml;type=entry;profile=opds-catalog";
    }
}
