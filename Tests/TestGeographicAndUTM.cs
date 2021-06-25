using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndUTM
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations(false);
        }

        [TestMethod()]
        public void TransformGeographicToUTM()
        {
            IPoint input = new TestPoint(42.450682, 24.749747);
            IPoint expected = new TestPoint(4702270.179, 314955.869);
            //IPoint result = this.tr.TransformGeographicToUTM(input);
            IPoint result = tr.Transform(input, Constants.enumProjection.WGS84_GEOGRAPHIC, Constants.enumProjection.UTM35N);

            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformUTMToGeographic()
        {
            IPoint input = new TestPoint(4702270.179, 314955.869);
            IPoint expected = new TestPoint(42.450682, 24.749747);
            //IPoint result = this.tr.TransformUTMToGeographic(input);
            IPoint result = tr.Transform(input, Constants.enumProjection.UTM35N, Constants.enumProjection.WGS84_GEOGRAPHIC);

            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
