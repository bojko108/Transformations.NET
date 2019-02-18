using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndGeocentric
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void TransformGeographicToGeocentricTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(4280410.654, 1973273.422, 4282674.061);
            GeoPoint result = this.tr.TransformGeographicToGeocentric(input);
            Common.CheckResults(expected, result, Common.DELTA_METERS, true);
        }

        [TestMethod()]
        public void TransformGeocentricToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4280410.654, 1973273.422, 4282674.061);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);
            GeoPoint result = this.tr.TransformGeocentricToGeographic(input);
            Common.CheckResults(expected, result, Common.DELTA_DEGREES, true);
        }
    }
}
