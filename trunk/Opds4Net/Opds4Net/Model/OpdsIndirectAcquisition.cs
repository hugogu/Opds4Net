using System.Collections.ObjectModel;

namespace Opds4Net.Model
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Collection<OpdsIndirectAcquisition> Items
        {
            get
            {
                if (items == null)
                    items = new Collection<OpdsIndirectAcquisition>();

                return items;
            }
        }
    }
}
