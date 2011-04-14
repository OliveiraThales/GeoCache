using System.Collections.ObjectModel;
using System.Net;
using System.Xml.Linq;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Vishcious.ArcGIS.SLContrib
{
    /// <summary>
    /// This DataSource is designed to read "W3C geo GeoRSS" feeds
    /// </summary>
    public class RSSDataSource : ObservableCollection<RSSItem>
    {
        public ObservableCollection<SimpleElement> RSSElementAttributes
        {
            get;
            set;
        }

        public event EventHandler<EventArgs> LoadCompleted;
        public event EventHandler<EventArgs<string>> LoadFailed;
        public Uri URL
        {
            get;
            set;
        }

        public RSSDataSource()
        {
            RSSElementAttributes = new ObservableCollection<SimpleElement>();            
        }

        public void Load()
        {
            WebClient rssService = new WebClient();
            rssService.DownloadStringCompleted += new DownloadStringCompletedEventHandler( DownloadCompleted );
            rssService.DownloadStringAsync( URL );
        }

        private void DownloadCompleted( object sender, DownloadStringCompletedEventArgs e )
        {
            if( e.Error == null )
            {
                LoadRSS( e.Result );
                RaiseLoadCompletedEvent();
            }
            else
            {
                RaiseLoadFailedEvent( e.Error.Message );
            }
        }

        private void RaiseLoadCompletedEvent()
        {
            if( LoadCompleted != null )
            {
                LoadCompleted( this, new EventArgs() );
            }
        }

        private void RaiseLoadFailedEvent(string errorMsg)
        {
            if( LoadFailed != null )
            {
                LoadFailed( this, new EventArgs<string>( errorMsg ) );
            }
        }

        private void LoadRSS( string rss )
        {
            XElement feed = XElement.Parse( rss );
            RSSElementAttributes = SimpleElement.GetAttributes( feed );

            if( feed.Element( "channel" ) != null )
            {
                IEnumerable items =
                    from item in feed.Element( "channel" ).Elements( "item" )
                    select new RSSItem
                    {
                        // standard RSS properties
                        Title = item.Element( "title" ).Value,
                        Link = item.Element( "link" ).Value,
                        Description = item.Element( "description" ).Value,
                        Date = item.Element( "description" ).Value, //DateTime.Parse( item.Element( "pubDate" ).Value ),
                        Elements = GetElements(item)

                        // custom properties (be sure to update the RSSItem class too)
                        // FlickrThumbnail = new BitmapImage(new Uri((string)item.Element("{http://search.yahoo.com/mrss/}thumbnail").Attribute("url"))),
                    };

                foreach( RSSItem item in items )
                {
                    this.Add( item );
                }
            }
        }

        private ObservableCollection<SimpleElement> GetElements( XElement xElement )
        {
            ObservableCollection<SimpleElement> list = new ObservableCollection<SimpleElement>();

            if( xElement != null )
            {
                foreach(XElement item in xElement.Elements())
                {
                    list.Add( SimpleElement.Create(item) );
                }
            }

            return list;
        }
    }

}
    
        
        