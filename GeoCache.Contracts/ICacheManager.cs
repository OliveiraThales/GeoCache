using System.Collections.Generic;

namespace GeoCache.Contracts
{
    /// <summary>
    /// Cache Manager
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// List of cache levels
        /// </summary>
        IList<ICache> Caches { get; set; }

        /// <summary>
        /// Retrive data from cache
        /// </summary>
        /// <param name="envelop">Envelope for search</param>
        /// <param name="outerData">Output data</param>
        /// <returns>True if data exists in cache</returns>
        bool RetriveData(IEnvelope envelop, ref IList<IGeometry> outerData);
    }
}
