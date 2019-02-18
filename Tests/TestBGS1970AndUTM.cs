using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BojkoSoft.Transformations.Constants;

namespace BojkoSoft.Transformations.Tests
{
    [TestClass()]
    public class TestBGS1970AndUTM
    {
        private Transformations tr = new Transformations();

        [TestMethod()]
        public void Transform1970toUTMTest()
        {
            // K3
            GeoPoint input = new GeoPoint(4738563.049, 8496424.783);
            GeoPoint expected = new GeoPoint(4832666.465, 192546.481);
            GeoPoint result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К3);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);

            // K5
            input = new GeoPoint(4601646.686, 9492261.737);
            expected = new GeoPoint(4665836.785, 444734.294);
            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К5);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);

            // K7
            input = new GeoPoint(4727661.403, 9563268.559);
            expected = new GeoPoint(4826823.258, 497499.587);
            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К7);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);

            // K9
            input = new GeoPoint(4577015.806, 8615896.123);
            expected = new GeoPoint(4702270.179, 314955.869);
            result = this.tr.Transform1970ToUTM(input, enumProjections.BGS_1970_К9);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);
        }

        [TestMethod()]
        public void TransformUTMto1970Test()
        {
            // K3
            GeoPoint input = new GeoPoint(4832666.465, 192546.481);
            GeoPoint expected = new GeoPoint(4738563.049, 8496424.783);
            GeoPoint result = this.tr.TransformUTMTo1970(input);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);

            // K5
            input = new GeoPoint(4665836.785, 444734.294);
            expected = new GeoPoint(4601646.686, 9492261.737);
            result = this.tr.TransformUTMTo1970(input);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);

            // K7
            input = new GeoPoint(4826823.258, 497499.587);
            expected = new GeoPoint(4727661.403, 9563268.559);
            result = this.tr.TransformUTMTo1970(input);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);

            // K9
            input = new GeoPoint(4702270.179, 314955.869);
            expected = new GeoPoint(4577015.806, 8615896.123);
            result = this.tr.TransformUTMTo1970(input);
            Common.CheckResults(expected, result, Common.DELTA_BGS1970);
        }
    }
}
