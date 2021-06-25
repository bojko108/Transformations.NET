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
    }
}
