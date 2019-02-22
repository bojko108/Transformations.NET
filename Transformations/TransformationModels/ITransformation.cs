using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        GeoPoint Transform(GeoPoint inputPoint);
    }
}
