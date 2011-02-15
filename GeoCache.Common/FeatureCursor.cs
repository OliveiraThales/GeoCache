using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using GeoCache.Contracts;

namespace GeoCache.Common
{
    /// <summary>
    /// Generic FeatureCursor
    /// </summary>
    public class FeatureCursor : IEnumerator<IGeometry>, IEnumerable<IGeometry>
    {
        #region Field

        /// <summary>
        /// Esri feature cursor
        /// </summary>
        private readonly IFeatureCursor _cursor;

        /// <summary>
        /// Current feature
        /// </summary>
        private IFeature _currentFeature;

        /// <summary>
        /// Features count
        /// </summary>
        private readonly int _count;

        /// <summary>
        /// Actual index
        /// </summary>
        private int _actual;

        /// <summary>
        /// Chunck number
        /// </summary>
        private readonly int _chunck;

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
        /// <param name="featureCursor">feature cursor</param>
        /// <param name="featureCount">Feature count</param>
        public FeatureCursor(IFeatureCursor featureCursor, int featureCount)
        {
            this._count = featureCount;
            this._cursor = featureCursor;

            this._chunck = this._count / 100;
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Marshal.ReleaseComObject(this._cursor);
        }

        #endregion

        #region IEnumerator

        /// <summary>
        /// Move mext
        /// </summary>
        /// <returns>True if exists next</returns>
        public bool MoveNext()
        {
            var hasNext = (this._currentFeature = this._cursor.NextFeature()) != null;

            if (OnProgress != null && hasNext && ++this._actual % this._chunck == 0)
            {
                OnProgress(this._actual / this._chunck);
            }

            return hasNext;
        }

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Get current
        /// </summary>
        public IGeometry Current
        {
            get
            {
                return Parser(this._currentFeature);
            }
        }

        /// <summary>
        /// Get current
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        #endregion

        #region IEnumerable

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns>IEnumerable of geometries</returns>
        public IEnumerator<IGeometry> GetEnumerator()
        {
            return this;
        }

        /// <summary>
        /// Get enumerator
        /// </summary>
        /// <returns>IEnumerable of geometries</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Parse from IFeature to Generic Geometry
        /// </summary>
        /// <param name="feature">Feature</param>
        /// <returns>Generic geometry</returns>
        private static IGeometry Parser(IFeature feature)
        {
            var envelope = feature.Extent;
            var geometry = new Geometry.Geometry(envelope.XMax + 1, envelope.XMin - 1, envelope.YMax + 1, envelope.YMin - 1)
                { Oid = feature.OID };

            return geometry;
        }

        #endregion
    }
}
