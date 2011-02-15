using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoCache.Cache.Test
{
    using GeoCache.Common.Geometry;
    using GeoCache.Common.Repository;
    using GeoCache.Contracts;

    [TestClass]
    public class NoSqlCacheTests
    {
        [TestMethod]
        public void Method01()
        {
            var sde = new SdeRepository("srvprodist", "5151", "bdgd", "bdgd", "sde.DEFAULT");
            var nosql = new MongoRepository();

            var cache = new NoSqlCache(sde, nosql, "admgid.Switch_PT");
            cache.BuildAllCache();

            IList<IGeometry> list = new List<IGeometry>();

            var envelop = new Envelope(325017, 320003, 7392018, 7391712);
            IEnvelope affected = null;

            var inicio = DateTime.Now;
            var result = cache.RetriveData(envelop, ref list, ref affected);
            var tempo1 = (DateTime.Now - inicio).TotalMilliseconds;

            inicio = DateTime.Now;
            var fromSde = sde.GetByEnvelope("admgid.Switch_PT", envelop);
            var tempo2 = (DateTime.Now - inicio).TotalMilliseconds;

            Assert.AreEqual(list.Count, fromSde.Count());
            Assert.IsTrue(tempo1 < tempo2);
        }
    }
}
