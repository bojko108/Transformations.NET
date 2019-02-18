using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BojkoSoft.Transformations.Constants;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TransformationsTests
    {
        private Transformations tr;

        private readonly double deltaInMeters = 0.2;             // 20cm - precision for BGS 1970 is around 50cm
        private readonly double deltaInDegrees = 0.0000001;

        public TransformationsTests()
        {
            this.tr = new Transformations();
        }

        public void CheckResults(GeoPoint expected, GeoPoint result, double delta, bool checkZ = false)
        {
            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= delta, "Latitude, Northing or X is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= delta, "Longitude, Easting or Y is not calculated correctly");

            if (checkZ)
            {
                double deltaZ = Math.Abs(expected.Z - result.Z);
                Assert.IsTrue(deltaZ <= delta, "H, N or Z is not calculated correctly");
                Console.WriteLine(String.Format("expected: {0}\nreceived: {1}\ndeltaX: {2}\ndeltaY: {3}\ndeltaZ: {4}", expected.ToString(), result.ToString(), deltaX, deltaY, deltaZ));
            }
            else
            {
                Console.WriteLine(String.Format("expected: {0}\nreceived: {1}\ndeltaX: {2}\ndeltaY: {3}", expected.ToString(), result.ToString(), deltaX.ToString(), deltaY.ToString()));
            }
        }

        [TestMethod()]
        public void TransformLambertToUTMTest()
        {
            GeoPoint input = new GeoPoint(4701972.999, 438286.035);
            GeoPoint expected = new GeoPoint(4702270.179, 314955.869);

            // go from Lambert to Geographic and then to UTM
            GeoPoint result = this.tr.TransformLambertToGeographic(input);
            result = this.tr.TransformGeographicToUTM(result);

            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformGeographicToUTMTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(4702270.179, 314955.869);
            GeoPoint result = this.tr.TransformGeographicToUTM(input);

            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformUTMToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4702270.179, 314955.869);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);
            GeoPoint result = this.tr.TransformUTMToGeographic(input);

            this.CheckResults(expected, result, this.deltaInDegrees);
        }

        [TestMethod()]
        public void TransformGeographicToGaussTest()
        {
            GeoPoint input = new GeoPoint(42.7602978166667, 25.3824052611111);
            GeoPoint expected = new GeoPoint(4736629.503, 8613154.6069);
            GeoPoint result = this.tr.TransformGeographicToGauss(input);

            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformGaussToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4736629.503, 8613154.6069);
            GeoPoint expected = new GeoPoint(42.7602978166667, 25.3824052611111);
            GeoPoint result = this.tr.TransformGaussToGeographic(input);

            this.CheckResults(expected, result, this.deltaInDegrees);
        }

        [TestMethod()]
        public void Transform1970toUTMTest()
        {
            // K3
            GeoPoint input = new GeoPoint(4738563.049, 8496424.783);
            GeoPoint expected = new GeoPoint(4832666.465, 192546.481);
            GeoPoint result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К3);
            this.CheckResults(expected, result, this.deltaInMeters);

            // K5
            input = new GeoPoint(4601646.686, 9492261.737);
            expected = new GeoPoint(4665836.785, 444734.294);
            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К5);
            this.CheckResults(expected, result, this.deltaInMeters);

            // K7
            input = new GeoPoint(4727661.403, 9563268.559);
            expected = new GeoPoint(4826823.258, 497499.587);
            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К7);
            this.CheckResults(expected, result, this.deltaInMeters);

            // K9
            input = new GeoPoint(4577015.806, 8615896.123);
            expected = new GeoPoint(4702270.179, 314955.869);
            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К9);
            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformUTMto1970Test()
        {
            // K3
            GeoPoint input = new GeoPoint(4832666.465, 192546.481);
            GeoPoint expected = new GeoPoint(4738563.049, 8496424.783);
            GeoPoint result = this.tr.TransformUTMTo1970(input);
            this.CheckResults(expected, result, this.deltaInMeters);

            // K5
            input = new GeoPoint(4665836.785, 444734.294);
            expected = new GeoPoint(4601646.686, 9492261.737);
            result = this.tr.TransformUTMTo1970(input);
            this.CheckResults(expected, result, this.deltaInMeters);

            // K7
            input = new GeoPoint(4826823.258, 497499.587);
            expected = new GeoPoint(4727661.403, 9563268.559);
            result = this.tr.TransformUTMTo1970(input);
            this.CheckResults(expected, result, this.deltaInMeters);

            // K9
            input = new GeoPoint(4702270.179, 314955.869);
            expected = new GeoPoint(4577015.806, 8615896.123);
            result = this.tr.TransformUTMTo1970(input);
            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformGeographicToLambertProjectedTest()
        {
            GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
            GeoPoint expected = new GeoPoint(4735953.349, 490177.508);
            GeoPoint result = this.tr.TransformGeographicToLambert(input);
            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformLambertProjectedToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4735953.349, 490177.508);
            GeoPoint expected = new GeoPoint(42.7589996, 25.3799991);
            GeoPoint result = this.tr.TransformLambertToGeographic(input);
            this.CheckResults(expected, result, this.deltaInDegrees);
        }

        [TestMethod()]
        public void TransformGeographicToGeocentricTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(4280410.654, 1973273.422, 4282674.061);
            GeoPoint result = this.tr.TransformGeographicToGeocentric(input);
            this.CheckResults(expected, result, this.deltaInMeters, true);
        }

        [TestMethod()]
        public void TransformGeocentricToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4280410.654, 1973273.422, 4282674.061);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);
            GeoPoint result = this.tr.TransformGeocentricToGeographic(input);
            this.CheckResults(expected, result, this.deltaInMeters, true);
        }

        [TestMethod()]
        public void TransformGeographicToWebMercatorTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(2755129.23, 5228730.33);
            GeoPoint result = this.tr.TransformGeographicToWebMercator(input);
            this.CheckResults(expected, result, this.deltaInMeters);
        }

        [TestMethod()]
        public void TransformWebMercatorToGeographicTest()
        {
            GeoPoint input = new GeoPoint(2755129.23, 5228730.33);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);
            GeoPoint result = this.tr.TransformWebMercatorToGeographic(input);
            this.CheckResults(expected, result, this.deltaInDegrees);
        }

        [TestMethod()]
        public void ConvertDecimalDegreesToDMSTest()
        {
            double latitude = 42.336542;
            string expected = "422011.5512000000052";

            string result = tr.ConvertDecimalDegreesToDMS(latitude);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}", expected.ToString(), result.ToString()));
        }

        [TestMethod()]
        public void ConvertDMStoDecimalDegreesTest()
        {
            string dms = "422011.5512000000052";
            double expected = 42.336542;

            double result = tr.ConvertDMStoDecimalDegrees(dms);

            Assert.AreEqual(expected, result);

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}", expected.ToString(), result.ToString()));
        }
    }
}