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

namespace Vishcious.ArcGIS.SLContrib
{
    public class GeoRSSLayer : GraphicsLayer
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
            DependencyProperty.Register( "URL", typeof( Uri ), typeof( GeoRSSLayer ),
            new PropertyMetadata( new Uri( "http://earthquake.usgs.gov/eqcenter/catalogs/eqs7day-M5.xml" ), new PropertyChangedCallback( OnURLChanged ) ) );

        private static void OnURLChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ( ( GeoRSSLayer ) d ).OnURLChanged( e );
        }

        protected virtual void OnURLChanged( DependencyPropertyChangedEventArgs e )
        {
            Load( new Uri(e.NewValue.ToString()) );
        }

        #endregion

        public event EventHandler<EventArgs> LoadCompleted;
        public event EventHandler<EventArgs<string>> LoadFailed;
        RSSDataSource _source = new RSSDataSource();

        public GeoRSSLayer()
        {
            Renderer = new SimpleRenderer();

            _source.LoadCompleted += ( sender, e ) =>
                {
                    Graphics.Clear();
                    LoadGraphics();
                    if( LoadCompleted != null )
                    {
                        LoadCompleted( this, e );
                    }
                };

            _source.LoadFailed += ( sender, e ) =>
                {
                    if( LoadFailed != null )
                    {
                        LoadFailed( this, e );
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
            if( IsInitialized && URL != null)
            {
                _source.URL = URL;
                _source.Load();
            }
        }

        private void LoadGraphics()
        {
            if( _source.RSSElementAttributes.FirstOrDefault<SimpleElement>( item => item.Name.ToLower() == "georss" ) != null )
            {
                LoadGraphicsSimple();
            }

            if( _source.RSSElementAttributes.FirstOrDefault<SimpleElement>( item => item.Name.ToLower() == "geo" ) != null )
            {
                LoadGraphicsW3C();
            }

            return;
        }

        private void LoadGraphicsSimple()
        {
            foreach( RSSItem item in _source )
            {
                SimpleElement pointEl = item.Elements.FirstOrDefault<SimpleElement>( se => se.Name.ToLower() == "point" );

                if( pointEl != null )
                {
                    string[] coords = pointEl.Value.Split( ' ' );
                    if( coords.Length < 2 )
                        return;
                    string strLat = coords[0];
                    string strLong = coords[1];

                    if( !string.IsNullOrEmpty( strLat ) && !string.IsNullOrEmpty( strLong ) )
                    {
                        double lat;
                        double lon;
                        if( double.TryParse( strLat, out lat ) && double.TryParse( strLong, out lon ) )
                        {
                            MapPoint point = new MapPoint( lon, lat );
                            Graphic g = new Graphic()
                            {
                                Geometry = point,
                            };
                            g.Symbol = Renderer.GetSymbol( g );

                            //Add the GeoRSS item's child elements as Attributes to the graphic
                            int count = 0;
                            foreach( SimpleElement entry in item.Elements )
                            {
                                string key = entry.Name;

                                if( g.Attributes.ContainsKey( entry.Name ) )
                                    key = entry.Name + " " + count.ToString();

                                //Add the element name and its value
                                g.Attributes.Add( key, entry.Value );

                                //Add any attributes on the element if any
                                foreach( var i in entry.Attributes )
                                {
                                    g.Attributes.Add( key + "." + i.Name, i.Value );
                                }

                                count++;
                            }

                            Graphics.Add( g );
                        }
                    }
                }
            }
        }

        private void LoadGraphicsW3C()
        {
            foreach( RSSItem item in _source )
            {
                SimpleElement latEl = item.Elements.FirstOrDefault<SimpleElement>(se => se.Name.ToLower() == "lat");
                SimpleElement lonEl = item.Elements.FirstOrDefault<SimpleElement>( se => se.Name.ToLower() == "long" );

                if( latEl != null && lonEl != null )
                {
                    string strLat = latEl.Value;
                    string strLong = lonEl.Value;

                    if( !string.IsNullOrEmpty( strLat ) && !string.IsNullOrEmpty( strLong ) )
                    {
                        double lat;
                        double lon;
                        if( double.TryParse( strLat, out lat ) && double.TryParse( strLong, out lon ) )
                        {
                            MapPoint point = new MapPoint( lon, lat );
                            Graphic g = new Graphic()
                            {
                                Geometry = point,
                            };
                            g.Symbol = Renderer.GetSymbol( g );

                            //Add the GeoRSS item's child elements as Attributes to the graphic
                            int count = 0;
                            foreach( SimpleElement entry in item.Elements )
                            {
                                string key = entry.Name;

                                if( g.Attributes.ContainsKey( entry.Name ) )
                                    key = entry.Name + " " + count.ToString();

                                //Add the element name and its value
                                g.Attributes.Add( key, entry.Value );

                                //Add any attributes on the element if any
                                foreach( var i in entry.Attributes )
                                {
                                    g.Attributes.Add( key + "." + i.Name, i.Value );
                                }

                                count++;
                            }

                            Graphics.Add( g );
                        }
                    }
                }
            }
        }
    }
}
