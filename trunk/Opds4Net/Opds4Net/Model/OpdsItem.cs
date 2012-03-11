using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Serialization;
using Opds4Net.Util;

namespace Opds4Net.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsItem : SyndicationItem
    {
        #region Dublin Core Element
        /// <summary>
        /// The language of the book
        /// </summary>
        [XmlElement("language")]
        public string Language { get; set; }

        /// <summary>
        /// The time when the publisher publish the book.
        /// </summary>
        [XmlElement("issued")]
        public string Issued { get; set; }

        /// <summary>
        /// Unique identifier of the book. In the format of urn:isbn:xxxxxxxxxxxxxxxx.
        /// </summary>
        [XmlElement("identifier")]
        public string ISBN { get; set; }

        /// <summary>
        /// The publisher.
        /// </summary>
        [XmlElement("publisher")]
        public string Publisher { get; set; } 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public new static OpdsItem Load(XmlReader reader)
        {
            return SyndicationItem.Load<OpdsItem>(reader);
        }

        protected override SyndicationLink CreateLink()
        {
            return new OpdsLink();
        }

        protected override bool TryParseElement(XmlReader reader, string version)
        {
            if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore, "language"))
            {
                Language = reader.ReadElementContentAsString();
                return true;
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore, "issued"))
            {
                Issued = reader.ReadElementContentAsString();
                return true;
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore, "identifier"))
            {
                ISBN = reader.ReadElementContentAsString();
                return true;
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.DublinCore, "publisher"))
            {
                Publisher = reader.ReadElementContentAsString();
                return true;
            }
            else
            {
                return base.TryParseElement(reader, version);
            }
        }

        protected override void WriteElementExtensions(XmlWriter writer, string version)
        {
            if (!String.IsNullOrEmpty(Language))
                writer.WriteElementString("language", OpdsNamespaces.DublinCore, Language);
            if (!String.IsNullOrEmpty(Issued))
                writer.WriteElementString("issued", OpdsNamespaces.DublinCore, Issued);
            if (!String.IsNullOrEmpty(ISBN))
                writer.WriteElementString("identifier", OpdsNamespaces.DublinCore, ISBN);
            if (!String.IsNullOrEmpty(Publisher))
                writer.WriteElementString("publisher", OpdsNamespaces.DublinCore, Publisher);

            base.WriteElementExtensions(writer, version);
        }
    }
}
