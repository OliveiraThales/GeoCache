using System;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.ObjectModel;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class WGS84Utils
    {
        public static double EARTH_RADIUS_KMS = 6371;

        public static double GetHaversineDistance( double lat1, double long1, double lat2, double long2 )
        {
            double dLat = Utils.ConvertDegreesToRadians( lat2 - lat1 );
            double dLong = Utils.ConvertDegreesToRadians( long2 - long1 );

            double a = Math.Sin( dLat / 2 ) * Math.Sin( dLat / 2 ) +
                        Math.Cos( Utils.ConvertDegreesToRadians( lat1 ) ) * Math.Cos( Utils.ConvertDegreesToRadians( lat2 ) ) *
                        Math.Sin( dLong / 2 ) * Math.Sin( dLong / 2 );

            double c = 2 * Math.Atan2( Math.Sqrt( a ), Math.Sqrt( 1 - a ) );

            return c * EARTH_RADIUS_KMS;
        }

        public static double GetInitialBearing( double lat1, double long1, double lat2, double long2 )
        {
            double lat1Rads = Utils.ConvertDegreesToRadians( lat1 );
            double lat2Rads = Utils.ConvertDegreesToRadians( lat2 );
            double dLong = Utils.ConvertDegreesToRadians( long2 - long1 );

            double y = Math.Sin( dLong ) * Math.Cos( lat2Rads );
            double x = Math.Cos( lat1Rads ) * Math.Sin( lat2Rads ) - Math.Sin( lat1Rads ) * Math.Cos( lat2Rads ) * Math.Cos( dLong );

            var bearing = Math.Atan2( y, x );//value retuned it between -180 to 180 which needs to converted to 0 to 360 before being returned
            var bearingInDeg = Utils.ConvertRadiansToDegrees( bearing );
            return ( bearingInDeg + 360 ) % 360;
        }

        public static double GetFinalBearing( double lat1, double long1, double lat2, double long2 )
        {
            double initialBearing = GetInitialBearing( lat2, long2, lat1, long1 );
            return ( initialBearing + 180 ) % 360;
        }

        public static void GetMidPoint( double lat1, double long1, double lat2, double long2, out double lat3, out double long3 )
        {
            double dLat = Utils.ConvertDegreesToRadians( lat2 - lat1 );
            double dLong = Utils.ConvertDegreesToRadians( long2 - long1 );

            lat1 = Utils.ConvertDegreesToRadians( lat1 );
            long1 = Utils.ConvertDegreesToRadians( long1 );
            lat2 = Utils.ConvertDegreesToRadians( lat2 );
            long2 = Utils.ConvertDegreesToRadians( long2 );

            var Bx = Math.Cos( lat2 ) * Math.Cos( dLong );
            var By = Math.Cos( lat2 ) * Math.Sin( dLong );

            lat3 = Math.Atan2( Math.Sin( lat1 ) + Math.Sin( lat2 ),
                                  Math.Sqrt( ( Math.Cos( lat1 ) + Bx ) * ( Math.Cos( lat1 ) + Bx ) + By * By ) );
            long3 = Utils.ConvertDegreesToRadians( long1 ) + Math.Atan2( By, Math.Cos( lat1 ) + Bx );

            lat3 = Utils.ConvertRadiansToDegrees( lat3 );
            long3 = Utils.ConvertRadiansToDegrees( long3 );
        }

        public static void GetEndPoint( double lat1, double long1, double distance, double degrees, out double lat2, out double long2 )
        {
            double lat1Rads = Utils.ConvertDegreesToRadians( lat1 );
            double long1Rads = Utils.ConvertDegreesToRadians( long1 );

            lat2 = Math.Asin( Math.Sin( lat1Rads ) * Math.Cos( distance / EARTH_RADIUS_KMS ) +
                        Math.Cos( lat1Rads ) * Math.Sin( distance / EARTH_RADIUS_KMS ) * Math.Cos( Utils.ConvertDegreesToRadians( degrees ) ) );

            long2 = long1Rads + Math.Atan2( Math.Sin( Utils.ConvertDegreesToRadians( degrees ) ) * Math.Sin( distance / EARTH_RADIUS_KMS ) * Math.Cos( lat1Rads ),
                        Math.Cos( distance / EARTH_RADIUS_KMS ) - Math.Sin( lat1Rads ) * Math.Sin( lat2 ) );
            long2 = ( long2 + Math.PI ) % ( 2 * Math.PI ) - Math.PI;//Normalize to -180 to 180

            lat2 = Utils.ConvertRadiansToDegrees( lat2 );
            long2 = Utils.ConvertRadiansToDegrees( long2 );
        }

        public static Polygon Buffer( MapPoint point, double bufferDistance )
        {
            return Buffer( point, bufferDistance, 360 );
        }

        public static Polygon Buffer( MapPoint point, double bufferDistance, int numOfVertices )
        {
            PointCollection points = new PointCollection();
            double increment = 360 / numOfVertices;

            for( int i = 0; i < numOfVertices; i++ )
            {
                double degree = i * increment;
                double lat;
                double lon;
                GetEndPoint(point.Y, point.X, bufferDistance, degree, out lat, out lon);
                points.Add( new MapPoint( lon, lat )
                {
                    SpatialReference = point.SpatialReference
                } );
            }

            if( points.Count > 1 )
            {
                if( points[ 0 ].X != points[ points.Count - 1 ].X
                    && points[ 0 ].Y != points[ points.Count - 1 ].Y )
                {
                    points.Add( new MapPoint()
                    {
                        X = points[0].X,
                        Y = points[0].Y,
                        SpatialReference = points[0].SpatialReference
                    } );
                }
            }

            ObservableCollection<PointCollection> rings = new ObservableCollection<PointCollection>();
            rings.Add( points );
            return new Polygon()
            {
                Rings = rings,
                SpatialReference = point.SpatialReference
            };
        }

        
    }
}