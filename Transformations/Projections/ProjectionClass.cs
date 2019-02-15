namespace BojkoSoft.Transformations.Projections
{
    /// <summary>
    /// Base class for defining a Projection
    /// </summary>
    public class ProjectionClass
    {
        /// <summary>
        /// False Northing in meters
        /// </summary>
        public double X0 { get; private set; }
        /// <summary>
        /// False Easting in meters
        /// </summary>
        public double Y0 { get; private set; }
        /// <summary>
        /// Projection scale
        /// </summary>
        public double Scale { get; private set; }

        /// <summary>
        /// Creates a new Projection
        /// </summary>
        /// <param name="x0">False Northing in meters</param>
        /// <param name="y0">False Easting in meters</param>
        /// <param name="scale">Scale</param>
        public ProjectionClass(double x0, double y0, double scale)
        {
            this.X0 = x0;
            this.Y0 = y0;
            this.Scale = scale;
        }
    }
}
