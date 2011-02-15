using System.Collections.Generic;
using System.Linq;
using GeoCache.Contracts;

namespace GeoCache.Cache
{
    /// <summary>
    /// Sde cache
    /// </summary>
    public class SdeCache : ICache
    {
        #region Fields

        /// <summary>
        /// Repository
        /// </summary>
        private readonly IRepository _repository;

        /// <summary>
        /// featureClass name
        /// </summary>
        private readonly string _featureName;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="featureName"></param>
        public SdeCache(IRepository repository, string featureName)
        {
            this._repository = repository;
            this._featureName = featureName;
        }

        #endregion

        #region ICache Implementation

        /// <summary>
        /// Build cache based on cursor
        /// </summary>
        /// <param name="geometries">Geometries to add</param>
        public void BuildCursor(IEnumerable<IGeometry> geometries)
        {
            return;
        }

        /// <summary>
        /// Build a cache for a especific featureClass and extent
        /// </summary>
        public void BuildAllCache()
        {
            return;
        }

        /// <summary>
        /// Build a cache for a especific featureClass and extent
        /// </summary>
        /// <param name="envelope">Envelope</param>
        public void BuildEnvelope(IEnvelope envelope)
        {
            return;
        }

        /// <summary>
        /// Retrive data from cache
        /// </summary>
        /// <param name="envelop">Envelope for search</param>
        /// <param name="outerData">Output data</param>
        /// <param name="affectedEnvelop">Affected envelop in the grid</param>
        /// <returns>True if data exists in cache</returns>
        public bool RetriveData(IEnvelope envelop, ref IList<IGeometry> outerData, ref IEnvelope affectedEnvelop)
        {
            var restul = _repository.GetByEnvelope(this._featureName, envelop);

            affectedEnvelop = envelop;
            outerData = restul.ToList();

            return true;
        }

        #endregion

    }
}
