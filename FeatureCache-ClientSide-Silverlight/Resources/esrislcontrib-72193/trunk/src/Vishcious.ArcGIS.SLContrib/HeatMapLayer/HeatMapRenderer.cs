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
using ESRI.ArcGIS.Client.Geometry;
using System.Linq;

namespace Vishcious.ArcGIS.SLContrib
{
    public class HeatMapRenderer
    {
        public int Width
        {
            get;
            set;
        }
        public int Height
        {
            get;
            set;
        }
        public Color PixelColor
        {
            get;
            set;
        }

        double M11;
        double M22;
        double M12;
        double M21;
        double offsetX;
        double offsetY;
        Matrix transform;

        public HeatMapRenderer( int width, int height, Color pixelColor )
        {
            Width = width;
            Height = height;
            PixelColor = pixelColor;

            M11 = ( double ) Width / 360.00;
            M22 = -( double ) Height / 180.00;
            M12 = 0.00;
            M21 = 0.00;
            offsetX = 180.00 * ( ( double ) Width / 360.00 );
            offsetY = 90.00 * ( ( double ) Height / 180.0 );
            transform = new Matrix( M11, M12, M21, M22, offsetX, offsetY );
        }

        public HeatMapRenderer( int width, int height )
            : this( width, height, Colors.Red )
        {
        }

        public void Display( PngEncoder surface, IEnumerable<MapPoint> mapPoints )
        {
            if( surface == null )
                throw new ArgumentNullException( "surface" );
            if( mapPoints == null )
                throw new ArgumentNullException( "mapPoints" );

            //List<Point> points = new List<Point>();
            //foreach( MapPoint entry in mapPoints )
            //{
            //    points.Add( transform.Transform( new Point( entry.X, entry.Y ) ) );
            //}

            byte[,] transparencies = new byte[Width, Height];
            foreach( MapPoint point in mapPoints )
            {
                Point p = transform.Transform(new Point(point.X, point.Y));
                transparencies[Math.Max(Math.Min((int)p.X, Width-1), 0), Math.Max(Math.Min((int)p.Y, Height-1), 0)] = (Byte)255;
            }

            for( int y = 1; y < Height - 1; y++ )
            {
                int rowStart = surface.GetRowStart( y );
                for( int x = 1; x < Width - 1; x++ )
                {
                    //surface.SetPixelAtRowStart( x, rowStart, PixelColor.R, PixelColor.G, PixelColor.B, PixelColor.A );
                    surface.SetPixelAtRowStart( x, rowStart, PixelColor.R, PixelColor.G, PixelColor.B, transparencies[x, y] );
                }
            }
        }
    }
}
