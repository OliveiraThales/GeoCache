using System.Collections.Generic;

namespace GeoCache.Contracts
{
    /// <summary>
    /// Interface for cache
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Build cache based on cursor
        /// </summary>
        /// <param name="geometries">Geometries to add</param>
        void BuildCursor(IEnumerable<IGeometry> geometries);

        /// <summary>
        /// Build a cache for a especific featureClass and extent
        /// </summary>
        void BuildAllCache();

        /// <summary>
        /// Build a cache for a especific featureClass and extent
        /// </summary>
        /// <param name="envelope">Envelope</param>
        void BuildEnvelope(IEnvelope envelope);

        /// <summary>
        /// Retrive data from cache
        /// </summary>
        /// <param name="envelop">Envelope for search</param>
        /// <param name="outerData">Output data</param>
        /// <param name="affectedEnvelop">Affected envelop in the grid</param>
        /// <returns>True if data exists in cache</returns>
        bool RetriveData(IEnvelope envelop, ref IList<IGeometry> outerData, ref IEnvelope affectedEnvelop);
    }
}
