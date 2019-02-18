using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndLambert
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void TransformGeographicToLambertProjectedTest()
        {
            GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
            GeoPoint expected = new GeoPoint(4735953.349, 490177.508);
            GeoPoint result = this.tr.TransformGeographicToLambert(input);
            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformLambertProjectedToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4735953.349, 490177.508);
            GeoPoint expected = new GeoPoint(42.7589996, 25.3799991);
            GeoPoint result = this.tr.TransformLambertToGeographic(input);
            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
