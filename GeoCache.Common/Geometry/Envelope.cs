using GeoCache.Contracts;

namespace GeoCache.Common.Geometry
{
    /// <summary>
    /// Generic envelope
    /// </summary>
    public class Envelope : IEnvelope
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="maxX">maxX</param>
        /// <param name="minX">minX</param>
        /// <param name="maxY">maxY</param>
        /// <param name="minY">minY</param>
        public Envelope(double maxX, double minX, double maxY, double minY)
        {
            this.MaxX = maxX;
            this.MinX = minX;
            this.MaxY = maxY;
            this.MinY = minY;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the minimum x-value
        /// </summary>
        /// <returns>The minimum x-coordinate.</returns>
        public double MinX { get; set; }

        /// <summary>
        /// Returns the maximum x-value
        /// </summary>
        /// <returns>The maximum x-coordinate.</returns>
        public double MaxX{ get; set; }

        /// <summary>
        /// Returns the minimum y-value
        /// </summary>
        /// <returns>The minimum y-coordinate.</returns>
        public double MinY{ get; set; }

        /// <summary>
        /// Returns the maximum y-value
        /// </summary>
        /// <returns>The maximum y-coordinate.</returns>
        public double MaxY{ get; set; }

        /// <summary>
        /// The envelop width
        /// </summary>
        public double Width
        { 
            get
            {
                return this.MaxX - this.MinX;
            }
        }

        /// <summary>
        /// The envelop height
        /// </summary>
        public double Height
        {
            get
            {
                return this.MaxY - this.MinY;
            }
        }

        #endregion
    }
}
