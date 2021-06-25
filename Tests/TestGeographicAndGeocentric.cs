using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndGeocentric
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations(false);
        }

        [TestMethod()]
        public void TransformGeographicToGeocentric()
        {
            IPoint input = new TestPoint(42.450682, 24.749747);
            IPoint expected = new TestPoint(4280410.654, 1973273.422, 4282674.061);
            IPoint result = tr.TransformGeographicToGeocentric(input);
            Common.CheckResults(expected, result, Common.DELTA_METERS, true);
        }

        [TestMethod()]
        public void TransformGeocentricToGeographic()
        {
            IPoint input = new TestPoint(4280410.654, 1973273.422, 4282674.061);
            IPoint expected = new TestPoint(42.450682, 24.749747);
            IPoint result = tr.TransformGeocentricToGeographic(input);
            Common.CheckResults(expected, result, Common.DELTA_DEGREES, true);
        }
    }
}
