using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class OpdsHelperTest
    {
        /// <summary>
        /// A test for GetXmlEnumName
        /// </summary>
        [TestMethod]
        public void GetXmlEnumNameTest()
        {
            Assert.AreEqual("last", FeedLinkRelation.Last.GetXmlEnumName());
        }
    }
}
