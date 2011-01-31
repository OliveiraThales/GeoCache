using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoCache.Contracts;

namespace GeoCache.Common.MongoRepository
{
    public class MongoRepository : IRepository
    {
        public void Insert(Dictionary<string, object> document)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Dictionary<string, object>> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(string query, Dictionary<string, object> updatingDocument)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
