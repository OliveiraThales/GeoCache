using System;
using System.ComponentModel;

namespace Vishcious.ArcGIS.SLContrib
{
    /// <summary>
    /// IAttributes interface
    /// </summary>
    public interface IAttributes
    {
        object GetValue( string name );
        void SetValue( string name, object value );
        event PropertyChangedEventHandler ValueChanged;
    }
}