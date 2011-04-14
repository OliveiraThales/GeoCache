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
using System.IO;
using Vishcious.ArcGIS.SLContrib;
using ESRI.ArcGIS;
using System.Windows.Browser;
using ESRI.ArcGIS.Client.Symbols;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Tasks;

namespace SLMaps
{
    public partial class Page : UserControl
    {

        public Page()
        {
            InitializeComponent();
        }

        private void MyMap_Loaded( object sender, RoutedEventArgs e )
        {

        }

    }
}
