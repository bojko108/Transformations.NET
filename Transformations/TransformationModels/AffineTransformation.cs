using System;
using System.Collections.Generic;

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
        public AffineTransformation(List<IPoint> sourcePoints, List<IPoint> targetPoints)
        {
            this.CalculateAffineParameters(sourcePoints.ConvertAll(p => p.Clone()), targetPoints.ConvertAll(p => p.Clone()));
        }

        /// <summary>
        /// Transforms a point using Affine transformation
        /// </summary>
        /// <param name="inputPoint"></param>
        /// <returns></returns>
        public IPoint Transform(IPoint inputPoint)
        {
            ControlPoint resultPoint = new ControlPoint
            {
                N = this.A * inputPoint.N + this.B * inputPoint.E + this.C,
                E = this.D * inputPoint.N + this.E * inputPoint.E + this.F
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
        private void CalculateAffineParameters(List<IPoint> sourcePoints, List<IPoint> targetPoints)
        {
            int i;
            double xcg = 0.0, ycg = 0.0, xcl = 0.0, ycl = 0.0;

            if (sourcePoints.Count < 3) throw new Exception("Need at least 3 points!");
            if (sourcePoints.Count != targetPoints.Count) throw new Exception("Source and target points do not match!");

            for (i = 0; i < sourcePoints.Count; i++)
            {
                xcg = xcg + targetPoints[i].N;
                ycg = ycg + targetPoints[i].E;
                xcl = xcl + sourcePoints[i].N;
                ycl = ycl + sourcePoints[i].E;
            }
            xcg /= sourcePoints.Count;
            ycg /= sourcePoints.Count;
            xcl /= sourcePoints.Count;
            ycl /= sourcePoints.Count;
            for (i = 0; i < sourcePoints.Count; i++)
            {
                targetPoints[i].N -= xcg;
                targetPoints[i].E -= ycg;
                sourcePoints[i].N -= xcl;
                sourcePoints[i].E -= ycl;
            }
            double x1 = 0; double y1 = 0;
            double x2 = 0; double y2 = 0;
            double x3 = 0; double y3 = 0;
            double x4 = 0; double n1 = 0;
            for (i = 0; i < sourcePoints.Count; i++)
            {
                x1 += sourcePoints[i].N * targetPoints[i].N;
                y1 += sourcePoints[i].E * targetPoints[i].N;
                x2 += sourcePoints[i].N * sourcePoints[i].N;
                y2 += sourcePoints[i].E * targetPoints[i].E;
                x3 += sourcePoints[i].N * targetPoints[i].E;
                y3 += sourcePoints[i].E * sourcePoints[i].E;
                x4 += sourcePoints[i].N * sourcePoints[i].E;
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
