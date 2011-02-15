using SharpMap.Geometries;

namespace GeoCache.Cache
{
    /// <summary>
    /// Index Entry
    /// </summary>
    public class IndexEntry
    {
        /// <summary>
        /// BoundingBox
        /// </summary>
        public BoundingBox Box { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        public object Data { get; set; }
    }
}
