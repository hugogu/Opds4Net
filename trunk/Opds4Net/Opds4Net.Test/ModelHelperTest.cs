using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Opds4Net.Reflection;
using Opds4Net.Reflection.Extension;
using Opds4Net.Test.Common;
using Opds4Net.Test.Model;
using Opds4Net.Util;

namespace Opds4Net.Test
{
    [TestClass]
    public class ModelHelperTest
    {
        [TestMethod]
        public void GetPropertyTest()
        {
            var name = "xailjg";
            var model = new DataModel() { Name = name };

            var value = AdaptedPropertyAccessor<DataModel>.GetProperty(model, "Name");
            Assert.AreEqual(name, value);
            value = AdaptedPropertyAccessor<DataModel>.GetProperty(model, "Title");
            Assert.AreEqual(name, value);
            value = AdaptedPropertyAccessor<DataModel>.GetProperty(model, "CategoryNameNotExists");
            Assert.AreEqual(null, value);
        }

        [TestMethod]
        public void GetPropertyByExtensionTest()
        {
            var name = "xailjg";
            var model = new DataModel() { Name = name };

            Assert.AreEqual(name, model.GetProperty("Name"));
            Assert.AreEqual(name, model.GetProperty("Title"));
        }

        [TestMethod]
        public void GetComplexOpdsPropertyTest()
        {
            var model = new DataEntry();

            Assert.AreEqual(model.AuthorInfo.Name, model.GetProperty("AuthorName"));
            Assert.AreEqual(model.AuthorInfo.Email, model.GetProperty("AuthorEmail"));
            Assert.AreEqual(model.AuthorInfo.Address.Country, model.GetProperty("AuthorAddress"));
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void GetPropertyPerformanceTest()
        {
            var time = 10000000;
            var name = "Dummy";
            var model = new DataModel() { Name = name };
            var classSpecifiedAdapter = AdaptedAccessorFactory.Instance.GetAccessor(model);
            var globalTarget = String.Empty;

            var timeDynamic = new TestTimer(() =>
            {
                globalTarget = classSpecifiedAdapter.GetProperty(model, "Name").ToNullableString();
            }).TimeForTimes(time);
            Assert.AreEqual(name, globalTarget);

            var timeStatic = new TestTimer(() =>
            {
                globalTarget = model.Name;
            }).TimeForTimes(time);
            Assert.AreEqual(name, globalTarget);

            var timeRandomClass = new TestTimer(() =>
            {
                globalTarget = model.GetProperty("Name").ToNullableString();
            }).TimeForTimes(time);
            Assert.AreEqual(name, globalTarget);

            Assert.IsTrue(timeDynamic > timeStatic);
            Assert.IsTrue(timeRandomClass > timeDynamic);

            Assert.IsTrue(timeDynamic.TotalMilliseconds < timeStatic.TotalMilliseconds * 10);
            Assert.IsTrue(timeRandomClass.TotalMilliseconds < timeStatic.TotalMilliseconds * 20);
        }

        [TestMethod]
        [TestCategory("Performance")]
        public void MultiThreadGetPropertyPerformanceTest()
        {
            var time = 20000000;
            var name = "Dummy";
            var model = new DataModel() { Name = name };
            var classSpecifiedAdapter = AdaptedAccessorFactory.Instance.GetAccessor(model);
            var globalTarget = String.Empty;

            var timeDynamic = new TestTimer(() =>
            {
                globalTarget = classSpecifiedAdapter.GetProperty(model, "Name").ToNullableString();
            }).TimeForTimes(time);
            Assert.AreEqual(name, globalTarget);

            var timeDynamicMT = new TestTimer(() =>
            {
                globalTarget = classSpecifiedAdapter.GetProperty(model, "Name").ToNullableString();
            }).TimeForTimesParallel(time, 4);
            Assert.AreEqual(name, globalTarget);

            // The performance is not improved too much by using multi-thread.
            Assert.IsTrue(timeDynamic.TotalMilliseconds > timeDynamicMT.TotalMilliseconds * 1.4);
        }
    }
}
