using System;
using System.Json;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.Generic;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class ArcJSONReader
    {
        #region "CONSTANTS"

        public const string LYR_ID = "id";
        public const string LYR_NAME = "name";
        public const string LYR_TYPE = "type";
        public const string LYR_GEOMETRY_TYPE = "geometryType";
        public const string LYR_DESCRIPTION = "description";
        public const string LYR_DEFINITION_EXPRESSION = "definitionExpression";
        public const string LYR_COPYRIGHT_TEXT = "copyrightText";
        public const string LYR_MINIMUM_SCALE = "minScale";
        public const string LYR_MAXIMUM_SCALE = "maxScale";
        public const string LYR_EXTENT = "extent";
        public const string LYR_DISPLAY_FIELD = "displayField";
        public const string LYR_FIELDS = "fields";
        public const string LYR_PARENT_LAYER = "parentLayer";
        public const string LYR_SUBLAYERS = "subLayers";
        public const string LYR_LAYER_REF_ID = "id";
        public const string LYR_LAYER_REF_NAME = "name";

        public const string FIELD_NAME = "name";
        public const string FIELD_TYPE = "type";
        public const string FIELD_ALIAS = "alias";

        public const string SPATIALREFERENCE_WKID = "wkid";
        public const string GEOMETRY_SPATIALREFERENCE = "spatialReference";
        public const string GEOMETRY_ENVELOPE_XMIN = "xmin";
        public const string GEOMETRY_ENVELOPE_YMIN = "ymin";
        public const string GEOMETRY_ENVELOPE_XMAX = "xmax";
        public const string GEOMETRY_ENVELOPE_YMAX = "ymax";

        #endregion

        public static LayerMetadata ReadLayerMetadata( JsonObject json )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();

            LayerMetadata lyr = new LayerMetadata();

            lyr.ID = (int)GetRequiredEntryValue( json, LYR_ID );
            lyr.Name = (string)GetRequiredEntryValue( json, LYR_NAME );
            lyr.Type = (string)GetRequiredEntryValue( json, LYR_TYPE );
            lyr.GeometryType = ( string ) GetRequiredEntryValue( json, LYR_GEOMETRY_TYPE );
            lyr.Description = ( string ) GetRequiredEntryValue( json, LYR_DESCRIPTION );
            lyr.DefinitionExpression = ( string ) GetRequiredEntryValue( json, LYR_DEFINITION_EXPRESSION );
            lyr.CopyrightText = ( string ) GetRequiredEntryValue( json, LYR_COPYRIGHT_TEXT );
            lyr.MinimumScale = ( double ) GetRequiredEntryValue( json, LYR_MINIMUM_SCALE );
            lyr.MaximumScale = ( double ) GetRequiredEntryValue( json, LYR_MAXIMUM_SCALE );

            string extent_entry = GetMatchingKey( json, LYR_EXTENT );
            if( string.IsNullOrEmpty( extent_entry ) )
                throw new FormatException( "An extent member is required in the layer metadata object." );
            if( !( json[ LYR_EXTENT ] is JsonObject ) )
                throw new FormatException( "The extent member should be of type JsonObject." );

            lyr.Extent = ReadEnvelope( json[ LYR_EXTENT ] as JsonObject);
            lyr.DisplayField = ( string ) GetRequiredEntryValue( json, LYR_DISPLAY_FIELD );

            string fields_entry = GetMatchingKey( json, LYR_FIELDS );
            if( string.IsNullOrEmpty( fields_entry ) )
                throw new FormatException( "A fields member is required in the layer metadata object." );
            if( json[ LYR_FIELDS ] != null )
            {
                if( !( json[ LYR_FIELDS ] is JsonArray ) )
                    throw new FormatException( "The fields member should be of type JsonArray." );

                lyr.Fields = ReadFieldMetadataArray( json[ LYR_FIELDS ] as JsonArray );
            }

            string parentLayer_entry = GetMatchingKey( json, LYR_PARENT_LAYER );
            if( string.IsNullOrEmpty( parentLayer_entry ) )
                throw new FormatException( "An parentLayer member is required in the layer metadata object." );
            if(json[LYR_PARENT_LAYER] != null)
            {
                if( !( json[ LYR_PARENT_LAYER ] is JsonObject ) )
                    throw new FormatException( "The parentLayer member should be of type JsonObject." );

                lyr.ParentLayer = ReadLayerReference( json[ LYR_PARENT_LAYER ] as JsonObject);
            }

            string subLayers_entry = GetMatchingKey( json, LYR_SUBLAYERS );
            if( string.IsNullOrEmpty( subLayers_entry ) )
                throw new FormatException( "A subLayers member is required in the layer metadata object." );
            if( json[ LYR_SUBLAYERS ] != null )
            {
                if( !( json[ LYR_SUBLAYERS ] is JsonArray ) )
                    throw new FormatException( "The subLayers member should be of type JsonArray." );

                lyr.SubLayers = ReadLayerReferenceArray( json[ LYR_SUBLAYERS ] as JsonArray );
            }

            return lyr;
        }

        public static List<FieldMetadata> ReadFieldMetadataArray( JsonArray json )
        {
            json.RequireArgument<JsonArray>( "json" ).NotNull<JsonArray>();

            List<FieldMetadata> flds = new List<FieldMetadata>();
            foreach( JsonValue item in json )
            {
                if( !( item is JsonObject ) )
                    throw new FormatException( "All entries in a field metadata array should be of type JsonObject." );

                FieldMetadata fld = ReadFieldMetadata( item as JsonObject );
                flds.Add( fld );
            }

            return flds;
        }

        public static FieldMetadata ReadFieldMetadata( JsonObject json )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();

            FieldMetadata fldMetadata = new FieldMetadata();

            fldMetadata.Name = ( string ) GetRequiredEntryValue( json, FIELD_NAME );
            fldMetadata.Type = ( string ) GetRequiredEntryValue( json, FIELD_TYPE );
            fldMetadata.Alias = ( string ) GetRequiredEntryValue( json, FIELD_ALIAS );

            return fldMetadata;
        }

        public static Dictionary<int, string> ReadLayerReferenceArray( JsonArray json )
        {
            json.RequireArgument<JsonArray>( "json" ).NotNull<JsonArray>();

            Dictionary<int, string> lyrRefs = new Dictionary<int, string>();
            foreach( JsonValue item in json )
            {
                if( !( item is JsonObject ) )
                    throw new FormatException( "All entries in a layer reference array should be of type JsonObjects." );

                KeyValuePair<int, string> entry = ReadLayerReference( item as JsonObject );
                lyrRefs.Add(entry.Key, entry.Value );
            }

            return lyrRefs;
        }

        public static KeyValuePair<int, string> ReadLayerReference( JsonObject json )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();

            int id = (int)GetRequiredEntryValue( json, LYR_LAYER_REF_ID );
            string name = ( string ) GetRequiredEntryValue( json, LYR_LAYER_REF_NAME );
            KeyValuePair<int, string> lyrRef = new KeyValuePair<int, string>(id, name);
            return lyrRef;
        }

        public static Envelope ReadEnvelope( JsonObject json )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();

            Envelope env = new Envelope();

            env.XMin = ( double ) GetRequiredEntryValue(json, GEOMETRY_ENVELOPE_XMIN);
            env.YMin = ( double ) GetRequiredEntryValue(json, GEOMETRY_ENVELOPE_YMIN);
            env.XMax = ( double ) GetRequiredEntryValue(json, GEOMETRY_ENVELOPE_XMAX);
            env.YMax = ( double ) GetRequiredEntryValue( json, GEOMETRY_ENVELOPE_YMAX );

            string sr_entry = GetMatchingKey( json, GEOMETRY_SPATIALREFERENCE);
            if( string.IsNullOrEmpty( sr_entry ) )
                throw new FormatException( string.Format( "A {0} member is expected in a Envelope object.", GEOMETRY_SPATIALREFERENCE ) );

            if( json[ sr_entry ] == null )
                return env;

            if( !( json[ sr_entry ] is JsonObject ) )
                throw new FormatException( "The Spatial reference member should be of type JsonObject" );
                
            env.SpatialReference = ReadSpatialReference( json[sr_entry] as JsonObject );

            return env;
        }

        public static SpatialReference ReadSpatialReference( JsonObject json )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();

            SpatialReference sr = new SpatialReference();
            string wkid_entry = GetMatchingKey( json, SPATIALREFERENCE_WKID );
            //Read the WKID value if it is present
            //If WKID is not present, do not throw an exception here for now
            //because a lot of times, the Spatial reference is an empty object if the user doesn't set it specifically
            if( !string.IsNullOrEmpty( wkid_entry ) )
                sr.WKID = (int)json[ wkid_entry ];

            return sr;
        }

        public static bool IsMatchingKeyPresent( JsonObject json, string key, bool isCaseSensitive )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();
            key.RequireArgument<string>( "key" ).NotNullOrEmpty();

            StringComparison sc = isCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
            foreach( string entry in json.Keys )
            {
                if( string.Compare( entry, key, sc ) == 0 )
                    return true;
            }

            return false;
        }

        public static bool IsMatchingKeyPresent( JsonObject json, string key )
        {
            return IsMatchingKeyPresent( json, key, false );
        }

        public static string GetMatchingKey( JsonObject json, string key, bool isCaseSensitive )
        {
            json.RequireArgument<JsonObject>( "json" ).NotNull<JsonObject>();
            key.RequireArgument<string>( "key" ).NotNullOrEmpty();

            StringComparison sc = isCaseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
            foreach( string entry in json.Keys )
            {
                if( string.Compare( entry, key, sc ) == 0 )
                    return entry;
            }

            return null;
        }

        public static string GetMatchingKey( JsonObject json, string key)
        {
            return GetMatchingKey( json, key, false );
        }

        private static JsonValue GetRequiredEntryValue( JsonObject json, string key )
        {
            string entry = GetMatchingKey( json, key );
            if( string.IsNullOrEmpty( entry ) )
                throw new FormatException( string.Format( "A {0} member is expected in the json object.", key ) );
            return json[ entry ];
        }
    }
}