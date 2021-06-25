namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represents a point geometry, ready for transformation
    /// </summary>
    public interface IPoint
    {
        /// <summary>
        /// The value has different meaning in different coordinate systems:
        /// - Geographic: represents Latitude
        /// - Lambert: represents Northing
        /// - UTM: represents Northing
        /// - Old BGS: represents X
        /// - Cartesian: y
        /// </summary>
        double N { get; set; }
        /// <summary>
        /// The value has different meaning in different coordinate systems:
        /// - Geographic: represents Longitude
        /// - Lambert: represents Easting
        /// - UTM: represents Easting
        /// - Old BGS: represents Y
        /// - Cartesian: x
        /// </summary>
        double E { get; set; }
        /// <summary>
        /// Z coordinate of a point:
        /// - in geocentric: Z
        /// - in others: Elevation
        /// </summary>
        double Z { get; set; }
        /// <summary>
        /// Clones this object
        /// </summary>
        /// <returns>new IPoint(this.N, this.E, this.Z)</returns>
        IPoint Clone();
    }
}
