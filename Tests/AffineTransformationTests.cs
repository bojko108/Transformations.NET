using System;
using System.Collections.Generic;
using System.Linq;
using BojkoSoft.Transformations.Constants;
using BojkoSoft.Transformations.TransformationModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass]
    public class AffineTransformationTests
    {
        [TestMethod]
        public void CreatesNewAffineTransformation()
        {
            AffineTransformation affine = new AffineTransformation();

            Assert.AreEqual(affine.A, 1);
            Assert.AreEqual(affine.E, 1);
            Assert.AreEqual(affine.B, 1);
            Assert.AreEqual(affine.D, 1);
            Assert.AreEqual(affine.C, 0);
            Assert.AreEqual(affine.F, 0);
        }

        [TestMethod]
        public void SetParameters()
        {
            double[] parameters = new double[6] { 0.9996709948388145, 0.99966986731795959, -0.024615185435017371, 0.024613381723631412, 338539.26990495855, -8287418.5593307121 };
            AffineTransformation affine = new AffineTransformation(parameters);

            Assert.AreEqual(affine.A, parameters[0]);
            Assert.AreEqual(affine.E, parameters[1]);
            Assert.AreEqual(affine.B, parameters[2]);
            Assert.AreEqual(affine.D, parameters[3]);
            Assert.AreEqual(affine.C, parameters[4]);
            Assert.AreEqual(affine.F, parameters[5]);
        }

        [TestMethod]
        public void SetParametersFromWorldFile()
        {
            string worldFile = $"0.9996709948388145\n0.99966986731795959\n-0.024615185435017371\n0.024613381723631412\n338539.26990495855\n-8287418.5593307121";
            AffineTransformation affine = new AffineTransformation();
            affine.SetParameters(worldFile);

            double[] parameters = Array.ConvertAll(worldFile.Split('\n'), Double.Parse);
            Assert.AreEqual(affine.A, parameters[0]);
            Assert.AreEqual(affine.E, parameters[1]);
            Assert.AreEqual(affine.B, parameters[2]);
            Assert.AreEqual(affine.D, parameters[3]);
            Assert.AreEqual(affine.C, parameters[4]);
            Assert.AreEqual(affine.F, parameters[5]);
        }

        [TestMethod]
        public void TransformPointWithProvidedParameters()
        {
            double[] parameters = new double[6] { 0.9996709948388145, 0.99966986731795959, -0.024615185435017371, 0.024613381723631412, 338539.26990495855, -8287418.5593307121 };
            AffineTransformation affine = new AffineTransformation(parameters);
            Transformations tr = new Transformations();

            // K9
            IPoint input = new TestPoint(4547844.976, 8508858.179);
            IPoint expected = new TestPoint(4675440.847, 330568.434);

            IPoint result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod]
        public void CalculateParametersForExtent()
        {
            IExtent extent = new TestExtent(4590706, 4556298, 8561889, 8519105);
            extent.Expand(20000);
            Transformations tr = new Transformations();
            double[] parameters = tr.CalculateAffineTransformationParameters(extent, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);

            // K9
            IPoint input = new TestPoint(4573488, 8539465);
            IPoint expected = new TestPoint(4700322.190, 361795.526);

            IPoint result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);

            Common.CheckResults(expected, result, Common.DELTA_BGS_EXTENT);


            input = new TestPoint(4557529, 8530750);
            expected = new TestPoint(4684583.019, 352691.179);

            result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_BGS_EXTENT);


            input = new TestPoint(4589108, 8551915);
            expected = new TestPoint(4715630.512, 374624.861);

            result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_BGS_EXTENT);


            input = new TestPoint(4573488, 8517394);
            expected = new TestPoint(4700865.033, 339732.391);

            result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_BGS_EXTENT);


            input = new TestPoint(4574394, 8583155);
            expected = new TestPoint(4700154.937, 405492.239);

            result = tr.TransformBGSCoordinates(input, parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
            Common.CheckResults(expected, result, Common.DELTA_BGS_EXTENT);
        }
        
        [TestMethod]
        public void BatchTransformWithParameters()
        {
            IExtent extent = new TestExtent(4590706, 4556298, 8561889, 8519105);
            extent.Expand(20000);
            Transformations tr = new Transformations();
            double[] parameters = tr.CalculateAffineTransformationParameters(extent, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);

            int count = 100000;
            IPoint[] input = new TestPoint[count];
            IPoint[] expected = new TestPoint[count];
            for (int i = 0; i < count; i++)
            {
                input[i] = new TestPoint(4573488, 8539465);
                expected[i] = new TestPoint(4700322.190, 361795.526);
            }

            // check results
            for (int i = 0; i < count; i++)
            {
                IPoint result = tr.TransformBGSCoordinates(input[i], parameters, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK);
                Common.CheckResults(expected[i], result, Common.DELTA_BGS_EXTENT);
            }
        }

        [TestMethod]
        public void BatchTransformWithoutParameters()
        {
            Transformations tr = new Transformations();

            int count = 100000;
            IPoint[] input = new TestPoint[count];
            IPoint[] expected = new TestPoint[count];
            for (int i = 0; i < count; i++)
            {
                input[i] = new TestPoint(4573488, 8539465);
                expected[i] = new TestPoint(4700322.190, 361795.526);
            }

            // check results
            for (int i = 0; i < count; i++)
            {
                IPoint result = tr.TransformBGSCoordinates(input[i], enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK, false);
                Common.CheckResults(expected[i], result, Common.DELTA_BGS_EXTENT);
            }
        }
    }
}