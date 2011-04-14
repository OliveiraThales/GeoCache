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
using System.Collections.ObjectModel;

namespace Vishcious.ArcGIS.SLContrib
{
    public class RSSItem
    {
        // standard RSS properties
        public string Title
        {
            get;
            set;
        }
        public string Link
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Date
        {
            get;
            set;
        }

        public IEnumerable<SimpleElement> Elements
        {
            get;
            set;
        }

        // custom properties (be sure to update the LoadRSS method too)
        // public BitmapImage FlickrThumbnail { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
