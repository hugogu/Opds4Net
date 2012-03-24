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
        private IOpdsItemConverter mockSource;
        private NamingDataSource itemsDataSource;

        [TestInitialize]
        public void TestStartup()
        {
            mockSource = TestInitializer.Container.GetExportedValue<IOpdsItemConverter>();
            Assert.IsNotNull(mockSource);

            itemsDataSource = new NamingDataSource()
            {
                Data = MockupNamingDataSource.GetItems()
            };
        }

        [TestMethod]
        public void GetCategoriesTest()
        {
            var result = mockSource.GetItems(itemsDataSource);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.IsTrue(result.Items.Count() > 0);
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Items.First().LastUpdatedTime.DateTime);
        }

        [TestMethod]
        public void GetItemsTest()
        {
            var dataSource = new NamingDataSource()
            {
                Data = new [] { MockupNamingDataSource.GetDetailedItems() }
            };
            var item = mockSource.GetItems(dataSource).Items.Single();

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
        [TestCategory("Performance")]
        public void ItemsGenerationPerformanceTest()
        {
            var duration = new TimeSpan(0, 0, 1);
            var request = new MockupNamingDataSource();
            var result = mockSource.GetItems(itemsDataSource);
            var timer = new TestTimer(() => Assert.IsTrue(mockSource.GetItems(itemsDataSource).Items.Count() == 10));
            var timesMT = timer.TimesInTimeParallel(duration, 4);
            var times = timer.TimesInTime(duration);

            Assert.IsTrue(timesMT > times * 1.7);
        }
    }
}
