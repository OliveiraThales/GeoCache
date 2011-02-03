namespace GeoCache.Contracts
{
    /// <summary>
    /// Generic geometry
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// Geometry´s envelope
        /// </summary>
        IEnvelope Envelop { get; }
    }
}
