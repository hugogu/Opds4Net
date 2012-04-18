using System.Collections.Generic;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        public OpdsDataSource()
        {
            Data = new object[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public OpdsDataSource(IEnumerable<object> data)
        {
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> Data { get; set; }
    }
}
