using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Model.Validation;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class OpdsValidationTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void TestInitialization()
        {
            WebRequestHelper.SetAllowUnsafeHeaderParsing();
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void ValidateJiuYueOpds()
        {
            var errors = 0;
            var address = "http://opds.9yue.com/detail/958.atom?site=";
            var xmlReader = new XmlTextReader(address);
            Stopwatch watch = Stopwatch.StartNew();
            var validator = new OpdsValidateReader(@"..\..\..\Opds4Net\Schemas\opds_catalog.rng");
            Trace.TraceInformation(String.Format("Schema File Loaded, {0} ms used.", watch.ElapsedMilliseconds));
            validator.ValidationError += (sender, args) =>
            {
                errors++;
                Trace.WriteLine(args.Message);
            };
            watch.Restart();
            // Why the validator takes so many time?
            validator.Validate(xmlReader);
            // The validate operation is damn slow.
            Trace.TraceInformation(String.Format("Schema File Validated, {0} ms used.", watch.ElapsedMilliseconds));

            Assert.AreEqual(1, errors);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void CharacterEscapingTest()
        {
            var xmlBuilder = new MemoryStream();
            var writer = new XmlTextWriter(xmlBuilder, Encoding.UTF8);
            writer.WriteStartDocument();
            writer.WriteStartElement("doc");
            writer.WriteAttributeString("attr", "http://www.baidu.com/get?id=5&{}");
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            xmlBuilder.Seek(0, SeekOrigin.Begin);
            var result = new StreamReader(xmlBuilder).ReadToEnd();
            // xml Writer don't escape {} 
            Assert.IsTrue(result.Contains("{}"));

            xmlBuilder = new MemoryStream();
            writer = new XmlTextWriter(xmlBuilder, Encoding.UTF8);
            SyndicationItem item = new SyndicationItem("下载", "", new Uri("http://www.baidu.com/get?id=5&{}"));
            SyndicationFeed feed = new SyndicationFeed(new [] { item });
            feed.SaveAsAtom10(writer);
            writer.Flush();
            xmlBuilder.Seek(0, SeekOrigin.Begin);
            result = new StreamReader(xmlBuilder).ReadToEnd();

            // feed Writer escape {} to %7B%7D, fuck! Why? 
            // {} is valid in Atom and Url.
            Assert.IsFalse(result.Contains("{}"));
        }
    }
}
