using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoCache.Cache.Test
{
    using GeoCache.Common.Geometry;
    using GeoCache.Contracts;

    [TestClass]
    public class CacheManagerTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var manager = new CacheManager();
            var envelop = new Envelope(325017, 320003, 7392018, 7391712);
            
            IList<IGeometry> list = new List<IGeometry>();

            var start = DateTime.Now;
            manager.RetriveData(envelop, ref list);
            var first = (DateTime.Now - start).TotalMilliseconds;

            start = DateTime.Now;
            manager.RetriveData(envelop, ref list);
            var second = (DateTime.Now - start).TotalMilliseconds;

            Assert.IsTrue(first > second);
        }
    }
}
