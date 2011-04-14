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
    public class EventArgs<T>: EventArgs
    {
        T EventData
        {
            get;
            set;
        }

        public EventArgs(T eventData)
        {
            EventData = eventData;
        }
    }
}
