using System;
using System.Linq;
using System.Collections.Generic;

namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Some usefull functions
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// transforms value to radians
        /// </summary>
        /// <param name="degrees">value in degrees</param>
        /// <returns>value in radians</returns>
        public static double ToRad(this double degrees)
        {
            return degrees * Math.PI / 180;
        }

        /// <summary>
        /// transforms value to degrees
        /// </summary>
        /// <param name="radians">value in radians</param>
        /// <returns>value in degrees</returns>
        public static double ToDeg(this double radians)
        {
            return radians * 180 / Math.PI;
        }

        /// <summary>
        /// Calculates distance from the equator to the specified Latitude.
        /// </summary>
        /// <param name="latitude">base latitude value in radians</param>
        /// <param name="a">ellipsoid's semi major axis</param>
        /// <param name="b">ellipsoid's semi minor axis</param>
        /// <returns></returns>
        public static double ArcLengthOfMeridian(double latitude, double a, double b)
        {
            double alpha, beta, gamma, delta, epsilon, n;

            n = (a - b) / (a + b);
            alpha = ((a + b) / 2.0) * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));
            beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) / 16.0) + (-3.0 * Math.Pow(n, 5.0) / 32.0);
            gamma = (15.0 * Math.Pow(n, 2.0) / 16.0) + (-15.0 * Math.Pow(n, 4.0) / 32.0);
            delta = (-35.0 * Math.Pow(n, 3.0) / 48.0) + (105.0 * Math.Pow(n, 5.0) / 256.0);
            epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);

            return alpha * (latitude + (beta * Math.Sin(2.0 * latitude))
                    + (gamma * Math.Sin(4.0 * latitude))
                    + (delta * Math.Sin(6.0 * latitude))
                    + (epsilon * Math.Sin(8.0 * latitude)));
        }

        /// <summary>
        /// Calculates base latitude from x coordinate of a point
        /// </summary>
        /// <param name="x">distance from the equator in meters</param>
        /// <param name="a">ellipsoid's semi major axis</param>
        /// <param name="b">ellipsoid's semi minor axis</param>
        /// <returns></returns>
        public static double FootpointLatitude(double x, double a, double b)
        {
            double x_, alpha_, beta_, gamma_, delta_, epsilon_, n;

            n = ((a - b) / (a + b));
            alpha_ = ((a + b) / 2.0) * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));
            x_ = x / alpha_;
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0) + (269.0 * Math.Pow(n, 5.0) / 512.0);
            gamma_ = (21.0 * Math.Pow(n, 2.0) / 16.0) + (-55.0 * Math.Pow(n, 4.0) / 32.0);
            delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0) + (-417.0 * Math.Pow(n, 5.0) / 128.0);
            epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);

            return x_ + (beta_ * Math.Sin(2.0 * x_))
                + (gamma_ * Math.Sin(4.0 * x_))
                + (delta_ * Math.Sin(6.0 * x_))
                + (epsilon_ * Math.Sin(8.0 * x_));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static double CalculateWParameter(double latitude, double e2)
        {
            return Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(latitude), 2));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double CalculateQParameter(double latitude, double e)
        {
            return (
              (1.0 / 2.0) *
              (Math.Log((1 + Math.Sin(latitude)) / (1 - Math.Sin(latitude))) -
                e * Math.Log((1 + e * Math.Sin(latitude)) / (1 - e * Math.Sin(latitude))))
            );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat0"></param>
        /// <param name="a"></param>
        /// <param name="e2"></param>
        /// <returns></returns>
        public static double CalculateCentralPointX(double lat0, double a, double e2)
        {
            double m0 = a * (1 - e2),
              m2 = 1.5 * e2 * m0,
              m4 = 1.25 * e2 * m2,
              m6 = (7 / 6) * (e2 * m4),
              m8 = 1.125 * e2 * m6,
              a0 = m0 + 0.5 * m2 + 0.375 * m4 + 0.3125 * m6 + 0.2734375 * m8,
              a2 = 0.5 * m2 + 0.5 * m4 + 0.46875 * m6 + 0.4375 * m8,
              a4 = 0.125 * m4 + 0.1875 * m6 + 0.21875 * m8,
              a6 = 0.03125 * m6 + 0.0625 * m8;

            return (
              a0 * lat0 -
              Math.Sin(lat0) *
                Math.Cos(lat0) *
                (a2 - a4 + a6 + (2 * a4 - (16.0 / 3.0) * a6) * Math.Pow(Math.Sin(lat0), 2) + (16.0 / 3.0) * a6 * Math.Pow(Math.Sin(lat0), 4))
            );
        }

        /// <summary>
        /// Calculate parameters for Affine transformation.
        /// </summary>
        /// <param name="numberPoints"></param>
        /// <param name="Xg"></param>
        /// <param name="Yg"></param>
        /// <param name="xl"></param>
        /// <param name="yl"></param>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>Next avaliable entity number.</returns>
        public static void GetAffineTransformationParameters
          (int numberPoints, List<double> xl, List<double> yl, List<double> Xg, List<double> Yg,
          out double a1, out double a2, out double b1, out double b2, out double c1, out double c2)
        {
            try
            {
                int i;
                double xcg, ycg, xcl, ycl;

                a1 = b1 = a2 = b2 = c1 = c2 = 0;

                if (numberPoints < 3) return;

                xcg = ycg = xcl = ycl = 0;
                for (i = 0; i < numberPoints; i++)
                {
                    xcg = xcg + Xg[i];
                    ycg = ycg + Yg[i];
                    xcl = xcl + xl[i];
                    ycl = ycl + yl[i];
                }
                xcg /= numberPoints;
                ycg /= numberPoints;
                xcl /= numberPoints;
                ycl /= numberPoints;
                for (i = 0; i < numberPoints; i++)
                {
                    Xg[i] -= xcg;
                    Yg[i] -= ycg;
                    xl[i] -= xcl;
                    yl[i] -= ycl;
                }
                double x1 = 0; double y1 = 0;
                double x2 = 0; double y2 = 0;
                double x3 = 0; double y3 = 0;
                double x4 = 0; double n1 = 0;
                for (i = 0; i < numberPoints; i++)
                {
                    x1 += xl[i] * Xg[i];
                    y1 += yl[i] * Xg[i];
                    x2 += xl[i] * xl[i];
                    y2 += yl[i] * Yg[i];
                    x3 += xl[i] * Yg[i];
                    y3 += yl[i] * yl[i];
                    x4 += xl[i] * yl[i];
                }
                a1 = (x1 * y3) - (y1 * x4);
                b1 = (y1 * x2) - (x1 * x4);
                a2 = (y2 * x2) - (x3 * x4);
                b2 = (x3 * y3) - (y2 * x4);
                n1 = (x2 * y3) - (x4 * x4);
                a1 /= n1;
                b1 /= n1;
                a2 /= n1;
                b2 /= n1;
                c1 = xcg - (a1 * xcl) - (b1 * ycl);
                c2 = ycg - (a2 * ycl) - (b2 * xcl);
            }
            catch
            {
                a1 = b1 = a2 = b2 = c1 = c2 = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberPoints"></param>
        /// <param name="inputPoints"></param>
        /// <param name="outputPoints"></param>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        public static void CalculateAffineTransformationParameters
          (int numberPoints, List<GeoPoint> inputPoints, List<GeoPoint> outputPoints,
          out double a1, out double a2, out double b1, out double b2, out double c1, out double c2)
        {
            Helpers.GetAffineTransformationParameters(numberPoints, inputPoints.Select(p => p.X).ToList(), inputPoints.Select(p => p.Y).ToList(), outputPoints.Select(p => p.X).ToList(), outputPoints.Select(p => p.Y).ToList(), out a1, out a2, out b1, out b2, out c1, out c2);
        }
    }
}
