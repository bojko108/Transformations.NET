using System;
using KDBush;

namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represents a 2D point
    /// </summary>
    internal class ControlPoint : IPoint
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
        public double N { get; set; }
        /// <summary>
        /// The value has different meaning in different coordinate systems:
        /// - Geographic: represents Longitude
        /// - Lambert: represents Easting
        /// - UTM: represents Easting
        /// - Old BGS: represents Y
        /// - Cartesian: x
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// Z coordinate of a point:
        /// - in geocentric: Z
        /// - in others: Elevation
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Create a new Point with (0, 0, 0) coordinates
        /// </summary>
        public ControlPoint() : this(-1, 0.0, 0.0, 0.0) { }

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
        public ControlPoint(double x, double y) : this(-1, x, y, 0.0) { }

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
        public ControlPoint(double x, double y, double z) : this(-1, x, y, z) { }

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
        public ControlPoint(int id, double x, double y, double z)
        {
            this.ID = id;
            this.N = x;
            this.E = y;
            this.Z = z;
        }

        /// <summary>
        /// Converts this point to KDBush.Point(this)
        /// </summary>
        /// <returns></returns>
        public Point<ControlPoint> ToPoint()
        {
            return new Point<ControlPoint>(this.N, this.E, this);
        }

        /// <summary>
        /// Clone this point
        /// </summary>
        /// <returns>new IPoint</returns>
        public IPoint Clone()
        {
            return new ControlPoint(this.ID, this.N, this.E, this.Z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("ID: {0}, N: {1}, E: {2}, Z: {3}", this.ID, this.N, this.E, this.Z);
        }
    }
}
