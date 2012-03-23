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
        public MockupDataModelOpdsDataSource(
            [Import]IOpdsLinkGenerator linkGenerator,
            [Import("OpdsData")]IDataTypeDetector typeDetector)
            : base(linkGenerator, typeDetector)
        {
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
