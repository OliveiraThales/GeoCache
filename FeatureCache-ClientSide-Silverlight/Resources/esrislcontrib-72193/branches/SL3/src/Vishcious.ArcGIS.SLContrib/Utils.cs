using System;
using ESRI.ArcGIS.Client.Geometry;
using System.Net;
using System.IO;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class Utils
    {
        public static void DoGET( Uri address, Action<Stream> resultHandler, Action<Exception> errorHandler )
        {
            WebClient webClient = new WebClient();
            webClient.AllowReadStreamBuffering = true;
            object userState = Guid.NewGuid();
            
            webClient.OpenReadCompleted += ( sender, e ) =>
            {
                if( e.UserState != userState )
                    return;

                if( e.Error != null )
                {
                    errorHandler( e.Error );
                    return;
                }

                resultHandler( e.Result );
            };

            webClient.OpenReadAsync(address, userState);
        }

        public static void DoGET( Uri address, Action<string> resultHandler, Action<Exception> errorHandler )
        {
            WebClient webClient = new WebClient();
            webClient.AllowReadStreamBuffering = true;
            object userState = Guid.NewGuid();

            webClient.DownloadStringCompleted += ( sender, e ) =>
            {
                if( e.UserState != userState )
                    return;

                if( e.Error != null )
                {
                    errorHandler( e.Error );
                    return;
                }

                resultHandler( e.Result );
            };

            webClient.DownloadStringAsync( address, userState );
        }

        public static bool IsPointInPolygon( PointCollection points, MapPoint point )
        {
            int i;
            int j = points.Count - 1;
            bool inPoly = false;

            for( i = 0; i < points.Count; i++ )
            {
                if( points[ i ].X < point.X && points[ j ].X >= point.X
                  || points[ j ].X < point.X && points[ i ].X >= point.X )
                {
                    if( points[ i ].Y + ( point.X - points[ i ].X ) / ( points[ j ].X - points[ i ].X ) * ( points[ j ].Y - points[ i ].Y ) < point.Y )
                    {
                        inPoly = !inPoly;
                    }
                }
                j = i;
            }
            return inPoly;
        }

        public static double ConvertDegreesToRadians( double degrees )
        {
            return degrees * ( Math.PI / 180 );
        }

        public static double ConvertRadiansToDegrees( double radians )
        {
            return radians * ( 180 / Math.PI );
        }
    }
}