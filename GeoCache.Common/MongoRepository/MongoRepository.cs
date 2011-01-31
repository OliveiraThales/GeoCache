using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoCache.Contracts;
using MongoDB.Driver;
using MongoDB.Bson;

namespace GeoCache.Common.MongoRepository
{
    /// <summary>
    /// Implementation class for Mongo data repository
    /// </summary>
    public class MongoRepository : IRepository
    {
        private MongoCollection collection;
        private MongoDatabase database;
        private MongoServer server;

        public MongoRepository()
        {
            Connect();
        }

        private void Connect()
        {
            server = MongoServer.Create(string.Format("mongodb://localhost"));
            database = server["GeoCache"];
            collection = database["Features"];
        }

        public void Insert(Dictionary<string, object> document)
        {
            collection.Insert(document);
        }

        public void RemoveAll()
        {
            collection.RemoveAll();
        }

        public IEnumerable<Dictionary<string, object>> GetAll()
        {
            return collection.FindAllAs<Dictionary<string, object>>();
        }

        public void Update(Dictionary<string, object> queryDocument, Dictionary<string, object> updatingDocument)
        {
            var query = MongoDB.Driver.Builders.Query.Wrap(queryDocument);
            var updating = MongoDB.Driver.Builders.Update.Wrap(updatingDocument);
            collection.Update(query, updating);
        }

        public void Dispose()
        {
            server.Disconnect();
        }
    }
}
