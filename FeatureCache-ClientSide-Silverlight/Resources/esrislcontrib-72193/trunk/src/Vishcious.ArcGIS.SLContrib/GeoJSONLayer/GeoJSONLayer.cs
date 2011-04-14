using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client;
using System.IO;
using System.Collections.Generic;

namespace Vishcious.ArcGIS.SLContrib
{
    public class GeoJSONLayer : GraphicsLayer
    {

        #region URL (DependencyProperty)

        /// <summary>
        /// A description of the property.
        /// </summary>
        public Uri URL
        {
            get
            {
                return ( Uri ) GetValue( URLProperty );
            }
            set
            {
                SetValue( URLProperty, value );
            }
        }
        public static readonly DependencyProperty URLProperty =
            DependencyProperty.Register( "URL", typeof( Uri ), typeof( GeoJSONLayer ),
            new PropertyMetadata( new Uri( "http://earthquake.usgs.gov/eqcenter/catalogs/eqs7day-M5.xml" ), new PropertyChangedCallback( OnURLChanged ) ) );

        private static void OnURLChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ( ( GeoJSONLayer ) d ).OnURLChanged( e );
        }

        protected virtual void OnURLChanged( DependencyPropertyChangedEventArgs e )
        {
            Load( new Uri( e.NewValue.ToString() ) );
        }

        #endregion

        private Action<Stream> _completeHandler;
        private Action<Exception> _errorHandler;
        private List<Graphic> _graphics;

        public event EventHandler<EventArgs> LoadCompleted;
        public event EventHandler<EventArgs<string>> LoadFailed;

        public GeoJSONLayer()
        {
            Renderer = new SimpleRenderer();

            _completeHandler = ( stm ) =>
            {
                Graphics.Clear();
                _graphics = GeoJSONReader.ReadGeoJSON( stm );
                LoadGraphics();
                if( LoadCompleted != null )
                {
                    LoadCompleted( this, new EventArgs() );
                }
            };

            _errorHandler = ( ex ) =>
            {
                if( LoadFailed != null )
                {
                    LoadFailed( this, new EventArgs<string>(ex.Message) );
                }
            };
        }

        public override void Initialize()
        {
            base.Initialize();
            Load( URL );
        }

        private void Load( Uri URL )
        {
            if( IsInitialized && URL != null )
            {
                Utils.DoGET( URL, _completeHandler, _errorHandler );
            }
        }

        private void LoadGraphics()
        {
            foreach( Graphic item in _graphics )
            {
                Graphics.Add( item );
            }
        }
    }
}
