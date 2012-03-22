using System;
using System.Collections.Generic;
using Opds4Net.Model;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MockupOpdsCategoryItemsRequest : OpdsItemsRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override OpdsResponse Process()
        {
            return new OpdsResponse()
            {
                Data = new List<IOpdsData>()
                {
                    new DataModel()
                    {
                        Id = Guid.Empty.ToString(),
                        Name = "小说",
                        Summary = "Summary",
                        UpdateTime = new DateTime(2012, 1, 1),
                    },
                    new DataModel()
                    {
                        Id = Guid.Empty.ToString(),
                        Name = "历史",
                        Summary = "Summary",
                        UpdateTime = new DateTime(2012, 1, 1),
                    }
                },
                TotalCount = 10
            };
        }
    }
}
