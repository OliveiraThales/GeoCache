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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="featureCursor">feature cursor</param>
        public FeatureCursor(IFeatureCursor featureCursor)
        {
            this._cursor = featureCursor;
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
            return (this._currentFeature = this._cursor.NextFeature()) != null;
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
                return this.Parser(this._currentFeature);
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
        private IGeometry Parser(IFeature feature)
        {
            //TODO: Implement!
            return null;
        }

        #endregion
    }
}
