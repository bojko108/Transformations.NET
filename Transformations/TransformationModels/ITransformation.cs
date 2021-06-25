namespace BojkoSoft.Transformations.TransformationModels
{
    /// <summary>
    /// Interface for using transformation models
    /// </summary>
    internal interface ITransformation
    {
        /// <summary>
        /// Transforms a point
        /// </summary>
        /// <param name="inputPoint"></param>
        /// <returns></returns>
        IPoint Transform(IPoint inputPoint);
    }
}
