using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Model;

namespace Opds4Net.Test
{
    /// <summary>
    /// This is a test class for CurrencyCodesTest and is intended
    /// to contain all CurrencyCodesTest Unit Tests
    /// </summary>
    [TestClass]
    public class CurrencyCodesTest
    {
        /// <summary>
        ///A test for IsValidCurrencyCode
        ///</summary>
        [TestMethod()]
        public void IsValidCurrencyCodeTest()
        {
            Assert.AreEqual(true, CurrencyCodes.IsValid("CNY"));
            Assert.AreEqual(true, CurrencyCodes.IsValid("USD"));
            Assert.AreEqual(false, CurrencyCodes.IsValid("BTC"));
        }

        /// <summary>
        ///A test for GetCurrencyCodeByCulture
        ///</summary>
        [TestMethod()]
        public void GetCurrencyCodeByCultureTest()
        {
            Assert.AreEqual("CNY", CurrencyCodes.GetCurrencyCode(CultureInfo.GetCultureInfo("zh-CN")));
        }
    }
}
