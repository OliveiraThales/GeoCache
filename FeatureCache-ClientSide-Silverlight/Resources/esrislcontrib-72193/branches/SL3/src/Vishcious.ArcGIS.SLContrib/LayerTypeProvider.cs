using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ESRI.ArcGIS.Client;
using System.Collections.ObjectModel;
using ESRI.ArcGIS.Client.Geometry;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class LayerTypeProvider
    {
        public static Type CreateLayerRecordType(LayerMetadata metadata)
        {
            Dictionary<string, Type> types = new Dictionary<string, Type>();
            metadata.Fields.ForEach<FieldMetadata>( fld =>
            {
                if( fld.Type != "esriFieldTypeGeometry" )
                    types.Add(fld.Name, GetCorrespondingType(fld.Type));
            } );

            return TypeFactory.CreateType( metadata.Name, types );
        }

        public static ObservableCollection<object> CreateLayerRecords( Type recordType, IList<Graphic> graphics )
        {
            Type genericType = typeof( ObservableCollection<> );
            Type listType = genericType.MakeGenericType( recordType );
            ObservableCollection<object> list = new ObservableCollection<object>();

            foreach( Graphic entry in graphics )
            {
                list.Add(LayerTypeProvider.CreateLayerRecordObject(recordType, entry));
            }

            return list;
        }

        public static ObservableCollection<T> CreateLayerRecords<T>( IList<Graphic> graphics )where T: AttributesBase
        {
            Type genericType = typeof( ObservableCollection<> );
            Type listType = genericType.MakeGenericType(typeof(T));
            ObservableCollection<T> list = Activator.CreateInstance( listType ) as ObservableCollection<T>;

            foreach( Graphic entry in graphics )
            {
                list.Add( LayerTypeProvider.CreateLayerRecordObject<T>( entry ) );
            }

            return list;
        }

        public static object CreateLayerRecordObject( Type type, IDictionary<string, object> attributes )
        {
            type.RequireArgument<Type>( "type" ).NotNull<Type>();
            attributes.RequireArgument<IDictionary<string, object>>( "attributes" ).NotNull<IDictionary<string, object>>();

            return Activator.CreateInstance( type, new DefaultAttributeProvider( attributes ) );
        }

        public static object CreateLayerRecordObject( Type type, Graphic graphic )
        {
            return CreateLayerRecordObject( type, graphic.Attributes );
        }

        public static T CreateLayerRecordObject<T>( IDictionary<string, object> attributes )where T: AttributesBase
        {
            attributes.RequireArgument<IDictionary<string, object>>( "attributes" ).NotNull<IDictionary<string, object>>();

            return Activator.CreateInstance(typeof(T), new DefaultAttributeProvider(attributes)) as T;
        }

        public static T CreateLayerRecordObject<T>( Graphic graphic ) where T : AttributesBase
        {
            graphic.RequireArgument<Graphic>( "graphic" ).NotNull<Graphic>();

            return Activator.CreateInstance( typeof( T ), new DefaultAttributeProvider( graphic.Attributes ) ) as T;
        }

        private static Type GetCorrespondingType( string p )
        {
            switch( p )
            {
                case "esriFieldTypeSmallInteger":
                    return typeof( Int16 );
                case "esriFieldTypeInteger":
                    return typeof( Int32 );
                case "esriFieldTypeSingle":
                    return typeof( Single );
                case "esriFieldTypeDouble":
                    return typeof( double );
                case "esriFieldTypeString":
                    return typeof( string );
                case "esriFieldTypeDate":
                    return typeof( DateTime );
                case "esriFieldTypeOID":
                    return typeof( Int32 );
                case "esriFieldTypeGeometry":
                    return typeof( Geometry );
                case "esriFieldTypeBlob":
                    return typeof( object );
                case "esriFieldTypeRaster":
                    return typeof( object );
                case "esriFieldTypeGUID":
                    return typeof( Guid );
                case "esriFieldTypeGlobalID":
                    return typeof( Guid );
                default:
                    return typeof( string );
            }
        }
    }
}