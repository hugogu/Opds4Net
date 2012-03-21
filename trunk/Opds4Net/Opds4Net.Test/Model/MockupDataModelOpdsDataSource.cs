using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Opds4Net.Model;
using Opds4Net.Server;

namespace Opds4Net.Test.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Export("DataModel", typeof(IOpdsDataSource))]
    public class MockupDataModelOpdsDataSource : DataModelOpdsDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="linkGenerator"></param>
        [ImportingConstructor]
        public MockupDataModelOpdsDataSource([Import]IOpdsLinkGenerator linkGenerator)
            : base(linkGenerator)
        {
        }

        protected override IEnumerable<IOpdsData> ExtractItems(string id)
        {
            yield return new DataModel()
            {
                Id = Guid.Empty.ToString(),
                Name = "小说",
                Summary = "Summary",
                UpdateTime = new DateTime(2012, 1, 1),
            };
            yield return new DataModel()
            {
                Id = Guid.Empty.ToString(),
                Name = "历史",
                Summary = "Summary",
                UpdateTime = new DateTime(2012, 1, 1),
            };
        }

        protected override IOpdsData ExtraceDetail(string id)
        {
            return new DataModel()
            {
                Id = Guid.Empty.ToString(),
                Name = "书名",
                Summary = "Summary",
                UpdateTime = new DateTime(2012, 1, 1),
                Price = 5M,
                DataType = OpdsDataType.Entity,
                MimeType = "application/epub+zip"
            };
        }
    }
}
