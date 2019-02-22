using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BojkoSoft.Transformations.Tests
{
    public static class Common
    {
        public static readonly double DELTA_OLD_BGS = 0.2;          // 10cm
        public static readonly double DELTA_METERS = 0.01;          //  1cm
        public static readonly double DELTA_DEGREES = 0.0000001;    //  1cm

        public static void CheckResults(GeoPoint expected, GeoPoint result, double delta, bool checkZ = false)
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
    }
}
