/**
 * author: bojko108 (bojko108@gmail.com)
 */

using System;
using System.Collections.Generic;

using BojkoSoft.Transformations.Constants;

namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Transform points between different coordinate systems
    /// </summary>
    public class Transformations
    {
        private Ellipsoids ellipsoids;
        private Projections projections;

        /// <summary>
        /// Transform points between different coordinate systems.
        /// </summary>
        /// <param name="useControlPoints">Wheather or not the control points for transforming between KC1970 and UTM will be initialized.</param>
        public Transformations(bool useControlPoints = true)
        {
            // control points are needed when transforming from and to KC 1970
            if (useControlPoints && ControlPoints.ControlPoints.areControlPointsLoaded == false)
            {
                ControlPoints.ControlPoints.LoadControlPoints();
            }

            this.ellipsoids = new Ellipsoids();
            this.projections = new Projections();
        }


        #region GEOGRAPHIC AND PROJECTED

        /// <summary>
        /// Transforms geographic coordinates (Latitude, Longitude - EPSG:4326) to projected - Northing, Easting (UTM).
        /// </summary>
        /// <param name="inputPoint">Input point</param>
        /// <param name="targetUtmProjection">Target UTM projection</param>
        /// <param name="inputEllipsoid">Input point ellipsod</param>
        /// <returns></returns>
        public GeoPoint TransformGeographicToUTM(GeoPoint inputPoint, enumProjections targetUtmProjection = enumProjections.UTM35N, enumEllipsoids inputEllipsoid = enumEllipsoids.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            inputPoint.X = (inputPoint.X * Math.PI) / 180;
            inputPoint.Y = (inputPoint.Y * Math.PI) / 180;

            Projection targetProjection = this.projections[targetUtmProjection];
            Ellipsoid sourceEllipsoid = this.ellipsoids[inputEllipsoid];

            double n, nu2, t, t2, l,
                coef13, coef14, coef15, coef16,
                coef17, coef18, cf;
            double phi;

            phi = Helpers.ArcLengthOfMeridian(inputPoint.X, sourceEllipsoid.a, sourceEllipsoid.b);
            cf = Math.Cos(inputPoint.X);
            nu2 = sourceEllipsoid.ep2 * Math.Pow(Math.Cos(inputPoint.X), 2.0);
            n = Math.Pow(sourceEllipsoid.a, 2.0) / (sourceEllipsoid.b * Math.Sqrt(1 + nu2));
            t = Math.Tan(inputPoint.X);
            t2 = t * t;

            l = inputPoint.Y - (targetProjection.Lon0 * Math.PI) / 180;

            coef13 = 1.0 - t2 + nu2;
            coef14 = 5.0 - t2 + 9 * nu2 + 4.0 * (nu2 * nu2);
            coef15 = 5.0 - 18.0 * t2 + (t2 * t2) + 14.0 * nu2 - 58.0 * t2 * nu2;
            coef16 = 61.0 - 58.0 * t2 + (t2 * t2) + 270.0 * nu2 - 330.0 * t2 * nu2;
            coef17 = 61.0 - 479.0 * t2 + 179.0 * (t2 * t2) - (t2 * t2 * t2);
            coef18 = 1385.0 - 3111.0 * t2 + 543.0 * (t2 * t2) - (t2 * t2 * t2);

            resultPoint.Y = n * Math.Cos(inputPoint.X) * l
                + (n / 6.0 * Math.Pow(Math.Cos(inputPoint.X), 3.0) * coef13 * Math.Pow(l, 3.0))
                + (n / 120.0 * Math.Pow(Math.Cos(inputPoint.X), 5.0) * coef15 * Math.Pow(l, 5.0))
                + (n / 5040.0 * Math.Pow(Math.Cos(inputPoint.X), 7.0) * coef17 * Math.Pow(l, 7.0));

            resultPoint.X = phi
                + (t / 2.0 * n * Math.Pow(Math.Cos(inputPoint.X), 2.0) * Math.Pow(l, 2.0))
                + (t / 24.0 * n * Math.Pow(Math.Cos(inputPoint.X), 4.0) * coef14 * Math.Pow(l, 4.0))
                + (t / 720.0 * n * Math.Pow(Math.Cos(inputPoint.X), 6.0) * coef16 * Math.Pow(l, 6.0))
                + (t / 40320.0 * n * Math.Pow(Math.Cos(inputPoint.X), 8.0) * coef18 * Math.Pow(l, 8.0));

            resultPoint.X *= targetProjection.Scale;
            resultPoint.Y *= targetProjection.Scale;
            resultPoint.Y += targetProjection.Y0;

            return resultPoint;
        }

        /// <summary>
        /// Transforms projected coordinates (Northing, Easting - UTM) to geographic - Latitude, Longitude (EPSG:4326).
        /// </summary>
        /// <param name="inputPoint">Input point</param>
        /// <param name="sourceUtmProjection">Input point projection</param>
        /// <param name="outputEllipsoid">Output ellipsoid</param>
        /// <returns></returns>
        public GeoPoint TransformUTMToGeographic(GeoPoint inputPoint, enumProjections sourceUtmProjection = enumProjections.UTM35N, enumEllipsoids outputEllipsoid = enumEllipsoids.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Projection sourceProjection = this.projections[sourceUtmProjection];
            Ellipsoid targetEllipsoid = this.ellipsoids[outputEllipsoid];

            inputPoint.Y -= sourceProjection.Y0;
            inputPoint.Y /= sourceProjection.Scale;
            inputPoint.X /= sourceProjection.Scale;

            double phif, Nf, Nfpow, nuf2, tf, tf2, tf4, cf, x1frac, x2frac,
                x3frac, x4frac, x5frac, x6frac, x7frac, x8frac, x2poly,
                x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            phif = Helpers.FootpointLatitude(inputPoint.X, targetEllipsoid.a, targetEllipsoid.b);

            cf = Math.Cos(phif);
            nuf2 = targetEllipsoid.ep2 * Math.Pow(cf, 2.0);
            Nf = Math.Pow(targetEllipsoid.a, 2.0) / (targetEllipsoid.b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;
            x1frac = 1.0 / (Nfpow * cf);
            Nfpow *= Nf;
            x2frac = tf / (2.0 * Nfpow);
            Nfpow *= Nf;
            x3frac = 1.0 / (6.0 * Nfpow * cf);
            Nfpow *= Nf;
            x4frac = tf / (24.0 * Nfpow);
            Nfpow *= Nf;
            x5frac = 1.0 / (120.0 * Nfpow * cf);
            Nfpow *= Nf;
            x6frac = tf / (720.0 * Nfpow);
            Nfpow *= Nf;
            x7frac = 1.0 / (5040.0 * Nfpow * cf);
            Nfpow *= Nf;
            x8frac = tf / (40320.0 * Nfpow);

            x2poly = -1 - nuf2;
            x3poly = -1 - 2 * tf2 - nuf2;
            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2 - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);
            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;
            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2 + 162.0 * tf2 * nuf2;
            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);
            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            resultPoint.X = phif
                + x2frac * x2poly * (Math.Pow(inputPoint.Y, 2))
                + x4frac * x4poly * Math.Pow(inputPoint.Y, 4.0)
                + x6frac * x6poly * Math.Pow(inputPoint.Y, 6.0)
                + x8frac * x8poly * Math.Pow(inputPoint.Y, 8.0);

            resultPoint.Y = ((sourceProjection.Lon0 * Math.PI) / 180)
                + x1frac * inputPoint.Y
                + x3frac * x3poly * Math.Pow(inputPoint.Y, 3.0)
                + x5frac * x5poly * Math.Pow(inputPoint.Y, 5.0)
                + x7frac * x7poly * Math.Pow(inputPoint.Y, 7.0);

            resultPoint.X = resultPoint.X / Math.PI * 180;
            resultPoint.Y = resultPoint.Y / Math.PI * 180;

            return resultPoint;
        }

        #endregion


        #region 1970 AND UTM

        /// <summary>
        /// Transform between KC1970 and UTM Projection (WGS84)
        /// </summary>
        /// <param name="inputPoint">X coordinate in KC1970</param>
        /// <param name="source1970Projection">Input coordinates are in that zone (KC1970)</param>
        /// <returns></returns>
        public GeoPoint Transform1970ToUTM(GeoPoint inputPoint, enumProjections source1970Projection = enumProjections.BGS_1970_К9)
        {
            GeoPoint resultPoint = new GeoPoint();

            double a1 = 0, b1 = 0, a2 = 0, b2 = 0, c1 = 0, c2 = 0;
            string proba, klist;

            string[] lol;

            int zone;
            switch (source1970Projection)
            {
                case enumProjections.BGS_1970_К3: zone = 3; break;
                case enumProjections.BGS_1970_К5: zone = 5; break;
                case enumProjections.BGS_1970_К7: zone = 7; break;
                case enumProjections.BGS_1970_К9: zone = 9; break;
                default: zone = 0; break;
            }

            proba = this.MapList1970Name(zone, inputPoint);

            List<double> VertexX = new List<double>();
            List<double> VertexY = new List<double>();
            List<double> n = new List<double>();
            List<double> e = new List<double>();

            if (this.MapList1970XY(proba, out VertexX, out VertexY) == true)
            {
                if (ControlPoints.ControlPoints.dicMapList.ContainsKey(proba))
                {
                    klist = ControlPoints.ControlPoints.dicMapList[proba];
                    klist = klist.Trim();

                    lol = klist.Split(' ');

                    n.Add(double.Parse(lol[0])); e.Add(double.Parse(lol[1]));
                    n.Add(double.Parse(lol[2])); e.Add(double.Parse(lol[3]));
                    n.Add(double.Parse(lol[4])); e.Add(double.Parse(lol[5]));
                    n.Add(double.Parse(lol[6])); e.Add(double.Parse(lol[7]));

                    Helpers.GetAffineTransformationParameters(4, n, e, VertexX, VertexY, out a1, out a2, out b1, out b2, out c1, out c2);

                    resultPoint.X = a1 * inputPoint.X + b1 * inputPoint.Y + c1;
                    resultPoint.Y = b2 * inputPoint.X + a2 * inputPoint.Y + c2;
                }
                else
                {
                    resultPoint.X = inputPoint.X;
                    resultPoint.Y = inputPoint.Y;
                }
            }
            else
            {
                resultPoint.X = inputPoint.X;
                resultPoint.Y = inputPoint.Y;
            }

            return resultPoint;
        }

        /// <summary>
        /// Transform between UTM (WGS84) and KC1970. There is no need of passing UTM or KC1970 zone numbers as 
        /// the transformation is done by calculating local transformation parameters.
        /// </summary>
        /// <param name="inputPoint">Northing coordinate in UTM Projection</param>
        /// <returns></returns>
        public GeoPoint TransformUTMTo1970(GeoPoint inputPoint)
        {
            GeoPoint resultPoint = new GeoPoint();

            double a1 = 0, b1 = 0, a2 = 0, b2 = 0, c1 = 0, c2 = 0;

            string klist = "", proba;
            string[] lol;

            proba = ControlPoints.ControlPoints.GetUTMListNumber(inputPoint);

            List<double> VertexX = new List<double>();
            List<double> VertexY = new List<double>();
            List<double> n = new List<double>();
            List<double> e = new List<double>();

            if (this.MapList1970XY(proba, out VertexX, out VertexY) == true)
            {
                klist = ControlPoints.ControlPoints.dicMapList[proba];
                klist = klist.Trim();
            }

            lol = klist.Split(' ');

            n.Add(double.Parse(lol[0])); e.Add(double.Parse(lol[1]));
            n.Add(double.Parse(lol[2])); e.Add(double.Parse(lol[3]));
            n.Add(double.Parse(lol[4])); e.Add(double.Parse(lol[5]));
            n.Add(double.Parse(lol[6])); e.Add(double.Parse(lol[7]));

            Helpers.GetAffineTransformationParameters(4, VertexX, VertexY, n, e, out a1, out a2, out b1, out b2, out c1, out c2);

            resultPoint.X = a1 * inputPoint.X + b1 * inputPoint.Y + c1;
            resultPoint.Y = b2 * inputPoint.X + a2 * inputPoint.Y + c2;

            return resultPoint;
        }

        /// <summary>
        /// Calculates coordinates of a map sheet in 1970 (M1:5000)
        /// </summary>
        /// <param name="numberList"></param>
        /// <param name="VertexX"></param>
        /// <param name="VertexY"></param>
        /// <returns></returns>
        private bool MapList1970XY(string numberList, out List<double> VertexX, out List<double> VertexY)
        {
            VertexX = new List<double>();
            VertexY = new List<double>();

            bool sheet;
            string L1 = "", L2 = "", L3 = "", L4 = "", L5 = "", L6 = "";
            string[] ClassList;
            int i = 0, j = 0, zone = 0, class2 = 0, class3 = 0;
            int X0 = 0, Y0 = 0;
            double x1 = 0, y1 = 0, x = 0, y = 0;
            double vertexN = 0, vertexW = 0;

            if (numberList.Length < 3) return false;

            ClassList = numberList.Split('-');

            if (ClassList.Length > 0) L1 = ClassList[0];
            if (ClassList.Length > 1) L2 = ClassList[1];
            if (ClassList.Length > 2) L3 = ClassList[2];
            if (ClassList.Length > 3) L4 = ClassList[3];
            if (ClassList.Length > 4) L5 = ClassList[4];
            if (ClassList.Length > 5) L6 = ClassList[5];
            if (L2.Length < 1) return false;

            zone = int.Parse(L2);


            switch (zone)
            {
                case 3: X0 = 4880000; Y0 = 8340000; break;
                case 5: X0 = 4740000; Y0 = 9340000; break;
                case 7: X0 = 4860000; Y0 = 9360000; break;
                case 9: X0 = 4720000; Y0 = 8320000; break;
                default: return false;
            }

            vertexN = X0;
            vertexW = Y0;

            if (L3.Length < 1) return false;
            sheet = false;
            class2 = int.Parse(L3);
            for (i = 0; i <= 9; i++)
            {
                x1 = X0 - i * 40000;
                for (j = 0; j <= 9; j++)
                {
                    y1 = Y0 + j * 40000;
                    if (class2 == i * 10 + j)
                    {
                        sheet = true;
                        break;
                    }
                }
                if (sheet == true) break;
            }
            vertexN = x1;
            vertexW = y1;
            if (L4.Length < 1) return false;
            sheet = false;
            class3 = int.Parse(L4);
            for (i = 0; i <= 15; i++)
            {
                x = x1 - i * 2500;
                for (j = 0; j <= 15; j++)
                {
                    y = y1 + j * 2500;
                    if (class3 == i * 16 + j + 1)
                    {
                        sheet = true;
                        break;
                    }
                }
                if (sheet == true) break;
            }

            if (sheet == false) return false;

            vertexN = x;
            vertexW = y;

            VertexX.Add(vertexN);          // 0 -> 1
            VertexY.Add(vertexW);          // 0 -> 1
            VertexX.Add(vertexN);          // 1 -> 2
            VertexY.Add(vertexW + 2500);   // 1 -> 2
            VertexX.Add(vertexN - 2500);   // 2 -> 3
            VertexY.Add(vertexW + 2500);   // 2 -> 3
            VertexX.Add(vertexN - 2500);   // 3 -> 4
            VertexY.Add(vertexW);          // 3 -> 4

            return true;
        }

        /// <summary>
        /// Calculates map sheet number in 1970 (M1:5000)
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private string MapList1970Name(int zone, GeoPoint point)
        {
            int i, j;
            bool sheet = false;
            long X0, Y0;
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            int zone1 = 0, zone2 = 0;

            switch (zone)
            {
                case 3: X0 = 4880000; Y0 = 8340000; break;
                case 5: X0 = 4740000; Y0 = 9340000; break;
                case 7: X0 = 4860000; Y0 = 9360000; break;
                case 9: X0 = 4720000; Y0 = 8320000; break;
                default: return "";
            }

            sheet = false;
            for (i = 0; i <= 9; i++)
            {
                x1 = X0 - i * 40000;
                for (j = 0; j <= 9; j++)
                {
                    y1 = Y0 + j * 40000;
                    if (x1 > point.X && y1 < point.Y && (x1 - 40000) < point.X && (y1 + 40000) > point.Y)
                    {
                        zone1 = i * 10 + j;
                        sheet = true;
                        break;
                    }
                }
                if (sheet == true) break;
            }

            sheet = false;
            for (i = 0; i <= 15; i++)
            {
                x2 = x1 - i * 2500;
                for (j = 0; j <= 15; j++)
                {
                    y2 = y1 + j * 2500;
                    if (x2 > point.X && y2 < point.Y && (x2 - 2500) < point.X && (y2 + 2500) > point.Y)
                    {
                        zone2 = i * 16 + j + 1;
                        sheet = true;
                        break;
                    }
                }
                if (sheet == true) break;
            }

            if (sheet == false) return "Точката не е в зона K-" + zone;
            if (zone2 < 10) return "K-" + zone.ToString() + "-" + zone1.ToString() + "-00" + zone2.ToString();
            if (zone2 < 100 && zone2 >= 10) { return "K-" + zone.ToString() + "-" + zone1.ToString() + "-0" + zone2.ToString(); }
            else { return "K-" + zone.ToString() + "-" + zone1.ToString() + "-" + zone2.ToString(); }
        }

        #endregion


        #region KKC 2005 AND GEOGRAPHIC

        /// <summary>
        /// Transforms projected coordinates (Northing, Easting - Lambert Conformal Conic Projection 2SP) to geographic - Latitude, Longitude (EPSG:4326).
        /// </summary>
        /// <param name="inputPoint">Input coordinates</param>
        /// <param name="inputProjection">Input coordinates</param>
        /// <param name="outputEllipsoid">Input coordinates</param>
        /// <returns></returns>
        public GeoPoint TransformLambertProjectedToGeographic(GeoPoint inputPoint, enumProjections inputProjection = enumProjections.BGS_2005_KK, enumEllipsoids outputEllipsoid = enumEllipsoids.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Projection sourceProjection = this.projections[inputProjection];
            Ellipsoid targetEllipsoid = this.ellipsoids[outputEllipsoid];

            double Lon0 = (sourceProjection.Lon0 * Math.PI) / 180,
                Lat1 = (sourceProjection.Lat1 * Math.PI) / 180,
                Lat2 = (sourceProjection.Lat2 * Math.PI) / 180,
                w1 = Helpers.CalculateWParameter((sourceProjection.Lat1 * Math.PI) / 180, targetEllipsoid.e2),
                w2 = Helpers.CalculateWParameter((sourceProjection.Lat2 * Math.PI) / 180, targetEllipsoid.e2),
                Q1 = Helpers.CalculateQParameter((sourceProjection.Lat1 * Math.PI) / 180, targetEllipsoid.e),
                Q2 = Helpers.CalculateQParameter((sourceProjection.Lat2 * Math.PI) / 180, targetEllipsoid.e),
                Lat0 = Math.Asin(Math.Log((w2 * Math.Cos(Lat1)) / (w1 * Math.Cos(Lat2))) / (Q2 - Q1)),
                Q0 = Helpers.CalculateQParameter(Lat0, targetEllipsoid.e),
                Re = (targetEllipsoid.a * Math.Cos(Lat1) * Math.Exp(Q1 * Math.Sin(Lat0))) / w1 / Math.Sin(Lat0),
                R0 = Re / Math.Exp(Q0 * Math.Sin(Lat0)),
                x0 = Helpers.CalculateCentralPointX(Lat0, targetEllipsoid.a, targetEllipsoid.e2);

            double lat = 0.0,
             lon = 0.0,
             f1 = 0.0,
             f2 = 0.0,
             Latp = 0.0,
             R = 0.0,
             Q = 0.0,
             gama = 0.0,
             x = inputPoint.X,
             y = inputPoint.Y;

            // determine latitude iteratively
            R = Math.Sqrt(Math.Pow(y - sourceProjection.Y0, 2) + Math.Pow(R0 + x0 - x, 2));
            Q = Math.Log(Re / R) / Math.Sin(Lat0);
            Latp = Math.Asin((Math.Exp(2 * Q) - 1) / (Math.Exp(2 * Q) + 1));

            for (int i = 0; i < 10; i++)
            {
                f1 =
                  (Math.Log((1 + Math.Sin(Latp)) / (1 - Math.Sin(Latp))) -
                    targetEllipsoid.e * Math.Log((1 + targetEllipsoid.e * Math.Sin(Latp)) / (1 - targetEllipsoid.e * Math.Sin(Latp)))) /
                    2 -
                  Q;
                f2 = 1.0 / (1 - Math.Pow(Math.Sin(Latp), 2)) - targetEllipsoid.e2 / (1 - targetEllipsoid.e2 * Math.Pow(Math.Sin(Latp), 2));
                lat = Math.Asin(Math.Sin(Latp) - f1 / f2);

                if (Math.Abs(lat - Latp) <= 0.0000000001)
                {
                    break;
                }
                else
                {
                    Latp = lat;
                }
            }

            // determine longitude
            gama = Math.Atan((y - sourceProjection.Y0) / (R0 + x0 - x));
            lon = gama / Math.Sin(Lat0) + Lon0;

            resultPoint.X = lat / Math.PI * 180;
            resultPoint.Y = lon / Math.PI * 180;

            return resultPoint;
        }

        /// <summary>
        /// Transforms Geographic coordinates (Latitude, Longitude - EPSG:4326) to projected - Northing, Easting (Lambert Conformal Conic Projection 2SP).
        /// </summary>
        /// <param name="inputPoint">Input coordinates</param>
        /// <param name="outputProjection">Input coordinates</param>
        /// <param name="inputEllipsoid">Input coordinates</param>
        /// <returns></returns>
        public GeoPoint TransformGeographicToLambertProjected(GeoPoint inputPoint, enumProjections outputProjection = enumProjections.BGS_2005_KK, enumEllipsoids inputEllipsoid = enumEllipsoids.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Projection targetProjection = this.projections[outputProjection];
            Ellipsoid sourceEllipsoid = this.ellipsoids[inputEllipsoid];

            double Lon0 = (targetProjection.Lon0 * Math.PI) / 180,
                Lat1 = (targetProjection.Lat1 * Math.PI) / 180,
                Lat2 = (targetProjection.Lat2 * Math.PI) / 180,
                w1 = Helpers.CalculateWParameter((targetProjection.Lat1 * Math.PI) / 180, sourceEllipsoid.e2),
                w2 = Helpers.CalculateWParameter((targetProjection.Lat2 * Math.PI) / 180, sourceEllipsoid.e2),
                Q1 = Helpers.CalculateQParameter((targetProjection.Lat1 * Math.PI) / 180, sourceEllipsoid.e),
                Q2 = Helpers.CalculateQParameter((targetProjection.Lat2 * Math.PI) / 180, sourceEllipsoid.e),
                Lat0 = Math.Asin(Math.Log((w2 * Math.Cos(Lat1)) / (w1 * Math.Cos(Lat2))) / (Q2 - Q1)),
                Q0 = Helpers.CalculateQParameter(Lat0, sourceEllipsoid.e),
                Re = (sourceEllipsoid.a * Math.Cos(Lat1) * Math.Exp(Q1 * Math.Sin(Lat0))) / w1 / Math.Sin(Lat0),
                R0 = Re / Math.Exp(Q0 * Math.Sin(Lat0)),
                x0 = Helpers.CalculateCentralPointX(Lat0, sourceEllipsoid.a, sourceEllipsoid.e2);

            double R = 0.0,
                Q = 0.0,
                gama = 0.0,
                lat = (inputPoint.X * Math.PI) / 180,
                lon = (inputPoint.Y * Math.PI) / 180;

            double A = Math.Log((1 + Math.Sin(lat)) / (1 - Math.Sin(lat))),
              B = sourceEllipsoid.e * Math.Log((1 + sourceEllipsoid.e * Math.Sin(lat)) / (1 - sourceEllipsoid.e * Math.Sin(lat)));

            Q = (A - B) / 2;
            R = Re / Math.Exp(Q * Math.Sin(Lat0));

            gama = (lon - Lon0) * Math.Sin(Lat0);

            resultPoint.X = R0 + x0 - R * Math.Cos(gama);
            resultPoint.Y = targetProjection.Y0 + R * Math.Sin(gama);

            return resultPoint;
        }

        #endregion


        #region WEB MERCATOR AND GEOGRAPHIC

        /// <summary>
        /// Transforms Geographic coordinates (EPSG:4326) to Web Mercator (EPSG:3857)
        /// </summary>
        /// <param name="inputPoint">Geographic coordinates</param>
        /// <returns></returns>
        public GeoPoint TransformGeographicToWebMercator(GeoPoint inputPoint)
        {
            GeoPoint resultPoint = new GeoPoint();

            double latitude = inputPoint.X,
              longitude = inputPoint.Y,
              halfRadius = Math.PI * this.ellipsoids[enumEllipsoids.SPHERE].a;

            resultPoint.X = (longitude * halfRadius) / 180;
            resultPoint.Y = Math.Log(Math.Tan(((90 + latitude) * Math.PI) / 360)) / (Math.PI / 180);

            resultPoint.Y = (resultPoint.Y * halfRadius) / 180;

            return resultPoint;
        }

        /// <summary>
        /// Transforms Web Mercator (EPSG:3857) to Geographic coordinates (EPSG:4326)
        /// </summary>
        /// <param name="inputPoint">Coordinates in Web Mercator (EPSG:3857)</param>
        /// <returns></returns>
        public GeoPoint TransformWebMercatorToGeographic(GeoPoint inputPoint)
        {
            GeoPoint resultPoint = new GeoPoint();

            double x = inputPoint.Y,
                y = inputPoint.X,
                halfRadius = Math.PI * this.ellipsoids[enumEllipsoids.SPHERE].a;


            resultPoint.X = (x / halfRadius) * 180;
            resultPoint.Y = (y / halfRadius) * 180;

            resultPoint.X = (180 / Math.PI) * (2 * Math.Atan(Math.Exp((resultPoint.X * Math.PI) / 180)) - Math.PI / 2);

            return resultPoint;
        }

        #endregion


        #region FORMAT COORDINATE VALUES

        /// <summary>
        /// Convert decimal degrees to degrees, minutes, seconds.
        /// </summary>
        /// <param name="DEG">Value in decimal degrees</param>
        /// <returns>Input value in degrees, minutes, seconds</returns>
        public string ConvertDecimalDegreesToDMS(double DEG)
        {
            double MINPART, Inpt = DEG;
            string MIN, SEC;
            DEG = (int)DEG;
            MINPART = (Inpt - DEG) * 60;
            MIN = ((int)MINPART).ToString();
            SEC = ((MINPART - (double.Parse(MIN))) * 60).ToString();
            if (double.Parse(MIN) < 10) MIN = "0" + String.Format(MIN, "0");
            if (double.Parse(SEC) < 10) { SEC = "0" + String.Format(SEC, "0"); }
            else { SEC = String.Format(SEC, "0.0000"); }
            return ((DEG.ToString()) + MIN + SEC);
        }

        /// <summary>
        /// Convert degrees, minutes, seconds to decimal degrees.
        /// </summary>
        /// <param name="DMS">Value in dd mm ss.ss (42d23m43.23s should be past as 422343.23)</param>
        /// <returns>Input value in decimal degrees</returns>
        public double ConvertDMStoDecimalDegrees(string DMS)
        {
            double DEG;
            string MIN, SEC;
            if (DMS.Substring(0, 1) == "0")
            {
                DEG = double.Parse(DMS.Substring(1, 1));
                MIN = DMS.Substring(3, 2);
            }
            else
            {
                DEG = double.Parse(DMS.Substring(0, 2));
                MIN = DMS.Substring(2, 2);
            }
            SEC = DMS.Replace((DEG + MIN), "");
            return DEG + (double.Parse(MIN) / 60) + (double.Parse(SEC) / 60 / 60);
        }

        #endregion






        //#region STEREO70

        ///// <summary>
        ///// Transforms coordinates (X, Y - Stereo 70) to projected - x, y (UTM).
        ///// </summary>
        //public void TransformStereo70toProjected()
        //{
        //    // X zone 34                   X zone 35
        //    double A0l = 5103872.2794, A0r = 5098031.383;
        //    double A1l = 99991.570883628, A1r = 99997.950226236;
        //    double A2l = 5029.728551310, A2r = -2511.943262396;
        //    double A3l = -3.839048190, A3r = -0.959342938;
        //    double A4l = 75.880520708, A4r = -38.041410912;
        //    double A5l = 3.839142446, A5r = 0.959319603;
        //    double A6l = -2.046110300, A6r = -2.047545359;
        //    double A7l = -0.940612290, A7r = 0.467711919;
        //    double A8l = 6.138442312, A8r = 6.142636945;
        //    double A9l = 0.313594180, A9r = -0.155909332;
        //    double A10l = 0.000162981, A10r = 0.000045486;
        //    double A11l = -0.009387844, A11r = 0.004648853;
        //    double A12l = -0.002785369, A12r = -0.000701483;
        //    double A13l = 0.009439363, A13r = -0.004655353;
        //    double A14l = 0.000478559, A14r = 0.000120254;
        //    double A15l = 0.000059164, A15r = 0.000059285;
        //    double A16l = -0.000108944, A16r = 0.000053628;
        //    double A17l = -0.000869465, A17r = -0.000787626;
        //    double A18l = -0.000176462, A18r = 0.000090620;
        //    double A19l = 0.000452289, A19r = 0.000398146;
        //    double A20l = 0.000019460, A20r = -0.000010016;
        //    // Y zone 34                   Y zone 35
        //    double B0l = 809844.6172, B0r = 345074.4564;
        //    double B1l = -5029.728476949, B1r = 2511.943259802;
        //    double B2l = 99991.570907694, B2r = 99997.950239040;
        //    double B3l = -37.940246783, B3r = 19.020700041;
        //    double B4l = -7.678165281, B4r = -1.918631365;
        //    double B5l = 37.940266665, B5r = -19.020706824;
        //    double B6l = 0.313566511, B6r = -0.155906067;
        //    double B7l = -6.138435165, B7r = -6.142632353;
        //    double B8l = -0.940705735, B8r = 0.467718429;
        //    double B9l = 2.046148908, B9r = 2.047546054;
        //    double B10l = 0.002311102, B10r = -0.001157834;
        //    double B11l = 0.001887233, B11r = 0.000474435;
        //    double B12l = -0.014158883, B12r = 0.006982459;
        //    double B13l = -0.001889491, B13r = -0.000474864;
        //    double B14l = 0.002360195, B14r = -0.001163969;
        //    double B15l = -0.000016649, B15r = 0.000008273;
        //    double B16l = 0.000405208, B16r = 0.000386424;
        //    double B17l = 0.000187044, B17r = -0.000095796;
        //    double B18l = -0.000905397, B18r = -0.000796359;
        //    double B19l = -0.000093187, B19r = 0.000048027;
        //    double B20l = 0.000091010, B20r = 0.000080117;

        //    double x = this.Xro, y = this.Yro;

        //    x = 1.000250063 * (x - 500000);
        //    y = 1.000250063 * (y - 500000);
        //    x *= Math.Pow(10, -5);
        //    y *= Math.Pow(10, -5);

        //    if (this.ZoneUTM == 34)
        //    {
        //        this.Northing = A0l + A1l * x + A2l * y + A3l * x * x + A4l * x * y + A5l * y * y +
        //            A6l * x * x * x + A7l * x * x * y + A8l * x * y * y + A9l * y * y * y +
        //            A10l * x * x * x * x + A11l * x * x * x * y + A12l * x * x * y * y +
        //            A13l * x * y * y * y + A14l * y * y * y * y + A15l * x * x * x * x * x +
        //            A16l * x * x * x * x * y + A17l * x * x * x * y * y + A18l * x * x * y * y * y +
        //            A19l * x * y * y * y * y + A20l * y * y * y * y * y;

        //        this.Easting = B0l + B1l * x + B2l * y + B3l * x * x + B4l * x * y + B5l * y * y +
        //             B6l * x * x * x + B7l * x * x * y + B8l * x * y * y + B9l * y * y * y +
        //             B10l * x * x * x * x + B11l * x * x * x * y + B12l * x * x * y * y +
        //             B13l * x * y * y * y + B14l * y * y * y * y + B15l * x * x * x * x * x +
        //             B16l * x * x * x * x * y + B17l * x * x * x * y * y + B18l * x * x * y * y * y +
        //             B19l * x * y * y * y * y + B20l * y * y * y * y * y;

        //        this.Northing *= 0.9996;
        //        this.Easting *= 0.9996;
        //        this.Easting += 500000 * (1 - 0.9996); ;

        //        return;
        //    }

        //    if (this.ZoneUTM == 35)
        //    {
        //        this.Northing = A0r + A1r * x + A2r * y + A3r * x * x + A4r * x * y + A5r * y * y +
        //            A6r * x * x * x + A7r * x * x * y + A8r * x * y * y + A9r * y * y * y +
        //            A10r * x * x * x * x + A11r * x * x * x * y + A12r * x * x * y * y +
        //            A13r * x * y * y * y + A14r * y * y * y * y + A15r * x * x * x * x * x +
        //            A16r * x * x * x * x * y + A17r * x * x * x * y * y + A18r * x * x * y * y * y +
        //            A19r * x * y * y * y * y + A20r * y * y * y * y * y;

        //        this.Easting = B0r + B1r * x + B2r * y + B3r * x * x + B4r * x * y + B5r * y * y +
        //             B6r * x * x * x + B7r * x * x * y + B8r * x * y * y + B9r * y * y * y +
        //             B10r * x * x * x * x + B11r * x * x * x * y + B12r * x * x * y * y +
        //             B13r * x * y * y * y + B14r * y * y * y * y + B15r * x * x * x * x * x +
        //             B16r * x * x * x * x * y + B17r * x * x * x * y * y + B18r * x * x * y * y * y +
        //             B19r * x * y * y * y * y + B20r * y * y * y * y * y;

        //        this.Northing *= 0.9996;
        //        this.Easting *= 0.9996;
        //        this.Easting += 500000 * (1 - 0.9996);

        //        return;
        //    }
        //}

        //#endregion
    }
}
