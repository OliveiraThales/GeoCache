namespace GeoCache.Contracts
{
    /// <summary>
    /// Generic geometry
    /// </summary>
    public interface IGeometry
    {
        /// <summary>
        /// Object ID
        /// </summary>
        int Oid { get; set; }

        /// <summary>
        /// Geometry´s envelope
        /// </summary>
        IEnvelope Envelop { get; }
    }
}
