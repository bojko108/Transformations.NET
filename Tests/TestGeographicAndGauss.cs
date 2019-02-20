using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndGauss
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void TransformGeographicToGauss()
        {
            GeoPoint input = new GeoPoint(42.7602978166667, 25.3824052611111);
            GeoPoint expected = new GeoPoint(4736629.503, 8613154.6069);
            GeoPoint result = this.tr.TransformGeographicToGauss(input);

            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformGaussToGeographic()
        {
            GeoPoint input = new GeoPoint(4736629.503, 8613154.6069);
            GeoPoint expected = new GeoPoint(42.7602978166667, 25.3824052611111);
            GeoPoint result = this.tr.TransformGaussToGeographic(input);

            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
