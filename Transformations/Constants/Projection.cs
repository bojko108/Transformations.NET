namespace BojkoSoft.Transformations.Constants
{
    /// <summary>
    /// Class for creating a Projection
    /// </summary>
    public class Projection
    {
        /// <summary>
        /// Latitude of first standard parallel
        /// </summary>
        public double Lat1 { get; private set; }
        /// <summary>
        /// Latitude of second standard parallel
        /// </summary>
        public double Lat2 { get; private set; }
        /// <summary>
        /// Latitude of false origin
        /// </summary>
        public double Lat0 { get; private set; }
        /// <summary>
        /// Longitude of false origin
        /// </summary>
        public double Lon0 { get; private set; }
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
        /// Creates a Lambert Conformal Conic Projection with 2SP
        /// </summary>
        /// <param name="lat1">Latitude of first standard parallel</param>
        /// <param name="lat2">Latitude of second standard parallel</param>
        /// <param name="lat0">Latitude of false origin</param>
        /// <param name="lon0">Longitude of false origin</param>
        /// <param name="x0">False Easting in meters</param>
        /// <param name="y0">False Easting in meters</param>
        /// <param name="scale">Projection scale</param>
        public Projection(double lat1, double lat2, double lat0, double lon0, double x0, double y0, double scale)
        {
            this.Lat1 = lat1;
            this.Lat2 = lat2;
            this.Lat0 = lat0;
            this.Lon0 = lon0;
            this.X0 = x0;
            this.Y0 = y0;
            this.Scale = scale;
        }

        /// <summary>
        /// Creates a new Gauss or UTM Projection
        /// </summary>
        /// <param name="lon0">Latitude of central meridian</param>
        /// <param name="y0">False Easting in meters</param>
        /// <param name="scale">Scale</param>
        public Projection(int lon0, double y0, double scale)
        {
            this.Lon0 = lon0;
            this.Y0 = y0;
            this.Scale = scale;
        }

        /// <summary>
        /// Creates a new Projection
        /// </summary>
        /// <param name="x0">False Northing in meters</param>
        /// <param name="y0">False Easting in meters</param>
        public Projection(double x0, double y0)
        {
            this.X0 = x0;
            this.Y0 = y0;
        }

        /// <summary>
        /// Creates a new Projection
        /// </summary>
        /// <param name="name">Projection name</param>
        public Projection(string name) { }
    }
}
