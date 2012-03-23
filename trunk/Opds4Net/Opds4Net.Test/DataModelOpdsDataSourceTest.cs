using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Model;
using Opds4Net.Server;
using Opds4Net.Test.Common;
using Opds4Net.Test.Model;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class DataModelOpdsDataSourceTest
    {
        private IOpdsDataSource mockSource;

        [TestInitialize]
        public void TestStartup()
        {
            mockSource = TestInitializer.Container.GetExportedValue<IOpdsDataSource>("DataModel");
            Assert.IsNotNull(mockSource);
        }

        [TestMethod]
        public void GetCategoriesTest()
        {
            var request = new MockupOpdsCategoryItemsRequest();
            var result = mockSource.GetItems(request);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Items.Count() > 0);
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Items.First().LastUpdatedTime.DateTime);
        }

        [TestMethod]
        public void GetItemsTest()
        {
            var item = mockSource.GetDetail(String.Empty);

            Assert.IsNotNull(item);
            Assert.AreEqual(new DateTime(2012, 1, 1), item.LastUpdatedTime.DateTime);
            Assert.AreEqual(2, item.Links.Count);
            Assert.IsInstanceOfType(item.Links.First(), typeof(OpdsLink));
            foreach (var link in item.Links)
            {
                if (link.RelationshipType != null && link.RelationshipType == OpdsRelations.OpenAcquisition)
                {
                    Assert.AreEqual("application/epub+zip", link.MediaType);
                }
            }
            Assert.AreEqual("CNY", (item.Links.First() as OpdsLink).Prices.Single().CurrencyCode);
            Assert.AreEqual(5M, (item.Links.First() as OpdsLink).Prices.Single().Price);
        }

        [TestMethod]
        public void ItemsGenerationPerformanceTest()
        {
            var request = new MockupOpdsCategoryItemsRequest();
            var result = mockSource.GetItems(request);
            var timer = new TestTimer(() => Assert.IsTrue(mockSource.GetItems(request).Items.Count() == 10));
            var times = timer.TimesInTime(new TimeSpan(0, 0, 1));

            Assert.IsTrue(times > 20000);
        }
    }
}
