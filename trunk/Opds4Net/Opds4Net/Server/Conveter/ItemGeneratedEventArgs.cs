using System;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemGeneratedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="data"></param>
        public ItemGeneratedEventArgs(SyndicationItem item, object data)
        {
            Item = item;
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public SyndicationItem Item { get; private set; }
    }
}
