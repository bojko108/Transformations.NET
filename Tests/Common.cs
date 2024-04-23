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

        public static void CheckResults(IPoint expected, IPoint result, double delta, bool checkZ = false)
        {
            double deltaX = Math.Abs(expected.N - result.N);
            double deltaY = Math.Abs(expected.E - result.E);
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

        public static void CheckResults(double[] expected, double[] result, double delta)
        {
            double deltaX = Math.Abs(expected[0] - result[0]);
            double deltaY = Math.Abs(expected[1] - result[1]);
            double dS = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (expected.Length == 3)
            {
                double deltaZ = Math.Abs(expected[2] - result[2]);
                Assert.IsTrue(deltaZ <= delta, "dZ is too high: " + deltaZ);
                Console.WriteLine(String.Format("expected: {0}\nreceived: {1}\ndeltaX: {2}\ndeltaY: {3}\ndeltaZ: {4}", expected.ToString(), result.ToString(), deltaX, deltaY, deltaZ));
            }
            else
            {
                Assert.IsTrue(dS <= delta, "dS is too high: " + dS);
                Console.WriteLine(String.Format("expected: {0}; result: {1}\ndS: {2}", expected, result, dS));
            }
        }

        public static void CheckResults(Tuple<double, double> expected, Tuple<double, double> result, double delta)
        {
            double deltaX = Math.Abs(expected.Item1 - result.Item1);
            double deltaY = Math.Abs(expected.Item2 - result.Item2);
            double dS = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            Assert.IsTrue(dS <= delta, "dS is too high: " + dS);
            Console.WriteLine(String.Format("expected: {0}; result: {1}\ndS: {2}", expected, result, dS));
        }

        public static void CheckResults(Tuple<double, double, double> expected, Tuple<double, double, double> result, double delta)
        {
            double deltaX = Math.Abs(expected.Item1 - result.Item1);
            double deltaY = Math.Abs(expected.Item2 - result.Item2);
            double dS = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            Assert.IsTrue(dS <= delta, "dS is too high: " + dS);

            double deltaZ = Math.Abs(expected.Item3 - result.Item3);
            Assert.IsTrue(deltaZ <= delta, "dZ is too high: " + deltaZ);
            Console.WriteLine(String.Format("expected: {0}\nreceived: {1}\ndeltaX: {2}\ndeltaY: {3}\ndeltaZ: {4}", expected.ToString(), result.ToString(), deltaX, deltaY, deltaZ));
        }
    } 
}