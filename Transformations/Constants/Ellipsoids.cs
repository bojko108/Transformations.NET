using System.Collections.Generic;

namespace BojkoSoft.Transformations.Constants
{
    /// <summary>
    /// Types of ellipsoids
    /// </summary>
    public enum enumEllipsoids
    {
        /// <summary>
        /// GRS 1980
        /// </summary>
        GRS80,
        /// <summary>
        /// Bessel 1841
        /// </summary>
        BESSEL_1841,
        /// <summary>
        /// Clarke 1866
        /// </summary>
        CLARKE_1866,
        /// <summary>
        /// Everest (Sabah and Sarawak)
        /// </summary>
        EVEREST,
        /// <summary>
        /// Helmert 1906
        /// </summary>
        HELMERT,
        /// <summary>
        /// Hayford
        /// </summary>
        HAYFORD,
        /// <summary>
        /// Krassovsky, 1942
        /// </summary>
        KRASSOVSKY,
        /// <summary>
        /// Walbeck
        /// </summary>
        WALBECK,
        /// <summary>
        /// WGS 1972
        /// </summary>
        WGS72,
        /// <summary>
        /// WGS 1984
        /// </summary>
        WGS84,
        /// <summary>
        /// Normal Sphere (R=6378137)
        /// </summary>
        SPHERE
    }

    /// <summary>
    /// Class for accessing available ellipsoids
    /// </summary>
    public class Ellipsoids
    {
        /// <summary>
        /// Available ellipsoids
        /// </summary>
        private Dictionary<enumEllipsoids, Ellipsoid> ellipsoids;

        /// <summary>
        /// Available ellipsoids
        /// </summary>
        public Ellipsoids()
        {
            this.ellipsoids = new Dictionary<enumEllipsoids, Ellipsoid>();

            this.Init();
        }

        /// <summary>
        /// Get an ellipsoid
        /// </summary>
        /// <param name="ellipsoid">ellipsoid</param>
        /// <returns></returns>
        public Ellipsoid this[enumEllipsoids ellipsoid]
        {
            get
            {
                return this.ellipsoids[ellipsoid];
            }
        }

        /// <summary>
        /// Initialize all available ellipsoids
        /// </summary>
        private void Init()
        {
            
            this.ellipsoids.Add(enumEllipsoids.GRS80, new Ellipsoid(6378137.0, 6356752.31414));
            this.ellipsoids.Add(enumEllipsoids.BESSEL_1841, new Ellipsoid(6377397.155, 6356078.963));
            this.ellipsoids.Add(enumEllipsoids.CLARKE_1866, new Ellipsoid(6378206.4, 6356583.8));
            this.ellipsoids.Add(enumEllipsoids.EVEREST, new Ellipsoid(6377298.556, 6356097.55));
            this.ellipsoids.Add(enumEllipsoids.HELMERT, new Ellipsoid(6378200.0, 6356818.17));
            this.ellipsoids.Add(enumEllipsoids.HAYFORD, new Ellipsoid(6378388, 6356911.946));
            this.ellipsoids.Add(enumEllipsoids.KRASSOVSKY, new Ellipsoid(6378245.0, 6356863.019));
            this.ellipsoids.Add(enumEllipsoids.WALBECK, new Ellipsoid(6376896.0, 6355834.8467));
            this.ellipsoids.Add(enumEllipsoids.WGS72, new Ellipsoid(6378135.0, 6356750.52));
            this.ellipsoids.Add(enumEllipsoids.WGS84, new Ellipsoid(6378137.0, 6356752.314245));
            this.ellipsoids.Add(enumEllipsoids.SPHERE, new Ellipsoid(6378137.0, 6378137.0));
        }
    }
}