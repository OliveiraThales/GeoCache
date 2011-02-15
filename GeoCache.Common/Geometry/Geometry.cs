using GeoCache.Contracts;

namespace GeoCache.Common.Geometry
{
    using System;

    /// <summary>
    /// Generic Geometry
    /// </summary>
    [Serializable]
    public class Geometry : IGeometry
    {
        #region Constructor

        /// <summary>
        /// Constrctor
        /// </summary>
        /// <param name="maxX">maxX</param>
        /// <param name="minX">minX</param>
        /// <param name="maxY">maxY</param>
        /// <param name="minY">minY</param>
        public Geometry(double maxX, double minX, double maxY, double minY)
        {
            this.Envelop = new Envelope(maxX, minX, maxY, minY);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Envelope
        /// </summary>
        public IEnvelope Envelop { get; set; }

        /// <summary>
        /// Object ID
        /// </summary>
        public int Oid { get; set; }
        
        #endregion
    }
}
