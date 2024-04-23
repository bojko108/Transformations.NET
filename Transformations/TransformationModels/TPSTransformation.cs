using System;
using System.Collections.Generic;

namespace BojkoSoft.Transformations.TransformationModels
{
    /// <summary>
    /// Class for transforming points using Thin Plate Splite (TPS) transformation
    /// </summary>
    internal class TPSTransformation : ITransformation
    {
        private int m { get; set; }
        private double[] Xc { get; set; }
        private double[] Yc { get; set; }
        private List<IPoint> SourcePoints { get; set; }

        /// <summary>
        /// Calcualtes TPS transformation based on input points
        /// </summary>
        /// <param name="sourcePoints">used to transform from</param>
        /// <param name="targetPoints">used to transform to</param>
        public TPSTransformation(List<IPoint> sourcePoints, List<IPoint> targetPoints)
        {
            this.SourcePoints = sourcePoints.ConvertAll(p => p.Clone());
            this.CalculateTPSParameters(targetPoints.ConvertAll(p => p.Clone()));
        }

        /// <summary>
        /// Transforms a point using TPS transformation
        /// </summary>
        /// <param name="inputPoint"></param>
        /// <returns></returns>
        public IPoint Transform(IPoint inputPoint)
        {
            double Xo = this.Xc[0] + this.Xc[1] * inputPoint.N + this.Xc[2] * inputPoint.E,
              Yo = this.Yc[0] + this.Yc[1] * inputPoint.N + this.Yc[2] * inputPoint.E;

            for (int r = 0; r < this.m; r++)
            {
                double tmp = this.kernelFunction(inputPoint, this.SourcePoints[r]);
                Xo += this.Xc[r + 3] * tmp;
                Yo += this.Yc[r + 3] * tmp;
            }

            return new ControlPoint(Xo, Yo);
        }

        /// <summary>
        /// Radial base function - r^2 * log(r)
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private double kernelFunction(IPoint first, IPoint second)
        {
            if (first.N == second.N && first.E == second.E) return 0;
            double dist = (second.N - first.N) * (second.N - first.N) + (second.E - first.E) * (second.E - first.E);
            return dist * Math.Log(dist);
        }

        /// <summary>
        /// Calculates TPS transformation parameters
        /// </summary>
        /// <param name="targetPoints"></param>
        private void CalculateTPSParameters(List<IPoint> targetPoints)
        {
            if (this.SourcePoints.Count != targetPoints.Count) throw new Exception("Source and target points do not match!");

            this.m = this.SourcePoints.Count;

            Matrix A = new Matrix(this.m + 3, this.m + 3);

            for (int i = 0; i < this.m; i++)
            {
                // top right part of matrix
                A[0, 3 + i] = 1;
                A[1, 3 + i] = this.SourcePoints[i].N;
                A[2, 3 + i] = this.SourcePoints[i].E;

                // bottom left part of matrix
                A[3 + i, 0] = 1;
                A[3 + i, 1] = this.SourcePoints[i].N;
                A[3 + i, 2] = this.SourcePoints[i].E;
            }

            // the lower right part of the matrix
            for (int r = 0; r < this.m; r++)
            {
                for (int c = 0; c < this.m; c++)
                {
                    A[r + 3, c + 3] = this.kernelFunction(this.SourcePoints[r], this.SourcePoints[c]);
                    A[c + 3, r + 3] = A[r + 3, c + 3];
                }
            }

            Matrix invertedA = A.Invert();

            // compute arrays
            this.Xc = new double[this.m + 3];
            this.Yc = new double[this.m + 3];

            for (int r = 0; r < this.m + 3; r++)
            {
                for (int c = 0; c < this.m; c++)
                {
                    this.Xc[r] += invertedA[r, c + 3] * targetPoints[c].N;
                    this.Yc[r] += invertedA[r, c + 3] * targetPoints[c].E;
                }
            }
        }
    }
}


