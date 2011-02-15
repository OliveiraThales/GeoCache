using System.Collections.Generic;
using System.Linq;
using GeoCache.Common.Geometry;
using GeoCache.Contracts;

namespace GeoCache.Cache
{
    /// <summary>
    /// NoSql cache implementation
    /// </summary>
    public class NoSqlCache : ICache
    {
        #region Fields

        /// <summary>
        /// Repository
        /// </summary>
        private readonly IRepository _repository;

        /// <summary>
        /// NoSql Repository
        /// </summary>
        private readonly INoSqlRepository _noSqlRepository;

        /// <summary>
        /// FeatureClass name
        /// </summary>
        private string _featureName;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="noqlRepository">NoSql Repository</param>
        /// <param name="featureClass">FeatureClass name</param>
        public NoSqlCache(IRepository repository, INoSqlRepository noqlRepository, string featureClass)
        {
            this._repository = repository;
            this._noSqlRepository = noqlRepository;
            this._featureName = featureClass;
        }

        #endregion

        #region ICache Implementation

        /// <summary>
        /// Build cache based on cursor
        /// </summary>
        /// <param name="geometries">Geometries to add</param>
        public void BuildCursor(IEnumerable<IGeometry> geometries)
        {
            this.AddGeometries(geometries);
        }

        /// <summary>
        /// Build the cache for a featureclass
        /// </summary>
        public void BuildAllCache()
        {
            this._noSqlRepository.RemoveAll(this._featureName);
            this.AddGeometries(this._repository.GetAll(this._featureName));
        }

        /// <summary>
        /// Build a cache for a especific featureClass and extent
        /// </summary>
        /// <param name="envelope">Envelope</param>
        public void BuildEnvelope(IEnvelope envelope)
        {
            this.AddGeometries(this._repository.GetByEnvelope(this._featureName, envelope));
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
            affectedEnvelop = new Envelope(envelop.MaxX, envelop.MinX, envelop.MaxY, envelop.MinY);

            var result = this._noSqlRepository.GetByEnvelope(this._featureName, envelop);

            outerData = result.ToList();

            return result.Count() > 0;
        }

        #endregion

        #region Private Methods

        private void AddGeometries(IEnumerable<IGeometry> geometries)
        {
            foreach (var geometry in geometries)
            {
                this._noSqlRepository.Insert(this._featureName, geometry);
            }
        }

        #endregion
    }
}
