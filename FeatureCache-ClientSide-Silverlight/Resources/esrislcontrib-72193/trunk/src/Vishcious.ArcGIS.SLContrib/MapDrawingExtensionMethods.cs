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
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;
using System.Linq;
using ESRI.ArcGIS.Client.Geometry;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Concurrency;
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class MapDrawingExtensionMethods
    {
        static ResourceDictionary resources = new ResourceDictionary();
        static ControlTemplate drawingPointTemplate;
        static ControlTemplate drawingLineTemplate;
        static ControlTemplate drawingPolygonTemplate;

        static MapDrawingExtensionMethods()
        {
            var sri = Application.GetResourceStream(new Uri("Vishcious.ArcGIS.SLContrib;component/resources/simplesymbols.xaml", UriKind.Relative));
            StreamReader sr = new StreamReader(sri.Stream);
            resources = XamlReader.Load(sr.ReadToEnd()) as ResourceDictionary;
            drawingPointTemplate = resources["SimpleMarkerSymbol_Circle"] as ControlTemplate;
            drawingLineTemplate = resources["LineSymbol"] as ControlTemplate;
            drawingPolygonTemplate = resources["FillSymbol"] as ControlTemplate;
        }

        public static IObservable<MapPoint> DrawPoint(this Map map)
        {
            return from item in map.GetClick()
                   select map.ScreenToMap(item.EventArgs.GetPosition(map));
        }

        public static IObservable<ESRI.ArcGIS.Client.Geometry.PointCollection> DrawPoints(this Map map)
        {
            ESRI.ArcGIS.Client.Geometry.PointCollection pc = new ESRI.ArcGIS.Client.Geometry.PointCollection();
            Subject<ESRI.ArcGIS.Client.Geometry.PointCollection> obvle = new Subject<ESRI.ArcGIS.Client.Geometry.PointCollection>();

            map.GetClick()
                .Select(item => map.ScreenToMap(item.EventArgs.GetPosition(map)))
                .Subscribe(next => pc.Add(next));

            map.GetDoubleClick()
                .Subscribe(next =>
                {
                    obvle.OnNext(pc);
                    pc = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                });
            
            return obvle;
        }

        public static KeyValuePair<IObservable<MapPoint>, IObservable<ESRI.ArcGIS.Client.Geometry.PointCollection>> DrawPointsDynamic(this Map map)
        {
            var click = map.GetClick()
                .TakeUntil(map.GetDoubleClick())
                .Select(next => map.ScreenToMap(next.EventArgs.GetPosition(map)));
            var singleClicks = click.Publish();

            var completion = singleClicks.Aggregate(
                new ESRI.ArcGIS.Client.Geometry.PointCollection(),
                (acc, point) =>
                {
                    acc.Add(point);
                    return acc;
                });
            singleClicks.Connect();

            return new KeyValuePair<IObservable<MapPoint>, IObservable<ESRI.ArcGIS.Client.Geometry.PointCollection>>
            (singleClicks, completion);
        }

        public static IObservable<ESRI.ArcGIS.Client.Geometry.PointCollection> DrawPointsDynamic2(this Map map)
        {
            var pcs = map.GetDoubleClick().Select(_ => new ESRI.ArcGIS.Client.Geometry.PointCollection());
            var ps = map.GetClick().Select(next => map.ScreenToMap(next.EventArgs.GetPosition(map)));
            var ppcs = pcs.SelectMany(pc => ps.Select(p => { pc.Add(p); return pc; }).TakeUntil(pcs));

            var obs = pcs.Merge(ppcs);

            return obs;
        }

        //public static KeyValuePair<IObservable<MapPoint>, IObservable<ESRI.ArcGIS.Client.Geometry.PointCollection>> DrawPointsDynamic(this Map map)
        //{
        //    var pc = new ESRI.ArcGIS.Client.Geometry.PointCollection();
        //    var ptSubject = map.GetClick().Select(next => map.ScreenToMap(next.EventArgs.GetPosition(map))).Publish();
        //    var ptsSubject = map.GetDoubleClick().Publish();

        //    Subject<MapPoint> sequenceSubject = new Subject<MapPoint>(Scheduler.Dispatcher);
        //    Subject<ESRI.ArcGIS.Client.Geometry.PointCollection> completedSubject = new Subject<ESRI.ArcGIS.Client.Geometry.PointCollection>(Scheduler.Dispatcher);

        //    ptSubject.Subscribe(next =>
        //    {
        //        pc.Add(next);
        //        sequenceSubject.OnNext(next);
        //    });
        //    ptsSubject.Subscribe(next =>
        //    {
        //        completedSubject.OnNext(pc);
        //        pc = new ESRI.ArcGIS.Client.Geometry.PointCollection();
        //    });

        //    ptSubject.Connect();
        //    ptsSubject.Connect();

        //    return new KeyValuePair<IObservable<MapPoint>, IObservable<ESRI.ArcGIS.Client.Geometry.PointCollection>>
        //    (sequenceSubject.AsObservable(), completedSubject.AsObservable());
        //}

        public static IObservable<MultiPoint> DrawMultiPoint(this Map map)
        {
            return from pts in map.DrawPoints()
                   select new MultiPoint( pts );
        }

        public static IObservable<MultiPoint> DrawMultiPointDynamic(this Map map, GraphicsLayer gLayer)
        {
            var obs = map.DrawPointsDynamic();
            MultiPoint mps = new MultiPoint();
            Graphic g = new Graphic()
            {
                Symbol = new SimpleMarkerSymbol() { ControlTemplate = drawingPointTemplate },
                Geometry = mps
            };
            gLayer.Graphics.Add(g);
            gLayer.Refresh();

            var click = obs.Key.Publish();
            var dblClick = obs.Value.Publish();

            click.Subscribe(next =>
                {
                    mps.Points.Add(next);
                    gLayer.Refresh();
                });
            dblClick.Subscribe(next =>
                {
                    RemoveEndingDuplicatePoint(mps.Points);
                    //mps.Points.Clear();
                    gLayer.Graphics.Remove(g);
                    gLayer.Refresh();
                    click.Connect();
                    dblClick.Connect();
                });

            click.Connect();
            dblClick.Connect();

            return from pts in dblClick
                   select mps;
        }

        public static void RemoveEndingDuplicatePoint(ESRI.ArcGIS.Client.Geometry.PointCollection pts)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");
            if (pts.Count < 2)
                return;
            var lastPoint = pts[pts.Count - 1];
            var lastButOnePoint = pts[pts.Count - 2];
            if (lastPoint.X == lastButOnePoint.X && lastPoint.Y == lastButOnePoint.Y)
                pts.RemoveAt(pts.Count - 1);
        }

        public static IObservable<ESRI.ArcGIS.Client.Geometry.Polyline> DrawPolyline(this Map map)
        {
            return from pts in map.DrawPoints()
                   let paths = new ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>(new ESRI.ArcGIS.Client.Geometry.PointCollection[1]{pts})
                   let line = new ESRI.ArcGIS.Client.Geometry.Polyline() { Paths = paths }
                   select line;
        }

        public static IObservable<ESRI.ArcGIS.Client.Geometry.Polygon> DrawPolygon(this Map map)
        {
            return from pts in map.DrawPoints()
                   let closed = pts.Close()
                   let paths = new ObservableCollection<ESRI.ArcGIS.Client.Geometry.PointCollection>
                       (new ESRI.ArcGIS.Client.Geometry.PointCollection[1] { closed })
                   let line = new ESRI.ArcGIS.Client.Geometry.Polygon() { Rings = paths }
                   select line;
        }

        public static ESRI.ArcGIS.Client.Geometry.PointCollection Close(this ESRI.ArcGIS.Client.Geometry.PointCollection pts)
        {
            if(pts.Count == 0)
                return pts;
            pts.Add(pts[0]);
            return pts;
        }
    }
}
