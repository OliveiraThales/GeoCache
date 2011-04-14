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
using System.Collections.Generic;
using ESRI.ArcGIS.Client;
using System.IO;
using System.Xml.Linq;
using Vishcious.ArcGIS.SLContrib;

namespace Vishcious.GPX
{
    public static class GPXReader
    {

        public static gpxType ReadGPX( Stream inputStream )
        {
            return ReadGPX(XElement.Load(inputStream));
        }

        public static gpxType ReadGPX( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            if( element.Name.LocalName != "gpx" )
                throw new FormatException( string.Format("An element named 'gpx' was expected. But the element found was {0}.", element.Name) );

            gpxType gpx = new gpxType();
            XNamespace ns = element.Name.Namespace;
            gpx.version = element.Attribute( "version" ).Value;
            if(element.Element(ns + "metadata") != null)
                gpx.metadata = ReadMetadata( element.Element( ns + "metadata" ) );
            if(element.Elements(ns + "wpt") != null)
                gpx.wpt = ReadWayPoints( element.Elements( ns + "wpt" ) );
            if(element.Elements( ns + "rte") != null)
                gpx.rte = ReadRoutes( element.Elements( ns + "rte" ) );
            if(element.Elements( ns + "trk") != null)
                gpx.trk = ReadTracks( element.Elements( "trk" ) );
            //The extensions cannot be read because they are custom elements and can be in any format preferred

            return gpx;
        }

        private static wptTypeCollection ReadWayPoints( IEnumerable<XElement> elements )
        {
            elements.RequireArgument<IEnumerable<XElement>>( "elements" ).NotNull<IEnumerable<XElement>>();

            wptTypeCollection wpts = new wptTypeCollection();

            foreach( XElement element in elements )
            {
                wpts.Add( ReadWayPoint(element) );
            }

            return wpts;
        }

        private static wptType ReadWayPoint( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            wptType wpt = new wptType();
            wpt.lat = Convert.ToDecimal(element.Attribute("lat").Value);
            wpt.lon = Convert.ToDecimal(element.Attribute("lon").Value);
            if( element.Element( ns + "ele" ) != null )
                wpt.ele = Convert.ToDecimal(element.Element( ns + "ele" ).Value);
            if( element.Element( ns + "time" ) != null )
                wpt.time = Convert.ToDateTime(element.Element( ns + "time").Value);
            if( element.Element( ns + "magvar" ) != null )
                wpt.magvar = Convert.ToDecimal(element.Element( ns + "magvar").Value);
            if( element.Element( ns + "geoidheight" ) != null )
                wpt.geoidheight = Convert.ToDecimal(element.Element( ns + "geoidheight").Value);
            if( element.Element( ns + "name" ) != null )
                wpt.name = element.Element( ns + "name" ).Value;
            if( element.Element( ns + "cmt" ) != null )
                wpt.cmt = element.Element(ns + "cmt" ).Value;
            if( element.Element( ns + "desc" ) != null )
                wpt.desc = element.Element( ns + "desc" ).Value;
            if( element.Element( ns + "src" ) != null )
                wpt.src = element.Element( ns + "src" ).Value;
            if( element.Element( ns + "link" ) != null )
                wpt.link = ReadLinks(element.Elements( ns + "link"));
            if( element.Element( ns + "sym" ) != null )
                wpt.sym = element.Element( ns + "sym" ).Value;
            if( element.Element( ns + "type" ) != null )
                wpt.type = element.Element( ns + "type" ).Value;
            if( element.Element( ns + "fix" ) != null )
                wpt.fix = ReadFix( element.Element( ns + "fix" ) );
            if( element.Element( ns + "sat" ) != null )
                wpt.sat = element.Element( ns + "sat" ).Value;
            if( element.Element( ns + "hdop" ) != null )
                wpt.hdop = Convert.ToDecimal(element.Element( ns + "hdop").Value);
            if( element.Element( ns + "vdop" ) != null )
                wpt.vdop = Convert.ToDecimal(element.Element(ns + "vdop").Value);
            if( element.Element( ns + "pdop" ) != null )
                wpt.pdop = Convert.ToDecimal(element.Element( ns + "pdop").Value);
            if( element.Element( ns + "ageofdgpsdata" ) != null )
                wpt.ageofdgpsdata = Convert.ToDecimal(element.Element( ns + "ageofdgpsdata").Value);
            if( element.Element( ns + "dgpsid" ) != null )
                wpt.dgpsid = element.Element( ns + "dgpsid" ).Value;
            //Ignore the extensions

            return wpt;
        }

        private static fixType ReadFix( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            fixType fix = (fixType)Enum.Parse(typeof(fixType), element.Value, true);

            return fix;
        }

        private static rteTypeCollection ReadRoutes( IEnumerable<XElement> elements )
        {
            elements.RequireArgument<IEnumerable<XElement>>( "elements" ).NotNull<IEnumerable<XElement>>();

            rteTypeCollection rtes = new rteTypeCollection();

            foreach( XElement element in elements )
            {
                rtes.Add(ReadRoute(element));
            }

            return rtes;
        }

        private static rteType ReadRoute( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            rteType rte = new rteType();
            if( element.Element( ns + "name" ) != null )
                rte.name = element.Element( ns + "name" ).Value;
            if( element.Element( ns + "cmt" ) != null )
                rte.cmt = element.Element( ns + "cmt" ).Value;
            if( element.Element( ns + "desc" ) != null )
                rte.desc = element.Element( ns + "desc" ).Value;
            if( element.Element( ns + "src" ) != null )
                rte.src = element.Element( ns + "src" ).Value;
            if( element.Element( ns + "link" ) != null )
                rte.link = ReadLinks(element.Elements(ns + "link" ) );
            if( element.Element( ns + "number" ) != null )
            rte.number = element.Element( ns + "number" ).Value;
            if( element.Element( ns + "type" ) != null )
                rte.type = element.Element( ns + "type" ).Value;
            //Ignore the extensions
            if( element.Element( ns + "rtept" ) != null )
                rte.rtept = ReadWayPoints(element.Elements( ns + "rtept" ) );

            return rte;
        }

        private static trkTypeCollection ReadTracks( IEnumerable<XElement> elements )
        {
            elements.RequireArgument<IEnumerable<XElement>>( "elements" ).NotNull<IEnumerable<XElement>>();

            trkTypeCollection trks = new trkTypeCollection();

            foreach( XElement element in elements )
            {
                trks.Add(ReadTrack(element));
            }

            return trks;
        }

        private static trkType ReadTrack( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            trkType trk = new trkType();
            if( element.Element( ns + "name" ) != null )
                trk.name = element.Element( ns + "name" ).Value;
            if( element.Element( ns + "cmt" ) != null )
                trk.cmt = element.Element( ns + "cmt" ).Value;
            if( element.Element( ns + "desc" ) != null )
                trk.desc = element.Element( ns + "desc" ).Value;
            if( element.Element( ns + "src" ) != null )
                trk.src = element.Element( ns + "src" ).Value;
            if( element.Element( ns + "link" ) != null )
                trk.link = ReadLinks(element.Elements(ns + "link" ) );
            if( element.Element( ns + "number" ) != null )
                trk.number = element.Element( ns + "number" ).Value;
            //Ignore the extensions
            if( element.Element( ns + "trkseg" ) != null )
                trk.trkseg = ReadTrackSegments(element.Elements( ns + "trkseg" ) );

            return trk;
        }

        private static trksegTypeCollection ReadTrackSegments( IEnumerable<XElement> elements )
        {
            elements.RequireArgument<IEnumerable<XElement>>( "elements" ).NotNull<IEnumerable<XElement>>();

            trksegTypeCollection trksegs = new trksegTypeCollection();

            foreach( XElement element in elements )
            {
                trksegs.Add(ReadTrackSegment(element));
            }

            return trksegs;
        }

        private static trksegType ReadTrackSegment( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            trksegType trkseg = new trksegType();
            if( element.Element( ns + "trkpt" ) != null )
                trkseg.trkpt = ReadWayPoints(element.Elements( ns + "trkpt" ) );
            //Ignore the extensions

            return trkseg;
        }

        private static metadataType ReadMetadata( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            metadataType metadata = new metadataType();
            if(element.Element( ns + "name" ) != null)
                metadata.name = element.Element( ns + "name" ).Value;
            if( element.Element( ns + "desc" ) != null )
                metadata.desc = element.Element( ns + "desc" ).Value;
            if( element.Element( ns + "author" ) != null )
                metadata.author = ReadPerson( element.Element( ns + "author" ) );
            if( element.Element( ns + "copyright" ) != null )
                metadata.copyright = ReadCopyright(element.Element( ns + "copyright" ) );
            if( element.Element( ns + "link" ) != null )
                metadata.link = ReadLinks( element.Elements( ns + "link" ) );
            if( element.Element( ns + "time" ) != null )
                metadata.time = Convert.ToDateTime( element.Element( ns + "time" ).Value );
            if( element.Element( ns + "keywords" ) != null )
                metadata.keywords = element.Element( ns + "keywords" ).Value;
            if( element.Element( ns + "bounds" ) != null )
                metadata.bounds = ReadBounds( element.Element( ns + "bounds" ) );
            //The extensions cannot be read

            return metadata;
        }

        private static boundsType ReadBounds( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            boundsType bounds = new boundsType();
            bounds.maxlat = Convert.ToDecimal(element.Attribute("maxlat").Value);
            bounds.maxlon = Convert.ToDecimal( element.Attribute( "maxlon" ).Value );
            bounds.minlat = Convert.ToDecimal( element.Attribute( "minlat" ).Value );
            bounds.minlon = Convert.ToDecimal( element.Attribute( "minlon" ).Value );

            return bounds;
        }

        private static linkTypeCollection ReadLinks( IEnumerable<XElement> elements )
        {
            elements.RequireArgument<IEnumerable<XElement>>("elements").NotNull<IEnumerable<XElement>>();

            linkTypeCollection links = new linkTypeCollection();
            foreach( XElement element in elements )
            {
                links.Add(ReadLink(element));
            }

            return links;
        }

        private static linkType ReadLink( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            linkType link = new linkType();
            if( element.Element( ns + "text" ) != null )
                link.text = element.Element( ns + "text" ).Value;
            if( element.Element( ns + "type" ) != null )
                link.type = element.Element( ns + "type" ).Value;
            if( element.Element( ns + "href" ) != null )
                link.href = element.Attribute( ns + "href" ).Value;

            return link;
        }

        private static copyrightType ReadCopyright( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            copyrightType copyright = new copyrightType();
            if( element.Element( ns + "year" ) != null )
                copyright.year = element.Element( ns + "year" ).Value;
            if( element.Element( ns + "license" ) != null )
                copyright.license = element.Element( ns + "license" ).Value;
            if( element.Element( ns + "author" ) != null )
                copyright.author = element.Attribute( ns + "author" ).Value;

            return copyright;
        }

        private static personType ReadPerson( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            XNamespace ns = element.Name.Namespace;
            personType person = new personType();
            if( element.Element( ns + "name" ) != null )
                person.name = element.Element( ns + "name" ).Value;
            if( element.Element( ns + "link" ) != null )
                person.link = ReadLink(element.Element( ns + "link" ) );
            if( element.Element( ns + "email" ) != null )
                person.email = ReadEmail(element.Element( ns + "email" ) );

            return person;
        }

        private static emailType ReadEmail( XElement element )
        {
            element.RequireArgument<XElement>( "element" ).NotNull<XElement>();

            emailType email = new emailType();
            email.id = element.Attribute( "id" ).Value;
            email.domain = element.Attribute( "domain" ).Value;

            return email;
        }
    }
}
