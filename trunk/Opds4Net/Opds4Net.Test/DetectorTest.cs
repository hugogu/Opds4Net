using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Model;
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
        private IDataTypeDetector dynamicDetector;
        private IDataTypeDetector opdsDataDetector;

        /// <summary>
        /// 
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            dynamicDetector = TestInitializer.Container.GetExportedValue<IDataTypeDetector>("Dynamic");
            opdsDataDetector = TestInitializer.Container.GetExportedValue<IDataTypeDetector>("OpdsData");
            Assert.IsNotNull(dynamicDetector);
            Assert.IsNotNull(opdsDataDetector);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetDataTypeFromValidDynamicObjectTest()
        {
            var obj = new DataEntry();

            var dataType = dynamicDetector.DetectType(obj);
            Assert.AreEqual(OpdsDataType.Entity, dataType);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetDataTypeFromOpdsDatTest()
        {
            var obj = new DataEntry();

            var dataType = opdsDataDetector.DetectType(obj);
            Assert.AreEqual(OpdsDataType.Entity, dataType);
        }
    }
}
