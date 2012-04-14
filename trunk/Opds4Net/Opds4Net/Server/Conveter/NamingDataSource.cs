using System.Collections.Generic;

namespace Opds4Net.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class NamingDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        public NamingDataSource()
        {
            Data = new object[] { };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public NamingDataSource(IEnumerable<object> data)
        {
            Data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<object> Data { get; set; }
    }
}
