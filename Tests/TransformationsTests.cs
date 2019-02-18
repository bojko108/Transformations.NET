using BojkoSoft.Transformations;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BojkoSoft.Transformations.Constants;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TransformationsTests
    {
        private Transformations tr;

        private double deltaInMeters = 0.2;             // 20cm - precision for 1970 is around 50cm
        private double deltaInDegrees = 0.0000001;

        public TransformationsTests()
        {
            this.tr = new Transformations();
        }

        [TestMethod()]
        public void TransformLambertToUTMTest()
        {
            GeoPoint input = new GeoPoint(4701972.999, 438286.035);
            GeoPoint expected = new GeoPoint(4702270.179, 314955.869);

            // go from Lambert to Geographic and then to UTM
            GeoPoint result = this.tr.TransformLambertProjectedToGeographic(input);
            result = this.tr.TransformGeographicToUTM(result);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "Northing is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "Easting is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void TransformGeographicToUTMTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(4702270.179, 314955.869);

            GeoPoint result = this.tr.TransformGeographicToUTM(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "Northing is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "Easting is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void TransformUTMToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4702270.179, 314955.869);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);

            GeoPoint result = this.tr.TransformUTMToGeographic(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInDegrees, "Latitude is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInDegrees, "Longitude is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void Transform1970toUTMTest()
        {
            // K3
            GeoPoint input = new GeoPoint(4738563.049, 8496424.783);
            GeoPoint expected = new GeoPoint(4832666.465, 192546.481);

            GeoPoint result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К3);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K3 Northing is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K3 Easting is not calculated correctly");

            Console.WriteLine(String.Format("K3 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));


            // K5
            input = new GeoPoint(4601646.686, 9492261.737);
            expected = new GeoPoint(4665836.785, 444734.294);

            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К5);

            deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K5 Northing is not calculated correctly");

            deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K5 Easting is not calculated correctly");

            Console.WriteLine(String.Format("K5 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));


            // K7
            input = new GeoPoint(4727661.403, 9563268.559);
            expected = new GeoPoint(4826823.258, 497499.587);

            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К7);

            deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K7 Northing is not calculated correctly");

            deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K7 Easting is not calculated correctly");

            Console.WriteLine(String.Format("K7 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));


            // K9
            input = new GeoPoint(4577015.806, 8615896.123);
            expected = new GeoPoint(4702270.179, 314955.869);

            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К9);

            deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K9 Northing is not calculated correctly");

            deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K9 Easting is not calculated correctly");

            Console.WriteLine(String.Format("K9 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));



        }

        [TestMethod()]
        public void TransformUTMto1970Test()
        {
            // K3
            GeoPoint input = new GeoPoint(4832666.465, 192546.481);
            GeoPoint expected = new GeoPoint(4738563.049, 8496424.783);

            GeoPoint result = this.tr.TransformUTMTo1970(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K3 X is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K3 Y is not calculated correctly");

            Console.WriteLine(String.Format("K3 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));


            // K5
            input = new GeoPoint(4665836.785, 444734.294);
            expected = new GeoPoint(4601646.686, 9492261.737);

            result = this.tr.TransformUTMTo1970(input);

            deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K5 X is not calculated correctly");

            deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K5 Y is not calculated correctly");

            Console.WriteLine(String.Format("K5 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));


            // K7
            input = new GeoPoint(4826823.258, 497499.587);
            expected = new GeoPoint(4727661.403, 9563268.559);

            result = this.tr.TransformUTMTo1970(input);

            deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K7 X is not calculated correctly");

            deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K7 Y is not calculated correctly");

            Console.WriteLine(String.Format("K7 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));


            // K9
            input = new GeoPoint(4702270.179, 314955.869);
            expected = new GeoPoint(4577015.806, 8615896.123);

            result = this.tr.TransformUTMTo1970(input);

            deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "K9 X is not calculated correctly");

            deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "K9 Y is not calculated correctly");

            Console.WriteLine(String.Format("K9 expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void TransformGeographicToLambertProjectedTest()
        {
            GeoPoint input = new GeoPoint(42.7589996, 25.3799991);
            GeoPoint expected = new GeoPoint(4735953.349, 490177.508);

            GeoPoint result = this.tr.TransformGeographicToLambertProjected(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "Northing is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "Easting is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void TransformLambertProjectedToGeographicTest()
        {
            GeoPoint input = new GeoPoint(4735953.349, 490177.508);
            GeoPoint expected = new GeoPoint(42.7589996, 25.3799991);

            GeoPoint result = this.tr.TransformLambertProjectedToGeographic(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInDegrees, "Latitude is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInDegrees, "Longitude is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void TransformGeographicToWebMercatorTest()
        {
            GeoPoint input = new GeoPoint(42.450682, 24.749747);
            GeoPoint expected = new GeoPoint(2755129.23, 5228730.33);

            GeoPoint result = this.tr.TransformGeographicToWebMercator(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInMeters, "X is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInMeters, "Y is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
        }

        [TestMethod()]
        public void TransformWebMercatorToGeographicTest()
        {
            GeoPoint input = new GeoPoint(2755129.23, 5228730.33);
            GeoPoint expected = new GeoPoint(42.450682, 24.749747);

            GeoPoint result = this.tr.TransformWebMercatorToGeographic(input);

            double deltaX = Math.Abs(expected.X - result.X);
            Assert.IsTrue(deltaX <= this.deltaInDegrees, "Latitude is not calculated correctly");

            double deltaY = Math.Abs(expected.Y - result.Y);
            Assert.IsTrue(deltaY <= this.deltaInDegrees, "Longitude is not calculated correctly");

            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}, deltaX: {2}, deltaY: {3}", expected.ToString(), result.ToString(), deltaX, deltaY));
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