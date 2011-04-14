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
using Vishcious.ArcGIS.SLContrib;
using System.IO;
using System.Windows.Browser;

namespace SLMaps
{
    public partial class ShapefileViewer : UserControl
    {
        public ShapefileViewer()
        {
            InitializeComponent();
        }

        private void openFileDialog_Click( object sender, RoutedEventArgs e )
        {
            //Create the dialog allowing the user to select the "*.shp" and the "*.dbf" files
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;

            if( !( ofd.ShowDialog() ?? false ) )
                return;

            //Get the file info objects for the SHP and the DBF file selected by the user
            FileInfo shapeFile = null;
            FileInfo dbfFile = null;
            foreach( FileInfo fi in ofd.Files )
            {
                if( fi.Extension.ToLower() == ".shp" )
                {
                    shapeFile = fi;
                }
                if( fi.Extension.ToLower() == ".dbf" )
                {
                    dbfFile = fi;
                }
            }

            //Read the SHP and DBF files into the ShapeFileReader
            ShapeFile shapeFileReader = new ShapeFile();
            if( shapeFile != null && dbfFile != null )
            {
                shapeFileReader.Read( shapeFile, dbfFile );
            }
            else
            {
                HtmlPage.Window.Alert( "Please select a SP and a DBF file to proceed." );
                return;
            }

            //Add the shapes from the shapefile into a graphics layer named "shapefileGraphicsLayer"
            //the greaphics layer should be present in the XAML are created earliers
            GraphicsLayer graphicsLayer = MyMap.Layers[ "shapefileGraphicsLayer" ] as GraphicsLayer;
            foreach( ShapeFileRecord record in shapeFileReader.Records )
            {
                Graphic graphic = record.ToGraphic();
                if( graphic != null )
                    graphicsLayer.Graphics.Add( graphic );
            }

            MyMapTip.GraphicsLayer = graphicsLayer;
        }
    }
}
