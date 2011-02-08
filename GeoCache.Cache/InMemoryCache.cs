using System;
using System.Collections.Generic;
using System.Linq;
using GisSharpBlog.NetTopologySuite.Geometries;
using GisSharpBlog.NetTopologySuite.Index.Quadtree;
using GisSharpBlog.NetTopologySuite.Index.Strtree;
using GeoCache.Contracts;

namespace GeoCache.Cache
{
    using GeoCache.Common;

    /// <summary>
    /// InMemeory implementation of ICache
    /// </summary>
    public class InMemoryCache : ICache
    {
        #region Fields

        /// <summary>
        /// Repository
        /// </summary>
        private readonly IRepository _repository;

        /// <summary>
        /// Grid Width
        /// </summary>
        private readonly int _gridWidth;

        /// <summary>
        /// Grid Height
        /// </summary>
        private readonly int _gridHeight;

        /// <summary>
        /// QuadTree index structure
        /// </summary>
        private Quadtree _indexBound;

        #endregion

        #region Events

        /// <summary>
        /// Event delegate
        /// </summary>
        /// <param name="percentage">percentage</param>
        public delegate void OnProgressDelegate(int percentage);

        /// <summary>
        /// Event OnProgress
        /// </summary>
        public event OnProgressDelegate OnProgress;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">The repository</param>
        /// <param name="gridWidth">Grid width</param>
        /// <param name="gridHeight">Grid height</param>
        public InMemoryCache(IRepository repository, int gridWidth, int gridHeight)
        {
            this._repository = repository;
            this._gridWidth = gridWidth;
            this._gridHeight = gridHeight;
        }

        #endregion

        #region ICache Implementation

        /// <summary>
        /// Build the cache for a featureclass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        public void BuildCache(string featureClassName)
        {
            this.BuildGrid(this._repository.GetFullExtent(featureClassName));
            this.AddGeometries(this._repository.GetAll(featureClassName));
        }

        /// <summary>
        /// Retrive data from cache
        /// </summary>
        /// <param name="envelop">Envelope for search</param>
        /// <param name="outerData">Output data</param>
        /// <returns>True if data exists in cache</returns>
        public bool RetriveData(IEnvelope envelop, ref IList<IGeometry> outerData)
        {
            if(outerData == null)
            {
                outerData = new List<IGeometry>();
            }

            var envelope = GetEnvelopeFromGenericEnvelope(envelop);
            var quad = this._indexBound.Query(envelope);

            foreach (var frame in quad)
            {
                var tree = frame as STRtree;
                if (tree != null)
                {
                    var result = tree.Query(envelope);
                    foreach (var item in result)
                    {
                        outerData.Add(item as IGeometry);
                    }
                }
            }

            return outerData.Count > 0;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add geometries to cache
        /// </summary>
        /// <param name="geometries">IEnumable of geometries</param>
        private void AddGeometries(IEnumerable<IGeometry> geometries)
        {
            if (geometries is FeatureCursor)
            {
                var temp = geometries as FeatureCursor;
                temp.OnProgress += new FeatureCursor.OnProgressDelegate(this.OnProgress);
            }

            foreach (var geometry in geometries)
            {
                var envelope = GetEnvelopeFromGenericEnvelope(geometry.Envelop);
                var query = this._indexBound.Query(envelope);

                foreach (var cel in query)
                {
                    var index = cel as STRtree;

                    if (index != null)
                    {
                        index.Insert(envelope, geometry);
                    }
                }
            }   
        }

        /// <summary>
        /// Get the envelope to do a search
        /// </summary>
        /// <param name="envelop">Generic envelop</param>
        /// <returns>Envelope for a search</returns>
        private static GeoAPI.Geometries.IEnvelope GetEnvelopeFromGenericEnvelope(IEnvelope envelop)
        {
            return new Envelope(envelop.MaxX, envelop.MinX, envelop.MaxY, envelop.MinY);
        }

        /// <summary>
        /// Build the grid based on Quadtree algorithm
        /// </summary>
        /// <param name="envelop">Full extent envelop</param>
        private void BuildGrid(IEnvelope envelop)
        {
            var numColumns = Math.Ceiling(envelop.Width / this._gridWidth);
            var numLines = Math.Ceiling(envelop.Height / this._gridHeight);

            this._indexBound = new Quadtree();

            double y1 = envelop.MinY;
            double y2 = y1 + this._gridHeight;

            for (double i = 1; i <= numLines; i++)
            {
                double x1 = envelop.MinX;
                double x2 = x1 + this._gridWidth;

                for (double j = 1; j <= numColumns; j++)
                {
                    var envelope = new Envelope(x1, x2, y1, y2);

                    this._indexBound.Insert(envelope, new STRtree());

                    x1 += this._gridWidth;
                    x2 += this._gridWidth;
                }

                y1 += this._gridHeight;
                y2 += this._gridHeight;
            }
        }

        #endregion
    }
}
