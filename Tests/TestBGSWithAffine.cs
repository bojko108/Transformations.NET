﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using BojkoSoft.Transformations.Constants;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestBGSWithAffine
    {
        private Transformations tr = new Transformations();

        #region BGS 1930

        [TestMethod()]
        public void TransformFromBGS1930()
        {
            // 24 degrees
            GeoPoint input = new GeoPoint(4728966.163, 8607005.227);
            GeoPoint expected = new GeoPoint(4728401.432, 483893.508);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1930_24, enumProjection.BGS_2005_KK, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_1930_24, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 27 degrees
            input = new GeoPoint(4729531.133, 9361175.733);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1930_27, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1930_27, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGS1930()
        {
            // 24 degrees
            GeoPoint input = new GeoPoint(4728401.432, 483893.508);
            GeoPoint expected = new GeoPoint(4728966.163, 8607005.227);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_24, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_24, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 27 degrees
            expected = new GeoPoint(4729531.133, 9361175.733);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_27, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_27, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS 1950

        [TestMethod()]
        public void TransformFromBGS1950()
        {
            // 3 deg 24 degrees
            GeoPoint input = new GeoPoint(4729331.175, 8606933.614);
            GeoPoint expected = new GeoPoint(4728401.432, 483893.508);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_3_24, enumProjection.BGS_2005_KK, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_1950_3_24, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 3 deg 27 degrees
            input = new GeoPoint(4729899.053, 9361082.960);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_3_27, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1950_3_27, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 21 degrees
            input = new GeoPoint(4737501.141, 4852808.182);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_6_21, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1950_6_21, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 27 degrees
            input = new GeoPoint(4729899.053, 5361082.960);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_6_27, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1950_6_27, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGS1950()
        {
            // 3 deg 24 degrees
            GeoPoint input = new GeoPoint(4728401.432, 483893.508);
            GeoPoint expected = new GeoPoint(4729331.175, 8606933.614);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_24, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_24, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 3 deg 27 degrees
            expected = new GeoPoint(4729899.053, 9361082.960);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_27, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_27, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 21 degrees
            expected = new GeoPoint(4737501.141, 4852808.182);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_21, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_21, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 27 degrees
            expected = new GeoPoint(4729899.053, 5361082.960);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_27, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_27, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS Sofia

        [TestMethod()]
        public void TransformFromBGSSofia()
        {
            GeoPoint input = new GeoPoint(48276.705, 45420.988);
            GeoPoint expected = new GeoPoint(4730215.229, 322402.935);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_SOFIA, enumProjection.BGS_2005_KK, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_SOFIA, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGSSofia()
        {
            GeoPoint input = new GeoPoint(4730215.229, 322402.935);
            GeoPoint expected = new GeoPoint(48276.705, 45420.988);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_SOFIA, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_SOFIA, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS 1970

        [TestMethod()]
        public void TransformFromBGS1970()
        {
            // K3
            GeoPoint input = new GeoPoint(4725270.684, 8515734.475);
            GeoPoint expected = new GeoPoint(4816275.680, 332535.401);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK, false);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            input = new GeoPoint(4687680.0628, 8557573.3751);
            expected = new GeoPoint(4777555.3496506, 373330.7458905);
            result = this.tr.Transform(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);


            // K5
            input = new GeoPoint(4613479.192, 9493233.633);
            expected = new GeoPoint(4679669.825, 569554.918);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K5, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1970_K5, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K7
            input = new GeoPoint(4708089.898, 9570974.988);
            expected = new GeoPoint(4810276.431, 626498.611);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K7, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1970_K7, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K9
            input = new GeoPoint(4547844.976, 8508858.179);
            expected = new GeoPoint(4675440.847, 330568.434);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK, false);
            result = this.tr.Transform(input, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGS1970()
        {
            // K3
            GeoPoint input = new GeoPoint(4816275.680, 332535.401);
            GeoPoint expected = new GeoPoint(4725270.684, 8515734.475);
            GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K3, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K5
            input = new GeoPoint(4679669.825, 569554.918);
            expected = new GeoPoint(4613479.192, 9493233.633);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K5, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K5, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K7
            input = new GeoPoint(4810276.431, 626498.611);
            expected = new GeoPoint(4708089.898, 9570974.988);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K7, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K7, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K9
            input = new GeoPoint(4675440.847, 330568.434);
            expected = new GeoPoint(4547844.976, 8508858.179);
            //result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9, false);
            result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS 2005

        // same as above tests

        [TestMethod()]
        public void PointCloseToBorder()
        {
            // k3 : k9
            GeoPoint input = new GeoPoint(4753610.10997237, 318581.541736281);
            GeoPoint expected = new GeoPoint(4625700.505, 8494949.232);
            //GeoPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9);
            GeoPoint result = this.tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion region
    }
}
