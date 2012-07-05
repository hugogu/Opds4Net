using System.Collections.ObjectModel;

namespace Opds4Net.Model
{
    /// <summary>
    /// Represents the acquisition information for an OpdsLink.
    /// </summary>
    public class OpdsIndirectAcquisition
    {
        private Collection<OpdsIndirectAcquisition> items;

        /// <summary>
        /// 
        /// </summary>
        public OpdsIndirectAcquisition()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mimeType"></param>
        public OpdsIndirectAcquisition(string mimeType)
        {
            MimeType = mimeType;
        }

        /// <summary>
        /// The meida type of the acquisition.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// The sub acquisitions items. Usually used for a zip file that contains many files of different mime types.
        /// </summary>
        public Collection<OpdsIndirectAcquisition> Items
        {
            get { return items ?? (items = new Collection<OpdsIndirectAcquisition>()); }
        }
    }
}
