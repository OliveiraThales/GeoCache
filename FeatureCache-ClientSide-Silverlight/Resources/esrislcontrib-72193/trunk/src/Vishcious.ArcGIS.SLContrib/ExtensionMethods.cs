using System;
using ESRI.ArcGIS.Client;
using Vishcious.ArcGIS.SLContrib;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client.Symbols;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using System.Windows;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class ExtensionMethods
    {
        public static SimpleMarkerSymbol DEFAULT_MARKER_SYMBOL = new SimpleMarkerSymbol()
        {
            Style = SimpleMarkerSymbol.SimpleMarkerStyle.Circle,
            Color = new SolidColorBrush( Colors.Red )
        };

        public static SimpleLineSymbol DEFAULT_LINE_SYMBOL = new SimpleLineSymbol()
        {
            Color = new SolidColorBrush( Colors.Red ),
            Style = SimpleLineSymbol.LineStyle.Solid,
            Width = 2
        };

        public static SimpleFillSymbol DEFAULT_FILL_SYMBOL = new SimpleFillSymbol()
        {
            Fill = new SolidColorBrush( Colors.Red ),
            BorderBrush = new SolidColorBrush( Colors.Green ),
            BorderThickness = 2
        };

        public static Symbol GetDefaultSymbol( this ESRI.ArcGIS.Client.Geometry.Geometry geometry )
        {
            if( geometry == null )
                return null;

            Type t = geometry.GetType();
            if( t == typeof( MapPoint ) )
                return DEFAULT_MARKER_SYMBOL;
            else if( t == typeof( MultiPoint ) )
                return DEFAULT_MARKER_SYMBOL;
            else if( t == typeof( Polyline ) )
                return DEFAULT_LINE_SYMBOL;
            else if( t == typeof( Polygon ) )
                return DEFAULT_FILL_SYMBOL;
            else if( t == typeof( Envelope ) )
                return DEFAULT_FILL_SYMBOL;

            return null;
        }

        public static bool Contains<T>( this IEnumerable<T> collection, Func<T, bool> evaluator )
        {
            foreach( T local in collection )
            {
                if( evaluator( local ) )
                    return true;
            }
            return false;
        }

        public static void ForEach<T>( this IEnumerable<T> collection, Action<T> action )
        {
            foreach( T local in collection )
            {
                action( local );
            }
        }

        public static void ForEach<T>( this IEnumerable collection, Action<T> action ) where T : class
        {
            foreach( var entry in collection )
            {
                if( entry is T )
                {
                    action( entry as T );
                }
            }
        }

        public static ArgumentPropertyValue<T> RequireArgument<T>( this T item, string argName )
        {
            return new ArgumentPropertyValue<T>( argName, item );
        }

        public static ArgumentPropertyValue<T> NotNull<T>( this ArgumentPropertyValue<T> item ) where T : class
        {
            if( item.Value == null )
            {
                throw new ArgumentNullException( item.Name );
            }
            return item;
        }

        public static ArgumentPropertyValue<string> NotNullOrEmpty( this ArgumentPropertyValue<string> item )
        {
            if( string.IsNullOrEmpty( item.Value ) )
            {
                throw new ArgumentNullException( item.Name );
            }
            return item;
        }

        public static ArgumentPropertyValue<string> ShorterThan( this ArgumentPropertyValue<string> item, int limit )
        {
            if( item.Value.Length >= limit )
            {
                throw new ArgumentException( string.Format( "Parameter {0} must be shorter than {1} chars", item.Name, limit ) );
            }
            return item;
        }

        public static ArgumentPropertyValue<string> StartsWith( this ArgumentPropertyValue<string> item, string pattern )
        {
            if( !item.Value.StartsWith( pattern ) )
            {
                throw new ArgumentException( string.Format( "Parameter {0} must start with {1}", item.Name, pattern ) );
            }
            return item;
        }

        public static Graphic ToGraphic( this ShapeFileRecord record )
        {
            if( record == null )
                return null;

            Graphic graphic = new Graphic();
            //add all the attributes to the graphic
            foreach( var item in record.Attributes )
            {
                graphic.Attributes.Add( item.Key, item.Value );
            }

            //add the geometry to the graphic
            switch( record.ShapeType )
            {
                case ( int ) ShapeType.NullShape:
                    break;
                case ( int ) ShapeType.Multipoint:
                    graphic.Geometry = GetMultiPoint( record );
                    graphic.Symbol = DEFAULT_MARKER_SYMBOL;
                    break;
                case ( int ) ShapeType.Point:
                    graphic.Geometry = GetPoint( record );
                    graphic.Symbol = DEFAULT_MARKER_SYMBOL;
                    break;
                case ( int ) ShapeType.Polygon:
                    graphic.Geometry = GetPolygon( record );
                    graphic.Symbol = DEFAULT_FILL_SYMBOL;
                    break;
                case ( int ) ShapeType.PolyLine:
                    graphic.Geometry = GetPolyline( record );
                    graphic.Symbol = DEFAULT_LINE_SYMBOL;
                    break;
                default:
                    break;
            }

            return graphic;
        }

        private static ESRI.ArcGIS.Client.Geometry.Geometry GetPolyline( ShapeFileRecord record )
        {
            Polyline line = new Polyline();
            for( int i = 0; i < record.NumberOfParts; i++ )
            {
                // Determine the starting index and the end index
                // into the points array that defines the figure.
                int start = record.Parts[ i ];
                int end;
                if( record.NumberOfParts > 1 && i != ( record.NumberOfParts - 1 ) )
                    end = record.Parts[ i + 1 ];
                else
                    end = record.NumberOfPoints;

                ESRI.ArcGIS.Client.Geometry.PointCollection points = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                // Add line segments to the polyline
                for( int j = start; j < end; j++ )
                {
                    System.Windows.Point point = record.Points[ j ];
                    points.Add( new MapPoint( point.X, point.Y ) );
                }

                line.Paths.Add( points );
            }

            return line;
        }

        private static ESRI.ArcGIS.Client.Geometry.Geometry GetPolygon( ShapeFileRecord record )
        {
            Polygon polygon = new Polygon();
            for( int i = 0; i < record.NumberOfParts; i++ )
            {
                // Determine the starting index and the end index
                // into the points array that defines the figure.
                int start = record.Parts[ i ];
                int end;
                if( record.NumberOfParts > 1 && i != ( record.NumberOfParts - 1 ) )
                    end = record.Parts[ i + 1 ];
                else
                    end = record.NumberOfPoints;

                ESRI.ArcGIS.Client.Geometry.PointCollection points = new ESRI.ArcGIS.Client.Geometry.PointCollection();
                // Add line segments to the polyline
                for( int j = start; j < end; j++ )
                {
                    System.Windows.Point point = record.Points[ j ];
                    points.Add( new MapPoint( point.X, point.Y ) );
                }

                polygon.Rings.Add( points );
            }

            return polygon;
        }

        private static ESRI.ArcGIS.Client.Geometry.Geometry GetPoint( ShapeFileRecord record )
        {
            MapPoint point = new MapPoint();
            point.X = record.Points[ 0 ].X;
            point.Y = record.Points[ 0 ].Y;
            return point;
        }

        private static ESRI.ArcGIS.Client.Geometry.Geometry GetMultiPoint( ShapeFileRecord record )
        {
            record.RequireArgument<ShapeFileRecord>( "record" ).NotNull<ShapeFileRecord>();

            MultiPoint points = new MultiPoint();
            for( int i = 0; i < record.Points.Count; i++ )
            {
                System.Windows.Point point = record.Points[ i ];
                points.Points.Add( new MapPoint( point.X, point.Y ) );
            }

            return points;
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetClick(this UIElement target)
        {
            IObservable<IEvent<MouseButtonEventArgs>> gDown = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => target.MouseLeftButtonDown += ev,
                    ev => target.MouseLeftButtonDown -= ev
                );

            IObservable<IEvent<MouseButtonEventArgs>> gUp = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => target.MouseLeftButtonUp += ev,
                    ev => target.MouseLeftButtonUp -= ev
                );

            IObservable<IEvent<MouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => target.MouseMove += ev,
                    ev => target.MouseMove -= ev
                );

            // wait for any mouse left down event
            return gDown
            .SelectMany(
                mouseLeftButtonDownEvent =>
                    // then wait for a single mouse left up event
                    gUp
                    .TakeUntil(gMove)
                    .Take(1));
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetDoubleClick(this UIElement target, int millisecondsBetweenClick)
        {
            IObservable<IEvent<MouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => target.MouseMove += ev,
                    ev => target.MouseMove -= ev
                );

            var interval = TimeSpan.FromMilliseconds(millisecondsBetweenClick);

            return from first in target.GetClick()
                    from second in target.GetClick()
                    .TakeUntil(gMove)
                    .TakeUntil(
                        Observable
                        .Timer(interval)
                            .ObserveOnDispatcher()
                            .Take(1)
                    )
                    select first;
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetDoubleClick(this UIElement target)
        {
            return target.GetDoubleClick(500);
        }

        /// <summary>
        /// This click event allows for the mouse to move between mouse up and down events
        /// </summary>
        /// <param name="target">visual ui element where the click event should be observed</param>
        /// <returns></returns>
        public static IObservable<IEvent<MouseButtonEventArgs>> GetClick2(this UIElement target)
        {
            IObservable<IEvent<MouseButtonEventArgs>> gDown = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => target.MouseLeftButtonDown += ev,
                    ev => target.MouseLeftButtonDown -= ev
                );

            IObservable<IEvent<MouseButtonEventArgs>> gUp = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => target.MouseLeftButtonUp += ev,
                    ev => target.MouseLeftButtonUp -= ev
                );

            IObservable<IEvent<MouseEventArgs>> gEnter = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => target.MouseEnter += ev,
                    ev => target.MouseEnter -= ev
                );

            IObservable<IEvent<MouseEventArgs>> gLeave = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => target.MouseLeave += ev,
                    ev => target.MouseLeave -= ev
                );

            IObservable<IEvent<MouseButtonEventArgs>> appUp = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => Application.Current.RootVisual.MouseLeftButtonUp += ev,
                    ev => Application.Current.RootVisual.MouseLeftButtonUp -= ev
                );

            IObservable<IEvent<MouseEventArgs>> appLeave = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => Application.Current.RootVisual.MouseLeave += ev,
                    ev => Application.Current.RootVisual.MouseLeave -= ev
                );

            // wait for any mouse left down event
            return gDown
            .SelectMany(
                mouseLeftButtonDownEvent =>
                    // then wait for a single mouse left up event
                    gUp
                        .Take(1)
                        .TakeUntil(
                    // We want to merge two different stop conditions...
                            Observable.Merge(
                    // stop listening if the mouse goes outside
                    // the silvleright plug-in
                                appLeave
                    // We return unit so that we have the
                    // same type as the other observable
                    // we want to merge with
                                    .Select(_ => new Unit()),
                    // stop listening if the mouse goes outside the
                    // element and the mouse is released.
                                gLeave
                                    .SelectMany(
                                        mouseLeaveEvent =>
                                            // stop waiting for a mouse left up event
                                            // if the mouse leaves the element and the
                                            // button is released.
                                            // By listening for the event at the Root
                                            // Visual we ensure that we will get all
                                            // MouseLeftButtonUp events because this
                                            // event bubbles up.
                                            appUp
                                                .Take(1)
                                            // Return unit so that we can merge
                                                .Select(_ => new Unit())
                                            // don't cancel if the mouse enters
                                            // the element over which the mouse
                                            // was depressed.
                                                .TakeUntil(gEnter)))));
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetDoubleClick2(this UIElement target, int millisecondsBetweenClick)
        {
            IObservable<IEvent<MouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => target.MouseMove += ev,
                    ev => target.MouseMove -= ev
                );

            var interval = TimeSpan.FromMilliseconds(millisecondsBetweenClick);

            return from first in target.GetClick2()
                   from second in target.GetClick2()
                   .TakeUntil(gMove)
                   .TakeUntil(
                       Observable
                       .Timer(interval)
                           .ObserveOnDispatcher()
                           .Take(1)
                   )
                   select first;
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetDoubleClick2(this UIElement target)
        {
            return target.GetDoubleClick2(500);
        }

    }
}