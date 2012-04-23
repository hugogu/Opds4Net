using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Server;
using Opds4Net.Test.Common;
using Opds4Net.Test.Model;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class DetectorTest
    {
        private IDataTypeDetector opdsDataDetector;

        [TestInitialize]
        public void TestInitialize()
        {
            opdsDataDetector = TestInitializer.Container.GetExportedValue<IDataTypeDetector>("OpdsData");
            Assert.IsNotNull(opdsDataDetector);
        }

        [TestMethod]
        public void GetDataTypeFromValidDynamicObjectTest()
        {
            var obj = new DataEntry();

            var dataType = opdsDataDetector.DetectType(obj);
            Assert.AreEqual(OpdsDataType.Detial, dataType);
        }

        [TestMethod]
        public void GetDataTypeFromOpdsDataTest()
        {
            var obj = new DataEntry();

            var dataType = opdsDataDetector.DetectType(obj);
            Assert.AreEqual(OpdsDataType.Detial, dataType);
        }

        [TestMethod]
        public void AnonymousTypeTest()
        {
            var obj = new { OpdsDataType = OpdsDataType.Detial };

            // AnonymousType is compiled to internal class.
            // So requires the current Assembly Internal Visible to The Opds4Net assembly.
            var detectedType = opdsDataDetector.DetectType(obj);
            Assert.AreEqual(OpdsDataType.Detial, detectedType);
        }
    }
}
