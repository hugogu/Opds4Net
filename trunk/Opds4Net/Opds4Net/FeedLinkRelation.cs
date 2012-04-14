using System.Xml.Serialization;

namespace Opds4Net
{
    /// <summary>
    /// Feed related relation for links.
    /// </summary>
    public enum FeedLinkRelation
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("last")]
        Last,

        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("next")]
        Next,

        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("previous")]
        Previous,

        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("self")]
        Self,

        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("search")]
        Search,

        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("start")]
        Start,

        /// <summary>
        /// 
        /// </summary>
        [XmlEnum("up")]
        Up,
    }
}
