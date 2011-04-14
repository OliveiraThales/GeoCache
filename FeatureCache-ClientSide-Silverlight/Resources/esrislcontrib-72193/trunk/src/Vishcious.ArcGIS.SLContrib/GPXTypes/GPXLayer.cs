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
using ESRI.ArcGIS.Client;
using System.IO;
using System.Collections.Generic;
using Vishcious.GPX;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.ObjectModel;

namespace Vishcious.ArcGIS.SLContrib
{
    public class GPXLayer : GraphicsLayer
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
            DependencyProperty.Register( "URL", typeof( Uri ), typeof( GPXLayer ),
            new PropertyMetadata( new Uri( "http://www.topografix.com/fells_loop.gpx" ), new PropertyChangedCallback( OnURLChanged ) ) );

        private static void OnURLChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            ( ( GPXLayer ) d ).OnURLChanged( e );
        }

        protected virtual void OnURLChanged( DependencyPropertyChangedEventArgs e )
        {
            Load( new Uri( e.NewValue.ToString() ) );
        }

        #endregion

        private Action<Stream> _completeHandler;
        private Action<Exception> _errorHandler;
        private gpxType _source = new gpxType();

        public event EventHandler<EventArgs> LoadCompleted;
        public event EventHandler<EventArgs<string>> LoadFailed;

        public GPXLayer()
        {
            Renderer = new SimpleRenderer();

            _completeHandler = ( stm ) =>
            {
                Graphics.Clear();
                _source = GPXReader.ReadGPX( stm );
                Graphics = LoadGraphics(_source);
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

        private GraphicCollection LoadGraphics( gpxType source )
        {
            GraphicCollection gcs = GPXHelper.LoadGraphics( source );

            foreach( Graphic gr in gcs )
            {
                gr.Symbol = Renderer.GetSymbol( gr );
            }

            return gcs;
        }

        private void LoadGraphics(gpxType source, GraphicCollection graphics)
        {
            if( source == null )
                return;

            if( source.wpt != null )
                LoadWayPoints( source.wpt, graphics );
            if( source.rte != null )
                LoadRoutes( source.rte, graphics );
            if( source.trk != null )
                LoadTracks( source.trk, graphics );
        }

        private void LoadTracks( trkTypeCollection trks, GraphicCollection graphics )
        {
            trks.RequireArgument<trkTypeCollection>( "trks" ).NotNull<trkTypeCollection>();

            foreach( var trk in trks )
            {
                Graphic gr = new Graphic()
                {
                    Geometry = LoadTrack(trk.trkseg)
                };
                LoadTrackAttributes(trk, gr.Attributes);
                gr.Symbol = Renderer.GetSymbol( gr );
                graphics.Add( gr );
            }
        }

        private IDictionary<string, object> LoadTrackAttributes( trkType trk, IDictionary<string, object> attributes )
        {
            trk.RequireArgument<trkType>( "trk" ).NotNull<trkType>();
            attributes.RequireArgument<IDictionary<string, object>>( "attributes" ).NotNull<IDictionary<string, object>>();

            attributes.Add( "name", trk.name );
            attributes.Add( "cmt", trk.cmt );
            attributes.Add( "desc", trk.desc );
            attributes.Add( "src", trk.src );
            attributes.Add( "link", trk.link );
            attributes.Add( "number", trk.number );
            attributes.Add( "type", trk.type );

            return attributes;
        }

        private ESRI.ArcGIS.Client.Geometry.Polyline LoadTrack( trksegTypeCollection trksegs )
        {
            trksegs.RequireArgument<trksegTypeCollection>( "trksegs" ).NotNull<trksegTypeCollection>();

            ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
            line.Paths = new ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>();
            foreach( var trkseg in trksegs )
            {
                line.Paths.Add(LoadTrackSegment(trkseg.trkpt));
            }

            return line;
        }
 
        private ESRI.ArcGIS.Client.Geometry.PointCollection LoadTrackSegment( wptTypeCollection trksegs )
        {
            trksegs.RequireArgument<wptTypeCollection>( "trksegs" ).NotNull<wptTypeCollection>();

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            foreach( var trkseg in trksegs )
            {
                pts.Add( LoadWayPointGeometry(trkseg));
            }
            return pts;
        }

        private void LoadRoutes( rteTypeCollection rtes, GraphicCollection graphics )
        {
            rtes.RequireArgument<rteTypeCollection>( "rtes" ).NotNull<rteTypeCollection>();

            foreach( var rte in rtes )
            {
                Graphic gr = new Graphic()
                {
                    Geometry = LoadRoute(rte.rtept)
                };
                LoadRouteAttributes(rte, gr.Attributes);
                gr.Symbol = Renderer.GetSymbol( gr );
                graphics.Add( gr );
            }
        }

        private IDictionary<string, object> LoadRouteAttributes( rteType rte, IDictionary<string, object> attributes )
        {
            rte.RequireArgument<rteType>( "rte" ).NotNull<rteType>();
            attributes.RequireArgument<IDictionary<string, object>>( "attributes" ).NotNull<IDictionary<string, object>>();

            attributes.Add( "name", rte.name );
            attributes.Add( "cmt", rte.cmt );
            attributes.Add( "desc", rte.desc );
            attributes.Add( "src", rte.src );
            attributes.Add( "link", rte.link );
            attributes.Add( "number", rte.number );
            attributes.Add( "type", rte.type );

            return attributes;
        }

        private ESRI.ArcGIS.Client.Geometry.Polyline LoadRoute( wptTypeCollection rte )
        {
            rte.RequireArgument<wptTypeCollection>( "rte" ).NotNull<wptTypeCollection>();

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            foreach( var pt in rte )
            {
                pts.Add(LoadWayPointGeometry(pt));
            }

            ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline()
            {
                Paths = new ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>()
                {
                    pts
                }
            };
            return line;
        }

        private void LoadWayPoints( wptTypeCollection wpts, GraphicCollection graphics )
        {
            wpts.RequireArgument<wptTypeCollection>( "wpts" ).NotNull<wptTypeCollection>();

            foreach( var wpt in wpts )
            {
                Graphic gr = new Graphic()
                {
                    Geometry = LoadWayPointGeometry(wpt)
                };
                LoadWayPointAttributes( wpt, gr.Attributes );
                gr.Symbol = Renderer.GetSymbol( gr );
                graphics.Add( gr );
            }
        }

        private IDictionary<string, object> LoadWayPointAttributes( wptType wpt, IDictionary<string, object> attributes )
        {
            wpt.RequireArgument<wptType>( "wpt" ).NotNull<wptType>();
            attributes.RequireArgument<IDictionary<string, object>>( "attributes" ).NotNull<IDictionary<string, object>>();

            attributes.Add( "ele", wpt.ele );
            attributes.Add("time", wpt.time);
            attributes.Add("magvar", wpt.magvar);
            attributes.Add("geoidheight", wpt.geoidheight);
            attributes.Add("name", wpt.name);
            attributes.Add("cmt", wpt.cmt);
            attributes.Add("desc", wpt.desc);
            attributes.Add("src", wpt.src);
            attributes.Add("link", wpt.link);
            attributes.Add("sym", wpt.sym);
            attributes.Add("type", wpt.type);
            attributes.Add("fix", wpt.fix);
            attributes.Add("sat", wpt.sat);
            attributes.Add("hdop", wpt.hdop);
            attributes.Add("vdop", wpt.vdop);
            attributes.Add("pdop", wpt.pdop);
            attributes.Add("ageofdgpsdata", wpt.ageofdgpsdata);
            attributes.Add("dgpsid", wpt.dgpsid);

            return attributes;
        }

        private MapPoint LoadWayPointGeometry( wptType wpt )
        {
            wpt.RequireArgument<wptType>( "wpt" ).NotNull<wptType>();

            return new MapPoint((double)wpt.lon, (double)wpt.lat);
        }
    }
}
