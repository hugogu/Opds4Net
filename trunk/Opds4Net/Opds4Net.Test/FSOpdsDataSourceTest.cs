using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Server;
using Opds4Net.Server.FileSystem;
using Opds4Net.Test.Common;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class FSOpdsDataSourceTest
    {
        private IOpdsItemConverter mockSource;

        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // Change the book folder in TestInitializer to your local directory if test failed.
            mockSource = TestInitializer.Container.GetExportedValue<IOpdsItemConverter>("DataModel");
            Assert.IsNotNull(mockSource);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void FsRootCategoryTest()
        {
            var result = mockSource.GetItems(new FSCategoryRequest(@"C:\").Process());
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Items.Any());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void FsFileDetailTest()
        {
            var itemFounded = false;
            var result = mockSource.GetItems(new FSCategoryRequest(@"C:\").Process());
            foreach (var pair in result.Items)
            {
                var item = pair.Value;
                if (File.Exists(Path.Combine(@"C:\", item.Id)))
                {
                    itemFounded = true;
                    var detail = mockSource.GetItems(new FSDetailRequest(@"C:\") { Id = item.Id }.Process()).Items.Single().Value;
                    Assert.IsNotNull(detail);
                    Assert.IsNotNull(detail.Id);
                    Assert.IsNotNull(detail.Title);
                    Assert.IsTrue(detail.LastUpdatedTime.DateTime != default(DateTime));
                    Assert.IsTrue(detail.Links.Count > 0);
                }
            }

            if (!itemFounded)
                Assert.Inconclusive("Cannot fould a book to test detail");
        }
    }
}
