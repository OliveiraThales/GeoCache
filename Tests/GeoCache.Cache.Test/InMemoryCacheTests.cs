using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoCache.Common.Geometry;
using GeoCache.Contracts;

namespace GeoCache.Cache.Test
{
    using GeoCache.Common.Repository;

    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class InMemoryCacheTests
    {
        public InMemoryCacheTests()
        {
        }

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            int percent = 0;

            var repository = new SdeRepository("srvprodist", "5151", "bdgd", "bdgd", "sde.DEFAULT");
            var cache = new InMemoryCache(repository, repository.GetFullExtent("admgid.Switch_PT"), "admgid.Switch_PT");

            cache.OnProgress += delegate(int i)
                {
                    percent = i;
                };

            cache.BuildAllCache();

            IList<IGeometry> list = new List<IGeometry>();

            var envelop = new Envelope(325017, 320003, 7392018, 7391712);
            IEnvelope affected = null;

            var inicio = DateTime.Now;
            var result = cache.RetriveData(envelop, ref list, ref affected);
            var tempo1 = (DateTime.Now - inicio).TotalMilliseconds;

            inicio = DateTime.Now;
            var fromSde = repository.GetByEnvelope("admgid.Switch_PT", envelop);
            var tempo2 = (DateTime.Now - inicio).TotalMilliseconds;

            Assert.AreEqual(list.Count, fromSde.Count());
            Assert.IsTrue(tempo1 < tempo2);
            Assert.AreEqual(percent, 100);
        }
    }
}
