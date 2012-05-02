using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    [TestClass]
    public class OpdsHelperTest
    {
        [TestMethod]
        public void GetExtensionNameTest()
        {
            var ext = OpdsHelper.GetExtensionName("text/plain");

            Assert.AreEqual(".txt", ext);
        }

        [TestMethod]
        public void DetectFileMimeTypeTest()
        {
            var mime = OpdsHelper.DetectFileMimeType(@"C:\a.txt");

            Assert.AreEqual("text/plain", mime);
        }
    }
}
