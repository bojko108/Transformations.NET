using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BojkoSoft.Transformations.TransformationModels
{
    /// <summary>
    /// Class for transforming points using Affine transformation
    /// </summary>
    internal class AffineTransformation : ITransformation
    {
        private double a1 { get; set; }
        private double a2 { get; set; }
        private double b1 { get; set; }
        private double b2 { get; set; }
        private double c1 { get; set; }
        private double c2 { get; set; }
        
        /// <summary>
        /// Creates a new Affine transformation based on input source and target points
        /// </summary>
        /// <param name="sourcePoints">used to transform from</param>
        /// <param name="targetPoints">used to transform to</param>
        public AffineTransformation(List<GeoPoint> sourcePoints, List<GeoPoint> targetPoints)
        {
            this.CalculateAffineParameters(sourcePoints.ConvertAll(p => p.Clone()), targetPoints.ConvertAll(p => p.Clone()));
        }

        /// <summary>
        /// Transforms a point using Affine transformation
        /// </summary>
        /// <param name="inputPoint"></param>
        /// <returns></returns>
        public GeoPoint Transform(GeoPoint inputPoint)
        {
            GeoPoint resultPoint = new GeoPoint();
            resultPoint.X = this.a1 * inputPoint.X + this.b1 * inputPoint.Y + this.c1;
            resultPoint.Y = this.b2 * inputPoint.X + this.a2 * inputPoint.Y + this.c2;
            return resultPoint;
        }
        /// <summary>
        /// Calculates Affine transformation parameters
        /// </summary>
        /// <param name="sourcePoints"></param>
        /// <param name="targetPoints"></param>
        private void CalculateAffineParameters(List<GeoPoint> sourcePoints, List<GeoPoint> targetPoints)
        {
            int i;
            double xcg = 0.0, ycg = 0.0, xcl = 0.0, ycl = 0.0;

            if (sourcePoints.Count < 3) throw new Exception("Need at least 3 points!");
            if (sourcePoints.Count != targetPoints.Count) throw new Exception("Source and target points do not match!");

            for (i = 0; i < sourcePoints.Count; i++)
            {
                xcg = xcg + targetPoints[i].X;
                ycg = ycg + targetPoints[i].Y;
                xcl = xcl + sourcePoints[i].X;
                ycl = ycl + sourcePoints[i].Y;
            }
            xcg /= sourcePoints.Count;
            ycg /= sourcePoints.Count;
            xcl /= sourcePoints.Count;
            ycl /= sourcePoints.Count;
            for (i = 0; i < sourcePoints.Count; i++)
            {
                targetPoints[i].X -= xcg;
                targetPoints[i].Y -= ycg;
                sourcePoints[i].X -= xcl;
                sourcePoints[i].Y -= ycl;
            }
            double x1 = 0; double y1 = 0;
            double x2 = 0; double y2 = 0;
            double x3 = 0; double y3 = 0;
            double x4 = 0; double n1 = 0;
            for (i = 0; i < sourcePoints.Count; i++)
            {
                x1 += sourcePoints[i].X * targetPoints[i].X;
                y1 += sourcePoints[i].Y * targetPoints[i].X;
                x2 += sourcePoints[i].X * sourcePoints[i].X;
                y2 += sourcePoints[i].Y * targetPoints[i].Y;
                x3 += sourcePoints[i].X * targetPoints[i].Y;
                y3 += sourcePoints[i].Y * sourcePoints[i].Y;
                x4 += sourcePoints[i].X * sourcePoints[i].Y;
            }
            this.a1 = (x1 * y3) - (y1 * x4);
            this.b1 = (y1 * x2) - (x1 * x4);
            this.a2 = (y2 * x2) - (x3 * x4);
            this.b2 = (x3 * y3) - (y2 * x4);
            n1 = (x2 * y3) - (x4 * x4);
            this.a1 /= n1;
            this.b1 /= n1;
            this.a2 /= n1;
            this.b2 /= n1;
            this.c1 = xcg - (this.a1 * xcl) - (this.b1 * ycl);
            this.c2 = ycg - (this.a2 * ycl) - (this.b2 * xcl);
        }
    }
}
