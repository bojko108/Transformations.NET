namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represents an extent
    /// </summary>
    public interface IExtent
    {
        /// <summary>
        /// Maximum Northing (x)
        /// </summary>
        double MinN { get; set; }
        /// <summary>
        /// Minimum Easting (y)
        /// </summary>
        double MinE { get; set; }
        /// <summary>
        /// Maximum Northing (x)
        /// </summary>
        double MaxN { get; set; }
        /// <summary>
        /// Minimum Easting (y)
        /// </summary>
        double MaxE { get; set; }
        /// <summary>
        /// Represents the width of this extent in meters
        /// </summary>
        double Width { get; }
        /// <summary>
        /// Represents the height of this extent in meters
        /// </summary>
        double Height { get; }
        /// <summary>
        /// Returns True if this extent is empty: width and height are 0
        /// </summary>
        bool IsEmpty { get; }
        /// <summary>
        /// Increases this extent with provided value in meters
        /// </summary>
        /// <param name="meters">you can use negative values to shrink the extent</param>
        void Expand(double meters);
        /// <summary>
        /// Clones this object
        /// </summary>
        /// <returns>new IExtent(this.NorthEastCorner.Clone(), this.SouthWestCorner.Clone())</returns>
        IExtent Clone();
    }
}
