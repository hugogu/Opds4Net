using System;
using System.Diagnostics;
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
    }
}
