using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoCache.Cache
{
    using System.Collections;

    using GeoCache.Common.Repository;
    using GeoCache.Contracts;

    public class CacheManager : ICacheManager
    {
        private const string FeatureName = "admgid.Switch_PT";

        public CacheManager()
        {
            var sde = new SdeRepository("srvprodist", "5151", "bdgd", "bdgd", "sde.DEFAULT");
            var mongo = new MongoRepository();

            var cache1 = new InMemoryCache(mongo, sde.GetFullExtent(FeatureName), FeatureName);
            var cache2 = new NoSqlCache(sde, mongo, FeatureName);
            var cache3 = new SdeCache(sde, FeatureName);

            this.Caches = new List<ICache> { cache1, cache2, cache3 };
        }

        public IList<ICache> Caches { get; set; }


        public bool RetriveData(IEnvelope envelop, ref IList<IGeometry> outerData)
        {
            IEnvelope affected = envelop;
            var toUpdate = new List<ICache>();
            bool found = false;

            foreach (var cache in this.Caches)
            {
                var has = cache.RetriveData(affected, ref outerData, ref affected);

                if(!has)
                {
                    toUpdate.Add(cache);
                }
                else
                {
                    found = true;
                    break;
                }
            }

            if (found && toUpdate.Count > 0)
            {
                IEnumerable<IGeometry> cursor = outerData;
                toUpdate.ForEach(x => x.BuildCursor(cursor));

                var first = this.Caches.First();
                first.RetriveData(envelop, ref outerData, ref affected);
            }

            return true;
        }
    }
}
