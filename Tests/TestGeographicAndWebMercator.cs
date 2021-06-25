using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndWebMercator
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations(false);
        }

        [TestMethod()]
        public void TransformGeographicToWebMercatorTest()
        {
            IPoint input = new TestPoint(42.450682, 24.749747);
            IPoint expected = new TestPoint(2755129.23, 5228730.33);
            IPoint result = tr.TransformGeographicToWebMercator(input);

            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformWebMercatorToGeographicTest()
        {
            IPoint input = new TestPoint(2755129.23, 5228730.33);
            IPoint expected = new TestPoint(42.450682, 24.749747);
            IPoint result = tr.TransformWebMercatorToGeographic(input);

            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
