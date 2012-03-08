using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Model;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    [TestClass]
    public class OpdsFeedTest
    {
        [TestInitialize]
        public void Initialize()
        {
            WebRequestSettingsHelper.SetAllowUnsafeHeaderParsing();
        }

        [TestMethod]
        [DeploymentItem("Opds4Net.dll.config")]
        public void LoadingTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://opds.9yue.com/category.atom"));
            Assert.IsNotNull(feed);
            Assert.IsInstanceOfType(feed, typeof(OpdsFeed));
        }

        [TestMethod]
        public void LoadingDetailPageTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://opds.9yue.com/detail/1724.atom"));
            Assert.IsNotNull(feed);
            Assert.IsInstanceOfType(feed, typeof(OpdsFeed));
            Assert.IsInstanceOfType(feed.Items.First(), typeof(OpdsItem));
        }

        [TestMethod]
        public void OpdsPriceGenerationTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://opds.9yue.com/detail/1724.atom"));
            var xml = feed.ToXml();
            Trace.Write(xml);
            Assert.IsTrue(xml.Contains("opds:price"));
        }

        [TestMethod]
        public void IndirectAcquisitionReadingTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://opds.9yue.com/detail/1724.atom"));
            var entry = feed.Items.First();
            var link = entry.Links.Single(l => l.Prices.Count > 0);
            var indirectAcquisition = new OpdsIndirectAcquisition("application/zip");
            indirectAcquisition.Items.Add(new OpdsIndirectAcquisition("application/epub+zip"));
            indirectAcquisition.Items.Add(new OpdsIndirectAcquisition("application/pdf"));
            indirectAcquisition.Items.Add(new OpdsIndirectAcquisition("application/msword"));
            link.IndirectAcquisitions.Add(indirectAcquisition);

            var xml = feed.ToXml();

            feed = OpdsFeed.Load(new XmlTextReader(new StringReader(xml)));
            entry = feed.Items.First();
            link = entry.Links.Single(l => l.Prices.Count > 0);
            Assert.AreEqual(1, link.IndirectAcquisitions.Count);
            Assert.AreEqual(3, link.IndirectAcquisitions.First().Items.Count);
            Assert.AreEqual(xml, feed.ToXml());
        }

        /// <summary>
        ///A test for FindLink
        ///</summary>
        [TestMethod()]
        public void FindLinkTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://opds.9yue.com/category.atom"));
            Assert.AreEqual("http://opds.9yue.com/category.atom?site=", feed.FindLink(FeedLinkRelation.Self).Uri.ToString());
            Assert.AreEqual("http://opds.9yue.com/search.atom?key={searchTerms}&site=", feed.FindLink(FeedLinkRelation.Search).Uri.ToString());
            feed = OpdsFeed.Load(new XmlTextReader("http://opds.9yue.com/popular.atom"));
            Assert.AreEqual("http://opds.9yue.com/popular.atom?site=&page=2", feed.FindLink(FeedLinkRelation.Next).Uri.ToString());
            Assert.AreEqual("http://opds.9yue.com/popular.atom?site=&page=5", feed.FindLink(FeedLinkRelation.Last).Uri.ToString());
        }
    }
}
