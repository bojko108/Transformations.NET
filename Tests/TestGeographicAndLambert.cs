using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndLambert
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void TransformGeographicToLambertProjected()
        {
            GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
            GeoPoint expected = new GeoPoint(4735953.349, 490177.508);
            //GeoPoint result = this.tr.TransformGeographicToLambert(input);
            GeoPoint result = this.tr.Transform(input,Constants.enumProjection.WGS84_GEOGRAPHIC,Constants.enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformLambertProjectedToGeographic()
        {
            GeoPoint input = new GeoPoint(4735953.349, 490177.508);
            GeoPoint expected = new GeoPoint(42.7589996, 25.3799991);
            //GeoPoint result = this.tr.TransformLambertToGeographic(input);
            GeoPoint result = this.tr.Transform(input, Constants.enumProjection.BGS_2005_KK, Constants.enumProjection.WGS84_GEOGRAPHIC);
            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
