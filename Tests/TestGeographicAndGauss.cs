using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndGauss
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations(false);
        }

        [TestMethod()]
        public void TransformGeographicToGauss()
        {
            IPoint input = new TestPoint(42.7602978166667, 25.3824052611111);
            IPoint expected = new TestPoint(4736629.503, 8613154.6069);
            //IPoint result = tr.TransformGeographicToGauss(input);
            IPoint result = tr.Transform(input, Constants.enumProjection.WGS84_GEOGRAPHIC, Constants.enumProjection.BGS_1930_24, false);

            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformGaussToGeographic()
        {
            IPoint input = new TestPoint(4736629.503, 8613154.6069);
            IPoint expected = new TestPoint(42.7602978166667, 25.3824052611111);
            //IPoint result = tr.TransformGaussToGeographic(input);
            IPoint result = tr.Transform(input, Constants.enumProjection.BGS_1930_24, Constants.enumProjection.WGS84_GEOGRAPHIC, false);

            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
