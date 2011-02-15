using System.Collections.Generic;
using System.Linq;
using GeoCache.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;
using GeoCache.Common.Geometry;
using MongoDB.Bson.DefaultSerializer;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;

namespace GeoCache.Common.Repository
{
    /// <summary>
    /// Implementation class for Mongo data repository
    /// </summary>
    public class MongoRepository : INoSqlRepository
    {
        #region Fields

        private readonly IDictionary<string, MongoCollection> _collections;

        private MongoDatabase _database;
        
        private MongoServer _server;

        private double _indexRange = 999999999;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MongoRepository()
        {
            BsonClassMap.LookupClassMap(typeof(Geometry.Geometry));
            BsonClassMap.LookupClassMap(typeof(Envelope));

            this._collections = new Dictionary<string, MongoCollection>();
            
            Connect();
        }

        #endregion

        #region INoSqlRepository

        /// <summary>
        /// Insert a document in the data repository.
        /// </summary>
        /// <param name="featureClassName">featureClassName</param>
        /// <param name="geometry">
        /// O documento
        /// </param>
        public void Insert(string featureClassName, IGeometry geometry)
        {
            var bytes = Serialize(geometry as Geometry.Geometry);

            var point = new BsonDocument
                {
                    {
                        "coord",
                        BsonArray.Create(
                            BsonArray.Create(
                                new[] { geometry.Envelop.MaxX, geometry.Envelop.MaxY }))
                        }, {
                               "geometry", bytes
                               }
                };

            this.GetCollection(featureClassName).Insert(point, SafeMode.True);
        }

        /// <summary>
        /// Remove all the data from the repository.
        /// </summary>
        public void RemoveAll(string featureClassName)
        {
            this.GetCollection(featureClassName).RemoveAll();
        }

        #endregion

        #region IRepository

        /// <summary>
        /// Get all geometries from a featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <returns>Enumerable of geometries</returns>
        public IEnumerable<IGeometry> GetAll(string featureClassName)
        {
            return this.GetCollection(featureClassName).FindAllAs<BsonDocument>().Select(Parser);
        }

        /// <summary>
        /// Get all geometries inside the envelope from a featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <param name="envelope">Envelope</param>
        /// <returns>Enumerable of geometries</returns>
        public IEnumerable<IGeometry> GetByEnvelope(string featureClassName, IEnvelope envelope)
        {
            var result =
                this.GetCollection(featureClassName).FindAs<BsonDocument>(
                    Query.WithinRectangle("coord", envelope.MinX, envelope.MinY, envelope.MaxX, envelope.MaxY));

            return result.Select(Parser);
        }

        /// <summary>
        /// Get full extent of featureClass
        /// </summary>
        /// <param name="featureClassName">FeatureClass name</param>
        /// <returns>Envelope</returns>
        public IEnvelope GetFullExtent(string featureClassName)
        {
            return new Envelope(this._indexRange, this._indexRange, this._indexRange, this._indexRange);
        }

        #endregion

        #region IDisposeble

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _server.Disconnect();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parse mongo document to Geometry
        /// </summary>
        /// <param name="document">Document</param>
        /// <returns></returns>
        private static IGeometry Parser(BsonDocument document)
        {
            var bytes = document["geometry"].AsByteArray;

            return Deserialize(bytes);
        }

        /// <summary>
        /// Connect to mongo server
        /// </summary>
        private void Connect()
        {
            _server = MongoServer.Create(string.Format("mongodb://localhost"));
            _database = _server["GeoCache"];
        }

        /// <summary>
        /// Get the collection from cache
        /// </summary>
        /// <param name="collectionName">Collection name</param>
        /// <returns>Collection</returns>
        private MongoCollection GetCollection(string collectionName)
        {
            MongoCollection collection = null;
            if (!this._collections.TryGetValue(collectionName, out collection))
            {
                if (!this._database.CollectionExists(collectionName))
                {
                    this._database.CreateCollection(collectionName, null);
                }

                collection = this._database.GetCollection(collectionName);
                this._collections.Add(collectionName, collection);
                collection.EnsureIndex(
                    IndexKeys.GeoSpatial("coord"),
                    IndexOptions.SetGeoSpatialRange(-_indexRange, _indexRange));
            }

            return collection;
        }

        /// <summary>
        /// Serialize a geometry
        /// </summary>
        /// <param name="geometry">geometry</param>
        /// <returns>byte[]</returns>
        private static byte[] Serialize(Geometry.Geometry geometry)
        {
            byte[] bytes;
            using (var buffer = new BsonBuffer())
            {
                using (var bsonWriter = BsonWriter.Create(buffer))
                {
                    BsonSerializer.Serialize(bsonWriter, geometry);
                }
                bytes = buffer.ToByteArray();
            }

            return bytes;
        }

        /// <summary>
        /// Deserialize a geometry
        /// </summary>
        /// <param name="bytes">Butes</param>
        /// <returns>geometry</returns>
        private static Geometry.Geometry Deserialize(byte[] bytes)
        {
            var des = BsonSerializer.Deserialize<Geometry.Geometry>(bytes);

            return des;
        }

        #endregion
    }
}
