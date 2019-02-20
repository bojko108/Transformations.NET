using System.Collections.Generic;
using System.Linq;
using KDBush;

namespace BojkoSoft.Transformations.ControlPoints
{
    internal class ControlPointsClass
    {
        internal KDBush<GeoPoint> tree;
        internal List<Point<GeoPoint>> points;

        /// <summary>
        /// Get a control point by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GeoPoint GetPoint(int id)
        {
            for (int i = 0; i < this.points.Count; i++)
            {
                if (this.points[i].UserData.ID == id)
                {
                    return this.points[i].UserData;
                }
            }

            return null;
        }

        /// <summary>
        /// Get a set of control points by IDs 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<GeoPoint> GetPoints(int[] ids)
        {
            List<GeoPoint> result = new List<GeoPoint>();

            for (int i = 0; i < this.points.Count; i++)
            {
                if (ids.Contains(this.points[i].UserData.ID))
                {
                    result.Add(this.points[i].UserData);
                }
            }

            return result.OrderBy(p => p.ID).ToList();
        }

        /// <summary>
        /// Get a set of control points located inside a circle with radius
        /// </summary>
        /// <param name="point">center of circle</param>
        /// <param name="radius">radius of circle</param>
        /// <returns></returns>
        public List<GeoPoint> GetPoints(GeoPoint point, double radius = 20000)
        {
            List<Point<GeoPoint>> queried = this.tree.Query(point.X, point.Y, radius);
            return queried.Select(p => p.UserData).OrderBy(p => p.ID).ToList();
        }

        /// <summary>
        /// Get a set of control points located inside an extent 
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        public List<GeoPoint> GetPoints(GeoExtent extent)
        {
            List<Point<GeoPoint>> queried = this.tree.Query(extent.SouthWestCorner.X, extent.SouthWestCorner.Y, extent.NorthEastCorner.X, extent.NorthEastCorner.Y);
            return queried.Select(p => p.UserData).OrderBy(p => p.ID).ToList();
        }

        internal void InitTree()
        {
            this.tree = new KDBush<GeoPoint>();
            this.tree.Index(this.points);
        }
    }
}
