using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndLambert
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations(false);
        }

        [TestMethod()]
        public void TransformGeographicToLambertProjected()
        {
            IPoint input = new TestPoint(42.7589996, 25.3799991);
            IPoint expected = new TestPoint(4735953.349, 490177.508);
            //IPoint result = tr.TransformGeographicToLambert(input);
            IPoint result = tr.Transform(input,Constants.enumProjection.WGS84_GEOGRAPHIC,Constants.enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformLambertProjectedToGeographic()
        {
            IPoint input = new TestPoint(4735953.349, 490177.508);
            IPoint expected = new TestPoint(42.7589996, 25.3799991);
            //IPoint result = tr.TransformLambertToGeographic(input);
            IPoint result = tr.Transform(input, Constants.enumProjection.BGS_2005_KK, Constants.enumProjection.WGS84_GEOGRAPHIC);
            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
