using System.Collections.Generic;

namespace GeoCache.Contracts
{
    /// <summary>
    /// Generic geographic repository
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Get all geometries from a featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <returns>Enumerable of geometries</returns>
        IEnumerable<IGeometry> GetAll(string featureClassName);
    }
}
