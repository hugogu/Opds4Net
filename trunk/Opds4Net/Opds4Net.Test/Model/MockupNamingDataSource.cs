using System;
using System.Collections.Generic;
using System.Globalization;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MockupNamingDataSource : DataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override OpdsData Process()
        {
            return new OpdsData
            {
                Data = GetItems(),
                TotalCount = 10
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IOpdsDataTypeHost> GetItems()
        {
            for (var i = 1; i <= 10; i++)
            {
                yield return new DataModel
                {
                    Id = i.ToString(CultureInfo.InvariantCulture),
                    Name = "历史",
                    Summary = "Summary",
                    UpdateTime = new DateTime(2012, 1, i),
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IOpdsDataTypeHost GetDetailedItems()
        {
            return new DataEntry
            {
                Id = "1",
                Name = "历史",
                Summary = "Summary",
                UpdateTime = new DateTime(2012, 1, 1),
                MimeType = "application/epub+zip",
                Price = 5M,
            };
        }
    }
}
