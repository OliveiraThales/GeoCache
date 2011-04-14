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
using System.Windows.Media.Imaging;
using ESRI.ArcGIS.Client.Geometry;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using ESRI.ArcGIS.Client;

namespace Vishcious.ArcGIS.SLContrib
{
    public class PixelLayer:DynamicLayer
    {

        int _currentWidth = 0;
        int _currentHeight = 0;
        PixelPngRenderer _pngRenderer;
        BitmapImage _image;
        Random _random = new Random();

        public Color PixelColor
        {
            get;
            set;
        }
        public List<MapPoint> MapPoints;

        public override void Initialize()
        {
            base.Initialize();

            PixelColor = Colors.Red;
        }

        protected override void GetSource( Envelope extent, int width, int height, DynamicLayer.OnImageComplete onComplete )
        {
            _currentWidth = width;
            _currentHeight = height;

            MapPoints = new List<MapPoint>();

            //Make up some dummy data
            int count = 0;
            while( count < 1000 )
            {
                MapPoints.Add( new MapPoint( extent.XMin + ( extent.Width * ( _random.Next( 100 ) / 100.00 ) ),
                    extent.YMin + ( extent.Height * ( _random.Next( 100 ) / 100.00 ) ),
                    extent.SpatialReference ) );
                count++;
            }
            //Make up some dummy data

            _pngRenderer = new PixelPngRenderer( _currentWidth, _currentHeight, MapPoints );

            ParameterizedThreadStart starter = new ParameterizedThreadStart( ( pngRenderer ) =>
            {
                Stream imageStream = InitalizeImage( pngRenderer as PixelPngRenderer );

                Dispatcher.BeginInvoke( () =>
                {
                    _image = new BitmapImage();
                    _image.SetSource( imageStream );
                    onComplete( _image, width, height, extent );
                } );
            } );
            new Thread( starter ).Start( _pngRenderer );
        }

        private Stream InitalizeImage( PixelPngRenderer pngRenderer )
        {
            pngRenderer.Display();
            return pngRenderer.GetImageStream();
        }
    }
}
