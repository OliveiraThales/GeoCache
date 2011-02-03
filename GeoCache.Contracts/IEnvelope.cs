namespace GeoCache.Contracts
{
    /// <summary>
    /// Generic envelope
    /// </summary>
    public interface IEnvelope
    {
        /// <summary>
        /// Returns the minimum x-value
        /// </summary>
        /// <returns>The minimum x-coordinate.</returns>
        double MinX { get; set; }
        
        /// <summary>
        /// Returns the maximum x-value
        /// </summary>
        /// <returns>The maximum x-coordinate.</returns>
        double MaxX { get; set; }

        /// <summary>
        /// Returns the minimum y-value
        /// </summary>
        /// <returns>The minimum y-coordinate.</returns>
        double MinY { get; set; }

        /// <summary>
        /// Returns the maximum y-value
        /// </summary>
        /// <returns>The maximum y-coordinate.</returns>
        double MaxY { get; set; }

        /// <summary>
        /// The envelop width
        /// </summary>
        double Width { get; }

        /// <summary>
        /// The envelop height
        /// </summary>
        double Height { get; }
    }
}
