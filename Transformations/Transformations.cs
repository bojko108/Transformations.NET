//
// author: bojko108 (bojko108@gmail.com)
//

using System;
using System.Linq;
using System.Collections.Generic;
using BojkoSoft.Transformations.Constants;
using BojkoSoft.Transformations.ControlPoints;
using BojkoSoft.Transformations.TransformationModels;

namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Transform points between different coordinate systems
    /// </summary>
    public class Transformations
    {
        private readonly Ellipsoids ellipsoids = new Ellipsoids();
        private readonly Projections projections = new Projections();
        private readonly Dictionary<enumProjection, ControlPointsClass> controlPoints = new Dictionary<enumProjection, ControlPointsClass>();

        /// <summary>
        /// Transform points between different coordinate systems
        /// </summary>
        /// <param name="useControlPoints">Wheather or not the control points for transforming between BGS coordiante systems will be initialized.</param>
        public Transformations(bool useControlPoints = true)
        {
            if (useControlPoints)
            {
                this.controlPoints.Add(enumProjection.BGS_1930_24, new BGS193024());
                this.controlPoints.Add(enumProjection.BGS_1930_27, new BGS193027());
                this.controlPoints.Add(enumProjection.BGS_1950_3_24, new BGS1950324());
                this.controlPoints.Add(enumProjection.BGS_1950_3_27, new BGS1950327());
                this.controlPoints.Add(enumProjection.BGS_1950_6_21, new BGS1950621());
                this.controlPoints.Add(enumProjection.BGS_1950_6_27, new BGS1950627());
                this.controlPoints.Add(enumProjection.BGS_1970_K3, new BGS1970K3());
                this.controlPoints.Add(enumProjection.BGS_1970_K5, new BGS1970K5());
                this.controlPoints.Add(enumProjection.BGS_1970_K7, new BGS1970K7());
                this.controlPoints.Add(enumProjection.BGS_1970_K9, new BGS1970K9());
                this.controlPoints.Add(enumProjection.BGS_2005_KK, new BGS2005KK());
            }
        }


        /// <summary>
        /// Use this method to calculate transformation parameters for giver extent. All points will be transformed using calculated parameters.
        /// </summary>
        /// <param name="inputExtent">Extent for which to calculate parameters</param>
        /// <param name="inputProjection">input projection</param>
        /// <param name="outputProjection">output projection</param>
        /// <returns>transformation parameters</returns>
        public double[] CalculateAffineTransformationParameters(GeoExtent inputExtent, enumProjection inputProjection = enumProjection.BGS_1970_K9, enumProjection outputProjection = enumProjection.BGS_2005_KK)
        {
            if (inputProjection == enumProjection.BGS_SOFIA)
            {
                inputExtent.NorthEastCorner.X += this.projections[inputProjection].X0;
                inputExtent.NorthEastCorner.Y += this.projections[inputProjection].Y0;

                inputExtent.SouthWestCorner.X += this.projections[inputProjection].X0;
                inputExtent.SouthWestCorner.Y += this.projections[inputProjection].Y0;
            }

            ControlPointsClass inputControlPoints = inputProjection == enumProjection.BGS_SOFIA ? this.controlPoints[enumProjection.BGS_1950_3_24] : this.controlPoints[inputProjection];
            ControlPointsClass outputControlPoints = outputProjection == enumProjection.BGS_SOFIA ? this.controlPoints[enumProjection.BGS_1950_3_24] : this.controlPoints[outputProjection];
            List<GeoPoint> inputGeoPoints = inputControlPoints.GetPoints(inputExtent);
            List<GeoPoint> outputGeoPoints = outputControlPoints.GetPoints(inputGeoPoints.Select(p => p.ID).ToArray());

            AffineTransformation transformation = new AffineTransformation(inputGeoPoints, outputGeoPoints);
            return transformation.GetParameters();
        }


        #region GEOGRAPHIC AND LAMBERT

        /// <summary>
        /// Transforms geographic coordinates to projected
        /// </summary>
        /// <param name="inputPoint">input geographic coordinates</param>
        /// <param name="outputProjection">output Lambert projection</param>
        /// <param name="outputEllipsoid">output Lambert projection is using this ellipsoid</param>
        /// <returns>geographic coordinates projected to Lambert projection</returns>
        public GeoPoint TransformGeographicToLambert(GeoPoint inputPoint, enumProjection outputProjection = enumProjection.BGS_2005_KK, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Projection targetProjection = this.projections[outputProjection];
            Ellipsoid targetEllipsoid = this.ellipsoids[outputEllipsoid];

            double Lon0 = targetProjection.Lon0.ToRad(),
                Lat1 = targetProjection.Lat1.ToRad(),
                Lat2 = targetProjection.Lat2.ToRad(),
                w1 = Helpers.CalculateWParameter((targetProjection.Lat1 * Math.PI) / 180, targetEllipsoid.e2),
                w2 = Helpers.CalculateWParameter((targetProjection.Lat2 * Math.PI) / 180, targetEllipsoid.e2),
                Q1 = Helpers.CalculateQParameter((targetProjection.Lat1 * Math.PI) / 180, targetEllipsoid.e),
                Q2 = Helpers.CalculateQParameter((targetProjection.Lat2 * Math.PI) / 180, targetEllipsoid.e),
                Lat0 = Math.Asin(Math.Log((w2 * Math.Cos(Lat1)) / (w1 * Math.Cos(Lat2))) / (Q2 - Q1)),
                Q0 = Helpers.CalculateQParameter(Lat0, targetEllipsoid.e),
                Re = (targetEllipsoid.a * Math.Cos(Lat1) * Math.Exp(Q1 * Math.Sin(Lat0))) / w1 / Math.Sin(Lat0),
                R0 = Re / Math.Exp(Q0 * Math.Sin(Lat0)),
                x0 = Helpers.CalculateCentralPointX(Lat0, targetEllipsoid.a, targetEllipsoid.e2);

            double R = 0.0,
                Q = 0.0,
                gama = 0.0,
                latitude = inputPoint.X.ToRad(),
                longitude = inputPoint.Y.ToRad();

            double A = Math.Log((1 + Math.Sin(latitude)) / (1 - Math.Sin(latitude))),
              B = targetEllipsoid.e * Math.Log((1 + targetEllipsoid.e * Math.Sin(latitude)) / (1 - targetEllipsoid.e * Math.Sin(latitude)));

            Q = (A - B) / 2;
            R = Re / Math.Exp(Q * Math.Sin(Lat0));

            gama = (longitude - Lon0) * Math.Sin(Lat0);

            resultPoint.X = R0 + x0 - R * Math.Cos(gama);
            resultPoint.Y = targetProjection.Y0 + R * Math.Sin(gama);

            return resultPoint;
        }

        /// <summary>
        /// Transforms projected coordinates to geographic
        /// </summary>
        /// <param name="inputPoint">input coordinates in Lambert projection</param>
        /// <param name="inputProjection">input Lambert projection</param>
        /// <param name="inputEllipsoid">input Lambert projection is using this ellipsoid</param>
        /// <returns>geographic coordinates</returns>
        public GeoPoint TransformLambertToGeographic(GeoPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_2005_KK, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Projection sourceProjection = this.projections[inputProjection];
            Ellipsoid sourceEllipsoid = this.ellipsoids[inputEllipsoid];

            double Lon0 = sourceProjection.Lon0.ToRad(),
                Lat1 = sourceProjection.Lat1.ToRad(),
                Lat2 = sourceProjection.Lat2.ToRad(),
                w1 = Helpers.CalculateWParameter((sourceProjection.Lat1 * Math.PI) / 180, sourceEllipsoid.e2),
                w2 = Helpers.CalculateWParameter((sourceProjection.Lat2 * Math.PI) / 180, sourceEllipsoid.e2),
                Q1 = Helpers.CalculateQParameter((sourceProjection.Lat1 * Math.PI) / 180, sourceEllipsoid.e),
                Q2 = Helpers.CalculateQParameter((sourceProjection.Lat2 * Math.PI) / 180, sourceEllipsoid.e),
                Lat0 = Math.Asin(Math.Log((w2 * Math.Cos(Lat1)) / (w1 * Math.Cos(Lat2))) / (Q2 - Q1)),
                Q0 = Helpers.CalculateQParameter(Lat0, sourceEllipsoid.e),
                Re = (sourceEllipsoid.a * Math.Cos(Lat1) * Math.Exp(Q1 * Math.Sin(Lat0))) / w1 / Math.Sin(Lat0),
                R0 = Re / Math.Exp(Q0 * Math.Sin(Lat0)),
                x0 = Helpers.CalculateCentralPointX(Lat0, sourceEllipsoid.a, sourceEllipsoid.e2);

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
                    sourceEllipsoid.e * Math.Log((1 + sourceEllipsoid.e * Math.Sin(Latp)) / (1 - sourceEllipsoid.e * Math.Sin(Latp)))) /
                    2 -
                  Q;
                f2 = 1.0 / (1 - Math.Pow(Math.Sin(Latp), 2)) - sourceEllipsoid.e2 / (1 - sourceEllipsoid.e2 * Math.Pow(Math.Sin(Latp), 2));
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

            resultPoint.X = lat.ToDeg();
            resultPoint.Y = lon.ToDeg();

            return resultPoint;
        }

        #endregion


        #region GEOGRAPHIC AND UTM

        /// <summary>
        /// Transforms geographic coordinates in UTM projection
        /// </summary>
        /// <param name="inputPoint">input geographic coordinates</param>
        /// <param name="outputUtmProjection">target UTM projection</param>
        /// <param name="inputEllipsoid">input coordinates are for this ellipsoid</param>
        /// <returns>input coordinates in UTM projection</returns>
        public GeoPoint TransformGeographicToUTM(GeoPoint inputPoint, enumProjection outputUtmProjection = enumProjection.UTM35N, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
        {
            return this.TransformGeographicToGauss(inputPoint, outputUtmProjection, inputEllipsoid);
        }

        /// <summary>
        /// Transforms UTM coordinates to geographic
        /// </summary>
        /// <param name="inputPoint">input UTM coordinates</param>
        /// <param name="inputUtmProjection">input coordinates projection</param>
        /// <param name="outputEllipsoid">output ellipsoid</param>
        /// <returns>geographic coordinates</returns>
        public GeoPoint TransformUTMToGeographic(GeoPoint inputPoint, enumProjection inputUtmProjection = enumProjection.UTM35N, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
        {
            return this.TransformGaussToGeographic(inputPoint, inputUtmProjection, outputEllipsoid);
        }

        #endregion


        #region GEOGRAPHIC AND GAUSS

        /// <summary>
        /// Transforms geographic coordinates in Gauss projection
        /// </summary>
        /// <param name="inputPoint">input geographic coordinates</param>
        /// <param name="outputProjection">target gauss projection</param>
        /// <param name="inputEllipsoid">input coordinates are for this ellipsoid</param>
        /// <returns>projected coordinates in gauss projection</returns>
        public GeoPoint TransformGeographicToGauss(GeoPoint inputPoint, enumProjection outputProjection = enumProjection.BGS_1930_24, enumEllipsoid inputEllipsoid = enumEllipsoid.HAYFORD)
        {
            GeoPoint resultPoint = new GeoPoint();

            inputPoint.X = inputPoint.X.ToRad();
            inputPoint.Y = inputPoint.Y.ToRad();

            Projection targetProjection = this.projections[outputProjection];
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

            l = inputPoint.Y - targetProjection.Lon0.ToRad();

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
        /// Transforms projected coordinates to geographic
        /// </summary>
        /// <param name="inputPoint">input coordinates in gauss projection</param>
        /// <param name="inputProjection">input gauss projection</param>
        /// <param name="outputEllipsoid">target ellipsoid</param>
        /// <returns>geographic coordinates in target ellipsoid</returns>
        public GeoPoint TransformGaussToGeographic(GeoPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_1930_24, enumEllipsoid outputEllipsoid = enumEllipsoid.HAYFORD)
        {
            GeoPoint resultPoint = new GeoPoint();

            Projection sourceProjection = this.projections[inputProjection];
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

            resultPoint.X = resultPoint.X.ToDeg();
            resultPoint.Y = resultPoint.Y.ToDeg();

            return resultPoint;
        }

        #endregion


        #region GEOGRAPHIC AND WEB MERCATOR

        /// <summary>
        /// Transforms geographic coordinates to Web Mercator
        /// </summary>
        /// <param name="inputPoint">input geographic coordinates</param>
        /// <returns>geographic coordinates in Web Mercator projection</returns>
        public GeoPoint TransformGeographicToWebMercator(GeoPoint inputPoint)
        {
            GeoPoint resultPoint = new GeoPoint();

            double latitude = inputPoint.X,
              longitude = inputPoint.Y,
              halfRadius = Math.PI * this.ellipsoids[enumEllipsoid.SPHERE].a;

            resultPoint.X = (longitude * halfRadius) / 180;
            resultPoint.Y = Math.Log(Math.Tan(((90 + latitude) * Math.PI) / 360)) / (Math.PI / 180);

            resultPoint.Y = (resultPoint.Y * halfRadius) / 180;

            return resultPoint;
        }

        /// <summary>
        /// Transforms projected coordinates in Web Mercator to geographic coordinates
        /// </summary>
        /// <param name="inputPoint">input coordinates in Web Mercator</param>
        /// <returns>geographic coordinates</returns>
        public GeoPoint TransformWebMercatorToGeographic(GeoPoint inputPoint)
        {
            GeoPoint resultPoint = new GeoPoint();

            double x = inputPoint.Y,
                y = inputPoint.X,
                halfRadius = Math.PI * this.ellipsoids[enumEllipsoid.SPHERE].a;


            resultPoint.X = (x / halfRadius) * 180;
            resultPoint.Y = (y / halfRadius) * 180;

            resultPoint.X = (180 / Math.PI) * (2 * Math.Atan(Math.Exp((resultPoint.X * Math.PI) / 180)) - Math.PI / 2);

            return resultPoint;
        }

        #endregion


        #region GEOGRAPHIC AND GEOCENTRIC

        /// <summary>
        /// Transforms geographic coordiantes to geocentric
        /// </summary>
        /// <param name="inputPoint">input geographic coordinates</param>
        /// <param name="outputEllipsoid">output ellipsoid (same as input coordinates ellipsoid)</param>
        /// <returns>geocentric coordinates in output ellipsoid</returns>
        public GeoPoint TransformGeographicToGeocentric(GeoPoint inputPoint, enumEllipsoid outputEllipsoid = enumEllipsoid.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Ellipsoid ellipsoid = this.ellipsoids[outputEllipsoid];

            double latitude = inputPoint.X.ToRad(),
                longitude = inputPoint.Y.ToRad(),
                h = inputPoint.Z,
                N = Math.Pow(ellipsoid.a, 2) / Math.Sqrt(Math.Pow(ellipsoid.a, 2) * Math.Pow(Math.Cos(latitude), 2) + Math.Pow(ellipsoid.b, 2) * Math.Pow(Math.Sin(latitude), 2));

            resultPoint.X = (N + h) * Math.Cos(latitude) * Math.Cos(longitude);
            resultPoint.Y = (N + h) * Math.Cos(latitude) * Math.Sin(longitude);
            resultPoint.Z = ((Math.Pow(ellipsoid.b, 2) / Math.Pow(ellipsoid.a, 2)) * N + h) * Math.Sin(latitude);

            return resultPoint;
        }

        /// <summary>
        /// Transforms geocentric coordinates to geographic
        /// </summary>
        /// <param name="inputPoint">input geocentric coordinates</param>
        /// <param name="inputEllipsoid">input coordinates are for this ellipsoid</param>
        /// <returns>geographic coordinates</returns>
        public GeoPoint TransformGeocentricToGeographic(GeoPoint inputPoint, enumEllipsoid inputEllipsoid = enumEllipsoid.WGS84)
        {
            GeoPoint resultPoint = new GeoPoint();

            Ellipsoid ellipsoid = this.ellipsoids[inputEllipsoid];

            double p = Math.Sqrt(Math.Pow(inputPoint.X, 2) + Math.Pow(inputPoint.Y, 2));

            double latitude = 0.0,
              longitude = Math.Atan(inputPoint.Y / inputPoint.X),
              h = 0.0,
              latp = (inputPoint.Z / p) * Math.Pow(1 - ellipsoid.e2, -1),
              Np = 0.0;

            for (int i = 0; i < 10; i++)
            {
                Np =
                  Math.Pow(ellipsoid.a, 2) /
                  Math.Sqrt(Math.Pow(ellipsoid.a, 2) * Math.Pow(Math.Cos(latp), 2) + Math.Pow(ellipsoid.b, 2) * Math.Pow(Math.Sin(latp), 2));

                h = p / Math.Cos(latp) - Np;

                latitude = Math.Atan((inputPoint.Z / p) * Math.Pow(1 - ellipsoid.e2 * (Np / (Np + h)), -1));

                if (Math.Abs(latitude - latp) <= 0.0000000001)
                {
                    break;
                }
                else
                {
                    latp = latitude;
                }
            }

            resultPoint.X = latitude.ToDeg();
            resultPoint.Y = longitude.ToDeg();

            return resultPoint;
        }

        #endregion


        #region BGS Coordinates

        /// <summary>
        /// Transforms from BGS 1930, BGS1950, BGS Sofia, BGS 1970 or BGS 2005 projected coordinates to the specified projection.
        /// Transforms a point by calculating local transformation parameters. Transformation parameters are calculated using predefiend
        /// control points. Control points are searched within 20 000m around the input point. If the point is close to the border of 
        /// the projection an exception will be thrown.
        /// </summary>
        /// <param name="inputPoint">input projected coordinates in BGS 1930, BGS1950, BGS 1970 or BGS 2005</param>
        /// <param name="inputProjection">input coordinates projection</param>
        /// <param name="outputProjection">output projection</param>
        /// <param name="useTPS">use TPS or Affine transformations</param>
        /// <returns>coordinates in specified projection</returns>
        public GeoPoint TransformBGSCoordinates(GeoPoint inputPoint, enumProjection inputProjection = enumProjection.BGS_1970_K9, enumProjection outputProjection = enumProjection.BGS_2005_KK, bool useTPS = true)
        {
            double distance = 20000;

            if (inputProjection == enumProjection.BGS_SOFIA)
            {
                inputPoint.X += this.projections[inputProjection].X0;
                inputPoint.Y += this.projections[inputProjection].Y0;
            }

            ControlPointsClass inputControlPoints = inputProjection == enumProjection.BGS_SOFIA ? this.controlPoints[enumProjection.BGS_1950_3_24] : this.controlPoints[inputProjection];
            ControlPointsClass outputControlPoints = outputProjection == enumProjection.BGS_SOFIA ? this.controlPoints[enumProjection.BGS_1950_3_24] : this.controlPoints[outputProjection];
            List<GeoPoint> inputGeoPoints = inputControlPoints.GetPoints(inputPoint, distance);
            List<GeoPoint> outputGeoPoints = outputControlPoints.GetPoints(inputGeoPoints.Select(p => p.ID).ToArray());

            #region transform using Affine or TPS transformation

            ITransformation transformation;
            if (useTPS)
                transformation = new TPSTransformation(inputGeoPoints, outputGeoPoints);
            else
                transformation = new AffineTransformation(inputGeoPoints, outputGeoPoints);

            GeoPoint resultPoint = transformation.Transform(inputPoint);

            #endregion

            if (outputProjection == enumProjection.BGS_SOFIA)
            {
                resultPoint.X -= this.projections[outputProjection].X0;
                resultPoint.Y -= this.projections[outputProjection].Y0;
            }

            return resultPoint;
        }

        /// <summary>
        /// Transforms from BGS 1930, BGS1950, BGS Sofia, BGS 1970 or BGS 2005 projected coordinates to the specified projection.
        /// Transforms a point with provided transformation parameters.
        /// </summary>
        /// <param name="inputPoint">input projected coordinates in BGS 1930, BGS1950, BGS 1970 or BGS 2005</param>
        /// <param name="affineTransformationParameters"></param>
        /// <param name="inputProjection">input coordinates projection</param>
        /// <param name="outputProjection">output projection</param>
        /// <returns>coordinates in specified projection</returns>
        public GeoPoint TransformBGSCoordinates(GeoPoint inputPoint, double[] affineTransformationParameters, enumProjection inputProjection = enumProjection.BGS_1970_K9, enumProjection outputProjection = enumProjection.BGS_2005_KK)
        {
            if (inputProjection == enumProjection.BGS_SOFIA)
            {
                inputPoint.X += this.projections[inputProjection].X0;
                inputPoint.Y += this.projections[inputProjection].Y0;
            }

            AffineTransformation transformation = new AffineTransformation(affineTransformationParameters);

            GeoPoint resultPoint = transformation.Transform(inputPoint);

            if (outputProjection == enumProjection.BGS_SOFIA)
            {
                resultPoint.X -= this.projections[outputProjection].X0;
                resultPoint.Y -= this.projections[outputProjection].Y0;
            }

            return resultPoint;
        }

        #endregion


        #region FORMAT COORDINATE VALUES

        /// <summary>
        /// Converts decimal degrees to degrees, minutes and seconds
        /// </summary>
        /// <param name="DEG">value in decimal degrees</param>
        /// <returns>input value in degrees, minutes and seconds</returns>
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
        /// Converts degrees, minutes and seconds to decimal degrees
        /// </summary>
        /// <param name="DMS">value in ddmmss.ss (42d23m43.23s should be past as 422343.23)</param>
        /// <returns>input value in decimal degrees</returns>
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


        /// <summary>
        /// Transform a point
        /// </summary>
        /// <param name="inputPoint">input point</param>
        /// <param name="sourceProjection">input projection</param>
        /// <param name="targetProjection">output projection</param>
        /// <param name="useTPS">use TPS or Affine transformations</param>
        /// <returns></returns>
        public GeoPoint Transform(GeoPoint inputPoint, enumProjection sourceProjection, enumProjection targetProjection, bool useTPS = true)
        {
            GeoPoint outputPoint = inputPoint.Clone();

            switch (sourceProjection)
            {
                case enumProjection.BGS_1930_24:
                case enumProjection.BGS_1930_27:
                    {
                        switch (targetProjection)
                        {
                            case enumProjection.BGS_SOFIA:
                            case enumProjection.BGS_1930_24:
                            case enumProjection.BGS_1930_27:
                            case enumProjection.BGS_1950_3_24:
                            case enumProjection.BGS_1950_3_27:
                            case enumProjection.BGS_1950_6_21:
                            case enumProjection.BGS_1950_6_27:
                            case enumProjection.BGS_1970_K3:
                            case enumProjection.BGS_1970_K5:
                            case enumProjection.BGS_1970_K7:
                            case enumProjection.BGS_1970_K9:
                            case enumProjection.BGS_2005_KK:
                                {
                                    outputPoint = this.TransformBGSCoordinates(inputPoint, sourceProjection, targetProjection, useTPS);
                                    break;
                                }
                            case enumProjection.UTM34N:
                            case enumProjection.UTM35N:
                                {
                                    outputPoint = this.TransformBGSCoordinates(inputPoint, sourceProjection, enumProjection.BGS_2005_KK, useTPS);
                                    outputPoint = this.TransformLambertToGeographic(outputPoint);
                                    outputPoint = this.TransformGeographicToUTM(outputPoint, targetProjection);
                                    break;
                                }
                            case enumProjection.WGS84_GEOGRAPHIC:
                                {
                                    outputPoint = this.TransformGaussToGeographic(inputPoint, sourceProjection);
                                    break;
                                }
                        }
                        break;
                    }
                case enumProjection.BGS_SOFIA:
                case enumProjection.BGS_1950_3_24:
                case enumProjection.BGS_1950_3_27:
                case enumProjection.BGS_1950_6_21:
                case enumProjection.BGS_1950_6_27:
                case enumProjection.BGS_1970_K3:
                case enumProjection.BGS_1970_K5:
                case enumProjection.BGS_1970_K7:
                case enumProjection.BGS_1970_K9:
                    {
                        switch (targetProjection)
                        {
                            case enumProjection.BGS_SOFIA:
                            case enumProjection.BGS_1930_24:
                            case enumProjection.BGS_1930_27:
                            case enumProjection.BGS_1950_3_24:
                            case enumProjection.BGS_1950_3_27:
                            case enumProjection.BGS_1950_6_21:
                            case enumProjection.BGS_1950_6_27:
                            case enumProjection.BGS_1970_K3:
                            case enumProjection.BGS_1970_K5:
                            case enumProjection.BGS_1970_K7:
                            case enumProjection.BGS_1970_K9:
                            case enumProjection.BGS_2005_KK:
                                {
                                    outputPoint = this.TransformBGSCoordinates(inputPoint, sourceProjection, targetProjection, useTPS);
                                    break;
                                }
                            case enumProjection.UTM34N:
                            case enumProjection.UTM35N:
                                {
                                    outputPoint = this.TransformBGSCoordinates(inputPoint, sourceProjection, enumProjection.BGS_2005_KK, useTPS);
                                    outputPoint = this.TransformLambertToGeographic(outputPoint);
                                    outputPoint = this.TransformGeographicToUTM(outputPoint, targetProjection);
                                    break;
                                }
                            case enumProjection.WGS84_GEOGRAPHIC:
                                {
                                    outputPoint = this.TransformBGSCoordinates(inputPoint, sourceProjection, enumProjection.BGS_2005_KK, useTPS);
                                    outputPoint = this.TransformLambertToGeographic(outputPoint);
                                    break;
                                }
                        }
                        break;
                    }
                case enumProjection.BGS_2005_KK:
                    {
                        switch (targetProjection)
                        {
                            case enumProjection.BGS_SOFIA:
                            case enumProjection.BGS_1930_24:
                            case enumProjection.BGS_1930_27:
                            case enumProjection.BGS_1950_3_24:
                            case enumProjection.BGS_1950_3_27:
                            case enumProjection.BGS_1950_6_21:
                            case enumProjection.BGS_1950_6_27:
                            case enumProjection.BGS_1970_K3:
                            case enumProjection.BGS_1970_K5:
                            case enumProjection.BGS_1970_K7:
                            case enumProjection.BGS_1970_K9:
                                {
                                    outputPoint = this.TransformBGSCoordinates(inputPoint, sourceProjection, targetProjection, useTPS);
                                    break;
                                }
                            case enumProjection.UTM34N:
                            case enumProjection.UTM35N:
                                {
                                    outputPoint = this.TransformLambertToGeographic(inputPoint);
                                    outputPoint = this.TransformGeographicToUTM(outputPoint, targetProjection);
                                    break;
                                }
                            case enumProjection.WGS84_GEOGRAPHIC:
                                {
                                    outputPoint = this.TransformLambertToGeographic(inputPoint);
                                    break;
                                }
                        }
                        break;
                    }
                case enumProjection.UTM34N:
                case enumProjection.UTM35N:
                    {
                        switch (targetProjection)
                        {
                            case enumProjection.BGS_SOFIA:
                            case enumProjection.BGS_1930_24:
                            case enumProjection.BGS_1930_27:
                            case enumProjection.BGS_1950_3_24:
                            case enumProjection.BGS_1950_3_27:
                            case enumProjection.BGS_1950_6_21:
                            case enumProjection.BGS_1950_6_27:
                            case enumProjection.BGS_1970_K3:
                            case enumProjection.BGS_1970_K5:
                            case enumProjection.BGS_1970_K7:
                            case enumProjection.BGS_1970_K9:
                                {
                                    outputPoint = this.TransformUTMToGeographic(inputPoint, sourceProjection);
                                    outputPoint = this.TransformGeographicToLambert(outputPoint);
                                    outputPoint = this.TransformBGSCoordinates(outputPoint, enumProjection.BGS_2005_KK, targetProjection, useTPS);
                                    break;
                                }
                            case enumProjection.BGS_2005_KK:
                                {
                                    outputPoint = this.TransformUTMToGeographic(inputPoint, sourceProjection);
                                    outputPoint = this.TransformGeographicToLambert(outputPoint);
                                    break;
                                }
                            case enumProjection.UTM34N:
                            case enumProjection.UTM35N:
                                {
                                    outputPoint = this.TransformUTMToGeographic(inputPoint, sourceProjection);
                                    outputPoint = this.TransformGeographicToUTM(inputPoint, targetProjection);
                                    break;
                                }
                            case enumProjection.WGS84_GEOGRAPHIC:
                                {
                                    outputPoint = this.TransformUTMToGeographic(inputPoint, sourceProjection);
                                    break;
                                }
                        }
                        break;
                    }
                case enumProjection.WGS84_GEOGRAPHIC:
                    {
                        switch (targetProjection)
                        {
                            case enumProjection.BGS_1930_24:
                            case enumProjection.BGS_1930_27:
                                outputPoint = this.TransformGeographicToGauss(outputPoint);
                                break;
                            case enumProjection.BGS_SOFIA:
                            case enumProjection.BGS_1950_3_24:
                            case enumProjection.BGS_1950_3_27:
                            case enumProjection.BGS_1950_6_21:
                            case enumProjection.BGS_1950_6_27:
                            case enumProjection.BGS_1970_K3:
                            case enumProjection.BGS_1970_K5:
                            case enumProjection.BGS_1970_K7:
                            case enumProjection.BGS_1970_K9:
                                {
                                    outputPoint = this.TransformGeographicToLambert(outputPoint);
                                    outputPoint = this.TransformBGSCoordinates(outputPoint, enumProjection.BGS_2005_KK, targetProjection, useTPS);
                                    break;
                                }
                            case enumProjection.BGS_2005_KK:
                                {
                                    outputPoint = this.TransformGeographicToLambert(outputPoint);
                                    break;
                                }
                            case enumProjection.UTM34N:
                            case enumProjection.UTM35N:
                                {
                                    outputPoint = this.TransformGeographicToUTM(inputPoint, targetProjection);
                                    break;
                                }
                        }
                        break;
                    }
            }

            return outputPoint;
        }






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

