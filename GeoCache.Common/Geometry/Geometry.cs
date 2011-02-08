using GeoCache.Contracts;

namespace GeoCache.Common.Geometry
{
    /// <summary>
    /// Generic Geometry
    /// </summary>
    public class Geometry : IGeometry
    {
        #region Fields

        /// <summary>
        /// envelope
        /// </summary>
        private readonly IEnvelope _envelop;

        #endregion

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
            this._envelop = new Envelope(maxX, minX, maxY, minY);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Envelope
        /// </summary>
        public IEnvelope Envelop
        {
            get
            {
                return this._envelop;
            }
        }

        #endregion
    }
}
