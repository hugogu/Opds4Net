using System.Diagnostics;
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
            var validator = new OpdsValidateReader();
            validator.ValidationError += (sender, args) =>
            {
                errors++;
                Trace.WriteLine(args.Message);
            };
            // Why the validator takes so many time?
            validator.Validate(address, @"..\..\..\Opds4Net\Schemas\opds_catalog.rng");

            Assert.AreEqual(1, errors);
        }
    }
}
