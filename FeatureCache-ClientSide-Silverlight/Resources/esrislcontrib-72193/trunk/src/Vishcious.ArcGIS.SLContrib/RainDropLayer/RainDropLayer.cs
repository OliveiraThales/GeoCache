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
using ESRI.ArcGIS.Client.Geometry;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using ESRI.ArcGIS.Client;
using Vishcious.ArcGIS.SLContrib;

namespace Vishcious.ArcGIS.SLContrib
{
    public class RainDropLayer : DynamicLayer
    {
        int _currentWidth = 0;
        int _currentHeight = 0;
        RainDropPngRenderer _pngRenderer;
        BitmapImage _image;
        Random _random = new Random();
        Timer _timer;

        public Color Background
        {
            get;
            set;
        }

        public double RippleIntervalMilliSeconds
        {
            get;
            set;
        }


        public override void Initialize()
        {
            Background = Color.FromArgb( 255, 255, 0, 0 );
            RippleIntervalMilliSeconds = 60;

            base.Initialize();
        }

        void timer_Tick( object state )
        {
            RainDropPngRenderer pngRenderer = state as RainDropPngRenderer;
            pngRenderer.Splash( _random.Next( _currentWidth ), _random.Next( _currentHeight ), _random.Next( 4 ) );
            pngRenderer.Step();
            pngRenderer.Display( Background );
            Stream imageStream = pngRenderer.GetImageStream();

            Dispatcher.BeginInvoke( () =>
            {
                _image.SetSource( imageStream );
            } );
        }

        protected override void GetSource( Envelope extent, int width, int height, DynamicLayer.OnImageComplete onComplete )
        {
            //List<MapPoint> mapPoints = new List<MapPoint>();
            //List<Point> points = new List<Point>();

            //int count = 0;
            //while( count < 1000 )
            //{
            //    mapPoints.Add(new MapPoint(extent.XMin + (extent.Width * (_random.Next(100)/100.00)), 
            //        extent.YMin + (extent.Height * (_random.Next(100)/100.00)), 
            //        extent.SpatialReference));
            //    count++;
            //}

            //foreach( MapPoint entry in mapPoints )
            //{
            //    double M11 = ( double ) width / 360.00;
            //    double M22 = -( double ) height / 180.00;
            //    double M12 = 0.00;
            //    double M21 = 0.00;
            //    double offsetX = 180.00 * ( ( double ) width / 360.00 );
            //    double offsetY = 90.00 * ( ( double ) height / 180.0 );

            //    Matrix transform = new Matrix( M11, M12, M21, M22, offsetX, offsetY );
            //    points.Add( transform.Transform( new Point( entry.X, entry.Y ) ) );
            //}

            if( _timer != null )
                _timer.Dispose();

            _currentWidth = width;
            _currentHeight = height;

            _pngRenderer = new RainDropPngRenderer( _currentWidth, _currentHeight );

            ParameterizedThreadStart starter = new ParameterizedThreadStart( ( pngRenderer ) =>
            {
                Stream imageStream = InitalizeImage( pngRenderer as RainDropPngRenderer );

                Dispatcher.BeginInvoke( () =>
                {
                    _image = new BitmapImage();
                    _image.SetSource( imageStream );
                    onComplete( _image, width, height, extent );
                    _timer = new Timer( timer_Tick, _pngRenderer, new TimeSpan( 0, 0, 0 ), TimeSpan.FromMilliseconds( RippleIntervalMilliSeconds ) );
                } );
            } );
            new Thread( starter ).Start( _pngRenderer );
        }

        private Stream InitalizeImage( RainDropPngRenderer pngRenderer )
        {
            pngRenderer.Display( Background );
            return pngRenderer.GetImageStream();
        }
    }
}
