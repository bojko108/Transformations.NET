namespace BojkoSoft.Transformations
{
    /// <summary>
    /// Represents an extent
    /// </summary>
    internal class GeoExtent : IExtent
    {
        /// <summary>
        /// Maximum Northing (x)
        /// </summary>
        public double MinN { get; set; }
        /// <summary>
        /// Minimum Easting (y)
        /// </summary>
        public double MinE { get; set; }
        /// <summary>
        /// Maximum Northing (x)
        /// </summary>
        public double MaxN { get; set; }
        /// <summary>
        /// Minimum Easting (y)
        /// </summary>
        public double MaxE { get; set; }
        /// <summary>
        /// Represents the width of this extent in meters
        /// </summary>
        public double Width => this.MaxE - this.MinE;
        /// <summary>
        /// Represents the height of this extent in meters
        /// </summary>
        public double Height => this.MaxN - this.MinN;
        /// <summary>
        /// Returns True if this extent is empty: width and height are 0
        /// </summary>
        public bool IsEmpty => this.Width <= 0 && this.Height <= 0;

        /// <summary>
        /// Creates a new extent by specifying min and max values
        /// </summary>
        /// <param name="northingMax"></param>
        /// <param name="northingMin"></param>
        /// <param name="eastingMax"></param>
        /// <param name="eastingMin"></param>
        public GeoExtent(double northingMax, double northingMin, double eastingMax, double eastingMin)
        {
            this.MaxN = northingMax;
            this.MinN = northingMin;
            this.MaxE = eastingMax;
            this.MinE = eastingMin;
        }

        /// <summary>
        /// Increases this extent with provided value in meters
        /// </summary>
        /// <param name="meters">you can use negative values to shrink the extent</param>
        public void Expand(double meters)
        {
            this.MaxN += meters;
            this.MaxE += meters;
            this.MinN -= meters;
            this.MinE -= meters;
        }

        /// <summary>
        /// Clone this extent
        /// </summary>
        /// <returns>new GeoExtent</returns>
        public IExtent Clone()
            => new GeoExtent(this.MaxN, this.MinN, this.MaxE, this.MinE);
    }
}
