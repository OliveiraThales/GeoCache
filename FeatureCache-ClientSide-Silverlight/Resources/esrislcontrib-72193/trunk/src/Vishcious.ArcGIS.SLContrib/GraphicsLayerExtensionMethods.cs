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
using ESRI.ArcGIS.Client;
using System.Linq;

namespace Vishcious.ArcGIS.SLContrib
{
    public static class GraphicsLayerExtensionMethods
    {
        public static IObservable<IEvent<GraphicMouseButtonEventArgs>> GetClick(this GraphicsLayer gLayer)
        {
            IObservable<IEvent<GraphicMouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<GraphicMouseEventArgs> ev) => new GraphicsLayer.MouseEventHandler(ev),
                    ev => gLayer.MouseMove += ev,
                    ev => gLayer.MouseMove -= ev
                );

            IObservable<IEvent<GraphicMouseButtonEventArgs>> gUp = Observable.FromEvent
                (
                    (EventHandler<GraphicMouseButtonEventArgs> ev) => new GraphicsLayer.MouseButtonEventHandler(ev),
                    ev => gLayer.MouseLeftButtonUp += ev,
                    ev => gLayer.MouseLeftButtonUp -= ev
                );

            IObservable<IEvent<GraphicMouseButtonEventArgs>> gDown = Observable.FromEvent
                (
                    (EventHandler<GraphicMouseButtonEventArgs> ev) => new GraphicsLayer.MouseButtonEventHandler(ev),
                    ev => gLayer.MouseLeftButtonDown += ev,
                    ev => gLayer.MouseLeftButtonDown -= ev
                );

            // wait for any mouse left down event
            return gDown.SelectMany(e =>
                // then wait for a single mouse left up event
                    gUp.Take(1).TakeUntil(gMove));
        }

        public static IObservable<IEvent<GraphicMouseButtonEventArgs>> GetDoubleClick(this GraphicsLayer gLayer, int millisecondsBetweenClick)
        {
            IObservable<IEvent<GraphicMouseEventArgs>> gMove = Observable.FromEvent
                (
                    (EventHandler<GraphicMouseEventArgs> ev) => new GraphicsLayer.MouseEventHandler(ev),
                    ev => gLayer.MouseMove += ev,
                    ev => gLayer.MouseMove -= ev
                );

            return (from first in gLayer.GetClick()
                    from second in gLayer.GetClick()
                    .TakeUntil(gMove)
                    .TakeUntil(Observable.Timer(TimeSpan.FromMilliseconds(millisecondsBetweenClick)).Take(1))
                    select first).Repeat();
        }

        public static IObservable<IEvent<GraphicMouseButtonEventArgs>> GetDoubleClick(this GraphicsLayer gLayer)
        {
            return gLayer.GetDoubleClick(500);
        }

        public static IDisposable MakeDraggable(this GraphicsLayer gLayer, Map map)
        {
            gLayer.RequireArgument<GraphicsLayer>("gLayer").NotNull<GraphicsLayer>();
            map.RequireArgument<Map>("map").NotNull<Map>();

            IObservable<IEvent<MouseEventArgs>> dragger = Observable.FromEvent
                (
                    (EventHandler<MouseEventArgs> ev) => new MouseEventHandler(ev),
                    ev => map.MouseMove += ev,
                    ev => map.MouseMove += ev
                );

            IObservable<IEvent<GraphicMouseButtonEventArgs>> starter = Observable.FromEvent
                (
                    (EventHandler<GraphicMouseButtonEventArgs> ev) => new GraphicsLayer.MouseButtonEventHandler(ev),
                    ev => gLayer.MouseLeftButtonDown += ev,
                    ev => gLayer.MouseLeftButtonDown -= ev
                );

            IObservable<IEvent<MouseButtonEventArgs>> stopper = Observable.FromEvent
                (
                    (EventHandler<MouseButtonEventArgs> ev) => new MouseButtonEventHandler(ev),
                    ev => map.MouseLeftButtonUp += ev,
                    ev => map.MouseLeftButtonUp -= ev
                );

            IObservable<IEvent<MouseEventArgs>> drag = dragger.SkipUntil(starter)
                .TakeUntil(stopper)
                .Repeat();

            Guid state = Guid.NewGuid();
            Guid lastState = state;
            Graphic g = null;
            Func<IEvent<MouseEventArgs>, IEvent<MouseEventArgs>, Unit> handler = (prev, cur) =>
            {
                if (g == null)
                    return new Unit();
                if (state != lastState)
                {
                    lastState = state;
                    return new Unit();
                }
                var prevMapPoint = map.ScreenToMap(prev.EventArgs.GetPosition(map));
                var curMapPoint = map.ScreenToMap(cur.EventArgs.GetPosition(map));
                g.Geometry.Offset((curMapPoint.X - prevMapPoint.X), (curMapPoint.Y - prevMapPoint.Y));
                lastState = state;
                return new Unit();
            };

            Disposables removeHandlers = new Disposables();
            removeHandlers.Subscriptions.Add(starter.Subscribe(e =>
            {
                g = e.EventArgs.Graphic;
                state = Guid.NewGuid();
                e.EventArgs.Handled = true;
            }
            ));
            removeHandlers.Subscriptions.Add(stopper.Subscribe(e =>
            {
                state = Guid.NewGuid();
                g = null;
            }));
            removeHandlers.Subscriptions.Add
                (
                    drag
                    .Zip(drag.Skip(1), handler)
                    .Subscribe()
                );

            return removeHandlers;
        }
    }
}
