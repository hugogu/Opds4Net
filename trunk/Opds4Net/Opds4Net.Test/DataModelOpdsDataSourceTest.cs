using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Model;
using Opds4Net.Server;
using Opds4Net.Test.Common;
using Opds4Net.Test.Model;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class DataModelOpdsDataSourceTest
    {
        private IOpdsItemConverter mockSource;
        private OpdsData itemsDataSource;

        [TestInitialize]
        public void TestStartup()
        {
            mockSource = TestInitializer.Container.GetExportedValue<IOpdsItemConverter>();
            Assert.IsNotNull(mockSource);

            itemsDataSource = new OpdsData()
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
            Assert.IsTrue(result.Items.Any());
            Assert.AreEqual(new DateTime(2012, 1, 1), result.Items.First().Value.LastUpdatedTime.DateTime);
        }

        [TestMethod]
        public void GetItemsTest()
        {
            var dataSource = new OpdsData()
            {
                Data = new[] { MockupNamingDataSource.GetDetailedItems() }
            };
            var item = mockSource.GetItems(dataSource).Items.Single().Value;

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
            Assert.AreEqual(new DataEntry().ISBN, (item as OpdsItem).ISBN);
            // type of ISBN
            var xml = item.ToXml();
            Assert.IsTrue(xml.Contains(":type=\"URI\""));
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void ItemsGenerationPerformanceTest()
        {
            var duration = new TimeSpan(0, 0, 1);
            var request = new MockupNamingDataSource();
            var result = mockSource.GetItems(itemsDataSource);
            var timer = new TestTimer(() => Assert.IsTrue(mockSource.GetItems(itemsDataSource).Items.Count() == 10));
            var timesMt = timer.TimesInTimeParallel(duration, 4);
            var times = timer.TimesInTime(duration);

            Assert.IsTrue(timesMt > times * 1.7);
        }

        [TestMethod]
        public void SimpleItemsGenerationTest()
        {
            // Initialize data.
            var data = new List<Book>() { new Book() { } };
            // Create converter
            var converter = new NamingDataOpdsItemConverter();
            // Convert data to opds items
            var items = converter.GetItems(new OpdsData(data));
        }
    }
}
