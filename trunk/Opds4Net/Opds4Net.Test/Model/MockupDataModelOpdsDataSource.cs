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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override IEnumerable<IOpdsData> ExtractItems(OpdsCategoryItemsRequest request)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected override IOpdsData ExtractDetail(string id)
        {
            return new DataEntry()
            {
                Id = Guid.Empty.ToString(),
                Name = "书名",
                Summary = "Summary",
                UpdateTime = new DateTime(2012, 1, 1),
                Price = 5M,
                MimeType = "application/epub+zip"
            };
        }
    }
}
