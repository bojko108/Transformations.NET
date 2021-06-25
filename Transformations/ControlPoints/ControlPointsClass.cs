using System.Collections.Generic;
using System.Linq;
using KDBush;

namespace BojkoSoft.Transformations.ControlPoints
{
    internal class ControlPointsClass
    {
        internal KDBush<ControlPoint> tree;
        internal List<Point<ControlPoint>> points;

        /// <summary>
        /// Get a control point by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IPoint GetPoint(int id)
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
        public List<ControlPoint> GetPoints(int[] ids)
        {
            List<ControlPoint> result = new List<ControlPoint>();

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
        public List<ControlPoint> GetPoints(IPoint point, double radius = 20000)
        {
            List<Point<ControlPoint>> queried = this.tree.Query(point.N, point.E, radius);
            return queried.Select(p => p.UserData).OrderBy(p => p.ID).ToList();
        }

        /// <summary>
        /// Get a set of control points located inside an extent 
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        public List<ControlPoint> GetPoints(IExtent extent)
        {
            List<Point<ControlPoint>> queried = this.tree.Query(extent.MinN, extent.MinE, extent.MaxN, extent.MaxE);
            return queried.Select(p => p.UserData).OrderBy(p => p.ID).ToList();
        }

        internal void InitTree()
        {
            this.tree = new KDBush<ControlPoint>();
            this.tree.Index(this.points);
        }
    }
}
