using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestFormatters
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations(false);
        }

        [TestMethod()]
        public void ConvertDecimalDegreesToDMS()
        {
            double latitude = 42.336542;
            string expected = "422011.5512000000052";

            string result = tr.ConvertDecimalDegreesToDMS(latitude);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}", expected.ToString(), result.ToString()));
        }

        [TestMethod()]
        public void ConvertDMStoDecimalDegrees()
        {
            string dms = "422011.5512000000052";
            double expected = 42.336542;

            double result = tr.ConvertDMStoDecimalDegrees(dms);

            Assert.AreEqual(expected, result);

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}", expected.ToString(), result.ToString()));
        }
    }
}
