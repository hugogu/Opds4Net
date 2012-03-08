namespace Opds4Net
{
    /// <summary>
    /// Defines the relationships used in link that introduced by opds.
    /// </summary>
    public static class OpdsRelations
    {
        /// <summary>
        /// A generic relation that indicates that the complete representation of the content Resource may be retrieved.
        /// </summary>
        public static readonly string Acquisition = "http://opds-spec.org/acquisition";

        /// <summary>
        /// Indicates that the complete representation of the Resource can be retrieved without any requirement, which includes payment and registration.
        /// </summary>
        public static readonly string OpenAcquisition = "http://opds-spec.org/acquisition/open-access";

        /// <summary>
        /// Indicates that the complete representation of the content Resource may be retrieved as part of a lending transaction.
        /// </summary>
        public static readonly string Borrow = "http://opds-spec.org/acquisition/borrow";

        /// <summary>
        /// Indicates that the complete representation of the content Resource may be retrieved as part of a purchase.
        /// </summary>
        public static readonly string Buy = "http://opds-spec.org/acquisition/buy";

        /// <summary>
        /// Indicates that a subset of the content Resource may be retrieved.
        /// </summary>
        public static readonly string Sample = "http://opds-spec.org/acquisition/sample";

        /// <summary>
        ///  Indicates that the complete representation of the content Resource may be retrieved as part of a subscription.
        /// </summary>
        public static readonly string Subscribe = "http://opds-spec.org/acquisition/subscribe";

        /// <summary>
        /// a graphical Resource associated to the OPDS Catalog Entry
        /// </summary>
        public static readonly string Cover = "http://opds-spec.org/image";

        /// <summary>
        /// a reduced-size version of a graphical Resource associated to the OPS Catalog Entry
        /// </summary>
        public static readonly string Thumbnail = "http://opds-spec.org/image/thumbnail";

        /// <summary>
        /// An Acquisition Feed with newly released OPDS Catalog Entries. These Acquisition Feeds typically contain a subset of the OPDS Catalog Entries in an OPDS Catalog based on the publication date of the Publication.
        /// </summary>
        public static readonly string New = "http://opds-spec.org/sort/new";

        /// <summary>
        /// An Acquisition Feed with popular OPDS Catalog Entries. These Acquisition Feeds typically contain a subset of the OPDS Catalog Entries in an OPDS Catalog based on a numerical ranking criteria.
        /// </summary>
        public static readonly string Popular = "http://opds-spec.org/sort/popular";

        /// <summary>
        /// An Acquisition Feed with recommended OPDS Catalog Entries. These Acquisition Feeds typically contain a subset of the OPDS Catalog Entries in an OPDS Catalog that have been selected specifically for the user. Acquisition Feeds using the "http://opds-spec.org/recommended" relation SHOULD be ordered with the most recommended items first.
        /// </summary>
        public static readonly string Recommended = "http://opds-spec.org/recommended";

        /// <summary>
        /// A Resource that includes a user's existing set of Acquired Content, which MAY be represented as an OPDS Catalog.
        /// </summary>
        public static readonly string Shelf = "http://opds-spec.org/shelf";

        /// <summary>
        /// A Resource that includes a user's set of subscriptions, which MAY be represented as an OPDS Catalog.
        /// </summary>
        public static readonly string Subscriptions = "http://opds-spec.org/subscriptions";
    }
}
