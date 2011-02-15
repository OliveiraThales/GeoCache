using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using GeoCache.Common.Repository;

namespace GeoCache.Common.Test
{
    [TestFixture]
    public class MongoRepositoryTest
    {
        private MongoRepository repository;

        [SetUp]
        public void Setup()
        {
            repository = new MongoRepository();
        }

        [Test]
        public void MustInsertDocument()
        {
            //var document = new Dictionary<string, object> {{"key", "value"}};
            //repository.Insert(document);

            //var documentFound = repository.FindOne();

            //Assert.AreEqual("value", documentFound["key"]);
        }

        
    }
}
