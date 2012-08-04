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
            WebRequestHelper.SetAllowUnsafeHeaderParsing();
        }

        [TestMethod]
        [DeploymentItem("Opds4Net.dll.config")]
        public void LoadingTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://m.gutenberg.org/ebooks/?format=opds"));
            Assert.IsNotNull(feed);
            Assert.IsInstanceOfType(feed, typeof(OpdsFeed));
        }

        [TestMethod]
        public void LoadingDetailPageTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://m.gutenberg.org/ebooks/22945.opds"));
            Assert.IsNotNull(feed);
            Assert.IsInstanceOfType(feed, typeof(OpdsFeed));
            Assert.IsInstanceOfType(feed.Items.First(), typeof(OpdsItem));
        }

        [TestMethod]
        public void LoadDetailEntryWithoutFeedTest()
        {
            var entry = OpdsItem.Load(new XmlTextReader("http://www.feedbooks.com/item/220817.atom"));
            Assert.IsNotNull(entry);
            Assert.IsInstanceOfType(entry, typeof(OpdsItem));
        }

        [TestMethod]
        public void OpdsPriceGenerationTest()
        {
            var feed = OpdsItem.Load(new XmlTextReader("http://www.feedbooks.com/item/220817.atom"));
            var xml = feed.ToXml();
            Trace.Write(xml);
            Assert.IsTrue(xml.Contains("opds:price"));
        }

        [TestMethod]
        public void IndirectAcquisitionReadingTest()
        {
            var entry = OpdsItem.Load(new XmlTextReader("http://www.feedbooks.com/item/220817.atom"));
            var link = entry.Links.Single(l => (l as OpdsLink).Prices.Count > 0) as OpdsLink;
            var indirectAcquisition = new OpdsIndirectAcquisition("application/zip");
            indirectAcquisition.Items.Add(new OpdsIndirectAcquisition("application/epub+zip"));
            indirectAcquisition.Items.Add(new OpdsIndirectAcquisition("application/pdf"));
            indirectAcquisition.Items.Add(new OpdsIndirectAcquisition("application/msword"));
            link.IndirectAcquisitions.Clear();
            link.IndirectAcquisitions.Add(indirectAcquisition);

            var xml = entry.ToXml();

            entry = OpdsItem.Load(new XmlTextReader(new StringReader(xml)));
            link = entry.Links.Single(l => (l as OpdsLink).Prices.Count > 0) as OpdsLink;
            Assert.AreEqual(1, link.IndirectAcquisitions.Count);
            Assert.AreEqual(3, link.IndirectAcquisitions.First().Items.Count);
            Assert.AreEqual(xml, entry.ToXml());
        }

        [TestMethod()]
        public void FindLinkTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://m.gutenberg.org/ebooks/?format=opds"));
            Assert.AreEqual("/ebooks.opds/", feed.FindLink("self").Uri.ToString());
            Assert.AreEqual("http://m.gutenberg.org/catalog/osd-books.xml", feed.FindLink("search").Uri.ToString());

            feed = OpdsFeed.Load(new XmlTextReader("http://m.gutenberg.org/ebooks/search.opds/?sort_order=downloads"));
            Assert.AreEqual("/ebooks/search.opds/?sort_order=downloads&start_index=26", feed.FindLink("next").Uri.ToString());
        }

        [TestMethod]
        public void GetAllOpdsNamespacesTest()
        {
            var namespaces = OpdsNamespaces.GetAll();
            foreach (var ns in namespaces)
            {
                Assert.IsNotNull(ns);
                Assert.IsNotNull(ns.Value);
                Assert.IsNotNull(ns.Key);
                Assert.IsNotNull(ns.Key.Name);
            }
        }

        [TestMethod]
        public void LoadFeedWithBase64EncodedImage()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://m.gutenberg.org/ebooks/search.opds/?sort_order=downloads&start_index=1"));
            Assert.IsTrue(feed.Items.Count() > 0);
            var entry = feed.Items.FirstOrDefault();
            Assert.IsNotNull(entry);
            Assert.IsTrue(entry.Links.Count > 0);
            var link = entry.Links.FirstOrDefault(l => l.RelationshipType == OpdsRelations.Thumbnail);
            Assert.IsNotNull(link);
            Assert.AreEqual("http", link.Uri.Scheme);
        }

        [TestMethod]
        public void FacetGroupTest()
        {
            var feed = OpdsFeed.Load(new XmlTextReader("http://www.feedbooks.com/books/catalog.atom"));
            var facetGroups = feed.FacetGroups.ToList();

            Assert.IsTrue(facetGroups.Count > 1);
            foreach (var group in facetGroups)
            {
                Assert.IsNotNull(group.Key);
                Assert.IsTrue(group.Count() > 0);
            }
        }
    }
}
