using System.Collections.Generic;

namespace GeoCache.Contracts
{
    /// <summary>
    /// Interface for cache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Build a cache for a especific featureClass and extent
        /// </summary>
        /// <param name="fullExtent">Full extent for cache</param>
        /// <param name="featureClassName">FeatureClass name</param>
        void BuildCache(IEnvelope fullExtent, string featureClassName);

        /// <summary>
        /// Retrive data from cache
        /// </summary>
        /// <param name="envelop">Envelope for search</param>
        /// <param name="outerData">Output data</param>
        /// <returns>True if data exists in cache</returns>
        bool RetriveData(IEnvelope envelop, ref IList<IGeometry> outerData);
    }
}
