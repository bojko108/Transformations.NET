using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TransformationsTests
    {
        [TestMethod()]
        public void TestValueToRadians()
        {
            double degrees = 42.123456789;
            double result = degrees.ToRad();
            double expected = 0.7351930132896083;

            Assert.AreEqual(expected, result, 0.00000000000000000001);
        }

        [TestMethod()]
        public void TestValueToDegrees()
        {
            double radians = 0.7351930132896083;
            double result = radians.ToDeg();
            double expected = 42.123456789;

            Assert.AreEqual(expected, result, 0.00000000000000000001);
        }

        [TestMethod()]
        public void TestExtentExpansion()
        {
            /*
             *    Empty extent
             *      +
             */
            IExtent extent = new TestExtent(4740877.11612745, 4740877.11612745, 329020.170629896, 329020.170629896);

            Assert.IsTrue(extent.IsEmpty);
            Assert.AreEqual(0.0, extent.Width, 0.001);
            Assert.AreEqual(0.0, extent.Height, 0.001);

            /*
            *    Extent expanded with 20km on each side
            *    +-----+
            *    |  +  |
            *    +-----+
            */

            extent.Expand(20000);

            Assert.IsFalse(extent.IsEmpty);
            Assert.AreEqual(40000.0, extent.Width, 0.001);
            Assert.AreEqual(40000.0, extent.Height, 0.001);
        }

        [TestMethod()]
        public void TestExtentParameters()
        {
            /*
             *    Extent
             *    +-----+
             *    |     |
             *    |     |
             *    +-----+
             */
            IExtent extent = new TestExtent(4748066.96065451, 4740877.11612745, 329020.170629896, 321536.921920285);
            double width = 7483.249;
            double height = 7189.845;

            Assert.IsFalse(extent.IsEmpty);
            Assert.AreEqual(width, extent.Width, 0.001);
            Assert.AreEqual(height, extent.Height, 0.001);

            /*
             *    Extent with empty height
             *    +-----+
             */
            extent = new TestExtent(4748066.96065451, 4748066.96065451, 329020.170629896, 321536.921920285);
            Assert.IsFalse(extent.IsEmpty);
            Assert.AreEqual(7483.249, extent.Width, 0.001);
            Assert.AreEqual(0.0, extent.Height, 0.001);

            /*
             *    Extent with empty width
             *    +
             *    |
             *    |
             *    +
             */
            extent = new TestExtent(4748066.96065451, 4740877.11612745, 329020.170629896, 329020.170629896);
            Assert.IsFalse(extent.IsEmpty);
            Assert.AreEqual(0.0, extent.Width, 0.001);
            Assert.AreEqual(7189.845, extent.Height, 0.001);

            /*
             *    Empty extent
             *    +
             */
            extent = new TestExtent(4740877.11612745, 4740877.11612745, 329020.170629896, 329020.170629896);
            Assert.IsTrue(extent.IsEmpty);
            Assert.AreEqual(0.0, extent.Width, 0.001);
            Assert.AreEqual(0.0, extent.Height, 0.001);
        }
    }
}