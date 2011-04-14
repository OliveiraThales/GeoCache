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

namespace Vishcious.ArcGIS.SLContrib
{
    public class ActionDisposable : IDisposable
    {
        public IDisposable Subscription { get; set; }
        public Action PreDisposeAction { get; set; }

        public ActionDisposable()
        {

        }

        public ActionDisposable(IDisposable subscription, Action preDisposeAction)
        {
            Subscription = subscription;
            PreDisposeAction = preDisposeAction;
        }

        public void Dispose()
        {
            PreDisposeAction();
            Subscription.Dispose();
        }
    }
}
