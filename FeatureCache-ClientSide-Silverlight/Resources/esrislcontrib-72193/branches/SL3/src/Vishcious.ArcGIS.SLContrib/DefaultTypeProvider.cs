using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Vishcious.ArcGIS.SLContrib
{
    public class DefaultTypeProvider : IAttributes
    {
        public IDictionary<string, object> Attributes
        {
            get;
            set;
        }

        public DefaultTypeProvider()
        {
            Attributes = new Dictionary<string, object>();
        }

        public DefaultTypeProvider( IDictionary<string, object> attributes )
        {
            Attributes = attributes;
        }

        public object CreateDynamicWrapper(string className)
        {
            var type = TypeFactory.CreateType( className, Attributes.Keys.ToArray<string>() );
            return Activator.CreateInstance( type, this );
        }

        public object GetValue( string name )
        {
            return Attributes[ name ];
        }

        public void SetValue( string name, object value )
        {
            Attributes[ name ] = value;
            if( ValueChanged != null )
                ValueChanged( this, new PropertyChangedEventArgs( name ) );
        }

        public event PropertyChangedEventHandler ValueChanged;
    }
}