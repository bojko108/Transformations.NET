using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;
using BojkoSoft.Transformations.Constants;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TransformCoordinatesTest
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations();
        }

        [TestMethod()]
        public void TestTransformArrayOf2()
        {
            double[] input = new double[] { 4547844.976, 8508858.179 };
            double[] expected = new double[] { 42.195768710251066, 23.448408452380647 };

            double[] result = tr.Transform(input, enumProjection.BGS_1970_K9, enumProjection.WGS84_GEOGRAPHIC);

            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TestTransformArrayOf3()
        {
            double[] input = new double[] { 4547844.976, 8508858.179, 234 };
            double[] expected = new double[] { 42.195768710251066, 23.448408452380647, 234 };

            double[] result = tr.Transform(input, enumProjection.BGS_1970_K9, enumProjection.WGS84_GEOGRAPHIC);

            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TestTransformTupleOf2()
        {
            Tuple<double, double> input = new Tuple<double, double>(4547844.976, 8508858.179);
            Tuple<double, double> expected = new Tuple<double, double>(42.195768710251066, 23.448408452380647);

            Tuple<double, double> result = tr.Transform(input, enumProjection.BGS_1970_K9, enumProjection.WGS84_GEOGRAPHIC);

            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TestTransformTupleOf3()
        {
            Tuple<double, double, double> input = new Tuple<double, double, double>(4547844.976, 8508858.179, 234);
            Tuple<double, double, double> expected = new Tuple<double, double, double>(42.195768710251066, 23.448408452380647, 234);

            Tuple<double, double, double> result = tr.Transform(input, enumProjection.BGS_1970_K9, enumProjection.WGS84_GEOGRAPHIC);

            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }
    }
}