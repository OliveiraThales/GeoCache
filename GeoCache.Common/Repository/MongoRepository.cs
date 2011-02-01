using System.Collections.Generic;
using GeoCache.Contracts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GeoCache.Common.Repository
{
    /// <summary>
    /// Implementation class for Mongo data repository
    /// </summary>
    public class MongoRepository : IRepository
    {
        private MongoCollection _collection;
        private MongoDatabase _database;
        private MongoServer _server;

        public MongoRepository()
        {
            Connect();
        }

        private void Connect()
        {
            _server = MongoServer.Create(string.Format("mongodb://localhost"));
            _database = _server["GeoCache"];
            _collection = _database["Features"];
        }

        public Dictionary<string, object> FindOne()
        {
            return _collection.FindOneAs<Dictionary<string, object>>();
        }

        public void Insert(Dictionary<string, object> document)
        {
            var bsonDocument = new BsonDocument(document);
            _collection.Insert(bsonDocument);
        }

        public void RemoveAll()
        {
            _collection.RemoveAll();
        }

        public IEnumerable<Dictionary<string, object>> GetAll()
        {
            return _collection.FindAllAs<Dictionary<string, object>>();
        }

        public void Update(Dictionary<string, object> queryDocument, Dictionary<string, object> updatingDocument)
        {
            var query = MongoDB.Driver.Builders.Query.Wrap(queryDocument);
            var updating = MongoDB.Driver.Builders.Update.Wrap(updatingDocument);
            _collection.Update(query, updating);
        }

        public void Dispose()
        {
            _server.Disconnect();
        }
    }
}
