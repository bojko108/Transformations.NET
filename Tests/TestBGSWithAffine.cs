using Microsoft.VisualStudio.TestTools.UnitTesting;

using BojkoSoft.Transformations.Constants;
using Tests;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestBGSWithAffine
    {
        private static Transformations tr;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            tr = new Transformations();
        }

        #region BGS 1930

        [TestMethod()]
        public void TransformFromBGS1930()
        {
            // 24 degrees
            IPoint input = new TestPoint(4728966.163, 8607005.227);
            IPoint expected = new TestPoint(4728401.432, 483893.508);
            //IPoint result = this.tr.TransformBGSCoordinates(input, enumProjection.BGS_1930_24, enumProjection.BGS_2005_KK, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_1930_24, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 27 degrees
            input = new TestPoint(4729531.133, 9361175.733);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1930_27, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1930_27, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGS1930()
        {
            // 24 degrees
            IPoint input = new TestPoint(4728401.432, 483893.508);
            IPoint expected = new TestPoint(4728966.163, 8607005.227);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_24, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_24, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 27 degrees
            expected = new TestPoint(4729531.133, 9361175.733);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_27, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1930_27, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS 1950

        [TestMethod()]
        public void TransformFromBGS1950()
        {
            // 3 deg 24 degrees
            IPoint input = new TestPoint(4729331.175, 8606933.614);
            IPoint expected = new TestPoint(4728401.432, 483893.508);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_3_24, enumProjection.BGS_2005_KK, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_1950_3_24, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 3 deg 27 degrees
            input = new TestPoint(4729899.053, 9361082.960);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_3_27, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1950_3_27, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 21 degrees
            input = new TestPoint(4737501.141, 4852808.182);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_6_21, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1950_6_21, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 27 degrees
            input = new TestPoint(4729899.053, 5361082.960);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1950_6_27, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1950_6_27, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGS1950()
        {
            // 3 deg 24 degrees
            IPoint input = new TestPoint(4728401.432, 483893.508);
            IPoint expected = new TestPoint(4729331.175, 8606933.614);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_24, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_24, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 3 deg 27 degrees
            expected = new TestPoint(4729899.053, 9361082.960);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_27, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_3_27, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 21 degrees
            expected = new TestPoint(4737501.141, 4852808.182);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_21, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_21, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // 6 deg 27 degrees
            expected = new TestPoint(4729899.053, 5361082.960);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_27, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1950_6_27, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS Sofia

        [TestMethod()]
        public void TransformFromBGSSofia()
        {
            IPoint input = new TestPoint(48276.705, 45420.988);
            IPoint expected = new TestPoint(4730215.229, 322402.935);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_SOFIA, enumProjection.BGS_2005_KK, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_SOFIA, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGSSofia()
        {
            IPoint input = new TestPoint(4730215.229, 322402.935);
            IPoint expected = new TestPoint(48276.705, 45420.988);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_SOFIA, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_SOFIA, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS 1970

        [TestMethod()]
        public void TransformFromBGS1970()
        {
            // K3
            IPoint input = new TestPoint(4725270.684, 8515734.475);
            IPoint expected = new TestPoint(4816275.680, 332535.401);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK, false);
            IPoint result = tr.Transform(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            input = new TestPoint(4687680.0628, 8557573.3751);
            expected = new TestPoint(4777555.3496506, 373330.7458905);
            result = tr.Transform(input, enumProjection.BGS_1970_K3, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);


            // K5
            input = new TestPoint(4613479.192, 9493233.633);
            expected = new TestPoint(4679669.825, 569554.918);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K5, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1970_K5, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K7
            input = new TestPoint(4708089.898, 9570974.988);
            expected = new TestPoint(4810276.431, 626498.611);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K7, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1970_K7, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K9
            input = new TestPoint(4547844.976, 8508858.179);
            expected = new TestPoint(4675440.847, 330568.434);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK, false);
            result = tr.Transform(input, enumProjection.BGS_1970_K9, enumProjection.BGS_2005_KK, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        [TestMethod()]
        public void TransformToBGS1970()
        {
            // K3
            IPoint input = new TestPoint(4816275.680, 332535.401);
            IPoint expected = new TestPoint(4725270.684, 8515734.475);
            IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K3, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K5
            input = new TestPoint(4679669.825, 569554.918);
            expected = new TestPoint(4613479.192, 9493233.633);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K5, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K5, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K7
            input = new TestPoint(4810276.431, 626498.611);
            expected = new TestPoint(4708089.898, 9570974.988);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K7, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K7, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);

            // K9
            input = new TestPoint(4675440.847, 330568.434);
            expected = new TestPoint(4547844.976, 8508858.179);
            //result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9, false);
            result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion

        #region BGS 2005

        // same as above tests

        [TestMethod()]
        public void PointCloseToBorder()
        {
            // k3 : k9
            IPoint input = new TestPoint(4753610.10997237, 318581.541736281);
            IPoint expected = new TestPoint(4625700.505, 8494949.232);
            //IPoint result = tr.TransformBGSCoordinates(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9);
            IPoint result = tr.Transform(input, enumProjection.BGS_2005_KK, enumProjection.BGS_1970_K9, false);
            Common.CheckResults(expected, result, Common.DELTA_BGS);
        }

        #endregion region
    }
}
