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
using Vishcious.ArcGIS.SLContrib;
using System.Json;
using System.Windows.Browser;
using ESRI.ArcGIS.Client.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace SLMaps
{
    public partial class TypeFactoryDemo : UserControl
    {
        Type LayerType;
        LayerMetadata LayerMetadata;

        public TypeFactoryDemo()
        {
            InitializeComponent();
        }

        private void btnGetMetaData_Click( object sender, RoutedEventArgs e )
        {
            Uri lyr = new Uri( "http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer/5?f=json" );
            Utils.DoGET( lyr, HandleLayerMetaData, HandleLayerMetaDataError);
        }

        private void HandleLayerMetaData( string metadata )
        {
            JsonObject lyrMetadataJSON = JsonObject.Parse( metadata ) as JsonObject;
            LayerMetadata = ArcJSONReader.ReadLayerMetadata( lyrMetadataJSON );
            LayerType = LayerTypeProvider.CreateLayerRecordType( LayerMetadata );

            QueryTask queryTask = new QueryTask( "http://sampleserver1.arcgisonline.com/ArcGIS/rest/services/Demographics/ESRI_Census_USA/MapServer/5" );
            queryTask.ExecuteCompleted += QueryTask_ExecuteCompleted;
            queryTask.Failed += QueryTask_Failed;

            Query query = new Query();
            query.OutFields.Add( "*" );
            query.Where = "1=1";
            queryTask.ExecuteAsync( query );
        }

        private void QueryTask_ExecuteCompleted( object sender, QueryEventArgs args )
        {
            ObservableCollection<object> records = LayerTypeProvider.CreateLayerRecords( LayerType, args.FeatureSet.Features );

            layerDataGrid.Columns.Clear();
            int count = 0;
            foreach( FieldMetadata entry in LayerMetadata.Fields )
            {
                if( entry.Type != "esriFieldTypeGeometry" )
                {
                    layerDataGrid.Columns.Add( new DataGridTextColumn
                    {
                        Header = entry.Alias,
                        Binding = new Binding( entry.Name )
                    } );
                    count++;
                    if( count >= 10 )
                        break;
                }
            }

            layerDataGrid.ItemsSource = records;
        }

        private void QueryTask_Failed( object sender, TaskFailedEventArgs args )
        {
            HtmlPage.Window.Alert( args.Error.Message );
        }

        private void HandleLayerMetaDataError( Exception exception )
        {
            HtmlPage.Window.Alert( exception.Message );
        }
    }
}
