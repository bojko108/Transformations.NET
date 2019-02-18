using System;

#pragma warning disable IDE1006

namespace BojkoSoft.Transformations.Constants
{
    /// <summary>
    /// Class for creating an ellipsoid
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
        /// flattening
        /// </summary>
        public double f { get; private set; }
        /// <summary>
        /// inverse flattening
        /// </summary>
        public double invFlatt { get; private set; }
        /// <summary>
        /// nnnnnnnnnnn
        /// </summary>
        public double n { get; private set; }
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

            this.f = (this.a - this.b) / this.a;
            this.invFlatt = 1.0 / this.f;
            this.n = (this.a - this.b) / (this.a + this.b);
            this.e2 = (Math.Pow(this.a, 2.0) - Math.Pow(this.b, 2.0)) / Math.Pow(this.a, 2.0);
            this.e = Math.Sqrt(this.e2);
            this.ep2 = (Math.Pow(this.a, 2.0) - Math.Pow(this.b, 2.0)) / Math.Pow(this.b, 2.0);
        }
    }
}
