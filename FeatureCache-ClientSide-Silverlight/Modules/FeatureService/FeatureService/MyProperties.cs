using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoJSON;
using Newtonsoft.Json;

namespace FeatureService
{
    public class MyProperties
    {
        [JsonProperty("elevation")]
        public string Elevation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

}