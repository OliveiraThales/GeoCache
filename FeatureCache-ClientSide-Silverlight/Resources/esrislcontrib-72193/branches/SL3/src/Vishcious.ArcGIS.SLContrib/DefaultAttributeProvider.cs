using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Vishcious.ArcGIS.SLContrib
{
    public class DefaultAttributeProvider : IAttributes
    {
        public IDictionary<string, object> Attributes
        {
            get;
            set;
        }

        public DefaultAttributeProvider()
        {
            Attributes = new Dictionary<string, object>();
        }

        public DefaultAttributeProvider( IDictionary<string, object> attributes )
        {
            Attributes = attributes;
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