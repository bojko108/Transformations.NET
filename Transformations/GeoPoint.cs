using System;

namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represets a point
    /// </summary>
    public class GeoPoint
    {
        /// <summary>
        /// X coordinate of a point. The value has different meaning in different coordinate systems:
        /// - WGS84: represents latitude
        /// - BGS2005 KK: represents northing
        /// - UTM: represents northing
        /// - KC1970: represents x
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// X coordinate of a point. The value has different meaning in different coordinate systems:
        /// - WGS84: represents latitude
        /// - BGS2005 KK: represents easting
        /// - UTM: represents northing
        /// - KC1970: represents x
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Create a new Point with 0, 0 coordinates
        /// </summary>
        public GeoPoint() : this(0.0, 0.0) { }

        /// <summary>
        /// Create a new Point
        /// </summary>
        /// <param name="x">X coordiante of a point. The value has different meaning in different coordinate systems:
        /// - WGS84: represents latitude
        /// - UTM: represents northing
        /// - KC1970: represents x
        /// </param>
        /// <param name="y">Y coordinate of a point. The value has different meaning in different coordinate systems:
        /// - WGS84: represents latitude
        /// - UTM: represents northing
        /// - KC1970: represents x
        /// </param>
        public GeoPoint(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
        
        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}", this.X, this.Y);
        }
    }
}
