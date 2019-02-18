using System;
using System.Collections;
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
            this.ellipsoids.Add(enumEllipsoids.WGS84, new Ellipsoid(6378137.0, 6356752.314245));
            //export const WGS84 = {
            //  a: 6378137.0,
            //  b: 6356752.314245,
            //  n: 0.0016792203863978585,
            //  e: 0.08181919084296556,
            //  е2: 0.0066943799901976195,
            //  ep2: 0.0067394967423335395,
            //  invFlatt: 298.257223563,
            //  name: 'WGS 84'
            //};
            
            this.ellipsoids.Add(enumEllipsoids.GRS80, new Ellipsoid(6378137.0, 6356752.31414));
            //export const GRS80 = {
            //  a: 6378137.0,
            //  b: 6356752.31414,
            //  n: 0.0016792203946567046,
            //  e: 0.08181919104349496,
            //  e2: 0.0066943800230119255,
            //  ep2: 0.006739496775591602,
            //  invFlatt: 298.257222101,
            //  name: 'GRS 1980'
            //};

            this.ellipsoids.Add(enumEllipsoids.SPHERE, new Ellipsoid(6378137.0, 6378137.0));
            //export const SPHERE = {
            //  a: 6378137.0,
            //  b: 6378137.0,
            //  n: 0,
            //  e: 0,
            //  е2: 0,
            //  ep2: 0,
            //  name: 'Normal Sphere (r=6378137)'
            //};

            this.ellipsoids.Add(enumEllipsoids.BESSEL_1841, new Ellipsoid(6377397.155, 6356078.963));
            //export const BESSEL_1841 = {
            //  a: 6377397.155,
            //  b: 6356078.963,
            //  n: 0.0016741847868128074,
            //  e: 0.0816968308747341,
            //  e2: 0.006674372174974907,
            //  ep2: 0.006719218741581212,
            //  invFlatt: 299.1528128,
            //  name: 'Bessel 1841'
            //};

            this.ellipsoids.Add(enumEllipsoids.CLARKE_1866, new Ellipsoid(6378206.4, 6356583.8));
            //export const CLARKE_1866 = {
            //  a: 6378206.4,
            //  b: 6356583.8,
            //  n: 0.0016979156829769022,
            //  e: 0.0822718542230039,
            //  e2: 0.0067686579972912045,
            //  ep2: 0.006814784945915253,
            //  invFlatt: 294.978698214,
            //  name: 'Clarke 1866'
            //};

            this.ellipsoids.Add(enumEllipsoids.EVEREST, new Ellipsoid(6377298.556, 6356097.55));
            //export const EVEREST = {
            //  a: 6377298.556,
            //  b: 6356097.55,
            //  n: 0.001664992263141025,
            //  e: 0.0814729815598449,
            //  e2: 0.006637846724250828,
            //  ep2: 0.0066822021579557855,
            //  invFlatt: 300.8017,
            //  name: 'Everest (Sabah & Sarawak)'
            //};

            this.ellipsoids.Add(enumEllipsoids.HELMERT, new Ellipsoid(6378200.0, 6356818.17));
            //export const HELMERT = {
            //  a: 6378200.0,
            //  b: 6356818.17,
            //  n: 0.0016789791513897837,
            //  e: 0.08181333330622664,
            //  e2: 0.006693421506675734,
            //  ep2: 0.006738525296820841,
            //  invFlatt: 298.3,
            //  name: 'Helmert 1906'
            //};

            this.ellipsoids.Add(enumEllipsoids.HAYFORD, new Ellipsoid(6378388, 6356911.946));
            //export const HAYFORD = {
            //  a: 6378388,
            //  b: 6356911.946,
            //  n: 0.0016863406508729228,
            //  e: 0.0819918902228546,
            //  ep2: 0.00676817023775067,
            //  invFlatt: 297.0,
            //  name: 'Hayford'
            //};

            this.ellipsoids.Add(enumEllipsoids.KRASSOVSKY, new Ellipsoid(6378245.0, 6356863.019));
            //export const KRASSOVSKY = {
            //  a: 6378245.0,
            //  b: 6356863.019,
            //  n: 0.0016789791628071841,
            //  e: 0.0818133335834678,
            //  е2: 0.006722670062316639,
            //  invFlatt: 298.3,
            //  name: 'Krassovsky, 1942'
            //};

            this.ellipsoids.Add(enumEllipsoids.WALBECK, new Ellipsoid(6376896.0, 6355834.8467));
            //export const WALBECK = {
            //  a: 6376896.0,
            //  b: 6355834.8467,
            //  n: 0.0016540955395643873,
            //  e: 0.08120682292886269,
            //  е2: 0.00659454809019966,
            //  ep2: 0.006738525342798285,
            //  invFlatt: 302.78,
            //  name: 'Walbeck'
            //};

            this.ellipsoids.Add(enumEllipsoids.WGS72, new Ellipsoid(6378135.0, 6356750.52));
            //export const WGS72 = {
            //  a: 6378135.0,
            //  b: 6356750.52,
            //  n: 0.001679204729906394,
            //  e: 0.08181881069348491,
            //  е2: 0.006694317783296321,
            //  ep2: 0.0067394336941242194,
            //  invFlatt: 298.26,
            //  name: 'WGS 72'
            //};

            
        }
    }
}