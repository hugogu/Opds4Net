﻿using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsItemsResult
    {
        /// <summary>
        /// 
        /// </summary>
        public OpdsItemsResult()
        {
            Items = new SyndicationItem[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        public OpdsItemsResult(IEnumerable<SyndicationItem> items)
        {
            Items = items;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<SyndicationItem> Items { get; set; }
    }
}
