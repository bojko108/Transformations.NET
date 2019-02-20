using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestGeographicAndWebMercator
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void TransformGeographicToWebMercatorTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(2755129.23, 5228730.33);
            GeoPoint result = this.tr.TransformGeographicToWebMercator(input);

            Common.CheckResults(expected, result, Common.DELTA_METERS);
        }

        [TestMethod()]
        public void TransformWebMercatorToGeographicTest()
        {
            GeoPoint input = new GeoPoint(2755129.23, 5228730.33);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);
            GeoPoint result = this.tr.TransformWebMercatorToGeographic(input);

            Common.CheckResults(expected, result, Common.DELTA_DEGREES);
        }
    }
}
