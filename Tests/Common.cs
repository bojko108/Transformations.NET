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
        public static readonly double DELTA_BGS_EXTENT = 1.0;       // 100cm
        public static readonly double DELTA_BGS = 0.1;              //  10cm
        public static readonly double DELTA_METERS = 0.01;          //   1cm
        public static readonly double DELTA_DEGREES = 0.0000001;    //   1cm

        public static void CheckResults(GeoPoint expected, GeoPoint result, double delta, bool checkZ = false)
        {
            double deltaX = Math.Abs(expected.X - result.X);
            double deltaY = Math.Abs(expected.Y - result.Y);
            double dS = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (checkZ)
            {
                double deltaZ = Math.Abs(expected.Z - result.Z);
                Assert.IsTrue(deltaZ <= delta, "dZ is too high: " + deltaZ);
                Console.WriteLine(String.Format("expected: {0}\nreceived: {1}\ndeltaX: {2}\ndeltaY: {3}\ndeltaZ: {4}", expected.ToString(), result.ToString(), deltaX, deltaY, deltaZ));
            }
            else
            {
                Assert.IsTrue(dS <= delta, "dS is too high: " + dS);
                Console.WriteLine(String.Format("expected: {0}; result: {1}\ndS: {2}", expected, result, dS));
            }
        }
    }
}
