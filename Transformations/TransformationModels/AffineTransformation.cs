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
    public class AffineTransformation : ITransformation
    {
        /// <summary>
        /// x-component of the pixel width (x-scale)
        /// </summary>
        public double A { get; private set; }
        /// <summary>
        /// y-component of the pixel height (y-scale), typically negative
        /// </summary>
        public double E { get; private set; }
        /// <summary>
        /// rotation about x-axis
        /// </summary>
        public double B { get; private set; }
        /// <summary>
        /// y-component of the pixel width (y-skew)
        /// </summary>
        public double D { get; private set; }
        /// <summary>
        /// x-coordinate of the center of the upper left pixel
        /// </summary>
        public double C { get; private set; }
        /// <summary>
        /// y-coordinate of the center of the upper left pixel
        /// </summary>
        public double F { get; private set; }

        /// <summary>
        /// Creates a new Affine transformation
        /// </summary>
        public AffineTransformation() : this(new double[6] { 1, 1, 1, 1, 0, 0 }) { }

        /// <summary>
        /// Creates a new Affine transfromation with provided parameters
        /// </summary>
        /// <param name="parameters">List of transformation parameters - https://en.wikipedia.org/wiki/World_file#Definition</param>
        public AffineTransformation(double[] parameters)
        {
            this.SetParameters(parameters);
        }

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
            GeoPoint resultPoint = new GeoPoint
            {
                X = this.A * inputPoint.X + this.B * inputPoint.Y + this.C,
                Y = this.D * inputPoint.X + this.E * inputPoint.Y + this.F
            };
            return resultPoint;
        }

        /// <summary>
        /// Returns calculated parameters. Use this method to save the prameters for later use.
        /// </summary>
        /// <returns>Affine transformation parameters</returns>
        public double[] GetParameters()
        {
            return new double[6] { this.A, this.E, this.B, this.D, this.C, this.F };
        }

        /// <summary>
        /// Returns calculated parameters. Use this method to save the prameters in a world file - https://en.wikipedia.org/wiki/World_file
        /// </summary>
        /// <returns>parameters as text</returns>
        public string GetParametersAsText()
        {
            return $"{this.A}\n{this.E}\n{this.B}\n{this.D}\n{this.C}\n{this.F}";
        }

        /// <summary>
        /// Sets the transformation parameters
        /// </summary>
        /// <param name="parameters">List of transformation parameters (A, E, B, D, C, F) - https://en.wikipedia.org/wiki/World_file#Definition</param>
        public void SetParameters(double[] parameters)
        {
            if (parameters.Length != 6)
            {
                throw new ArgumentException("Length should be 6: A, E, B, D, C, F");
            }

            this.A = parameters[0];
            this.E = parameters[1];
            this.B = parameters[2];
            this.D = parameters[3];
            this.C = parameters[4];
            this.F = parameters[5];
        }

        /// <summary>
        /// Sets the transformation parameters from provided workd file text - https://en.wikipedia.org/wiki/World_file
        /// </summary>
        /// <param name="text">world file text</param>
        public void SetParameters(string text)
        {
            string[] parameters = text.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            this.SetParameters(Array.ConvertAll(parameters, Double.Parse));
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
            this.A = (x1 * y3) - (y1 * x4);
            this.B = (y1 * x2) - (x1 * x4);
            this.E = (y2 * x2) - (x3 * x4);
            this.D = (x3 * y3) - (y2 * x4);
            n1 = (x2 * y3) - (x4 * x4);
            this.A /= n1;
            this.B /= n1;
            this.E /= n1;
            this.D /= n1;
            this.C = xcg - (this.A * xcl) - (this.B * ycl);
            this.F = ycg - (this.E * ycl) - (this.D * xcl);
        }
    }
}
