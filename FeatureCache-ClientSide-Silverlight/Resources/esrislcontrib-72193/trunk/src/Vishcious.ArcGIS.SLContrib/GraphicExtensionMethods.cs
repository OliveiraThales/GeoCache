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
using System.Collections.Generic;
using System.Linq;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Symbols;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class GraphicExtensionMethods
    {
        public static IObservable<IEvent<MouseButtonEventArgs>> GetClick(this Graphic graphic)
        {
            IObservable<IEvent<MouseButtonEventArgs>> gDown = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => graphic.MouseLeftButtonDown += ev,
                    ev => graphic.MouseLeftButtonDown -= ev
                );

            IObservable<IEvent<MouseButtonEventArgs>> gUp = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => graphic.MouseLeftButtonUp += ev,
                    ev => graphic.MouseLeftButtonUp -= ev
                );

            IObservable<IEvent<MouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => graphic.MouseMove += ev,
                    ev => graphic.MouseMove += ev
                );

            // wait for any mouse left down event
            return gDown.SelectMany( e =>
                    // then wait for a single mouse left up event
                    gUp.Take(1).TakeUntil(gMove));
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetDoubleClick(this Graphic graphic, int millisecondsBetweenClick)
        {
            IObservable<IEvent<MouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => graphic.MouseMove += ev,
                    ev => graphic.MouseMove += ev
                );

            return (from first in graphic.GetClick()
                   from second in graphic.GetClick()
                   .TakeUntil(gMove)
                   .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(millisecondsBetweenClick)).Take(1))
                   select first).Repeat();
        }

        public static IObservable<IEvent<MouseButtonEventArgs>> GetDoubleClick(this Graphic graphic)
        {
            return graphic.GetDoubleClick(500);
        }

        public static IDisposable MakeDraggable(this Graphic graphic, Map map)
        {
            graphic.RequireArgument<Graphic>("graphic").NotNull<Graphic>();
            map.RequireArgument<Map>("map").NotNull<Map>();

            //IObservable<IEvent<MouseEventArgs>> graphicMouseMove = Observable.FromEvent
            //    (
            //        ( EventHandler<MouseEventArgs> ev ) => new MouseEventHandler( ev ),
            //        ev => graphic.MouseMove += ev,
            //        ev => graphic.MouseMove += ev
            //    );

            IObservable<IEvent<MouseEventArgs>> mapMouseMove = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => map.MouseMove += ev,
                    ev => map.MouseMove += ev
                );

            IObservable<IEvent<MouseButtonEventArgs>> graphicLeftMouseButtonDown = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => graphic.MouseLeftButtonDown += ev,
                    ev => graphic.MouseLeftButtonDown -= ev
                );

            //IObservable<IEvent<MouseButtonEventArgs>> graphicLeftMouseButtonUp = Observable.FromEvent
            //    (
            //        (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
            //        ev => graphic.MouseLeftButtonUp += ev,
            //        ev => graphic.MouseLeftButtonUp -= ev
            //    );

            IObservable<IEvent<MouseButtonEventArgs>> mapLeftMouseButtonUp = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => map.MouseLeftButtonUp += ev,
                    ev => map.MouseLeftButtonUp -= ev
                );

            //var stopper = graphicLeftMouseButtonUp.Merge(mapLeftMouseButtonUp);
            //var dragger = graphicMouseMove.Merge(mapMouseMove);
            var stopper = mapLeftMouseButtonUp;
            var dragger = mapMouseMove;

            IObservable<IEvent<MouseEventArgs>> drag = dragger.SkipUntil(graphicLeftMouseButtonDown)
                .TakeUntil(stopper)
                .Repeat();

            Func<IEvent<MouseEventArgs>, IEvent<MouseEventArgs>, Unit> handler = (prev, cur) =>
            {
                var prevMapPoint = map.ScreenToMap(prev.EventArgs.GetPosition(map));
                var curMapPoint = map.ScreenToMap(cur.EventArgs.GetPosition(map));
                var deltaX = curMapPoint.X - prevMapPoint.X;
                var deltaY = curMapPoint.Y - prevMapPoint.Y;
                graphic.Geometry.Offset(deltaX, deltaY);
                return new Unit();
            };

            Disposables removeHandlers = new Disposables();
            removeHandlers.Subscriptions.Add(graphicLeftMouseButtonDown.Subscribe(e => e.EventArgs.Handled = true));
            removeHandlers.Subscriptions.Add
                (
                    drag
                    .Zip(drag.Skip(1), handler)
                    .Subscribe()
                );

            return removeHandlers;
        }

        public static void Flash(this Graphic graphic, double milliseconds, int repeat)
        {
            graphic.RequireArgument<Graphic>("graphic").NotNull<Graphic>();

            int count = 1;
            repeat = repeat * 2;
            Symbol tempSymbol = graphic.Symbol;
            Storyboard storyboard = new Storyboard();
            storyboard.Duration = TimeSpan.FromMilliseconds(milliseconds);
            graphic.Symbol = null;
            storyboard.Completed += (sender, e) =>
            {
                if (count % 2 == 1)
                    graphic.Symbol = tempSymbol;
                else
                    graphic.Symbol = null;

                if (count <= repeat)
                    storyboard.Begin();

                count++;
            };
            storyboard.Begin();
        }

        public static void Flash(this Graphic graphic)
        {
            graphic.RequireArgument<Graphic>("graphic").NotNull<Graphic>();

            Flash(graphic, 200, 1);
        }
    }
}
