using System;
using System.Collections.Generic;
using System.Linq;
using SharpMap.Utilities.SpatialIndexing;
using GeoCache.Contracts;
using GeoCache.Common;
using GeoCache.Common.Geometry;
using SharpMap.Geometries;
using SharpMap.Indexing;
using IGeometry = GeoCache.Contracts.IGeometry;

namespace GeoCache.Cache
{
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
        /// QuadTree index structure
        /// </summary>
        private QuadTree _indexBound;

        /// <summary>
        /// Features count
        /// </summary>
        private int _featureCount;

        /// <summary>
        /// Quarter precision
        /// </summary>
        private double _quarterPrecision = 0.001;

        /// <summary>
        /// Level to create a grid index
        /// </summary>
        private int _quarterLevel = 8;

        /// <summary>
        /// FeatureClass name
        /// </summary>
        private string _featureName;

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
        /// <param name="fullExtent">Full Extent</param>
        /// <param name="featureClass">FeatureClass name</param>
        public InMemoryCache(IRepository repository, IEnvelope fullExtent, string featureClass)
        {
            this._repository = repository;
            this._featureName = featureClass;

            this.BuildGrid(fullExtent);
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
            if(outerData == null)
            {
                outerData = new List<IGeometry>();
            }

            outerData.Clear();

            var box = new BoundingBox(envelop.MinX, envelop.MinY, envelop.MaxX, envelop.MaxY);
            var quad = this._indexBound.SearchData(box);

            foreach (var frame in quad)
            {
                var entry = frame as IndexEntry;
                var tree = entry.Data as DynamicRTree;

                if (tree != null)
                {
                    var result = tree.Search(box);
                    foreach (var item in result)
                    {
                        outerData.Add(item as IGeometry);
                    }
                }
            }

            var fullBox = new BoundingBox(quad.Select(x => ((IndexEntry)x).Box).ToArray());
            affectedEnvelop = new Envelope(fullBox.Max.X, fullBox.Min.X, fullBox.Max.Y, fullBox.Min.Y);

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

            uint id = 0;
            foreach (var geometry in geometries)
            {
                var box = new BoundingBox(
                    geometry.Envelop.MinX, geometry.Envelop.MinY, geometry.Envelop.MaxX, geometry.Envelop.MaxY);

                var result = this._indexBound.SearchData(box);

                foreach (var cel in result)
                {
                    var entry = cel as IndexEntry;
                    var index = entry.Data as DynamicRTree;
                    
                    if (index != null)
                    {
                        this._featureCount++;
                        index.Insert(id, box, geometry);
                        id++;
                    }
                }
            }   
        }

        /// <summary>
        /// Build the grid based on Quadtree algorithm
        /// </summary>
        /// <param name="envelop">Full extent envelop</param>
        private void BuildGrid(IEnvelope envelop)
        {
            if (this._indexBound == null)
            {
                this._indexBound = this.CreateSpatialIndex(envelop);
            }
        }

        /// <summary>
        /// Generates a spatial index for a specified shape file.
        /// </summary>
        private QuadTree CreateSpatialIndex(IEnvelope envelop)
        {
            var objList = new List<QuadTree.BoxObjects>();
            uint i = 0;
            foreach (BoundingBox box in GetAllFeatureBoundingBoxes(envelop))
            {
                if (!double.IsNaN(box.Left) && !double.IsNaN(box.Right) && !double.IsNaN(box.Bottom) &&
                    !double.IsNaN(box.Top))
                {
                    var g = new QuadTree.BoxObjects { box = box, ID = i, Data = new IndexEntry { Box = box, Data = new DynamicRTree() } };
                    objList.Add(g);
                    i++;
                }
            }

            Heuristic heur;
            heur.maxdepth = (int)Math.Ceiling(Math.Log(objList.Count, 2));
            heur.minerror = 10;
            heur.tartricnt = 5;
            heur.mintricnt = 2;

            return new QuadTree(objList, 0, heur);
        }

        /// <summary>
		/// Reads all boundingboxes of features in the shapefile. This is used for spatial indexing.
		/// </summary>
		/// <returns></returns>
        private IEnumerable<BoundingBox> GetAllFeatureBoundingBoxes(IEnvelope envelop)
        {
            var fullExtent = new BoundingBox(envelop.MinX, envelop.MinY, envelop.MaxX, envelop.MaxY);
            var list = new List<BoundingBox>();

            BuildQuarter(fullExtent, 2, ref list);

            return list.AsEnumerable();
        }

        /// <summary>
        /// Build a quarter for index
        /// </summary>
        /// <param name="envelope">Envelope</param>
        /// <param name="level">level</param>
        /// <param name="order">list of objects</param>
        private void BuildQuarter(BoundingBox envelope, int level, ref List<BoundingBox> order)
        {
            if (level > this._quarterLevel)
                return;

            var quarter1 = new BoundingBox(
                envelope.Min.X,
                envelope.Min.Y,
                envelope.Min.X + (envelope.Width / 2) - this._quarterPrecision,
                envelope.Min.Y + (envelope.Height / 2) - this._quarterPrecision);

            var quarter2 = new BoundingBox(
                envelope.Min.X + (envelope.Width / 2),
                envelope.Min.Y,
                envelope.Max.X,
                envelope.Min.Y + (envelope.Height / 2) - this._quarterPrecision);

            var quarter3 = new BoundingBox(
                envelope.Min.X,
                envelope.Min.Y + (envelope.Height / 2),
                envelope.Min.X + (envelope.Width / 2) - this._quarterPrecision,
                envelope.Max.Y);

            var quarter4 = new BoundingBox(
                envelope.Min.X + (envelope.Width / 2),
                envelope.Min.Y + (envelope.Height / 2),
                envelope.Max.X,
                envelope.Max.Y);

            if (level == this._quarterLevel)
            {
                order.Add(quarter1);
                order.Add(quarter2);
                order.Add(quarter3);
                order.Add(quarter4);    
            }

            this.BuildQuarter(quarter1, level * 2, ref order);
            this.BuildQuarter(quarter2, level * 2, ref order);
            this.BuildQuarter(quarter3, level * 2, ref order);
            this.BuildQuarter(quarter4, level * 2, ref order);
        }

        #endregion
    }
}
