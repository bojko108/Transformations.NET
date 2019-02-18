//using System;

//using BojkoSoft.Transformations.Ellipsoids;

//namespace BojkoSoft.Transformations.Projections
//{
//    /// <summary>
//    /// Bulgarian KKS2005. Uses Lambert Conformal Conic with 2SP
//    /// </summary>
//    public class ProjectionKK2005 : ProjectionClass
//    {
//        /// <summary>
//        /// Ellipsoid on which this projection is used
//        /// </summary>
//        public Ellipsoid Ellipsoid { get; private set; }

//        // some additional coefficients
//        public double w1 { get; private set; }
//        public double w2 { get; private set; }
//        public double Q1 { get; private set; }
//        public double Q2 { get; private set; }

//        /// <summary>
//        /// Latitude of first standard parallel
//        /// </summary>
//        public double Lat1 { get; private set; }
//        /// <summary>
//        /// Latitude of second standard parallel
//        /// </summary>
//        public double Lat2 { get; private set; }
//        /// <summary>
//        /// Latitude of false origin
//        /// </summary>
//        public double Lat0 { get; private set; }
//        /// <summary>
//        /// Longitude of false origin
//        /// </summary>
//        public double Lon0 { get; private set; }

//        /// <summary>
//        /// Creates a new BGS KKS2005 projection
//        /// </summary>
//        /// <param name="ellipsoid"></param>
//        public ProjectionKK2005(Ellipsoid ellipsoid)
//            : base(0.0, 500000.0, 1.0)
//        {
//            this.Ellipsoid = ellipsoid;
//            this.Lat1 = 42.0;
//            this.Lat2 = 43.333333333333336;
//            this.Lat0 = 42.667875683333333;
//            this.Lon0 = 25.5;

//            this.w1 = Helpers.CalculateWParameter((this.Lat1 * Math.PI) / 180, this.Ellipsoid.e2);
//            this.w2 = Helpers.CalculateWParameter((this.Lat2 * Math.PI) / 180, this.Ellipsoid.e2);
//            this.Q1 = Helpers.CalculateQParameter((this.Lat1 * Math.PI) / 180, this.Ellipsoid.e);
//            this.Q2 = Helpers.CalculateQParameter((this.Lat2 * Math.PI) / 180, this.Ellipsoid.e);
//        }
//    }
//}
