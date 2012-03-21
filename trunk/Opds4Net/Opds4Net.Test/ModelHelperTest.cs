using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Test.Model;
using Opds4Net.Util;
using Opds4Net.Util.Extension;

namespace Opds4Net.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ModelHelperTest
    {
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetPropertyTest()
        {
            var name = "xailjg";
            var model = new DataModel() { Name = name };

            var value = ModelHelper<DataModel>.GetProperty(model, "Name");
            Assert.AreEqual(name, value);
            value = ModelHelper<DataModel>.GetProperty(model, "Title");
            Assert.AreEqual(name, value);
            value = ModelHelper<DataModel>.GetProperty(model, "CategoryNameNotExists");
            Assert.AreEqual(null, value);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void GetPropertyByExtensionTest()
        {
            var name = "xailjg";
            var model = new DataModel() { Name = name };

            Assert.AreEqual(name, model.GetProperty("Name"));
            Assert.AreEqual(name, model.GetProperty("Title"));
        }
    }
}
