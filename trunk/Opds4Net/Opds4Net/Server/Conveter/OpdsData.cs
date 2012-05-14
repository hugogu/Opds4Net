using System.Collections.Generic;

namespace Opds4Net.Server
{
    /// <summary>
    /// Represents the data set and relative information used to generate SyndicationItem.
    /// </summary>
    public class OpdsData
    {
        /// <summary>
        /// Default constructor of OpdsData, make sure the Data is empty instead of null.
        /// </summary>
        public OpdsData()
        {
            Data = new object[] { };
        }

        /// <summary>
        /// Constructor of OpdsData
        /// </summary>
        /// <param name="data">Data set to generate the OpdsData instance.</param>
        public OpdsData(IEnumerable<object> data)
        {
            Data = data;
        }

        /// <summary>
        /// The data set used to generate SyndicationItem.
        /// </summary>
        public virtual IEnumerable<object> Data { get; set; }

        /// <summary>
        /// Gets or sets the Count of data result.
        /// </summary>
        public virtual int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the category name the result data belongs to.
        /// </summary>
        public string CategoryName { get; set; }
    }
}
