using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Xml;
using Opds4Net.Util;

namespace Opds4Net.Model
{
    /// <summary>
    /// Represents an opds link element.
    /// </summary>
    public class OpdsLink : SyndicationLink
    {
        private Collection<OpdsPrice> prices = new Collection<OpdsPrice>();
        private Collection<OpdsIndirectAcquisition> indirectAcquisitions = new Collection<OpdsIndirectAcquisition>();

        /// <summary>
        /// 
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// The price information if the link an aquisition link requires purchase.
        /// </summary>
        public Collection<OpdsPrice> Prices { get { return prices; } }

        /// <summary>
        /// The information about the acquisition media type after purchase.
        /// </summary>
        public Collection<OpdsIndirectAcquisition> IndirectAcquisitions { get { return indirectAcquisitions; } }

        /// <summary>
        /// 
        /// </summary>
        public OpdsLink() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        public OpdsLink(Uri uri) : base(uri) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="relationship"></param>
        /// <param name="title"></param>
        /// <param name="mediaType"></param>
        /// <param name="length"></param>
        public OpdsLink(Uri uri, string relationship, string title, string mediaType, long length)
            : base(uri, relationship, title, mediaType, length) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="value"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override bool TryParseAttribute(string name, string ns, string value, string version)
        {
            if (name == "count" && ns == OpdsNamespaces.Threading.Value)
            {
                var count = 0;
                if (Int32.TryParse(value, out count))
                    Count = count;

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override bool TryParseElement(XmlReader reader, string version)
        {
            if (reader.IsReadingElementOf(OpdsNamespaces.Opds.Value, "price"))
            {
                var price = new OpdsPrice();
                if (reader.HasAttributes)
                {
                    if (reader.MoveToAttribute("currencycode"))
                    {
                        if (reader.ReadAttributeValue())
                        {
                            price.CurrencyCode = reader.Value;
                            if (!CurrencyCodes.IsValid(price.CurrencyCode))
                            {
                                // TODO: Show Warning.
                            }
                        }

                        reader.MoveToElement();
                    }
                }

                price.Price = reader.ReadElementContentAsDecimal();
                prices.Add(price);

                return true;
            }
            else if (reader.IsReadingElementOf(OpdsNamespaces.Opds.Value, "indirectAcquisition"))
            {
                ReadIndirectAcquisitions(reader, IndirectAcquisitions);
                return true;
            }
            else
            {
                return base.TryParseElement(reader, version);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="version"></param>
        protected override void WriteAttributeExtensions(XmlWriter writer, string version)
        {
            if (Count.HasValue)
            {
                writer.WriteAttributeString("count", OpdsNamespaces.Threading.Value, Count.Value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="version"></param>
        protected override void WriteElementExtensions(XmlWriter writer, string version)
        {
            foreach (var price in Prices)
            {
                writer.WriteStartElement("opds", "price", OpdsNamespaces.Opds.Value);
                writer.WriteAttributeString("currencycode", price.CurrencyCode);
                writer.WriteValue(price.Price);
                writer.WriteEndElement();
            }
            WriteIndirectAcquisitions(writer, IndirectAcquisitions);

            base.WriteElementExtensions(writer, version);
        }

        private static void ReadIndirectAcquisitions(XmlReader reader, IList<OpdsIndirectAcquisition> indirectAcquisitions)
        {
            while (reader.IsReadingElementOf(OpdsNamespaces.Opds.Value, "indirectAcquisition"))
            {
                var indirectAcquisition = new OpdsIndirectAcquisition();

                // The Current IndirectAcquisition Element is finished.
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    // Must read the end element before return.
                    reader.ReadEndElement();
                    break;
                }

                if (reader.MoveToAttribute("type"))
                {
                    if (reader.ReadAttributeValue())
                    {
                        indirectAcquisition.MimeType = reader.Value;
                        indirectAcquisitions.Add(indirectAcquisition);
                        // The the the beginning of the current element.
                        reader.MoveToElement();
                        // Then check if it has a sub element.
                        if (!reader.IsEmptyElement &&
                            reader.ReadToDescendant("indirectAcquisition", OpdsNamespaces.Opds.Value))
                            ReadIndirectAcquisitions(reader, indirectAcquisition.Items);
                        else
                        {
                            // Read the next element.
                            reader.Read();
                            continue;
                        }
                    }
                }
                else
                {
                    // TODO: Record Error.
                }
            }
        }

        private static void WriteIndirectAcquisitions(XmlWriter writer, IEnumerable<OpdsIndirectAcquisition> indirectAcquisitions)
        {
            foreach (var acquisition in indirectAcquisitions)
            {
                writer.WriteStartElement("opds", "indirectAcquisition", OpdsNamespaces.Opds.Value);
                writer.WriteAttributeString("type", acquisition.MimeType);
                WriteIndirectAcquisitions(writer, acquisition.Items);
                writer.WriteEndElement();
            }
        }
    }
}
