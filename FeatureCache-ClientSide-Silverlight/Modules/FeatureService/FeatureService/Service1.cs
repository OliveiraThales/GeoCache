using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using GeoJSON;

namespace FeatureService
{
    // Start the service and browse to http://<machine_name>:<port>/Service1/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    // NOTE: If the service is renamed, remember to update the global.asax.cs file
    public class Service1
    {
        // TODO: Implement the collection resource that will contain the SampleItem instances

        [WebGet(UriTemplate = "")]
        public List<SampleItem> GetCollection()
        {
            // TODO: Replace the current implementation to return a collection of SampleItem instances
            return new List<SampleItem>() { new SampleItem() { Id = 1, StringValue = "Hello" } };
        }

        [WebInvoke(UriTemplate = "", Method = "POST")]
        public SampleItem Create(SampleItem instance)
        {
            // TODO: Add the new instance of SampleItem to the collection
            throw new NotImplementedException();
        }

        [WebGet(UriTemplate = "{id}")]
        public SampleItem Get(string id)
        {
            // TODO: Return the instance of SampleItem with the given id
            throw new NotImplementedException();
        }

        [WebInvoke(UriTemplate = "{id}", Method = "PUT")]
        public SampleItem Update(string id, SampleItem instance)
        {
            // TODO: Update the given instance of SampleItem in the collection
            throw new NotImplementedException();
        }

        [WebInvoke(UriTemplate = "{id}", Method = "DELETE")]
        public void Delete(string id)
        {
            // TODO: Remove the instance of SampleItem with the given id from the collection
            throw new NotImplementedException();
        }

        [WebGet(UriTemplate = "getgeojson", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public object GetGeoJson()
        {
            var MyCollection = new MyFeatureCollection();
            MyCollection.Features = new MyFeature[]
                                        {
                                            new MyFeature()
                                                {
                                                    ID = "472",
                                                    Geometry = new GeoJSON.Point() {Lat = 6.18218, Lon = 45.5949},
                                                    Properties = new MyProperties()
                                                                     {
                                                                         Elevation = "1770",
                                                                         Name = "CollectionBase d'Arclusaz"
                                                                     }
                                                }
                                        };

            MyCollection.Features = new MyFeature[]
                                        {
                                            new MyFeature()
                                                {
                                                    ID = "472",
                                                    Geometry = new GeoJSON.Point() {Lat = 6.27827, Lon = 45.6769},
                                                    Properties = new MyProperties()
                                                                     {
                                                                         Elevation = "1831",
                                                                         Name = "CollectionBase d'Arclusaz"
                                                                     }
                                                }
                                        };

            var Builder = new System.Text.StringBuilder();

            var Writer = new System.IO.StringWriter(Builder);

            new Newtonsoft.Json.JsonSerializer().Serialize(Writer, MyCollection);

            var json = Builder.ToString();
            //byte[] vector = new byte[Builder.Length];
            //string json = Encoding.Default.GetString(new MemoryStream().ToArray()); 
            return json;

            //string json = Builder.ToString().Replace("\\","");
            //return json.Substring(1, json.Length - 2);

            /*Dim MyCollection = New MyFeatureCollection With {
            .Features = New MyFeature() {
                New MyFeature() With {
                    .ID = 472,
                    .Geometry = New langsamu.GeoJSON.Point(6.18218, 45.5949),
                    .Properties = New MyProperties() With {
                        .Elevation = 1770,
                        .Name = "CollectionBase d'Arclusaz"
                    }
                },
                New MyFeature() With {
                    .ID = 458,
                    .Geometry = New langsamu.GeoJSON.Point(6.27827, 45.6769),
                    .Properties = New MyProperties() With {
                        .Elevation = 1831,
                        .Name = "Pointe de C\u00f4te Favre"
                    }
                }
            }
        }

        Dim Builder As New System.Text.StringBuilder
        Dim Writer As New System.IO.StringWriter(Builder)

        Call New Newtonsoft.Json.JsonSerializer().Serialize(Writer, MyCollection)

        Return Builder.ToString
*/


        }
    }
}
