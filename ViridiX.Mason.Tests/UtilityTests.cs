using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ViridiX.Mason.Utilities;

namespace ViridiX.Mason.Tests
{
    [TestClass]
    public class UtilityTests
    {
        public int TestProperty { get; set; }

        [TestMethod]
        public void TemporaryAssignmentTest()
        {
            TestProperty = 123;
            Assert.AreEqual(TestProperty, 123);
            using (new TemporaryPropertyAssignment<int>(this, nameof(TestProperty), 10))
            {
                Assert.AreEqual(TestProperty, 10);
            }
            Assert.AreEqual(TestProperty, 123);

            try
            {
                using (new TemporaryPropertyAssignment<int>(this, nameof(TestProperty), 10))
                {
                    Assert.AreEqual(TestProperty, 10);
                    throw new Exception();
                }
            }
            catch
            {
                Assert.AreEqual(TestProperty, 123);
            }
        }
    }
}
