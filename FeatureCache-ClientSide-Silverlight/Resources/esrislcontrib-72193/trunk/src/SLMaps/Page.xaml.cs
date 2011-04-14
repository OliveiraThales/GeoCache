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
using ESRI.ArcGIS.Client;
using System.Concurrency;
using System.Windows.Markup;

namespace SLMaps
{
    public partial class Page : UserControl
    {
        GraphicsLayer gLayer = new GraphicsLayer();

        public Page()
        {
            InitializeComponent();
        }

        private void MyMap_Loaded( object sender, RoutedEventArgs e )
        {
            var sfs = new SimpleFillSymbol()
                {
                    BorderBrush = new SolidColorBrush(Colors.Blue),
                    BorderThickness = 5,
                    Fill = new SolidColorBrush(Colors.Brown)
                };
            var sls = new SimpleLineSymbol()
                {
                    Style = SimpleLineSymbol.LineStyle.Solid,
                    Width = 10,
                    Color = new SolidColorBrush(Colors.Green)
                };

            MyMap.Layers.Add(gLayer);

            Graphic gArea = new Graphic()
            {
                Geometry = new ESRI.ArcGIS.Client.Geometry.Polygon()
                {
                    Rings = new System.Collections.ObjectModel.ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>() 
                    {
                        new ESRI.ArcGIS.Client.Geometry.PointCollection()
                        {
                            new MapPoint(-20, -20, MyMap.SpatialReference),
                            new MapPoint(-20, 20, MyMap.SpatialReference),
                            new MapPoint(20, 20, MyMap.SpatialReference),
                            new MapPoint(20, -20, MyMap.SpatialReference),
                            new MapPoint(-20, -20, MyMap.SpatialReference)
                        }
                    }
                },
                Symbol = sfs
            };
            gLayer.Graphics.Add(gArea);
            //gArea.MakeDraggable(MyMap);

            Graphic gLine = new Graphic()
            {
                Geometry = new ESRI.ArcGIS.Client.Geometry.Polyline()
                {
                    Paths = new System.Collections.ObjectModel.ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>() 
                    {
                        new ESRI.ArcGIS.Client.Geometry.PointCollection()
                        {
                            new MapPoint(-10, -10, MyMap.SpatialReference),
                            new MapPoint(10, 10, MyMap.SpatialReference),
                            new MapPoint(5, 5, MyMap.SpatialReference)
                        }
                    }
                },
                Symbol = sls
            };
            gLayer.Graphics.Add(gLine);
            //gLine.MakeDraggable(MyMap);

            Graphic gLine1 = new Graphic()
            {
                Geometry = new ESRI.ArcGIS.Client.Geometry.Polyline()
                {
                    Paths = new System.Collections.ObjectModel.ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>() 
                    {
                        new ESRI.ArcGIS.Client.Geometry.PointCollection()
                        {
                            new MapPoint(-10, 10, MyMap.SpatialReference),
                            new MapPoint(10, -10, MyMap.SpatialReference)
                        }
                    }
                },
                Symbol = sls
            };
            gLayer.Graphics.Add(gLine1);
            //gLine.MakeDraggable(MyMap);

            Graphic gPoint = new Graphic()
            {
                Geometry = new MapPoint(0, 0, MyMap.SpatialReference),
                Symbol = new SimpleMarkerSymbol()
                {
                    Style = SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
                    Size = 20,
                    Color = new SolidColorBrush(Colors.Red)
                }
            };
            gLayer.Graphics.Add(gPoint);
            //gPoint.MakeDraggable(MyMap);

            gLayer.MakeDraggable(MyMap);

            //gArea.GetDoubleClick().Subscribe((arg) => HtmlPage.Window.Alert("Hey"));

            //gLayer.GetDoubleClick().Subscribe((arg) => HtmlPage.Window.Alert("Hey"));

            MyMap.GetClick().Subscribe((arg) => txtMessage.Text = "Click");

            MyMap.GetDoubleClick().Subscribe(arg => txtMessage.Text = "DoubleClick");

            var sym = new SimpleMarkerSymbol()
                            {
                                Style = SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
                                Size = 20,
                                Color = new SolidColorBrush(Colors.Green)
                            };

            MyMap.DrawMultiPointDynamic(gLayer).Repeat().Subscribe(next =>
                {
                    gLayer.Graphics.Add(new Graphic()
                        {
                            Geometry = next,
                            Symbol = sym
                        });
                });

            //MyMap.DrawPointsDynamic().Key.Subscribe(next =>
            //{
            //    gLayer.Graphics.Add(new Graphic()
            //        {
            //            Geometry = next,
            //            Symbol = sym
            //        });
            //});

            //MyMap.DrawPointsDynamic().Value.Subscribe(next =>
            //{
            //    ESRI.ArcGIS.Client.Geometry.Polygon polygon = new ESRI.ArcGIS.Client.Geometry.Polygon();
            //    polygon.Rings.Add(next);
            //    gLayer.Graphics.Add(new Graphic()
            //        {
            //            Geometry = polygon,
            //            Symbol = sfs
            //        });
            //});
        }

    }
}
