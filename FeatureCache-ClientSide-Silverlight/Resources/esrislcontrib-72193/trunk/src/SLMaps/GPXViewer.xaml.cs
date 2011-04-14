using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ESRI.ArcGIS.Client;
using System.Diagnostics;
using Vishcious.ArcGIS.SLContrib;

namespace SLMaps
{
    public partial class GPXViewer : UserControl
    {
        public GPXViewer()
        {
            InitializeComponent();
        }

        private void btnLoadGPX_Click( object sender, RoutedEventArgs e )
        {
            (MyMap.Layers["gpxLayer"] as GPXLayer).URL = new Uri( txtGPXurl.Text );
        }

        private void MyMap_Loaded( object sender, RoutedEventArgs e )
        {
            GraphicsLayer graphicsLayer = MyMap.Layers[ "gpxLayer" ] as GraphicsLayer;
            //MyMapTip.GraphicsLayer = graphicsLayer;

            graphicsLayer.MouseLeftButtonUp += new GraphicsLayer.MouseButtonEventHandler( graphicsLayer_MouseLeftButtonUp );
            graphicsLayer.MouseLeftButtonDown += new GraphicsLayer.MouseButtonEventHandler( graphicsLayer_MouseLeftButtonDown );
        }

        void graphicsLayer_MouseLeftButtonDown( object sender, GraphicMouseButtonEventArgs e )
        {
            double X = e.GetPosition( MyMap ).X;
            double Y = e.GetPosition( MyMap ).Y;
            Debug.WriteLine( string.Format( "X: {0}, Y: {1}", X, Y ) );
        }

        void graphicsLayer_MouseLeftButtonUp( object sender, GraphicMouseButtonEventArgs e )
        {
            double X = e.GetPosition( MyMap ).X;
            double Y = e.GetPosition( MyMap ).Y;
            Debug.WriteLine( string.Format( "X: {0}, Y: {1}", X, Y ) );
        }

    }
}
