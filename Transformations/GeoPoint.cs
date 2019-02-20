using System;
using KDBush;

namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represents a 2D point
    /// </summary>
    public class GeoPoint
    {
        /// <summary>
        /// Unique ID of a point, -1 by default
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// The value has different meaning in different coordinate systems:
        /// - Geographic: represents Latitude
        /// - Lambert: represents Northing
        /// - UTM: represents Northing
        /// - Old BGS: represents X
        /// - Cartesian: y
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// The value has different meaning in different coordinate systems:
        /// - Geographic: represents Longitude
        /// - Lambert: represents Easting
        /// - UTM: represents Easting
        /// - Old BGS: represents Y
        /// - Cartesian: x
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Z coordinate of a point:
        /// - in geocentric: Z
        /// - in others: Elevation
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Create a new Point with (0, 0, 0) coordinates
        /// </summary>
        public GeoPoint() : this(-1, 0.0, 0.0, 0.0) { }

        /// <summary>
        /// Create a new Point
        /// </summary>
        /// <param name="x">The value has different meaning in different coordinate systems:
        /// - Geographic: represents Latitude
        /// - Lambert: represents Northing
        /// - UTM: represents Northing
        /// - Old BGS: represents X
        /// - Cartesian: y
        /// </param>
        /// <param name="y">The value has different meaning in different coordinate systems:
        /// - Geographic: represents Longitude
        /// - Lambert: represents Easting
        /// - UTM: represents Easting
        /// - Old BGS: represents Y
        /// - Cartesian: x
        /// </param>
        public GeoPoint(double x, double y) : this(-1, x, y, 0.0) { }

        /// <summary>
        /// Create a new Point
        /// </summary>
        /// <param name="x">The value has different meaning in different coordinate systems:
        /// - Geographic: represents Latitude
        /// - Lambert: represents Northing
        /// - UTM: represents Northing
        /// - Old BGS: represents X
        /// - Cartesian: y
        /// </param>
        /// <param name="y">The value has different meaning in different coordinate systems:
        /// - Geographic: represents Longitude
        /// - Lambert: represents Easting
        /// - UTM: represents Easting
        /// - Old BGS: represents Y
        /// - Cartesian: x
        /// </param>
        /// <param name="z">Z coordinate of a point:
        /// - in geocentric: Z
        /// - in others: Elevation
        /// </param>
        public GeoPoint(double x, double y, double z) : this(-1, x, y, z) { }

        /// <summary>
        /// Create a new Point with ID
        /// </summary>
        /// <param name="id">Unique ID of a point</param>
        /// <param name="x">The value has different meaning in different coordinate systems:
        /// - Geographic: represents Latitude
        /// - Lambert: represents Northing
        /// - UTM: represents Northing
        /// - Old BGS: represents X
        /// - Cartesian: y
        /// </param>
        /// <param name="y">The value has different meaning in different coordinate systems:
        /// - Geographic: represents Longitude
        /// - Lambert: represents Easting
        /// - UTM: represents Easting
        /// - Old BGS: represents Y
        /// - Cartesian: x
        /// </param>
        /// <param name="z">Z coordinate of a point:
        /// - in geocentric: Z
        /// - in others: Elevation
        /// </param>
        public GeoPoint(int id, double x, double y, double z)
        {
            this.ID = id;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Converts this point to KDBush.Point(this)
        /// </summary>
        /// <returns></returns>
        public Point<GeoPoint> ToPoint()
        {
            return new Point<GeoPoint>(this.X, this.Y, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("ID: {0}, X: {1}, Y: {2}, Z: {3}", this.ID, this.X, this.Y, this.Z);
        }
    }
}
