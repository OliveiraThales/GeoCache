﻿using System;
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
using System.Collections.Generic;
using System.IO;

namespace Vishcious.ArcGIS.SLContrib
{
    public class HeatMapPngRenderer
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
        public PngEncoder encoder
        {
            get;
            set;
        }
        public HeatMapRenderer renderer
        {
            get;
            set;
        }
        public IEnumerable<MapPoint> MapPoints
        {
            get;
            set;
        }

        public HeatMapPngRenderer( int width, int height, IEnumerable<MapPoint> mapPoints )
        {
            encoder = new PngEncoder( width, height );
            renderer = new HeatMapRenderer( width, height );
            MapPoints = mapPoints;
        }

        public void Display()
        {
            renderer.Display( encoder, MapPoints );
        }

        public Stream GetImageStream()
        {
            return encoder.GetImageStream();
        }
    }
}
