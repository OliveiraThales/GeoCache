using System;
using System.ComponentModel;

namespace Vishcious.ArcGIS.SLContrib
{
    #region AttributesBase class

    /// <summary>
    /// Base for dynamic class
    /// </summary>
    public class AttributesBase : INotifyPropertyChanged, IDisposable
    {
        private IAttributes provider;

        public AttributesBase( IAttributes provider )
        {
            this.provider = provider;

            if( provider != null )
                provider.ValueChanged += OnPropertyChanged;
        }

        public object GetValue( string name )
        {
            return provider.GetValue( name );
        }

        public void SetValue( string name, object value )
        {
            provider.SetValue( name, value );
        }

        private void OnPropertyChanged( object sender, PropertyChangedEventArgs e )
        {
            if( PropertyChanged != null )
                PropertyChanged( this, e );
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if( provider != null )
                provider.ValueChanged -= OnPropertyChanged;
        }

        #endregion
    }

    #endregion
}