using System.Collections.Generic;

namespace Opds4Net.Server
{
    /// <summary>
    /// Represents the data set and relative information used to generate SyndicationItem.
    /// </summary>
    public class OpdsDataSource
    {
        /// <summary>
        /// Default constructor of OpdsDataSource, make sure the Data is empty instead of null.
        /// </summary>
        public OpdsDataSource()
        {
            Data = new object[] { };
        }

        /// <summary>
        /// Constructor of OpdsDataSource
        /// </summary>
        /// <param name="data">Data set to generate the OpdsDataSource instance.</param>
        public OpdsDataSource(IEnumerable<object> data)
        {
            Data = data;
        }

        /// <summary>
        /// The data set used to generate SyndicationItem.
        /// </summary>
        public IEnumerable<object> Data { get; set; }
    }
}
