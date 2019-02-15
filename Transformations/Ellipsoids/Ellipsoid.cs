using System;

namespace BojkoSoft.Transformations.Ellipsoids
{
    /// <summary>
    /// Represents ellipsoids.
    /// Parameters are calculated as follows:
    /// f = (a - b) / a
    /// invFlatt = 1 / f
    /// b = a * (1 - f)
    /// n = (a - b) / (a + b)
    /// e2 = (Math.pow(a, 2) - Math.pow(b, 2)) / (Math.pow(a, 2)))
    /// e = Math.sqrt(e2)
    /// ep2 = Math.sqrt((Math.pow(a, 2) - Math.pow(b, 2)) / (Math.pow(b, 2))))
    /// </summary>
    public class Ellipsoid
    {
        /// <summary>
        /// semi major axis
        /// </summary>
        public double a { get; private set; }
        /// <summary>
        /// semi minor axis
        /// </summary>
        public double b { get; private set; }
        /// <summary>
        /// first eccentricity
        /// </summary>
        public double e { get; private set; }
        /// <summary>
        /// first eccentricity squared
        /// </summary>
        public double e2 { get; private set; }
        /// <summary>
        /// second eccentricity squared
        /// </summary>
        public double ep2 { get; private set; }

        /// <summary>
        /// Creates a new ellipsoid with semi major and semi minor axis values
        /// </summary>
        /// <param name="a">semi major axis</param>
        /// <param name="b">semi minor axis</param>
        public Ellipsoid(double a, double b)
        {
            this.a = a;
            this.b = b;
            this.e2 = (Math.Pow(this.a, 2.0) - Math.Pow(this.b, 2.0)) / Math.Pow(this.a, 2.0);
            this.e = Math.Sqrt(this.e2);
            this.ep2 = (Math.Pow(this.a, 2.0) - Math.Pow(this.b, 2.0)) / Math.Pow(this.b, 2.0);
        }
    }
}
