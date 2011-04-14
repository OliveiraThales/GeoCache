using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using GeoJSON;

namespace FeatureService
{
    
    public class MyFeatureCollection : GeoJSON.FeatureCollection<MyFeature>
    {
    }

    public class MyFeature : GeoJSON.PointFeature<MyProperties>
    {
        [Newtonsoft.Json.JsonProperty("id")]
        public string ID { get; set; }
    }


}