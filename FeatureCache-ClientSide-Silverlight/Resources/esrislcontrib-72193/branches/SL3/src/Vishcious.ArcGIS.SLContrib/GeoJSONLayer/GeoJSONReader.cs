using System;
using ESRI.ArcGIS.Client.Geometry;
using System.IO;
using System.Json;
using System.Linq;
using System.Collections.Generic;
using ESRI.ArcGIS.Client;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class GeoJSONReader
    {
        public const string POINT_TYPE_NAME = "Point";
        public const string MULTIPOINT_TYPE_NAME = "MultiPoint";
        public const string LINESTRING_TYPE_NAME = "LineString";
        public const string MULTILINESTRING_TYPE_NAME = "MultiLineString";
        public const string POLYGON_TYPE_NAME = "Polygon";
        public const string MULTIPOLYGON_TYPE_NAME = "MultiPolygon";
        public const string GEOMETRY_COLLECTION_TYPE_NAME = "GeometryCollection";
        public const string FEATURE_TYPE_NAME = "Feature";
        public const string FEATURE_COLLECTION_TYPE_NAME = "FeatureCollection";

        public const string TYPE_IDENTIFIER = "type";
        public const string GEOMETRY_IDENTIFIER = "geometry";
        public const string COORDINATES_IDENTIFIER = "coordinates";
        public const string PROPERTIES_IDENTIFIER = "properties";
        public const string ID_IDENTIFIER = "id";
        public const string FEATURE_IDENTIFIER = "feature";
        public const string FEATURES_IDENTIFIER = "features";

        public static readonly string[] GEOMETRY_TYPES = new string[]{
                                                   POINT_TYPE_NAME,
                                                   MULTIPOINT_TYPE_NAME,
                                                   LINESTRING_TYPE_NAME,
                                                   MULTILINESTRING_TYPE_NAME,
                                                   POLYGON_TYPE_NAME,
                                                   MULTIPOLYGON_TYPE_NAME,
                                                   GEOMETRY_COLLECTION_TYPE_NAME
                                               };
        public static readonly string[] GEOJSON_TYPES = new string[]{
                                                  POINT_TYPE_NAME,
                                                   MULTIPOINT_TYPE_NAME,
                                                   LINESTRING_TYPE_NAME,
                                                   MULTILINESTRING_TYPE_NAME,
                                                   POLYGON_TYPE_NAME,
                                                   MULTIPOLYGON_TYPE_NAME,
                                                   GEOMETRY_COLLECTION_TYPE_NAME,
                                                   FEATURE_TYPE_NAME,
                                                   FEATURE_COLLECTION_TYPE_NAME
                                              };

        public const string ATTRIBUTES_FEATURE_IDENTIFIER = "FEATURE_IDENTIFIER";

        public static List<Graphic> ReadGeoJSON( Stream inputStream )
        {
            return ReadGeoJSON(JsonObject.Load(inputStream) as JsonObject);
        }

        public static List<Graphic> ReadGeoJSON( JsonObject jsonObject )
        {
            jsonObject.RequireArgument<JsonObject>( "jsonObject" ).NotNull<JsonObject>();

            if( !jsonObject.Keys.Contains( TYPE_IDENTIFIER ) )
                throw new InvalidOperationException( "The GeoJSON object must contain a VALID type property." );

            string type = ( string ) jsonObject[ TYPE_IDENTIFIER ];
            if( !GEOJSON_TYPES.Contains<string>( type ) )
                throw new InvalidOperationException( "The type property value '" + type + "' is not a valid GeoJSON type." );

            if( type == FEATURE_COLLECTION_TYPE_NAME )
                return ReadFeatures( jsonObject );

            if( type == FEATURE_TYPE_NAME )
                return ReadFeature( jsonObject );

            if( GEOMETRY_TYPES.Contains<string>( type ) )
            {
                List<Geometry> geometries = ReadGeometry( jsonObject );
                List<Graphic> graphics = new List<Graphic>();
                foreach( Geometry item in geometries )
                {
                    graphics.Add( new Graphic()
                    {
                        Geometry = item,
                        Symbol = item.GetDefaultSymbol()
                    } );
                }
                return graphics;
            }

            throw new InvalidOperationException( "The type property value '" + type + "' is not a valid GeoJSON type." );
        }

        public static List<Graphic> ReadFeatures( Stream inputStream )
        {
            return ReadFeatures(JsonObject.Load(inputStream) as JsonObject);
        }

        public static List<Graphic> ReadFeatures( JsonObject jsonObject )
        {
            jsonObject.RequireArgument<JsonObject>( "jsonObject" ).NotNull<JsonObject>();

            if( !jsonObject.Keys.Contains( TYPE_IDENTIFIER ) )
                throw new InvalidOperationException( "FeatureCollection objects should contain a VALID feature 'type' property value." );

            if( ( string ) jsonObject[ TYPE_IDENTIFIER ] != FEATURE_COLLECTION_TYPE_NAME )
                throw new InvalidOperationException( "The object is not of type 'FeatureCollection'. It is of type '" + ( string ) jsonObject[ FEATURE_IDENTIFIER ] + "'." );
            if( !jsonObject.Keys.Contains( FEATURES_IDENTIFIER ) )
                throw new InvalidOperationException( "The FeatureCollection object must contain a 'features' property." );
            if( !( jsonObject[ FEATURES_IDENTIFIER ].JsonType == JsonType.Array ) )
                throw new InvalidOperationException( "The value of the features property of a FeatureCollection object should be an array." );

            JsonArray features = jsonObject[ FEATURES_IDENTIFIER ] as JsonArray;
            List<Graphic> graphics = new List<Graphic>();
            foreach( JsonObject item in features )
            {
                List<Graphic> list = ReadFeature(item);
                graphics.AddRange( list );
            }

            return graphics;
        }

        public static List<Graphic> ReadFeature( Stream inputStream )
        {
            return ReadFeature(JsonObject.Load(inputStream) as JsonObject);
        }

        public static List<Graphic> ReadFeature( JsonObject jsonObject )
        {
            jsonObject.RequireArgument<JsonObject>( "jsonObject" ).NotNull<JsonObject>();

            if( !jsonObject.Keys.Contains( TYPE_IDENTIFIER ) )
                throw new InvalidOperationException( "Feature objects should contain a VALID feature 'type' property value." );

            if( (string)jsonObject[ TYPE_IDENTIFIER ] != FEATURE_TYPE_NAME )
                throw new InvalidOperationException( "The object is not of type 'Feature'. It is of type '" + (string)jsonObject[FEATURE_IDENTIFIER] + "'." );
            if( !jsonObject.Keys.Contains( GEOMETRY_IDENTIFIER ) )
                throw new InvalidOperationException( "The feature object must contain a 'geometry' property." );
            if( !jsonObject.Keys.Contains( PROPERTIES_IDENTIFIER ) )
                throw new InvalidOperationException( "The feature object must contain a 'properties' property." );

            string id = string.Empty;
            if( jsonObject.Keys.Contains( ID_IDENTIFIER ) )
            {
                id = ( string ) jsonObject[ ID_IDENTIFIER ];
            }

            List<Geometry> shapes = ReadGeometry( jsonObject[ GEOMETRY_IDENTIFIER ] as JsonObject);
            Dictionary<string, object> properties = ReadProperties(jsonObject[PROPERTIES_IDENTIFIER] as JsonObject);

            List<Graphic> graphics = new List<Graphic>();
            foreach( Geometry entry in shapes )
            {
                Graphic graphic = new Graphic()
                {
                    Geometry = entry,
                    Symbol = entry.GetDefaultSymbol()
                };
                foreach( var item in properties )
                {
                    graphic.Attributes.Add( item.Key, item.Value );
                }
                if( !string.IsNullOrEmpty( id ) )
                    graphic.Attributes.Add(ATTRIBUTES_FEATURE_IDENTIFIER, id);

                graphics.Add( graphic );
            }

            return graphics;
        }

        public static Dictionary<string, object> ReadProperties( JsonObject jsonObject )
        {
            jsonObject.RequireArgument<JsonObject>( "jsonObject" ).NotNull<JsonObject>();

            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach( string key in jsonObject.Keys )
            {
                if( jsonObject[ key ].JsonType == JsonType.String )
                    properties.Add( key, ( string ) jsonObject[ key ] );
                else if( jsonObject[ key ].JsonType == JsonType.Number )
                    properties.Add( key, ( double ) jsonObject[ key ] );
                else if( jsonObject[ key ].JsonType == JsonType.Boolean )
                    properties.Add( key, ( bool ) jsonObject[ key ] );
                else
                    properties.Add(key, jsonObject[key]);
            }

            return properties;
        }

        public static List<Geometry> ReadGeometry( Stream inputStream )
        {
            return ReadGeometry( JsonObject.Load( inputStream ) as JsonObject );
        }

        public static List<Geometry> ReadGeometry( JsonObject jsonObject )
        {
            jsonObject.RequireArgument<JsonObject>( "jsonObject" ).NotNull<JsonObject>();

            if( !jsonObject.Keys.Contains( TYPE_IDENTIFIER ) )
                throw new InvalidOperationException( "Geometry objects should contain a VALID geometry 'type' property value." );

            string type = (string)jsonObject[ TYPE_IDENTIFIER ];
            if( type == GEOMETRY_COLLECTION_TYPE_NAME )
            {
                return ReadGeometryCollection( jsonObject );
            }
            else
            {
                return new List<Geometry>(){ ReadIndividualGeometry( jsonObject ) };
            }

            return null;
        }

        public static List<Geometry> ReadGeometryCollection( JsonObject jsonObject )
        {
            throw new NotImplementedException();
        }

        public static Geometry ReadIndividualGeometry( JsonObject jsonObject )
        {
            jsonObject.RequireArgument<JsonObject>( "jsonObject" ).NotNull<JsonObject>();

            string geometryType = ( string ) jsonObject[ TYPE_IDENTIFIER ];
            if( string.IsNullOrEmpty( geometryType ) )
                throw new InvalidOperationException( "Geometry objects should contain a VALID geometry 'type' property value." );
            if( geometryType == GEOMETRY_COLLECTION_TYPE_NAME )
                throw new InvalidOperationException( "The object is a 'GeometryCollection' and not a 'Geometry'." );

            JsonArray coordinates = ( JsonArray ) jsonObject[ COORDINATES_IDENTIFIER ];
            if( coordinates == null )
                throw new InvalidOperationException( "Geometry objects should contain a valid 'coordinates' array." );

            Geometry geom = null;
            switch( geometryType )
            {
                case POINT_TYPE_NAME:
                    geom = ReadPoint( coordinates );
                    break;
                case MULTIPOINT_TYPE_NAME:
                    geom = ReadMultiPoint( coordinates );
                    break;
                case LINESTRING_TYPE_NAME:
                    Polyline line = new Polyline();
                    line.Paths.Add( ReadLineString( coordinates ) );
                    geom = line;
                    break;
                case MULTILINESTRING_TYPE_NAME:
                    geom = ReadMultiLineString( coordinates );
                    break;
                case POLYGON_TYPE_NAME:
                    geom = ReadPolygon( coordinates );
                    break;
                case MULTIPOLYGON_TYPE_NAME:
                    geom = ReadMultiPolygon( coordinates );
                    break;
                default:
                    break;
            }

            return geom;
        }

        private static Geometry ReadMultiPolygon( JsonArray polygons )
        {
            polygons.RequireArgument<JsonArray>( "polygons" ).NotNull<JsonArray>();

            Polygon areas = new Polygon();
            foreach( JsonValue polygon in polygons )
            {
                JsonArray polygonCoords = polygon as JsonArray;
                Polygon poly = ReadPolygon( polygonCoords );
                poly.Rings.ForEach<PointCollection>( item => areas.Rings.Add( item ) );
            }

            return areas;
        }

        private static Polygon ReadPolygon( JsonArray rings )
        {
            rings.RequireArgument<JsonArray>( "rings" ).NotNull<JsonArray>();

            return ReadRings( rings );
        }

        private static Polygon ReadRings( JsonArray rings )
        {
            rings.RequireArgument<JsonArray>( "rings" ).NotNull<JsonArray>();

            Polygon areas = new Polygon();
            foreach( JsonValue entry in rings )
            {
                JsonArray line = entry as JsonArray;
                areas.Rings.Add( ReadRing( line ) );
            }

            return areas;
        }

        private static PointCollection ReadRing( JsonArray points )
        {
            points.RequireArgument<JsonArray>( "points" ).NotNull<JsonArray>();

            PointCollection ringCoords = new PointCollection();
            foreach( JsonValue entry in points )
            {
                JsonArray coordinates = entry as JsonArray;
                ringCoords.Add( ReadPoint( coordinates ) );
            }
            return ringCoords;
        }

        private static Geometry ReadMultiLineString( JsonArray paths )
        {
            paths.RequireArgument<JsonArray>( "paths" ).NotNull<JsonArray>();

            Polyline lines = new Polyline();
            foreach( JsonValue entry in paths )
            {
                JsonArray line = entry as JsonArray;
                lines.Paths.Add(ReadLineString(line));
            }

            return lines;
        }

        public static PointCollection ReadLineString( JsonArray points )
        {
            points.RequireArgument<JsonArray>( "points" ).NotNull<JsonArray>();

            PointCollection linestring = new PointCollection();
            foreach( JsonValue entry in points )
            {
                JsonArray coordinates = entry as JsonArray;
                linestring.Add(ReadPoint(coordinates));
            }
            return linestring;
        }

        public static MultiPoint ReadMultiPoint( JsonArray points )
        {
            points.RequireArgument<JsonArray>( "points" ).NotNull<JsonArray>();

            MultiPoint multiPoint = new MultiPoint();
            foreach( JsonValue entry in points )
            {
                JsonArray coordinates = entry as JsonArray;
                multiPoint.Points.Add(ReadPoint(coordinates));
            }
            return multiPoint;
        }

        public static MapPoint ReadPoint( JsonArray coordinates )
        {
            coordinates.RequireArgument<JsonArray>( "coordinates" ).NotNull<JsonArray>();
            return ReadCoordinates( coordinates );
        }

        public static MapPoint ReadCoordinates( JsonArray coordinates )
        {
            coordinates.RequireArgument<JsonArray>( "coordinates" ).NotNull<JsonArray>();

            if( coordinates.Count > 3 )
                throw new InvalidOperationException( "Only a maximum of 3 coordinate values can be present." );
            if( coordinates.Count < 2 )
                throw new InvalidOperationException( "A minimum of 2 coordinate values must be present." );

            return new MapPoint( (double)coordinates[0], (double)coordinates[ 1 ] );
        }

    }
}