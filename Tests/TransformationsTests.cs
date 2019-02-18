using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void TestControlPoints()
        {
            ControlPoints.ControlPoints.LoadControlPoints();
            Assert.IsTrue(ControlPoints.ControlPoints.areControlPointsLoaded);
        }

        [TestMethod()]
        public void TestGetListByInputPoint()
        {
            ControlPoints.ControlPoints.LoadControlPoints();
            GeoPoint point = new GeoPoint(4832666.465, 192546.481);
            string expected = "K-3-33-143";
            string result = ControlPoints.ControlPoints.GetUTMListNumber(point);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }
    }
}