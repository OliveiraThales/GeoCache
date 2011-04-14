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
using Vishcious.GPX;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class GPXHelper
    {
        public static GraphicCollection LoadGraphics( gpxType source )
        {
            source.RequireArgument<gpxType>( "source" ).NotNull<gpxType>();

            GraphicCollection graphics = new GraphicCollection();

            if( source == null )
                return graphics;

            if( source.wpt != null )
            {
                GraphicCollection gcs = LoadWayPoints( source.wpt );
                gcs.ForEach<Graphic>( g => graphics.Add(g) );
            }
            if( source.rte != null )
            {
                GraphicCollection gcs = LoadRoutes( source.rte );
                gcs.ForEach<Graphic>( g => graphics.Add(g) );
            }
            if( source.trk != null )
            {
                GraphicCollection gcs = LoadTracks( source.trk );
                gcs.ForEach<Graphic>( g => graphics.Add( g ) );
            }

            return graphics;
        }

        public static GraphicCollection LoadTracks( trkTypeCollection trks )
        {
            trks.RequireArgument<trkTypeCollection>( "trks" ).NotNull<trkTypeCollection>();

            GraphicCollection lines = new GraphicCollection();
            foreach( var trk in trks )
            {
                Graphic gr = new Graphic();
                gr.Geometry = LoadTrack(trk.trkseg);
                LoadTrackAttributes( trk, gr.Attributes );
                lines.Add( gr );
            }

            return lines;
        }

        public static Dictionary<string, object> LoadTrackAttributes( trkType trk, Dictionary<string, object> attributes )
        {
            trk.RequireArgument<trkType>( "trk" ).NotNull<trkType>();
            attributes.RequireArgument<Dictionary<string, object>>( "attributes" ).NotNull<Dictionary<string, object>>();

            attributes.Add( "name", trk.name );
            attributes.Add( "cmt", trk.cmt );
            attributes.Add( "desc", trk.desc );
            attributes.Add( "src", trk.src );
            attributes.Add( "link", trk.link );
            attributes.Add( "number", trk.number );
            attributes.Add( "type", trk.type );

            return attributes;
        }

        public static ESRI.ArcGIS.Client.Geometry.Polyline LoadTrack( trksegTypeCollection trksegs )
        {
            trksegs.RequireArgument<trksegTypeCollection>( "trksegs" ).NotNull<trksegTypeCollection>();

            ESRI.ArcGIS.Client.Geometry.Polyline line = new ESRI.ArcGIS.Client.Geometry.Polyline();
            line.Paths = new ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>();
            foreach( var trkseg in trksegs )
            {
                line.Paths.Add( LoadTrackSegment( trkseg.trkpt ) );
            }

            return line;
        }

        public static ESRI.ArcGIS.Client.Geometry.PointCollection LoadTrackSegment( wptTypeCollection trksegs )
        {
            trksegs.RequireArgument<wptTypeCollection>( "trksegs" ).NotNull<wptTypeCollection>();

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            foreach( var trkseg in trksegs )
            {
                pts.Add( LoadWayPointGeometry( trkseg ) );
            }
            return pts;
        }

        public static GraphicCollection LoadRoutes( rteTypeCollection rtes )
        {
            rtes.RequireArgument<rteTypeCollection>( "rtes" ).NotNull<rteTypeCollection>();

            GraphicCollection routes = new GraphicCollection();
            foreach( var rte in rtes )
            {
                Graphic gr = new Graphic();
                gr.Geometry = LoadRoute( rte.rtept );
                LoadRouteAttributes( rte, gr.Attributes );
                routes.Add( gr );
            }

            return routes;
        }

        public static Dictionary<string, object> LoadRouteAttributes( rteType rte, Dictionary<string, object> attributes )
        {
            rte.RequireArgument<rteType>( "rte" ).NotNull<rteType>();
            attributes.RequireArgument<Dictionary<string, object>>( "attributes" ).NotNull<Dictionary<string, object>>();

            attributes.Add( "name", rte.name );
            attributes.Add( "cmt", rte.cmt );
            attributes.Add( "desc", rte.desc );
            attributes.Add( "src", rte.src );
            attributes.Add( "link", rte.link );
            attributes.Add( "number", rte.number );
            attributes.Add( "type", rte.type );

            return attributes;
        }

        public static ESRI.ArcGIS.Client.Geometry.Polyline LoadRoute( wptTypeCollection rte )
        {
            rte.RequireArgument<wptTypeCollection>( "rte" ).NotNull<wptTypeCollection>();

            ESRI.ArcGIS.Client.Geometry.PointCollection pts = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            foreach( var pt in rte )
            {
                pts.Add( LoadWayPointGeometry( pt ) );
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

        public static GraphicCollection LoadWayPoints( wptTypeCollection wpts )
        {
            wpts.RequireArgument<wptTypeCollection>( "wpts" ).NotNull<wptTypeCollection>();

            GraphicCollection pts = new GraphicCollection();
            foreach( var wpt in wpts )
            {
                Graphic gr = new Graphic();
                gr.Geometry = LoadWayPointGeometry( wpt );
                LoadWayPointAttributes( wpt, gr.Attributes );
                pts.Add( gr );
            }

            return pts;
        }

        public static Dictionary<string, object> LoadWayPointAttributes( wptType wpt, Dictionary<string, object> attributes )
        {
            wpt.RequireArgument<wptType>( "wpt" ).NotNull<wptType>();
            attributes.RequireArgument<Dictionary<string, object>>( "attributes" ).NotNull<Dictionary<string, object>>();

            attributes.Add( "ele", wpt.ele );
            attributes.Add( "time", wpt.time );
            attributes.Add( "magvar", wpt.magvar );
            attributes.Add( "geoidheight", wpt.geoidheight );
            attributes.Add( "name", wpt.name );
            attributes.Add( "cmt", wpt.cmt );
            attributes.Add( "desc", wpt.desc );
            attributes.Add( "src", wpt.src );
            attributes.Add( "link", wpt.link );
            attributes.Add( "sym", wpt.sym );
            attributes.Add( "type", wpt.type );
            attributes.Add( "fix", wpt.fix );
            attributes.Add( "sat", wpt.sat );
            attributes.Add( "hdop", wpt.hdop );
            attributes.Add( "vdop", wpt.vdop );
            attributes.Add( "pdop", wpt.pdop );
            attributes.Add( "ageofdgpsdata", wpt.ageofdgpsdata );
            attributes.Add( "dgpsid", wpt.dgpsid );

            return attributes;
        }

        public static MapPoint LoadWayPointGeometry( wptType wpt )
        {
            wpt.RequireArgument<wptType>( "wpt" ).NotNull<wptType>();

            return new MapPoint( ( double ) wpt.lon, ( double ) wpt.lat );
        }
    }
}
