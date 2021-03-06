﻿namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represents an extent
    /// </summary>
    public class GeoExtent
    {
        /// <summary>
        /// North east corner of the extent: max latitude and longitude
        /// </summary>
        public GeoPoint NorthEastCorner { get; private set; }
        /// <summary>
        /// South west corner of the extent: min latitude and longitude
        /// </summary>
        public GeoPoint SouthWestCorner { get; private set; }
        /// <summary>
        /// Represents the width of this extent in meters
        /// </summary>
        public double Width
        {
            get
            {
                return this.NorthEastCorner.Y - this.SouthWestCorner.Y;
            }
        }
        /// <summary>
        /// Represents the height of this extent in meters
        /// </summary>
        public double Height
        {
            get
            {
                return this.NorthEastCorner.X - this.SouthWestCorner.X;
            }
        }
        /// <summary>
        /// Returns True if this extent is empty: width and height are 0
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Width <= 0 && this.Height <= 0;
            }
        }

        /// <summary>
        /// Creates a new extent by specifying corner points
        /// </summary>
        /// <param name="northEastCorner"></param>
        /// <param name="southWestCorner"></param>
        public GeoExtent(GeoPoint northEastCorner, GeoPoint southWestCorner)
        {
            this.NorthEastCorner = northEastCorner.Clone();
            this.SouthWestCorner = southWestCorner.Clone();
        }

        /// <summary>
        /// Creates a new extent by specifying min and max values
        /// </summary>
        /// <param name="northingMax"></param>
        /// <param name="northingMin"></param>
        /// <param name="eastingMax"></param>
        /// <param name="eastingMin"></param>
        public GeoExtent(double northingMax, double northingMin, double eastingMax, double eastingMin)
        {
            this.NorthEastCorner = new GeoPoint(northingMax, eastingMax);
            this.SouthWestCorner = new GeoPoint(northingMin, eastingMin);
        }

        /// <summary>
        /// Increases this extent with provided value in meters
        /// </summary>
        /// <param name="meters">you can use negative values to shrink the extent</param>
        public void Expand(double meters)
        {
            this.NorthEastCorner.X += meters;
            this.NorthEastCorner.Y += meters;
            this.SouthWestCorner.X -= meters;
            this.SouthWestCorner.Y -= meters;
        }

        /// <summary>
        /// Clone this extent
        /// </summary>
        /// <returns>new GeoExtent</returns>
        public GeoExtent Clone()
        {
            return new GeoExtent(this.NorthEastCorner.Clone(), this.SouthWestCorner.Clone());
        }
    }
}
