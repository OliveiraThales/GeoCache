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

namespace Vishcious.ArcGIS.SLContrib
{
    public class Disposables : IDisposable
    {
        public List<IDisposable> Subscriptions { get; private set; }

        public Disposables()
        {
            Subscriptions = new List<IDisposable>();
        }

        public void Dispose()
        {
            if (Subscriptions != null)
            {
                Subscriptions.ForEach(i =>
                {
                    if (i != null)
                        i.Dispose();
                });
            }
        }
    }
}
