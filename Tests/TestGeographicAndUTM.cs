using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndUTM
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void TransformGeographicToUTM()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(4702270.179, 314955.869);
            //GeoPoint result = this.tr.TransformGeographicToUTM(input);
            GeoPoint result = this.tr.Transform(input, Constants.enumProjection.WGS84_GEOGRAPHIC, Constants.enumProjection.UTM35N);

            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformUTMToGeographic()
        {
            GeoPoint input = new GeoPoint(4702270.179, 314955.869);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);
            //GeoPoint result = this.tr.TransformUTMToGeographic(input);
            GeoPoint result = this.tr.Transform(input, Constants.enumProjection.UTM35N, Constants.enumProjection.WGS84_GEOGRAPHIC);

            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
