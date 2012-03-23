using System;
using System.Collections.Generic;
using Opds4Net.Model;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MockupOpdsCategoryItemsRequest : DataItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override DataResponse Process()
        {
            return new DataResponse()
            {
                Data = GetItems(),
                TotalCount = 10
            };
        }

        private IEnumerable<IOpdsData> GetItems()
        {
            for (int i = 1; i <= 10; i++)
            {
                yield return new DataModel()
                {
                    Id = i.ToString(),
                    Name = "历史",
                    Summary = "Summary",
                    UpdateTime = new DateTime(2012, 1, i),
                };
            }
        }
    }
}
