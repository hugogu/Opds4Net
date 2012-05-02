using System;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemDetailRequest : IDataRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OpdsData Process()
        {
            return new OpdsData();
        }
    }
}
