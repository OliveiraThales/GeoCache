﻿using System;
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
using Vishcious.ArcGIS.SLContrib;
using System.Windows.Browser;

namespace SLMaps
{
    public partial class GeoJSONViewer : UserControl
    {
        public GeoJSONViewer()
        {
            InitializeComponent();
        }

        private void MyMap_Loaded( object sender, RoutedEventArgs e )
        {
            GraphicsLayer graphicsLayer = MyMap.Layers[ "geoJSONLayer" ] as GraphicsLayer;
            MyMapTip.GraphicsLayer = graphicsLayer;
        }

        private void btnLoadGeoJSON_Click( object sender, RoutedEventArgs e )
        {
            GeoJSONLayer Layer = MyMap.Layers[ "geoJSONLayer" ] as GeoJSONLayer;
            if( Uri.IsWellFormedUriString( txtGeoJSONurl.Text, UriKind.Absolute ) )
            {
                Layer.URL = new Uri( txtGeoJSONurl.Text );
            }
            else
            {
                HtmlPage.Window.Alert( "Please enter a valid URL." + System.Environment.NewLine + string.Format( "The URL '{0}' is not valid.", txtGeoJSONurl.Text ) );
            }
        }
    }
}
