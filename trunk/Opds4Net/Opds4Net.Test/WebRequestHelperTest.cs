using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    [TestClass]
    public class WebRequestHelperTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void UpdateUrlParameterTest()
        {
            var url = "http://snda.com/";

            var newUrl = WebRequestHelper.UpdateUrlParameter(url, "page", "1");
            Assert.AreEqual("http://snda.com/?page=1", newUrl);

            url = "http://snda.com/?page=2";
            newUrl = WebRequestHelper.UpdateUrlParameter(url, "page", "1");
            Assert.AreEqual("http://snda.com/?page=1", newUrl);
        }
    }
}
